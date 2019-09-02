using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rebus.Configuration;

namespace HermaFx.Rebus
{
	public static class RebusSagaExtensions
	{
		public static RebusSagasConfigurer WithDeferredLocking(
			this RebusSagasConfigurer configurer,
			Func<Exception, bool> sagaLockedExceptionFilter
			)
		{

			configurer.Backbone.StoreSagaData = new MessageDeferringSagaPersister(configurer.Backbone.StoreSagaData, sagaLockedExceptionFilter);
			return configurer;
		}
	}
}
