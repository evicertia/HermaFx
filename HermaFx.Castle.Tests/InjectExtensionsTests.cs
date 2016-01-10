using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;

using NUnit.Framework;

namespace HermaFx.Castle
{
	[TestFixture]
	public class InjectExtensionsTests
	{
		#region Inner Classes

		public class SimpleClass
		{
			public IService Dependency { get; protected set; }
		}

		public interface IService
		{
		}

		public class ServiceImpl : IService
		{
		}

		#endregion

		[Test]
		public void InjectDependencies_ShouldPopulateInterfacePropertyOnObject_GivenTheInterfaceIsRegisteredWithTheContainer()
		{
			var container = new WindsorContainer();
			container.Register(Component.For<IService>().ImplementedBy<ServiceImpl>());

			var objectWithDependencies = new SimpleClass();
			container.Kernel.InjectDependencies(objectWithDependencies);
			container.Kernel.ReleaseDependencies(objectWithDependencies);

			Assert.That(objectWithDependencies.Dependency, Is.InstanceOf<ServiceImpl>());

		}

	}
}
