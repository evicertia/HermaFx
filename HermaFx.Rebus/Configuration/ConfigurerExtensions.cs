using Rebus.Bus;

using HermaFx;
using HermaFx.Logging;

namespace Rebus.Configuration
{
	public static class ConfigurerExtensions
	{
		public static RebusConfigurer LogIncomingMessages(this RebusConfigurer @this, LogLevel logLevel = LogLevel.Debug)
		{
			var logger = new IncommingMessageLogger(logLevel);
			@this.Events(e => e.BeforeTransportMessage += (bus, message) => logger.Log(message));
			return @this;
		}

		public static RebusConfigurer LogOutgoingMessages(this RebusConfigurer @this, LogLevel logLevel = LogLevel.Debug)
		{
			Guard.IsNotNull(@this.Backbone.SendMessages, "A transport sender interface (ISendMessages or IMulticastTransport) interface must be first configured.");

			var multicast = @this.Backbone.SendMessages as IMulticastTransport;

			if (multicast == null)
			{
				@this.Backbone.SendMessages = new OutgoingMessageLogger(@this.Backbone.SendMessages, logLevel);
				return @this;
			}

			@this.Backbone.SendMessages = new MulticastOutgoingMessageLogger(multicast, logLevel);

			return @this;
		}
	}
}
