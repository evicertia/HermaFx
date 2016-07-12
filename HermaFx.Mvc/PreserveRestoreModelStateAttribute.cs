using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using HermaFx;

namespace HermaFx.Mvc
{
	/// <summary>
	/// Base class for preserving/restoring ModelState between PRG requests.
	/// </summary>
	[global::System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
	public abstract class PreserveRestoreModelStateAttribute : ActionFilterAttribute
	{
		protected const string Key = "PreserveModelStateAttribute:TempDataKey";

		protected void PreserveModelState(ControllerContext context)
		{
			context.Controller.TempData[Key] = context.Controller.ViewData.ModelState;
		}

		protected void RestoreModelState(ControllerContext context)
		{
			var state = GetModelState(context);

			if (state != null)
			{
				context.Controller.ViewData.ModelState.Merge(state);
			}
		}

		protected ModelStateDictionary GetModelState(ControllerContext context)
		{
			return context.Controller.TempData[Key] as ModelStateDictionary;
		}

		protected void RemoveModelState(ControllerContext context)
		{
			context.Controller.TempData.Remove(Key);
		}
	}

	/// <summary>
	/// Preserves ModelState before redirecting a PRG request by saving it on TempData.
	/// </summary>
	[global::System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
	public sealed class PreserveModelStateAttribute : PreserveRestoreModelStateAttribute
	{
		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			if ((filterContext.Result is RedirectResult) || (filterContext.Result is RedirectToRouteResult))
			{
				PreserveModelState(filterContext);
			}

			base.OnActionExecuted(filterContext);
		}
	}

	/// <summary>
	/// Restores ModelState after redirection of a PRG request by reading it from TempData.
	/// </summary>
	[global::System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
	public sealed class RestoreModelStateAttribute : PreserveRestoreModelStateAttribute
	{
		/// <summary>
		/// Defines wether we should restore model state during OnActionExecuting (false)
		/// event or during OnActionExecuted (true - default).
		/// </summary>
		public bool OnEjecuted { get; set; }

		// XXX: Dunno if this one is really needed, so I'll let this wander around
		//              for a time. If no one ends up using it, it will be removed. (pruiz)
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (OnEjecuted) return;

			RestoreModelState(filterContext);

			base.OnActionExecuting(filterContext);
		}

		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			if (!OnEjecuted) return;

			//Only Import if we are viewing
			if (filterContext.Result is ViewResultBase)
			{
				RestoreModelState(filterContext);
			}
			else
			{
				RemoveModelState(filterContext);
			}

			base.OnActionExecuted(filterContext);
		}
	}

	/// From https://github.com/benfoster/Fabrik.Common/blob/master/src/Fabrik.Common.Web/Filters/ValidateModelStateAttribute.cs
	/// More info in: http://benfoster.io/blog/automatic-modelstate-validation-in-aspnet-mvc
	/// <summary>
	/// An ActionFilter for automatically validating ModelState before a controller action is executed.
	/// Performs a Redirect if ModelState is invalid. Assumes the <see cref="ImportModelStateFromTempDataAttribute"/> is used on the GET action.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class ValidateModelStateAttribute : PreserveRestoreModelStateAttribute
	{
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="ValidateModelStateAttribute"/> is redirect.
		/// </summary>
		/// <remarks>Defaults to 'true'</remarks>
		/// <value>
		///   <c>true</c> if redirect; otherwise, <c>false</c>.
		/// </value>
		public bool Redirect { get; set; }

		/// <summary>
		/// Gets or sets the name of the view to render in case of model validation failed.
		/// </summary>
		/// <value>
		/// The name of the view.
		/// </value>
		/// <remarks>
		/// This parameter is only taken into account when {Redirect} is false.
		/// </remarks>
		public string ViewName { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether binding model from ViewData should be attempted, 
		/// so 'Model' property is available during view rendering.
		/// </summary>
		/// <value>
		///   <c>true</c> if [bind model for view]; otherwise, <c>false</c>.
		/// </value>
		/// <remarks>
		/// This parameter is only taken into account when {Redirect} is false.
		/// </remarks>
		public bool BindModelForView { get; set; }

		public ValidateModelStateAttribute()
		{
			Redirect = true;
		}

		// FIXME: This is somewhat smelly, so I may end up deleting it if it's usefullness is really not justified. (pruiz)
		private void TryBindModel(ActionExecutingContext context)
		{
			if (context.Controller.ViewData != null &&
				context.ActionParameters.Count == 1
				&& context.Controller.ViewData.Model == null)
			{
				var modelParam = context.ActionDescriptor.GetParameters().First();
				var binder = ModelBinders.Binders.GetBinder(modelParam.ParameterType);
				var bindctx = new ModelBindingContext()
				{
					FallbackToEmptyPrefix = (modelParam.BindingInfo.Prefix == null),
					ModelName = modelParam.ParameterName,
					ModelState = context.Controller.ViewData.ModelState,
					ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, modelParam.ParameterType),
					ValueProvider = context.Controller.ValueProvider
				};
				var model = binder.BindModel(context, bindctx);
				context.Controller.ViewData.Model = model;
			}
		}

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			if (!context.Controller.ViewData.ModelState.IsValid)
			{
				if (Redirect)
				{
					// Export ModelState to TempData so it's available on next request
					PreserveModelState(context);

					// redirect back to GET action
					context.Result = new RedirectToRouteResult(context.RouteData.Values);
				}
				else
				{
					if (BindModelForView)
						TryBindModel(context);

					context.Result = new ViewResult()
					{
						ViewName = ViewName,
						ViewData = context.Controller.ViewData,
					};
				}
			}

			base.OnActionExecuting(context);
		}

	}
}