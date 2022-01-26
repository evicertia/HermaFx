using System;

namespace HermaFx.Rebus
{
	public class MessageDeferringSagaSettings
	{
		public TimeSpan LockedSagasDeferInterval { get; set; }

		public MessageDeferringSagaSettings()
		{
		}
	}
}
