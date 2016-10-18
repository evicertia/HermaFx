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

		public static void ReplyTo<TResponse>(this IBus bus, string originator, TResponse message)
		{
			Guard.IsNotNull(bus, nameof(bus));
			Guard.IsNotNullNorEmpty(originator, nameof(originator));
			Guard.IsNotNull(message, nameof(message));

			bus.Advanced.Routing.Send(originator, message);
		}

		/// <summary>
		/// Replies to origin saga correlating with sagaId parameter.
		/// </summary>
		/// <exception cref="System.InvalidOperationException">The message not contains saga context</exception>
		public static void ReplyToSaga<TResponse>(this IBus bus, string originator, Guid sagaId, TResponse message)
		{
			Guard.IsNotNull(bus, nameof(bus));
			Guard.IsNotNullNorEmpty(originator, nameof(originator));
			Guard.IsNotDefault(sagaId, nameof(sagaId));
			Guard.IsNotNull(message, nameof(message));

			var messageContext = MessageContext.GetCurrent();
			var sagaContextKey = SagaContext.SagaContextItemKey;

			// Ensure the SagaContext
			if (!messageContext.Items.ContainsKey(sagaContextKey))
			{
				throw new InvalidOperationException("The message context not contains a saga context");
			}

			// Current saga context
			var currentSagaContext = messageContext.Items[sagaContextKey];
			try
			{
				// XXX: The .Routing.Send() method is overwriting the AutoCorrelationSagaId header by current SagaConext.Id,
				//		so we need to (temporarilly) remove the existing sagaContext from messageContext.
				messageContext.Items.Remove(sagaContextKey);
				bus.AttachHeader(message, Headers.AutoCorrelationSagaId, sagaId.ToString());
				bus.ReplyTo(originator, message);
			}
			finally
			{
				// XXX: Restore the current SagaConext, no matter what.
				messageContext.Items[sagaContextKey] = currentSagaContext;
			}

		}

		/// <summary>
		/// If sagaId is not null or default then replies to origin saga correlating with sagaId parameter,
		/// else send normal replies to originator
		/// </summary>
		public static void ReplyTo<TResponse>(this IBus bus, string originator, TResponse message, Guid? sagaId)
		{
			Guard.IsNotNull(bus, nameof(bus));
			Guard.IsNotNullNorEmpty(originator, nameof(originator));
			Guard.IsNotNull(message, nameof(message));

			if (sagaId.GetValueOrDefault() != default(Guid))
			{
				bus.ReplyToSaga(originator, sagaId.Value, message);
			}
			else
			{
				bus.ReplyTo(originator, message);
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
