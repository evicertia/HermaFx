using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Rebus.Shared;

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
			Guard.IsNotNull(@this.Backbone.SendMessages, "A transport ISendMessages interface must be first configured");

			@this.Backbone.SendMessages = new OutgoingMessageLogger(@this.Backbone.SendMessages, logLevel);
			return @this;
		}
	}
}
