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

		private static ILog _Log = LogProvider.GetCurrentClassLogger();
		private readonly MessageDeferringSagaDataProxyGenerator _proxyGenerator = new MessageDeferringSagaDataProxyGenerator();
		private readonly IStoreSagaData _inner;
		private readonly Func<Exception, bool> _filter;

		public MessageDeferringSagaPersister(IStoreSagaData inner, Func<Exception, bool> filter)
		{
			_inner = inner.ThrowIfNull(nameof(inner));
			_filter = filter.ThrowIfNull(nameof(filter));
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
			catch (Exception ex) when (_filter(ex))
			{
				_Log.InfoFormat("Returning fake sagaData for locked saga of type {0}, with {1} = {2}.",
					typeof(T).Name, sagaDataPropertyPath, fieldFromMessage
				);

				return _proxyGenerator.CreateProxy<T>();
			}
		}
	}
}
