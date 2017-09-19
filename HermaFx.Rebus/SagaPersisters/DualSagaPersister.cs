using System;
using System.Collections.Generic;

using Rebus;

namespace HermaFx.Rebus
{
	public class DualSagaPersister : IStoreSagaData
	{
		private const string DUAL_SAGA_ID = "HermaFx.Rebus:Dual_Saga_";

		private IStoreSagaData _oldSagaPersister;
		private IStoreSagaData _newSagaPersister;

		public DualSagaPersister(IStoreSagaData oldSagaPersister, IStoreSagaData newSagaPersister)
		{
			_oldSagaPersister = oldSagaPersister;
			_newSagaPersister = newSagaPersister;
		}

		public void Insert(ISagaData sagaData, string[] sagaDataPropertyPathsToIndex)
		{
			// New insertions will always be persisted under the new IStoreSagaData
			_newSagaPersister.Insert(sagaData, sagaDataPropertyPathsToIndex);
		}

		public void Update(ISagaData sagaData, string[] sagaDataPropertyPathsToIndex)
		{
			if (!MessageContext.HasCurrent)
			{
				throw new InvalidOperationException("Rebus MessageContext has no Current context, DualSagaPersister can't update the saga");
			}

			(MessageContext.GetCurrent().Items[DUAL_SAGA_ID + sagaData.GetType().FullName] as IStoreSagaData).Update(sagaData, sagaDataPropertyPathsToIndex);
		}

		public void Delete(ISagaData sagaData)
		{
			if (!MessageContext.HasCurrent)
			{
				throw new InvalidOperationException("Rebus MessageContext has no Current context, DualSagaPersister can't delete the saga");
			}

			(MessageContext.GetCurrent().Items[DUAL_SAGA_ID + sagaData.GetType().FullName] as IStoreSagaData).Delete(sagaData);
		}

		public T Find<T>(string sagaDataPropertyPath, object fieldFromMessage) where T : class, ISagaData
		{
			if (!MessageContext.HasCurrent)
			{
				throw new InvalidOperationException("Rebus MessageContext has no Current context, DualSagaPersister can't find the saga");
			}

			var persisterData = _newSagaPersister.Find<T>(sagaDataPropertyPath, fieldFromMessage);
			if (persisterData != null)
			{
				MessageContext.GetCurrent().Items[DUAL_SAGA_ID + persisterData.GetType().FullName] = _newSagaPersister;
				return persisterData;
			}

			persisterData = _oldSagaPersister.Find<T>(sagaDataPropertyPath, fieldFromMessage);
			if (persisterData != null)
			{
				MessageContext.GetCurrent().Items[DUAL_SAGA_ID + persisterData.GetType().FullName] = _oldSagaPersister;
				return persisterData;
			}

			return null;
		}
	}
}
