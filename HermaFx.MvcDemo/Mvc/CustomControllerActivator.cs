#if false
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using Castle.Windsor;

namespace HermaFx.MvcDemo.Mvc
{
	public class CustomControllerActivator : IControllerActivator
	{
		private readonly IWindsorContainer container;

		public CustomControllerActivator(IWindsorContainer container)
		{
			this.container = container;
		}

		public IController Create(RequestContext requestContext, Type controllerType)
		{
			return (IController)((IServiceProvider)container.Kernel).GetService(controllerType);
		}
	}
}
#endif