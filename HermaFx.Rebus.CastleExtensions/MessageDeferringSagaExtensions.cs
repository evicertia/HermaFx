using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Castle.Windsor;
using Castle.MicroKernel.Registration;

using Rebus;
using Rebus.Configuration;
using Rebus.Castle.Windsor;

namespace HermaFx.Rebus
{
	public static class RebusSagaExtensions
	{
		private static IWindsorContainer GetContainer(IActivateHandlers activator)
		{
			var adapter = (activator as WindsorContainerAdapter).ThrowIfNull("Rebus backbone is not using a Castle Container?!");
			return adapter.Container;
		}

		public static RebusSagasConfigurer WithDeferredLocking(
			this RebusSagasConfigurer configurer,
			TimeSpan lockedSagasDeferInterval,
			Func<Exception, bool> sagaLockedExceptionFilter
			)
		{
			var container = GetContainer(configurer.Backbone.ActivateHandlers);

			container.Register(
				Component.For<MessageDeferringSagaInterceptor>(),
				Component.For<MessageDeferringSagaSettings>().Instance(
					new MessageDeferringSagaSettings()
					{
						LockedSagasDeferInterval = lockedSagasDeferInterval
					})
			);

			// When Deferring of messages targeting locked sagas is enabled
			// we need to register a SagaPersister wrapper in order to handle
			// saga-locked exceptions and convert them to fake-sagadata objects.
			configurer.Backbone.StoreSagaData = new MessageDeferringSagaPersister(configurer.Backbone.StoreSagaData, sagaLockedExceptionFilter);

			return configurer;
		}

		public static BasedOnDescriptor ConfigureMessageDeferringSelector(this BasedOnDescriptor descriptor, Func<Type, bool> selector)
		{
			Guard.IsNotNull(selector, "selector");

			descriptor
				.ConfigureIf(x => selector(x.Implementation),
					c => c.Interceptors<MessageDeferringSagaInterceptor>()
				);

			return descriptor;
		}
	}
}
