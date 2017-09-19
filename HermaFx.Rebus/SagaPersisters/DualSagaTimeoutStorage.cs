using System.Linq;

using Rebus.Timeout;

namespace HermaFx.Rebus
{
	public class DualSagaTimeoutStorage : IStoreTimeouts
	{
		private IStoreTimeouts _oldTimeoutStorage;
		private IStoreTimeouts _newTimeoutStorage;

		public DualSagaTimeoutStorage(IStoreTimeouts oldTimeoutStorage, IStoreTimeouts newTimeoutStorage)
		{
			_oldTimeoutStorage = oldTimeoutStorage;
			_newTimeoutStorage = newTimeoutStorage;
		}

		public void Add(Timeout newTimeout)
		{
			// New insertions will always be persisted under the new IStoreTimeouts
			_newTimeoutStorage.Add(newTimeout);
		}

		DueTimeoutsResult IStoreTimeouts.GetDueTimeouts()
		{
			return new DueTimeoutsResult(_oldTimeoutStorage.GetDueTimeouts().DueTimeouts.Union(_newTimeoutStorage.GetDueTimeouts().DueTimeouts));
		}
	}
}
