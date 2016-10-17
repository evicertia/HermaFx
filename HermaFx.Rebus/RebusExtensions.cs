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

		public static void ReplyTo<TResponse>(this IBus bus, string originator, TResponse message, Guid? autoCorrelationSagaId)
		{
			Guard.IsNotNull(bus, nameof(bus));
			Guard.IsNotNullNorEmpty(originator, nameof(originator));
			Guard.IsNotNull(message, nameof(message));

			var sagaKey = SagaContext.SagaContextItemKey;
			var messageContext = MessageContext.GetCurrent();

			if (autoCorrelationSagaId.HasValue && messageContext.Items.ContainsKey(sagaKey))
			{
				// Current saga context
				var currentSagaContext = messageContext.Items[sagaKey];

				// XXX: The .Routing.Send() method is overwriting the AutoCorrelationSagaId header by current SagaConext.Id,
				//		then we must to replace the current SagaConext.Id of message context by 'autoCorrelationSagaId' in order to reply
				//		to original saga wicht send the request
				messageContext.Items[sagaKey] = new SagaContext(autoCorrelationSagaId.Value);
				bus.Advanced.Routing.Send(originator, message);

				// XXX: Dont forquet restore the current SagaConext in order to correlate the Timeouts message of current saga.
				messageContext.Items[sagaKey] = currentSagaContext;
			}
			else
			{
				bus.Advanced.Routing.Send(originator, message);
			}
		}

		public static void ReplyTo<TResponse>(this IBus bus, string originator, Guid? autoCorrelationSagaId, Action<TResponse> customizer)
		{
			Guard.IsNotNull(bus, "bus");

			var message = Activator.CreateInstance<TResponse>();
			customizer(message);

			bus.ReplyTo(originator, message, autoCorrelationSagaId);
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
