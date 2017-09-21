using System;
using System.Collections.Generic;

using Rebus;

namespace HermaFx.Rebus
{
	public class DualSagaPersister : IStoreSagaData
	{
		private const string SAGA_PERSISTER_IMPL = nameof(DualSagaPersister) + ":PersisterImpl:";

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

			GetStoreSagaDataImplementation(
				(bool)MessageContext.GetCurrent().Items[SAGA_PERSISTER_IMPL + sagaData.GetType().FullName])
				.Update(sagaData, sagaDataPropertyPathsToIndex);
		}

		public void Delete(ISagaData sagaData)
		{
			if (!MessageContext.HasCurrent)
			{
				throw new InvalidOperationException("Rebus MessageContext has no Current context, DualSagaPersister can't delete the saga");
			}

			GetStoreSagaDataImplementation(
				(bool)MessageContext.GetCurrent().Items[SAGA_PERSISTER_IMPL + sagaData.GetType().FullName])
				.Delete(sagaData);
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
				MessageContext.GetCurrent().Items[SAGA_PERSISTER_IMPL + persisterData.GetType().FullName] = IsNewStoreSagaData(_newSagaPersister);
				return persisterData;
			}

			persisterData = _oldSagaPersister.Find<T>(sagaDataPropertyPath, fieldFromMessage);
			if (persisterData != null)
			{
				MessageContext.GetCurrent().Items[SAGA_PERSISTER_IMPL + persisterData.GetType().FullName] = IsNewStoreSagaData(_oldSagaPersister);
				return persisterData;
			}

			return null;
		}

		/// <summary>
		/// Gets the appropriate IStoreSagaData
		/// </summary>
		/// <param name="isNewStoreSagaData">True if new IStoreSagaData. False otherwise.</param>
		/// <returns>IStoreSagaData instance</returns>
		private IStoreSagaData GetStoreSagaDataImplementation(bool isNewStoreSagaData)
		{
			return isNewStoreSagaData ? _newSagaPersister : _oldSagaPersister;
		}

		private bool IsNewStoreSagaData(IStoreSagaData sagaPersister)
		{
			return sagaPersister == _newSagaPersister;
		}
	}
}
