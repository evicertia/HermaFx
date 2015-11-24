using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rebus;
using Rebus.Configuration;

namespace HermaFx.Rebus
{
	public static class SetDateOnSentMessagesExtensions
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
	}
}
