using System;
using Rebus;

namespace HermaFx.Rebus
{
	public interface ISagaWithCreatedOn : ISagaData
	{
		DateTime? CreatedOn { get; set; }
	}
}
