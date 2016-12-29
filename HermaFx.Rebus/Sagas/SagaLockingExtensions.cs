using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rebus;
using Rebus.Configuration;

namespace HermaFx.Rebus.Sagas
{
	public static class SagaLockingExtensions
	{
		public static RebusSagasConfigurer WithSagaLocking(this RebusSagasConfigurer configurer, ISagaLockingProvider provider
			, TimeSpan? timeout = null)
		{
			var manager = new SagaLockingManager(configurer.Backbone, provider, timeout);
			return configurer;
		}
	}
}