using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace HermaFx.Mvc.Html
{
	internal static class PathHelper
	{
#if false // XXX: Not sure wether supporting url-schemes is a good idea. (pruiz)
		private static bool IsRelativeToDefaultPath(string file)
		{
			return !(file.StartsWith("~", StringComparison.Ordinal) ||
					 file.StartsWith("/", StringComparison.Ordinal) ||
					 file.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
					 file.StartsWith("https://", StringComparison.OrdinalIgnoreCase));
		}
#endif

		public static string ResolveServerSidePath(this HtmlHelper html, string filename)
		{
			Guard.IsNotNullNorWhitespace(filename, nameof(filename));

			var server = html.ViewContext.HttpContext.Server;

			if (filename.StartsWith("~"))
				return server.MapPath(filename);

			var view = html.ViewContext.View;
			if (view is BuildManagerCompiledView)
			{
				var viewPath = ((BuildManagerCompiledView)view).ViewPath;
				var basePath = VirtualPathUtility.GetDirectory(viewPath);
				return server.MapPath(VirtualPathUtility.Combine(basePath, filename));
			}
			else if (html.ViewDataContainer is WebPageBase)
			{
				var viewPath = (html.ViewDataContainer as WebPageBase).VirtualPath;
				var basePath = VirtualPathUtility.GetDirectory(viewPath);
				return server.MapPath(VirtualPathUtility.Combine(basePath, filename));
			}

			// XXX: Should we throw here instead?? (pruiz)
			return filename;
		}

		public static string ResolveAppRelativePath(this HtmlHelper html, string filename)
		{
			Guard.IsNotNullNorWhitespace(filename, nameof(filename));

			var server = html.ViewContext.HttpContext.Server;

			if (VirtualPathUtility.IsAppRelative(filename)) return filename;

			var view = html.ViewContext.View;
			if (view is BuildManagerCompiledView)
			{
				var viewPath = ((BuildManagerCompiledView)view).ViewPath;
				var basePath = VirtualPathUtility.GetDirectory(viewPath);
				return VirtualPathUtility.Combine(basePath, filename);
			}
			else if (html.ViewDataContainer is WebPageBase)
			{
				var viewPath = (html.ViewDataContainer as WebPageBase).VirtualPath;
				var basePath = VirtualPathUtility.GetDirectory(viewPath);
				return VirtualPathUtility.Combine(basePath, filename);
			}

			// XXX: Should we throw here instead?? (pruiz)
			return filename;
		}
	}
}