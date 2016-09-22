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

		public static void ReplyToOriginator<TResponse>(this IBus bus, IMessageContext messageContext, TResponse response)
		{
			Guard.IsNotNull(bus, nameof(bus));
			Guard.IsNotNull(messageContext, nameof(messageContext));
			Guard.IsNotNull(response, nameof(response));

			var correlationKey = Headers.CorrelationId;

			bus.AttachHeader(response, correlationKey, messageContext.Headers[correlationKey]?.ToString());
			bus.Advanced.Routing.Send(messageContext.ReturnAddress, response);
		}
	}
}
