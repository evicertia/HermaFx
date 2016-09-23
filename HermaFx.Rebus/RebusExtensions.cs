using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

using Rebus;
using Rebus.Shared;

namespace HermaFx.Rebus
{
	public static class RebusExtensions
	{
		#region Send/Reply/ReplyTo

		public static void Send<TCommand>(this IBus bus, Action<TCommand> customizer)
			where TCommand : new()
		{
			Guard.IsNotNull(bus, nameof(bus));
			Guard.IsNotNull(customizer, nameof(customizer));

			var message = Activator.CreateInstance<TCommand>();
			customizer(message);

			bus.Send(message);
		}

		public static void SendLocal<TCommand>(this IBus bus, Action<TCommand> customizer)
			where TCommand : new()
		{
			Guard.IsNotNull(bus, nameof(bus));
			Guard.IsNotNull(customizer, nameof(customizer));

			var message = Activator.CreateInstance<TCommand>();
			customizer(message);

			bus.Send(message);
		}

		public static void Publish<TEvent>(this IBus bus, Action<TEvent> customizer)
			where TEvent : new()
		{
			Guard.IsNotNull(bus, nameof(bus));
			Guard.IsNotNull(customizer, nameof(customizer));

			var message = Activator.CreateInstance<TEvent>();
			customizer(message);

			bus.Publish(message);
		}

		public static void Reply<TResponse>(this IBus bus, Action<TResponse> customizer)
			where TResponse : new()
		{
			Guard.IsNotNull(bus, nameof(bus));
			Guard.IsNotNull(customizer, nameof(customizer));

			var message = Activator.CreateInstance<TResponse>();
			customizer(message);

			bus.Reply(message);
		}

		public static void ReplyTo<TResponse>(this IBus bus, string originator, string correlationId, TResponse message)
		{
			Guard.IsNotNull(bus, nameof(bus));
			Guard.IsNotNullNorEmpty(originator, nameof(originator));
			Guard.IsNotNullNorEmpty(correlationId, nameof(correlationId));
			Guard.IsNotNull(message, nameof(message));

			bus.AttachHeader(message, Headers.CorrelationId, correlationId);
			bus.Advanced.Routing.Send(originator, message);
		}

		public static void ReplyTo<TResponse>(this IBus bus, string originator, string correlationId, Action<TResponse> customizer)
		{
			Guard.IsNotNull(bus, "bus");

			var message = Activator.CreateInstance<TResponse>();
			customizer(message);

			bus.ReplyTo(originator, correlationId, message);
		}

		#endregion

		#region DeclareSubscriptionsFor

		public static void DeclareSubscriptionsFor(this IBus bus, Assembly assembly)
		{
			var busSubscribeMethod = bus.GetType().GetMethod("Subscribe", new Type[] { });
			var messageWeWantToSubscribeTo = assembly
					.ExportedTypes
					.SelectMany(x => x.GetInterfaces())
					.Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISubscribeTo<>))
					.Select(x => x.GetGenericArguments()[0])
					.ToArray();

			foreach (var msg in messageWeWantToSubscribeTo)
			{
				//_Log.InfoFormat("Subscribing via bus to: {0}", msg);
				var @delegate = busSubscribeMethod.MakeGenericMethod(msg);
				@delegate.Invoke(bus, new object[] { });
			}
		}

		#endregion
	}
}
