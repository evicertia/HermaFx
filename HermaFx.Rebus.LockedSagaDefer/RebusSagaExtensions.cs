using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rebus.Configuration;
using HermaFx.Rebus.LockedSagaDefer.MessageDeferringSaga;

namespace HermaFx.Rebus.LockedSagaDefer
{
	public static class RebusSagaExtensions
	{
		public static RebusSagasConfigurer WithDeferredLocking(this RebusSagasConfigurer configurer)
		{
			configurer.Backbone.StoreSagaData = new MessageDeferringSagaPersister(configurer.Backbone.StoreSagaData);

			return configurer;
		}
	}
}
