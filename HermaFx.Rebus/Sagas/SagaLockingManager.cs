using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

using Rebus;
using Rebus.Bus;
using Rebus.Configuration;
using Rebus.Messages;
using Rebus.Logging;

namespace HermaFx.Rebus.Sagas
{
	// FIXME: This may have some chances of dead locking when two messages
	//		  end up invoking the same two or more sagas on alternating order,
	//		  and the SagaStore allows multiple sagas to handle the same message.
	//		  Like:
	//			- Received Message-A (MessageContext established for A)
	//			- BeforeHandling Saga#1 for Message-A (SagaData#1 -- Locked)
	//			- Received Message-B (MessageContext established for B)
	//			- BeforeHandling Saga#2 for Message-B (SagaData#2 -- Locked)
	//			- BeforeHandling Saga#2 for Message-A (SagaData#2 -- Blocked.. [still locked by B])
	//			- BeforeHandling Saga#1 for Message-B (SagaData#1 -- Deadlock.. [still locked by A])
	public class SagaLockingManager
	{
		#region Fields
		private static ILog _log;
		private readonly ConfigurationBackbone _backbone;
		private readonly ISagaLockingProvider _provider;
		private readonly TimeSpan? _timeout;
		#endregion

		#region .ctor
		internal SagaLockingManager(ConfigurationBackbone backbone, ISagaLockingProvider provider, TimeSpan? timeout)
		{
			Guard.IsNotNull(backbone, "backbone");
			Guard.IsNotNull(provider, "provider");
			Guard.IsNotNull(timeout, nameof(timeout));

			this._backbone = backbone;
			this._provider = provider;
			this._timeout = timeout;

			RebusLoggerFactory.Changed += f => _log = f.GetCurrentClassLogger();
			this._backbone.ConfigureEvents(x => AttachEventHandlers(x));
		}
		#endregion

		#region GetLocksFor
		private static readonly string LOCK_ITEM_KEY = typeof(SagaLockingManager).Name + "::Locks";

		private ConcurrentDictionary<Type, IDisposable> GetLocksFor(IMessageContext context)
		{
			// I am afraid of await-based dispatching provoking
			// concurrent access to Items dictionary..
			lock (context.Items)
			{
				if (!context.Items.ContainsKey(LOCK_ITEM_KEY))
				{
					context.Items[LOCK_ITEM_KEY] = new ConcurrentDictionary<Type, IDisposable>();
				}

				return context.Items[LOCK_ITEM_KEY] as ConcurrentDictionary<Type, IDisposable>;
			}
		}
		#endregion

		#region GetSagaData
		private readonly ConcurrentDictionary<Type, PropertyInfo> _sagaDataGetters = new ConcurrentDictionary<Type, PropertyInfo>();

		private ISagaData GetSagaData(Saga saga)
		{
			var getter = _sagaDataGetters.GetOrAdd(saga.GetType(), x => x.GetProperty("Data"));
			return getter.GetValue(saga, null) as ISagaData;
		}
		#endregion

		#region Event Handlers

		/// <summary>
		/// Enables the ability to use saga locking.
		/// </summary>
		private IRebusEvents AttachEventHandlers(IRebusEvents events)
		{
			events.MessageContextEstablished += OnMessageContextEstablished;
			events.BeforeHandling += OnBeforeHandlingEvent;
			//events.AfterHandling += OnAfterHandlingEvent;
			//events.OnHandlingError += OnHandlingError;

			return events;
		}

		private void OnMessageContextEstablished(IBus bus, IMessageContext context)
		{
			context.Disposed += () => OnMessageContextDisposed(context);
		}

		private void OnBeforeHandlingEvent(IBus bus, object message, IHandleMessages handler)
		{
			if (!(handler is Saga)) return;

			var saga = handler as Saga;
			var data = GetSagaData(saga);
			var context = MessageContext.GetCurrent();
			var locks = GetLocksFor(context);
			IDisposable thelock = null;

			Guard.Against<InvalidOperationException>(data == null, "Missing SagaData.");
			Guard.Against<InvalidOperationException>(locks == null, "Missing locks dictionary.");

			//TODO: When _timeout has value we have to use Lock(SagaData, TimeSpan) instead of Lock(SagaData)
			_log.Debug("Trying to adquire lock for saga {0} with id {1}..", saga.GetType().Name, data.Id);
			if (!locks.TryAdd(data.GetType(), (thelock = _provider.Lock(data))))
			{
				ExceptionExtensions.Shallow(() => thelock.Dispose());
				throw new InvalidOperationException("Unable to store lock at locks dictionary.");
			}
			_log.Debug("Lock adquired for saga {0} with id {1}..", saga.GetType().Name, data.Id);
		}

		private void OnMessageContextDisposed(IMessageContext context)
		{
			if (context.CurrentMessage == null)
			{
				_log.Warn("Received event MessageContextDisposed with a null CurrentMessage?!");
			}

			var message = context.CurrentMessage;
			var messageTypeName = message != null ? message.GetType().Name : "(unknown)";
			var locks = GetLocksFor(context);

			_log.Debug("Disposing locks for message of type {0} with transport-id: {1}", messageTypeName, context.RebusTransportMessageId);

			foreach (var item in locks)
			{
				_log.Debug("Disposing lock of {0} with transport-id: {1}", item.Key.Name, context.RebusTransportMessageId);
				item.Value.Dispose();
				_log.Debug("Done disposing lock of {0} with transport-id: {1}", item.Key.Name, context.RebusTransportMessageId);
			}

			locks.Clear();

			_log.Debug("Done disposing locks for message of type {0} with transport-id: {1}", messageTypeName, context.RebusTransportMessageId);
		}
		#endregion
	}
}