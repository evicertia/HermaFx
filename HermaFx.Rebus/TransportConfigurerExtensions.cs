using System.Net;

using Rebus;
using Rebus.RabbitMQ;
using Rebus.Configuration;

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

		private static void DeleteQueue(string connectionString, string queueName)
		{
			//XXX: No need to remove subscriptions due to Error Queue nature that has not subscriptions like input's one.
			using (var connection = new RabbitMQ.Client.ConnectionFactory { Uri = connectionString }.CreateConnection())
			using (var model = connection.CreateModel())
			{
				// just ignore if it fails...
				try
				{
					model.QueueDelete(queueName, ifUnused: true, ifEmpty: true);
				}
				catch { }
			}
		}
		#endregion

		public static RabbitMqOptions UseRabbitMqFromConfigWithLocalName(this RebusTransportConfigurer configurer, string connectionString, string separator)
		{
			var section = RebusConfigurationSection.LookItUp();
			section.VerifyPresenceOfInputQueueConfig();
			section.VerifyPresenceOfErrorQueueConfig();
			var hostname = GetHostName();
			var inputQueue = $"{section.InputQueue}{separator}{hostname}";
			var errorQueue = $"{section.ErrorQueue}{separator}{hostname}";
			return configurer.UseRabbitMq(connectionString, inputQueue, errorQueue);
		}

		public static RabbitMqOptions UseRabbitMqFromConfigWithLocalName(this RebusTransportConfigurer configurer, string connectionString)
		{
			return configurer.UseRabbitMqFromConfigWithLocalName(connectionString, DEFAULT_SEPARATOR);
		}

		public static RebusTransportConfigurer RemoveErrorQueueOnDispose(this RebusTransportConfigurer configurer, string connectionString, string errorQueue)
		{
			Guard.IsNotNullNorEmpty(connectionString, nameof(connectionString));
			Guard.IsNotNullNorEmpty(errorQueue, nameof(errorQueue));

			configurer.AddDecoration(x => x.ConfigureEvents(ev =>
				ev.BusStopped += new BusStoppedEventHandler((bus) =>
					DeleteQueue(connectionString, errorQueue))));

			return configurer;
		}
	}
}
