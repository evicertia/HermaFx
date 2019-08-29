using System;
using System.Reflection;
using System.Collections.Concurrent;

using Rebus;

using Castle.DynamicProxy;


namespace HermaFx.Rebus.LockedSagaDefer.Sagas
{
	public class SagaDataTracingInterceptor : StandardInterceptor
	{
		private static readonly global::Common.Logging.ILog _Log = global::Common.Logging.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private static readonly Type IAmInitiatedByType = typeof(IAmInitiatedBy<>);
		private readonly ConcurrentDictionary<Type, PropertyInfo> _sagaDataCache = new ConcurrentDictionary<Type, PropertyInfo>();
		private readonly ConcurrentDictionary<Type, Type> _handlerCache = new ConcurrentDictionary<Type, Type>();

		private ITraceableSagaData GetSagaData(Saga saga)
		{
			var property = _sagaDataCache.GetOrAdd(saga.GetType(), x => saga.GetType().GetProperty(nameof(Saga<ISagaData>.Data)));
			var data = property.GetValue(saga) as ITraceableSagaData;

			Guard.Against<InvalidOperationException>(data == null,
				$"SagaData for saga {saga.GetType()} does not implemente {nameof(ITraceableSagaData)}?!"
			);

			return data;
		}

		private bool IsIAmInitiatedByInvocation(IInvocation invocation)
		{
			var messageType = invocation.Arguments[0].GetType();
			var handlerType = _handlerCache.GetOrAdd(messageType, x => IAmInitiatedByType.MakeGenericType(x));
			return handlerType.IsAssignableFrom(invocation.TargetType);
		}

		protected override void PerformProceed(IInvocation invocation)
		{
			Guard.Against<InvalidOperationException>(
				invocation.Method.Name != "Handle" || !invocation.Method.IsPublic || invocation.Arguments.Length != 1 ||
				invocation.InvocationTarget as Saga == null,
				$"Invoked {GetType().Name} interceptor for a non-handle method ({invocation.Method.Name})?!"
			);

			var saga = (invocation.InvocationTarget as Saga);
			var sagaData = GetSagaData(saga);

			if (IsIAmInitiatedByInvocation(invocation) && saga.IsNew && sagaData.CreatedOn == null)
			{
				sagaData.CreatedOn = DateTime.UtcNow;
			}
			base.PerformProceed(invocation);
		}
	}
}
