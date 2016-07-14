using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace HermaFx.Mvc.Html
{
	public static class CssHelpers
	{
		private const string STYLE_BEGIN_TAG = "<style>\n";
		private const string STYLE_END_TAG = "</style>\n";
		private static readonly DateTime _ts = DateTime.UtcNow;

		private static HtmlString CreateStyleTag(this HtmlHelper helper, string src, string mediaType = null)
		{
			TagBuilder linkTag = new TagBuilder("link");
			linkTag.MergeAttribute("type", "text/css");
			linkTag.MergeAttribute("rel", "stylesheet");
			if (mediaType != null)
			{
				linkTag.MergeAttribute("media", mediaType);
			}
			linkTag.MergeAttribute("href", UrlHelper.GenerateContentUrl(src, helper.ViewContext.HttpContext));
			return MvcHtmlString.Create(linkTag.ToString(TagRenderMode.SelfClosing));
		}

		public static HtmlString Style(this HtmlHelper html, string filename, bool versioned = false)
		{
			filename = html.ResolveAppRelativePath(filename);

			return versioned ?
				html.CreateStyleTag(string.Format("{0}?{1}", filename, _ts.ToString("s")))
				 : html.CreateStyleTag(filename);
		}

		public static HtmlString StyleBlock(this HtmlHelper html, string cssFile)
		{
			var sb = new StringBuilder();
			var path = html.ResolveServerSidePath(cssFile);
			var file = File.ReadAllText(path);

			sb.Append(STYLE_BEGIN_TAG);
			sb.Append(file);
			sb.Append(STYLE_END_TAG);

			return new HtmlString(sb.ToString());
		}

		#region RegisterStyle
		private static readonly string REGISTERED_STYLES_KEY = typeof(CssHelpers).FullName + ":RegisteredStyles";

		private static ICollection<string> GetRegisteredStylesCollection(this HtmlHelper html)
		{
			var items = html.ViewContext.HttpContext.Items;

			if (!items.Contains(REGISTERED_STYLES_KEY))
			{
				items[REGISTERED_STYLES_KEY] = new HashSet<string>();
			}

			return items[REGISTERED_STYLES_KEY] as ICollection<string>;
		}

		public static void RegisterStyle(this HtmlHelper html, string file)
		{
			var collection = html.GetRegisteredStylesCollection();
			var entry = html.ResolveAppRelativePath(file);
			if (!collection.Contains(entry)) collection.Add(entry);
		}

		public static HtmlString RenderRegisteredStyles(this HtmlHelper html, bool versioned = false)
		{
			var sb = new StringBuilder();

			foreach (var file in html.GetRegisteredStylesCollection())
			{
				sb.AppendLine(html.Style(file, versioned: versioned).ToString());
			}

			return MvcHtmlString.Create(sb.ToString());
		}

		#endregion

	}
}