using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HermaFx.Rebus.LockedSagaDefer.MessageDeferringSaga
{
	public class MessageDeferringSagaSettings
	{
		public TimeSpan LockedSagasDeferInterval { get; set; }

		public MessageDeferringSagaSettings()
		{
		}
	}
}
