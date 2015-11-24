using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using HermaFx.DataAnnotations;

using Rebus;
using Rebus.Shared;
using Rebus.Configuration;

namespace HermaFx.Rebus
{
	public static class RebusConfigurerExtensions
	{
		public static RebusConfigurer UseTimeToExpireFrom<TAttribute>(this RebusConfigurer configurer, Func<TAttribute, TimeSpan> getter)
			where TAttribute : System.Attribute
		{
			Guard.IsNotNull(() => configurer, configurer);
			Guard.IsNotNull(() => getter, getter);

			return configurer.Events(e =>
			{
				e.MessageSent += (advbus, destination, message) =>
				{
					var attribute = message.GetType()
						.GetCustomAttributes(typeof(TimeoutAttribute), false)
						.Cast<TimeoutAttribute>()
						.SingleOrDefault();

					if (attribute != null)
					{
						advbus.AttachHeader(message, Headers.TimeToBeReceived, getter.ToString());
					}
				};
			});
		}

		public static RebusConfigurer UseTimeoutAttribute(this RebusConfigurer configurer)
		{
			return configurer.UseTimeToExpireFrom<TimeoutAttribute>(x => x.Timeout);
		}

		public static RebusConfigurer RequireTimeToAs<TAttribute>(this RebusConfigurer configurer)
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
				};
			});
		}

		public static RebusConfigurer RequireTimeoutAttribute(this RebusConfigurer configurer)
		{
			return RequireTimeToAs<TimeoutAttribute>(configurer);
		}
	}

}
