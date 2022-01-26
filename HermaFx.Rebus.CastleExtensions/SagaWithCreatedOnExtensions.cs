using System;

using Castle.Windsor;
using Castle.MicroKernel.Registration;

using Rebus;
using Rebus.Configuration;
using Rebus.Castle.Windsor;

namespace HermaFx.Rebus
{
	public static class SagaWithCreatedOnExtensions
	{
		private static IWindsorContainer GetContainer(IActivateHandlers activator)
		{
			var adapter = (activator as WindsorContainerAdapter).ThrowIfNull("Rebus backbone is not using a Castle Container?!");
			return adapter.Container;
		}

		public static RebusSagasConfigurer WithSagaWithCreatedOn(this RebusSagasConfigurer configurer)
		{
			var container = GetContainer(configurer.Backbone.ActivateHandlers);

			container.Register(Component.For<SagaWithCreatedOnInterceptor>());
			container.Register(Component.For<SagaWithCreatedOnHandlerProxyGenerationHook>());

			return configurer;

		}

		public static BasedOnDescriptor ConfigureSagaWithCreatedOnHandling(this BasedOnDescriptor descriptor, Func<Type, bool> selector)
		{
			Guard.IsNotNull(selector, "selector");

			descriptor
				.ConfigureIf(x => selector(x.Implementation),
					c => c.Interceptors<SagaWithCreatedOnInterceptor>()
						  .Proxy.Hook(r => r.Service<SagaWithCreatedOnHandlerProxyGenerationHook>())
				);

			return descriptor;
		}
	}
}