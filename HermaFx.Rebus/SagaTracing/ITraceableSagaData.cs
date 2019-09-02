using System;
using Rebus;

namespace HermaFx.Rebus
{
	public interface ITraceableSagaData : ISagaData
	{
		DateTime? CreatedOn { get; set; }
	}
}
