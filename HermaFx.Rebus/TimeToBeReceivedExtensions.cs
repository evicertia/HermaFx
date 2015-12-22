using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using HermaFx.DataAnnotations;

using Rebus;
using Rebus.Shared;
using Rebus.Configuration;
using Rebus.Logging;

namespace HermaFx.Rebus
{
	public static class RebusConfigurerExtensions
	{
        private static ILog _log;

        #region SetTimeToBeRecived
        public static RebusConfigurer SetTimeToBeReceivedFrom<TAttribute>(this RebusConfigurer configurer, Func<TAttribute, TimeSpan> getter)
			where TAttribute : System.Attribute
		{
			Guard.IsNotNull(() => configurer, configurer);
			Guard.IsNotNull(() => getter, getter);

			return configurer.Events(e =>
			{
				e.MessageSent += (advbus, destination, message) =>
				{
					var attribute = message.GetType()
						.GetCustomAttributes(typeof(TAttribute), false)
						.Cast<TAttribute>()
						.SingleOrDefault();

					if (attribute != null)
					{
						advbus.AttachHeader(message, Headers.TimeToBeReceived, getter(attribute).ToString());
					}
				};
			});
		}

		public static RebusConfigurer UseTimeoutAttribute(this RebusConfigurer configurer)
		{
			return configurer.SetTimeToBeReceivedFrom<TimeoutAttribute>(x => x.Timeout);
		}
		#endregion

		#region RequireTimeToBeReceived
		private const string MESSAGE_DATE_HEADER = "message-date";

		private static bool MessageHasExpired(IMessageContext context)
		{

			if (!context.Headers.ContainsKey(Headers.TimeToBeReceived))
				return false;

			if (!context.Headers.ContainsKey(MESSAGE_DATE_HEADER))
				throw new InvalidOperationException("Message is missing a MESSAGE_DATE header?!");

			var messageDate = DateTime.Parse(context.Headers[MESSAGE_DATE_HEADER].ToString());
			var ttl = TimeSpan.Parse(context.Headers[Headers.TimeToBeReceived].ToString());

			if ((messageDate + ttl) > DateTime.UtcNow)
				return true;

			return false;
		}

		public static RebusConfigurer RequireTimeToBeReceivedUsing<TAttribute>(this RebusConfigurer configurer)
			where TAttribute : System.Attribute
		{
			Guard.IsNotNull(() => configurer, configurer);

			return configurer.Events(e =>
			{
				e.BeforeMessage += (bus, message) =>
				{
					if (message == null) return;

					var type = message.GetType();
					var context = MessageContext.GetCurrent();

					if (type.GetCustomAttribute<TAttribute>() != null
						&& !context.Headers.ContainsKey(Headers.TimeToBeReceived))
					{
						throw new InvalidOperationException("Message is missing 'TimeToBeReceived' header!");
					}

					if (MessageHasExpired(context))
					{
						RebusLoggerFactory.Changed += f => _log = f.GetCurrentClassLogger();
						_log.Debug("The Message with RebusTransportMessageId {0} expired and will be Abort.", context.RebusTransportMessageId);
						context.Abort();
					}
				};
			});
		}

		public static RebusConfigurer RequireTimeoutAttribute(this RebusConfigurer configurer)
		{
			return RequireTimeToBeReceivedUsing<TimeoutAttribute>(configurer);
		}
		#endregion
	}

}
