using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

using Castle.DynamicProxy;

using HermaFx.Logging;

using Rebus;
using Rebus.Shared;

namespace HermaFx.Rebus
{
	/// <summary>
	/// This interceptor is inteded to defer processing of incomming
	/// messages when the target saga is now being locked by another
	/// worker, so we can reduce contention on database and avoid
	/// having too many workers starving.
	/// </summary>
	public class MessageDeferringSagaInterceptor : StandardInterceptor
	{
		private const string HERMAFX_LOCKING_DEFER = "HermaFx:SagaLockingDeferred";

		private static ILog _Log = LogProvider.GetCurrentClassLogger();
		private readonly ConcurrentDictionary<Type, PropertyInfo> _sagaDataCache = new ConcurrentDictionary<Type, PropertyInfo>();

		private readonly IBus _bus;
		private readonly MessageDeferringSagaSettings _settings;
		private readonly Action<IBus, TimeSpan, object> _deferCallback;

		public MessageDeferringSagaInterceptor(IBus bus, MessageDeferringSagaSettings settings)
		{
			_bus = bus.ThrowIfNull(nameof(bus));
			_settings = settings;
			_deferCallback = settings?.DeferCallback ?? Defer;
		}

		private static void Defer(IBus bus, TimeSpan delay, object message) => bus.Defer(delay, message);

		private bool IsLockedSaga(Saga saga)
		{
			var property = _sagaDataCache.GetOrAdd(saga.GetType(), x => saga.GetType().GetProperty(nameof(Saga<ISagaData>.Data)));
			var sagaData = property.GetValue(saga) as ISagaData;

			return !saga.IsNew && MessageDeferringSagaPersister.IsLockedSagaData(sagaData);
		}

		private bool IsFlaggedAsDeferred()
		{
			if (!MessageContext.HasCurrent)
				return false;

			var context = MessageContext.GetCurrent();

			return context.Items.ContainsKey(HERMAFX_LOCKING_DEFER) && Convert.ToBoolean(context.Items[HERMAFX_LOCKING_DEFER]) == true;
		}

		private void FlagAsDeferred()
		{
			if (!MessageContext.HasCurrent)
				return;

			MessageContext.GetCurrent().Items[HERMAFX_LOCKING_DEFER] = true;
		}

		protected override void PerformProceed(IInvocation invocation)
		{
			Guard.Against<InvalidOperationException>(
				invocation.Method.Name != "Handle" || !invocation.Method.IsPublic || invocation.Arguments.Length != 1,
				$"Invoked {GetType().Name} interceptor for a non-handle method ({invocation.Method.Name})?!"
			);

			if (IsLockedSaga(invocation.InvocationTarget as Saga) && !IsFlaggedAsDeferred())
			{
				var message = invocation.GetArgumentValue(0);

				_Log.DebugFormat("Deferring handling of message of type {0}, due to target saga being currently locked.", message?.GetType());

				if (MessageContext.HasCurrent)
				{
					// The Rebus transport message ID can not be overwritten
					foreach (var header in MessageContext.GetCurrent().Headers.Where(x => x.Key != Headers.MessageId))
					{
						_bus.AttachHeader(message, header.Key, Convert.ToString(header.Value));
					}
				}

				_deferCallback(_bus, _settings.LockedSagasDeferInterval, message);

				MessageContext.GetCurrent().Abort();

				// Flag the MessageContext as deferred to avoid generating multiple timeouts
				// when a message that is handled by multiple sagas has at least one of them locked
				FlagAsDeferred();

				return;
			}

			base.PerformProceed(invocation);
		}
	}
}
