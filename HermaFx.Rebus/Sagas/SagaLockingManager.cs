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
	public class SagaLockingManager
	{
		#region Fields
		private static readonly string LOCK_ITEM_KEY = typeof(SagaLockingManager).Name + "::Lock";

		private static ILog _log;
		private readonly ConfigurationBackbone _backbone;
		private readonly ISagaLockingProvider _provider;
		#endregion

		#region .ctor
		internal SagaLockingManager(ConfigurationBackbone backbone, ISagaLockingProvider provider)
		{
			Guard.IsNotNull(backbone, "backbone");
			Guard.IsNotNull(provider, "provider");

			this._backbone = backbone;
			this._provider = provider;
			RebusLoggerFactory.Changed += f => _log = f.GetCurrentClassLogger();
			this._backbone.ConfigureEvents(x => AttachEventHandlers(x));
		}
		#endregion

		#region GetSagaData
		private readonly ConcurrentDictionary<Type, PropertyInfo> _sagaDataGetters = new ConcurrentDictionary<Type, PropertyInfo>();

		private ISagaData GetSagaData(Saga saga)
		{
			var getter = _sagaDataGetters.GetOrAdd(saga.GetType(), x => x.GetType().GetProperty("Data"));
			return getter.GetValue(saga, null) as ISagaData;
		}
		#endregion

		#region Event Handlers

		/// <summary>
		/// Enables the ability to use IdempotentSagas.
		/// </summary>
		private IRebusEvents AttachEventHandlers(IRebusEvents events)
		{
			events.BeforeHandling += OnBeforeHandlingEvent;
			events.AfterHandling += OnAfterHandlingEvent;
			events.OnHandlingError += OnHandlingError;

			return events;
		}

		private void OnBeforeHandlingEvent(IBus bus, object message, IHandleMessages handler)
		{
			if (!(handler is Saga)) return;

			var saga = handler as Saga;
			var data = GetSagaData(saga);
			var context = MessageContext.GetCurrent();

			Guard.Against<InvalidOperationException>(
				context.Items.ContainsKey(LOCK_ITEM_KEY), 
				"An existing lock has already been found at MessageContext.Items?!?!"
			);

			_log.Debug("Trying to adquire lock for saga {0} with id {1}..", saga.GetType().Name, data.Id);
			 context.Items[LOCK_ITEM_KEY] = _provider.Lock(saga);
			 _log.Debug("Lock adquired for saga {0} with id {1}..", saga.GetType().Name, data.Id);
		}

		private void OnAfterHandlingEvent(IBus bus, object message, IHandleMessages handler)
		{
			if (!(handler is Saga)) return;

			var saga = handler as Saga;
			var data = GetSagaData(saga);
			var context = MessageContext.GetCurrent();

			if (!context.Items.ContainsKey(LOCK_ITEM_KEY))
			{
				_log.Warn("No lock found at MessageContex.Items for saga {0}, with id {1}.", handler.GetType().Name, data.Id);
				return;
			}

			(context.Items[LOCK_ITEM_KEY] as IDisposable).Dispose();
			context.Items.Remove(LOCK_ITEM_KEY);
		}

		private void OnHandlingError(Exception exception)
		{
			if (MessageContext.HasCurrent) return;

			var context = MessageContext.GetCurrent();

			if (context.Items.ContainsKey(LOCK_ITEM_KEY))
			{
				(context.Items[LOCK_ITEM_KEY] as IDisposable).Dispose();
				context.Items.Remove(LOCK_ITEM_KEY);
			}
		}
		#endregion
	}
}
