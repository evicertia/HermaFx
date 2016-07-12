using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

//using MediatR;

namespace HermaFx.MvcDemo
{
	/// <summary>
	/// Contracts controller with Veritas-specific methods and properties.
	/// </summary>
	public abstract partial class CustomController : Controller
	{
		//public IMediator Mediator { get; set; }
		public ISettings Settings { get; set; }

		#region AddMessage methods..
		public void AddSuccessMessage(string message, params object[] args)
		{
			AlertsBarExtensions.AddSuccessMessage(this, message, args);
		}

		public void AddInfoMessage(string message, params object[] args)
		{
			AlertsBarExtensions.AddInfoMessage(this, message, args);
		}
		public void AddWarningMessage(string message, params object[] args)
		{
			AlertsBarExtensions.AddWarningMessage(this, message, args);
		}

		public void AddErrorMessage(string message, params object[] args)
		{
			AlertsBarExtensions.AddErrorMessage(this, message, args);
		}
		#endregion
	}
}