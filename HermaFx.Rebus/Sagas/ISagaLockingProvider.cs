using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Rebus;

namespace HermaFx.Rebus.Sagas
{
	/// <summary>
	/// Actual provider implementating locking primitives.
	/// </summary>
	/// <remarks>
	/// Lock provider should take care of automatically refresh locks if/when required.
	/// </remarks>
	public interface ISagaLockingProvider
	{
		ISagaLock TryLock(Saga handler);
		ISagaLock Lock(Saga handler);
		ISagaLock Lock(Saga handler, TimeSpan timeout);
	}
}
