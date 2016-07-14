using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace HermaFx.Mvc.Html
{
	/// <summary>
	/// HTML script node extensions.
	/// </summary>
	public static class ScriptExtensions
	{
		private const string SCRIPT_BEGIN_TAG = "<script type=\"text/javascript\">\n/* <![CDATA[ */\n";
		private const string SCRIPT_END_TAG = "/* ]]> */\n</script>\n";
		private static readonly DateTime _ts = DateTime.UtcNow;

		#region ScriptBlock

		public static HtmlString ScriptBlock(this HtmlHelper html, string jsFile)
		{
			var sb = new StringBuilder();
			var path = html.ResolveServerSidePath(jsFile);
			var file = File.ReadAllText(path);

			sb.Append(SCRIPT_BEGIN_TAG);
			sb.Append(file);
			sb.Append(SCRIPT_END_TAG);

			return new HtmlString(sb.ToString());
		}

		public static HtmlString ScriptBlock(this HtmlHelper html, string jsFile, IDictionary<string, object> variables)
		{
			var sb = new StringBuilder();
			var path = html.ResolveServerSidePath(jsFile);
			var file = File.ReadAllText(path);

			sb.Append(SCRIPT_BEGIN_TAG);
			sb.Append(";(function() {");
			if (variables != null)
			{
				var serializer = new JavaScriptSerializer();
				foreach (var v in variables)
					sb.AppendFormat("var {0} = {1};", v.Key, serializer.Serialize(v.Value));
			}
			sb.Append(file);
			sb.Append("})();");
			sb.Append(SCRIPT_END_TAG);

			return new HtmlString(sb.ToString());
		}

		public static HtmlString ScriptBlock(this HtmlHelper html, string jsFile, object variables)
		{
			return html.ScriptBlock(jsFile, ((IDictionary<string, object>)new RouteValueDictionary(variables)));
		}

		#endregion

		#region ScriptVars

		public static HtmlString ScriptVars(this HtmlHelper html, string variable, Func<string> code)
		{
			Guard.IsNotNullNorWhitespace(variable, nameof(variable));
			Guard.IsNotNull(code, nameof(code));

			return new HtmlString(string.Format("{0}var {1} = '{2}';{3}", SCRIPT_BEGIN_TAG, variable, code(), SCRIPT_END_TAG));
		}

		public static HtmlString ScriptVars(this HtmlHelper html, string variable, string value)
		{
			Guard.IsNotNullNorWhitespace(variable, nameof(variable));

			return new HtmlString(string.Format("{0}var {1} = '{2}';{3}", SCRIPT_BEGIN_TAG, variable, value, SCRIPT_END_TAG));
		}

		public static HtmlString ScriptVars(this HtmlHelper html, string variable, object value)
		{
			Guard.IsNotNullNorWhitespace(variable, nameof(variable));
			var jsval = new JavaScriptSerializer().Serialize(value);

			return new HtmlString(string.Format("{0}var {1} = '{2}';{3}", SCRIPT_BEGIN_TAG, variable, jsval, SCRIPT_END_TAG));
		}

		public static HtmlString ScriptVars(this HtmlHelper html, IDictionary<string, string> variables)
		{
			Guard.IsNotNull(variables, "variables");

			var sb = new StringBuilder();
			sb.Append(SCRIPT_BEGIN_TAG);

			foreach (var v in variables)
			{
				var value = v.Value == null ? "" : v.Value.Replace("'", @"\'");
				sb.AppendFormat("var {0} = '{1}';", v.Key, value);
			}

			sb.Append(SCRIPT_END_TAG);

			return new HtmlString(sb.ToString());
		}

		// TODO: Add an overload receiving an object, and exposing it's members as js variables.
		public static HtmlString ScriptVars(this HtmlHelper html, IDictionary<string, object> variables)
		{
			var sb = new StringBuilder();
			var serializer = new JavaScriptSerializer();
			serializer.MaxJsonLength = Int32.MaxValue;

			Guard.IsNotNull(variables, "variables");

			sb.Append(SCRIPT_BEGIN_TAG);

			foreach (var v in variables)
				sb.AppendFormat("var {0} = {1};", v.Key, serializer.Serialize(v.Value));

			sb.Append(SCRIPT_END_TAG);

			return new HtmlString(sb.ToString());
		}

		#endregion

		#region RegisterScript

		private static readonly string REGISTERED_SCRIPTS_KEY = typeof(ScriptExtensions).FullName + ":RegisteredScripts";

		private static MvcHtmlString CreateScriptTag(this HtmlHelper helper, string src)
		{
			TagBuilder scriptTag = new TagBuilder("script");
			scriptTag.MergeAttribute("type", "text/javascript");
			scriptTag.MergeAttribute("src", UrlHelper.GenerateContentUrl(src, helper.ViewContext.HttpContext));
			return MvcHtmlString.Create(scriptTag.ToString(TagRenderMode.Normal));
		}

		private static ICollection<string> GetRegisteredScriptsCollection(this HtmlHelper html)
		{
			var items = html.ViewContext.HttpContext.Items;

			if (!items.Contains(REGISTERED_SCRIPTS_KEY))
			{
				items[REGISTERED_SCRIPTS_KEY] = new HashSet<string>();
			}

			return items[REGISTERED_SCRIPTS_KEY] as ICollection<string>;
		}

		public static void RegisterScript(this HtmlHelper html, string releaseFile, string debugFile = null)
		{
			var collection = html.GetRegisteredScriptsCollection();
			var file = Debugger.IsAttached && !string.IsNullOrEmpty(debugFile) ? debugFile : releaseFile;
			var entry = html.ResolveAppRelativePath(file);

			if (!collection.Contains(entry)) collection.Add(entry);
		}

		public static HtmlString RenderRegisteredScripts(this HtmlHelper html)
		{
			var sb = new StringBuilder();

			foreach (var file in html.GetRegisteredScriptsCollection())
			{
				sb.AppendLine(html.CreateScriptTag(file).ToString());
			}

			return MvcHtmlString.Create(sb.ToString());
		}

		#endregion
	}
}