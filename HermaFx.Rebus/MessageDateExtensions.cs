using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rebus;
using Rebus.Configuration;

namespace HermaFx.Rebus
{
	public static class MessageDateExtensions
	{
		public static string MessageDataHeader = "message-date";

		private static void SetDateOnMessageSent(IBus bus, string destination, object message)
		{
			bus.AttachHeader(message, MessageDataHeader, DateTime.UtcNow.ToString("o"));
		}

		public static RebusConfigurer SetDateOnSentMessages(this RebusConfigurer configurer)
		{
			configurer.Events(e =>
			{
				e.MessageSent += SetDateOnMessageSent;
			});
			return configurer;
		}

		public static RebusConfigurer RequireDateOnMessages(this RebusConfigurer configurer, IEnumerable<Type> inclusions = null, IEnumerable<Type> exclusions = null)
		{
			configurer.Events(e =>
			{
				e.BeforeMessage += (bus, message) =>
				{
					if (message == null) return;
					if (exclusions != null && exclusions.Contains(message.GetType())) return;
					if (inclusions != null && !inclusions.Contains(message.GetType())) return;

					if (!MessageContext.GetCurrent().Headers.ContainsKey(MessageDataHeader))
						throw new ArgumentException("Message missing 'message-date' header!");
				};
			});
			return configurer;
		}
	}
}
