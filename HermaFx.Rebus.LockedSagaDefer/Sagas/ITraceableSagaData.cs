using System;
using Rebus;

namespace HermaFx.Rebus.LockedSagaDefer.Sagas
{
	public interface ITraceableSagaData : ISagaData
	{
		DateTime? CreatedOn { get; set; }
	}
}
