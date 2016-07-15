using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using Microsoft.Web.Mvc;

using HermaFx;
using HermaFx.Mvc;
using HermaFx.Utils;
using HermaFx.DataAnnotations;

namespace HermaFx.MvcDemo.Features
{
	[SessionState(SessionStateBehavior.Disabled)]
	public partial class TestController : CustomController
	{
		private static readonly global::Common.Logging.ILog _Log = global::Common.Logging.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		//private IUploadManager _manager;

		#region ControlTest

		[HttpGet, RestoreModelState]
		[ActionSessionState(SessionStateBehavior.Default)]
		public virtual ViewResult Index()
		{
			return View(TestStubs.PopulateControlData());
		}

		[HttpPost]
		[ValidateModelState, ValidateAntiForgeryToken]
		[ActionSessionState(SessionStateBehavior.Default)]
		public virtual RedirectToRouteResult Index(TestIndex model)
		{
			switch (model.ActionSubmit)
			{
				case TestIndex._Action.Submit:
					AddSuccessMessage("Submmit success");
					return RedirectToAction(MVC.Home.Index());
				default: throw new WebSiteException("Unknown action?!");
			}
		}
		#endregion
	}

}