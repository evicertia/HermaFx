using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using System.Reflection;

using HermaFx;
using HermaFx.Mvc;

namespace HermaFx.MvcDemo.Mvc
{
	public class CustomControllerFactory : DefaultControllerFactory
	{
		private static readonly global::Common.Logging.ILog _Log = global::Common.Logging.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

#if false
		private readonly IWindsorContainer _container;

		public VeritasControllerFactory(IWindsorContainer container)
		{
			this._container = container;
		}
#endif

		private static Type GetHttpAttributeFor(string method)
		{
			switch (method)
			{
				case "POST": return typeof(HttpPostAttribute);
				case "PUT": return typeof(HttpPutAttribute);
				case "OPTIONS": return typeof(HttpOptionsAttribute);
				case "DELETE": return typeof(HttpDeleteAttribute);
				default: return typeof(HttpGetAttribute);
			}
		}

		protected override SessionStateBehavior GetControllerSessionBehavior(RequestContext context, Type controllerType)
		{
			if (controllerType == null)
			{
				return SessionStateBehavior.Default;
			}

			var bflags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
			var actionName = context.RouteData.Values["action"].IfNotNull(x=>x.ToString().ToLowerInvariant());
			var actionMethods = controllerType.GetMethods(bflags)
				.Where(x => x.Name.ToLowerInvariant() == actionName)
				.Where(x => x.GetCustomAttribute<NonActionAttribute>() == null) // XXX: Avoid non-invocable methods (like T4MVC's stubs)
				.ToArray();
			var actionMethod = actionMethods.FirstOrDefault();

			if (actionMethods.Count() > 1)
			{
				var attr = GetHttpAttributeFor(context.HttpContext.Request.RequestType);
				actionMethod = actionMethods.FirstOrDefault(x => x.GetCustomAttribute(attr, false) != null);
			}

			if (actionMethod != null)
			{
				var actionSessionStateAttr = actionMethod.GetCustomAttribute<ActionSessionStateAttribute>(false);
				if (actionSessionStateAttr != null)
				{
					return actionSessionStateAttr.Behavior;
				}
			}

			return base.GetControllerSessionBehavior(context, controllerType);
		}

#if false
		protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
		{
			_Log.InfoFormat("Resolving controller of type: {0}", controllerType.IfNotNull(x => x.Name));

			if (controllerType == null)
			{
				throw new HttpException(404, string.Format("The controller for path '{0}' could not be found.", requestContext.HttpContext.Request.Path));
			}
			return (IController)_container.Resolve(controllerType);
		}

		public override void ReleaseController(IController controller)
		{
			// If controller implements IDisposable, clean it up responsibly
			var disposableController = controller as IDisposable;
			if (disposableController != null)
			{
				disposableController.Dispose();
			}

			_container.Release(controller);
		}
#endif
	}
}