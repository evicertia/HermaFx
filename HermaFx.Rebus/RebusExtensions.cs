using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Rebus;
using Rebus.Shared;

namespace HermaFx.Rebus
{
	public static class RebusExtensions
	{
		public static void Send<TCommand>(this IBus bus, Action<TCommand> customizer)
			where TCommand : new()
		{
			Guard.IsNotNull(bus, "bus");

			var message = Activator.CreateInstance<TCommand>();
			if (customizer != null) customizer(message);

			bus.Send(message);
		}

		public static void SendLocal<TCommand>(this IBus bus, Action<TCommand> customizer)
			where TCommand : new()
		{
			Guard.IsNotNull(bus, "bus");

			var message = Activator.CreateInstance<TCommand>();
			if (customizer != null) customizer(message);

			bus.Send(message);
		}

		public static void Publish<TEvent>(this IBus bus, Action<TEvent> customizer)
			where TEvent : new()
		{
			Guard.IsNotNull(bus, "bus");

			var message = Activator.CreateInstance<TEvent>();
			if (customizer != null) customizer(message);

			bus.Publish(message);
		}

		public static void Reply<TResponse>(this IBus bus, Action<TResponse> customizer)
			where TResponse : new()
		{
			Guard.IsNotNull(bus, "bus");

			var message = Activator.CreateInstance<TResponse>();
			if (customizer != null) customizer(message);

			bus.Reply(message);
		}

		public static void ReplyTo<TMessage>(this IBus bus, string originator, string correlationId, TMessage message)
		{
			Guard.IsNotNull(bus, nameof(bus));
			Guard.IsNotNullNorEmpty(originator, nameof(originator));
			Guard.IsNotNullNorEmpty(correlationId, nameof(correlationId));
			Guard.IsNotNull(message, nameof(message));

			bus.AttachHeader(message, Headers.CorrelationId, correlationId);
			bus.Advanced.Routing.Send(originator, message);
		}
	}
}
