using System;
using System.Reflection;
using Rebus;
using Rebus.Configuration;
using Rebus.Timeout;

namespace HermaFx.Rebus
{
	public static class DualSagaExtensions
	{
		private static readonly BindingFlags _PrivateBflags = BindingFlags.NonPublic | BindingFlags.Instance;

		// Access to RebusSagasConfigurer(Backbone) constructor through reflection
		private static readonly ConstructorInfo _RebusSagaConfigurerCtor = typeof(RebusSagasConfigurer).GetConstructor(_PrivateBflags, null, new Type[] {typeof(ConfigurationBackbone)}, null);
		private static readonly Func<ConfigurationBackbone, RebusSagasConfigurer> RebusSagasConfigurerFactory = x => _RebusSagaConfigurerCtor.Invoke(new object[] { x }) as RebusSagasConfigurer;

		// Access to RebusTimeoutsConfigurer(Backbone) constructor through reflection
		private static readonly ConstructorInfo _RebusTimeoutsConfigurerCtor = typeof(RebusTimeoutsConfigurer).GetConstructor(_PrivateBflags, null, new Type[] { typeof(ConfigurationBackbone) }, null);
		private static readonly Func<ConfigurationBackbone, RebusTimeoutsConfigurer> RebusTimeoutsConfigurerFactory = x => _RebusTimeoutsConfigurerCtor.Invoke(new object[] { x }) as RebusTimeoutsConfigurer;

		/// <summary>
		/// Store sagas using two IStoreSagaDatas.
		/// </summary>
		/// <param name="configurer">SagasConfigurer to use DualSagaPersister in</param>
		/// <param name="oldStoreSagaData">Sagas are found in this IStoreSagaData and updated.</param>
		/// <param name="newStoreSagaData">Sagas are found in this IStoreSagaData and updated, as well as inserted.</param>
		public static void StoreInDualPersister(this RebusSagasConfigurer configurer, Action<RebusSagasConfigurer> oldStoreSagaData, Action<RebusSagasConfigurer> newStoreSagaData)
		{
			var backbone1 = new ConfigurationBackbone((IContainerAdapter)configurer.Backbone.ActivateHandlers);
			var configurer1 = RebusSagasConfigurerFactory(backbone1);

			oldStoreSagaData(configurer1);

			var backbone2 = new ConfigurationBackbone((IContainerAdapter)configurer.Backbone.ActivateHandlers);
			var configurer2 = RebusSagasConfigurerFactory(backbone2);

			newStoreSagaData(configurer2);

			configurer.Use(new DualSagaPersister(configurer1.Backbone.StoreSagaData, configurer2.Backbone.StoreSagaData));
		}

		/// <summary>
		/// Store saga timeouts using two IStoreTimeouts.
		/// </summary>
		/// <param name="configurer">TimeoutsConfigurer to use DualSagaPersister in</param>
		/// <param name="oldStoreTimeouts">Timeouts are found in this IStoreTimeouts.</param>
		/// <param name="newStoreTimeouts">Timeouts are found in this IStoreSagaData as well as inserted.</param>
		public static void StoreInDualPersister(this RebusTimeoutsConfigurer configurer, Action<RebusTimeoutsConfigurer> oldStoreTimeouts, Action<RebusTimeoutsConfigurer> newStoreTimeouts)
		{
			var backbone1 = new ConfigurationBackbone((IContainerAdapter)configurer.Backbone.ActivateHandlers);
			var configurer1 = RebusTimeoutsConfigurerFactory(backbone1);

			oldStoreTimeouts(configurer1);

			var backbone2 = new ConfigurationBackbone((IContainerAdapter)configurer.Backbone.ActivateHandlers);
			var configurer2 = RebusTimeoutsConfigurerFactory(backbone2);

			newStoreTimeouts(configurer2);

			configurer.Use(new DualSagaTimeoutStorage(configurer1.Backbone.StoreTimeouts, configurer2.Backbone.StoreTimeouts));
		}
	}
}
