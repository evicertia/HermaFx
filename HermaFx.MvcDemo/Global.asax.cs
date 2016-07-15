using HermaFx.MvcDemo.App_Start;
using System;
using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Configuration;

using HermaFx.Mvc;
using HermaFx.Settings;

namespace HermaFx.MvcDemo
{
	public class Global : System.Web.HttpApplication
	{

		protected void Application_Start(object sender, EventArgs e)
		{
			var settings = new SettingsAdapter().Create<ISettings>(WebConfigurationManager.AppSettings);
			FilterConfig.Configure(GlobalFilters.Filters);
			RouteConfig.Configure(RouteTable.Routes);
			ControllerBuilder.Current.SetControllerFactory(new Mvc.CustomControllerFactory(settings));
			ViewEngines.Engines.Clear();
			ViewEngines.Engines.Add(new FeatureBasedRazorViewEngine());
		}

		protected void Session_Start(object sender, EventArgs e)
		{

		}

		protected void Application_BeginRequest(object sender, EventArgs e)
		{

		}

		protected void Application_AuthenticateRequest(object sender, EventArgs e)
		{

		}

		protected void Application_Error(object sender, EventArgs e)
		{
			Exception ex = Server.GetLastError();
			if (ex != null)
			{
				Trace.TraceError(ex.ToString());
			}
		}

		protected void Session_End(object sender, EventArgs e)
		{

		}

		protected void Application_End(object sender, EventArgs e)
		{

		}
	}
}