using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HermaFx.Rebus.Sagas
{
	/// <summary>
	/// A lock held against a saga.
	/// </summary>
	/// <remarks>
	/// In order to release the lock, just dispose this instance.
	/// </remarks>
	public interface ISagaLock : IDisposable
	{
		Guid SagaId { get; }
		string SagaName { get; }
	}
}
