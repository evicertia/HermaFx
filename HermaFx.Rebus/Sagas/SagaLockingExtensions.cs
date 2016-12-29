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
		/// <summary>
		/// Extension to allow saga locking while handling messages.
		/// </summary>
		/// <param name="configurer">The own RebusSagaConfigurer instance</param>
		/// <param name="provider">ISagaLockingProvider instance implementation</param>
		/// <param name="timeout">Nullable Timespan to allow lock retry until TimeOut reached</param>
		/// <returns>The own RebusSagaConfigurer Instance</returns>
		public static RebusSagasConfigurer WithSagaLocking(this RebusSagasConfigurer configurer, ISagaLockingProvider provider
			, TimeSpan? timeout = null)
		{
			var manager = new SagaLockingManager(configurer.Backbone, provider, timeout);
			return configurer;
		}
	}
}