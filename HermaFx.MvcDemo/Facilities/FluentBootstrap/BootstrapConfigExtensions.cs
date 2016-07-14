using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;

using HermaFx;
using HermaFx.Reflection;

namespace FluentBootstrap.Mvc
{
	public static class BootstrapConfigExtensions
	{
		private static readonly PropertyInfo _HtmlHelperAccessor = typeof(MvcBootstrapConfig<>).GetProperty("HtmlHelper", BindingFlags.Instance | BindingFlags.NonPublic);

		/*public static HtmlHelper<TModel> GetHtmlHelper<TModel>(this MvcBootstrapConfig<TModel> config)
		{	
			return (HtmlHelper<TModel>)_HtmlHelperAccessor.GetValue(config);
		}*/
	}
}