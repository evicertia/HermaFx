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

		/// <summary>
		/// Replies to a simple handler or a handler of saga and correlates the saga with sagaId parameter if it have value.
		/// </summary>
		/// <exception cref="System.InvalidOperationException">The message not contains saga context</exception>
		public static void ReplyTo<TResponse>(this IBus bus, string originator, TResponse message, Guid? sagaId)
		{
			Guard.IsNotNull(bus, nameof(bus));
			Guard.IsNotNullNorEmpty(originator, nameof(originator));
			Guard.IsNotNull(message, nameof(message));

			var messageContext = MessageContext.GetCurrent();
			var sagaContextKey = SagaContext.SagaContextItemKey;

			// If current message must be replayed to a saga handler then chenge currentSagaContext
			if (sagaId.HasValue)
			{
				if (!messageContext.Items.ContainsKey(sagaContextKey))
				{
					throw new InvalidOperationException("The message context not contains a saga context");
				}

				// Current saga context
				var currentSagaContext = messageContext.Items[sagaContextKey];
				try
				{
					// XXX: The .Routing.Send() method is overwriting the AutoCorrelationSagaId header by current SagaConext.Id,
					//		then we must to replace the current SagaConext.Id of message context by 'autoCorrelationSagaId' in order to reply
					//		to original saga wicht send the request
					messageContext.Items[sagaContextKey] = new SagaContext(sagaId.Value);
					bus.Advanced.Routing.Send(originator, message);
				}
				finally
				{
					// XXX: Dont forquet restore the current SagaConext in order to correlate the Timeouts message of current saga.
					messageContext.Items[sagaContextKey] = currentSagaContext;
				}
			}
			else
			{
				// At this point we reply to a simple handler (No saga)
				bus.Advanced.Routing.Send(originator, message);
			}
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
