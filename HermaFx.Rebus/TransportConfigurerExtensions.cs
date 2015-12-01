using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using Rebus;
using Rebus.Configuration;
using Rebus.RabbitMQ;

namespace HermaFx.Rebus
{
	public static class TransportConfigurerExtensions
	{
		private const string DEFAULT_SEPARATOR = "%";
		#region Private Helpers
		private static string GetHostName()
		{
			string hostName = Dns.GetHostName();
			return (hostName.Contains(".")) ? hostName.Substring(0, hostName.IndexOf(".")) : hostName;
		}
		#endregion

		public static RabbitMqOptions UseRabbitMqFromConfigWithLocalName(this RebusTransportConfigurer configurer, string connectionString, string separator)
		{
			var section = RebusConfigurationSection.LookItUp();
			section.VerifyPresenceOfInputQueueConfig();
			section.VerifyPresenceOfErrorQueueConfig();
			var hostname = GetHostName();
			var inputQueue = string.Format("{0}{2}{1}", section.InputQueue, hostname);
			var errorQueue = string.Format("{0}{2}{1}", section.ErrorQueue, hostname);
			return configurer.UseRabbitMq(connectionString, inputQueue, errorQueue);
		}

		public static RabbitMqOptions UseRabbitMqFromConfigForClient(this RebusTransportConfigurer configurer, string connectionString)
		{
			return configurer.UseRabbitMqFromConfigWithLocalName(connectionString, DEFAULT_SEPARATOR);
		}
	}
}
