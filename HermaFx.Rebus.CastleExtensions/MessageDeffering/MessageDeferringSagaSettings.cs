using System;

using Rebus;

namespace HermaFx.Rebus
{
	public class MessageDeferringSagaSettings
	{
		public TimeSpan LockedSagasDeferInterval { get; set; }

		public Action<IBus, TimeSpan, object> DeferCallback { get; set; }

		public MessageDeferringSagaSettings()
		{
		}
	}
}
