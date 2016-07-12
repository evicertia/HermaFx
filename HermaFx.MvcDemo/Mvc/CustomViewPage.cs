using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using FluentBootstrap;
using FluentBootstrap.Mvc;

namespace HermaFx.MvcDemo
{
	public abstract class CustomViewPage : WebViewPage
	{
		public MvcBootstrapHelper<object> Bootstrap { get; private set; }
		public ISettings Settings { get; set; }

		// TODO: At some point we should try to inject this during view creation, by means of ViewEngineFactory.
		//		 This is not really an issue for ISettings, as this is a singleton, not requiring disposal. (pruiz)
		public override void InitHelpers()
		{
			base.InitHelpers();

			Bootstrap = Html.Bootstrap();
			Settings = this.GetSettings();
		}
	}

	public abstract class CustomViewPage<TModel> : WebViewPage<TModel>
	{
		public MvcBootstrapHelper<TModel> Bootstrap { get; private set; }
		public ISettings Settings { get; set; }

		// TODO: At some point we should try to inject this during view creation, by means of ViewEngineFactory.
		//		 This is not really an issue for ISettings, as this is a singleton, not requiring disposal. (pruiz)
		public override void InitHelpers()
		{
			base.InitHelpers();

			Bootstrap = Html.Bootstrap();
			Settings = this.GetSettings();
		}
	}

	public static class ViewPageExtensions
	{
		public static ISettings GetSettings(this WebViewPage view)
		{
			var veritasController = (view.Html.ViewContext?.Controller as CustomController);
			if (veritasController != null && veritasController.Settings == null)
			{
				throw new ApplicationException("No ISettings available from within this view's controller instance.");
			}
			return veritasController?.Settings;
		}
	}
}
