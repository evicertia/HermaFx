using System;
using Rebus;

using HermaFx.Logging;

namespace HermaFx.Rebus
{
	public class MessageDeferringSagaPersister : IStoreSagaData, ICanUpdateMultipleSagaDatasAtomically
	{
		#region IMessageDeferringSagaData
		// Marker interface intended for identifying fake
		// ISagaData objects created by this persister.
		public interface ILockedSagaData
		{
		}
		#endregion

		private const string SAGA_LOCKED_EXCEPTION_NAME = "AdoNetSagaLockedException";

		private static ILog _Log = LogProvider.GetCurrentClassLogger();
		private readonly MessageDeferringSagaDataProxyGenerator _proxyGenerator = new MessageDeferringSagaDataProxyGenerator();
		private readonly IStoreSagaData _inner;

		public MessageDeferringSagaPersister(IStoreSagaData inner)
		{
			_inner = inner.ThrowIfNull(nameof(inner));
		}

		public static bool IsLockedSagaData(ISagaData obj)
		{
			return obj is MessageDeferringSagaPersister.ILockedSagaData;
		}

		public void Delete(ISagaData sagaData)
		{
			Guard.Against<InvalidOperationException>(IsLockedSagaData(sagaData), "Called 'Delete' on a fake SagaData?!");

			_inner.Delete(sagaData);
		}

		public void Insert(ISagaData sagaData, string[] sagaDataPropertyPathsToIndex)
		{
			Guard.Against<InvalidOperationException>(IsLockedSagaData(sagaData), "Called 'Insert' on a fake SagaData?!");

			_inner.Insert(sagaData, sagaDataPropertyPathsToIndex);
		}

		public void Update(ISagaData sagaData, string[] sagaDataPropertyPathsToIndex)
		{
			// On fake sagas we should skip invoking inner persister.
			if (!IsLockedSagaData(sagaData))
			{
				_inner.Update(sagaData, sagaDataPropertyPathsToIndex);
			}
		}

		public T Find<T>(string sagaDataPropertyPath, object fieldFromMessage)
			 where T : class, ISagaData
		{
			try
			{
				return _inner.Find<T>(sagaDataPropertyPath, fieldFromMessage);
			}
			catch (Exception exception)
			{
				if (exception.GetType().Name == SAGA_LOCKED_EXCEPTION_NAME)
				{
					_Log.InfoFormat("Returning fake sagaData for locked saga of type {0}, with {1} = {2}.",
						typeof(T).Name, sagaDataPropertyPath, fieldFromMessage
					);

					return _proxyGenerator.CreateProxy<T>();
				}

				throw;
			}
		}
	}
}
