using System;
using System.Text;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

using HermaFx;

namespace HermaFx.MvcDemo
{
	/// <summary>
	/// Allows delivering status information to users thru an status bar similiar to 
	/// the one provided by ValidationSummary.
	/// </summary>
	/// <remarks>
	/// Partially inspired by http://completedevelopment.blogspot.com/2010/12/status-message-handling-in-mvc.html
	/// </remarks>
	public static class AlertsBarExtensions
	{
		#region RenderData
		private class BarData
		{
			public string Kind;
			public string CssClassName;
			public Func<string> MessageDelegate;

			public string Message { get { return MessageDelegate(); } }
		}
		
		private static BarData Success = 
			new BarData() { Kind = "success",	CssClassName = "alert-success",	MessageDelegate = () => "Operación realizada" };
		private static BarData Info = 
			new BarData() { Kind = "info",		CssClassName = "alert-info",		MessageDelegate = () => "Información" };
		private static BarData Warning =
			new BarData() { Kind = "warning",	CssClassName = "alert-warning",	MessageDelegate = () => "¡Atención!" };
		private static BarData Error =
			new BarData() { Kind = "error",		CssClassName = "alert-danger",	MessageDelegate = () => "Error de validación" };

		// NOTE: Order matters (a lot), as we do priorize Errors over Alerts, etc. (pruiz)
		private static IEnumerable<BarData> Alerts = new[] { Error, Warning, Info, Success };
		#endregion

		#region Private methods..
		private static string GetTempDataKeyFor(BarData bar)
		{
			return typeof(AlertsBarExtensions).FullName + "_" + bar.Kind;
		}
		private static IList<string> GetDataFor(ViewContext context, BarData bar)
		{
			var key = GetTempDataKeyFor(bar);
			var bag = context.TempData;
			var data = bag[key] as IList<string>;
			// Ensure status is cleaned once displayed
			bag.Remove(key);

			return data ?? new List<string>();
		}

		private static string _GetUserErrorMessageOrDefault(ModelError error, ModelState modelState)
		{
			if (!string.IsNullOrEmpty(error.ErrorMessage))
			{
				return error.ErrorMessage;
			}
			if (modelState == null)
			{
				return null;
			}
			string str = (modelState.Value != null) ? modelState.Value.AttemptedValue : null;
			return string.Format(CultureInfo.CurrentCulture, "The value '{0}' is invalid.", new object[] { str });
		}

		private static IEnumerable<string> _ParseModelState(ModelStateDictionary modelstate)
		{
			var data = new List<string>();
			var values = modelstate.Values;

			if (values == null) return null;

			return values.SelectMany(x => x.Errors)
				.Select(x => _GetUserErrorMessageOrDefault(x, null))
				.Where(x => !string.IsNullOrEmpty(x))
				.ToArray();
		}

		private static MvcHtmlString _RenderStatusBar(this HtmlHelper @this, BarData bar, string message, bool includePropertyErrors, object htmlAttributes)
		{
			bool hidden = true;
			IList<string> data = new List<string>();

			if (@this == null) throw new ArgumentNullException("HtmlHelper");
			if (@this.ViewContext == null) throw new ArgumentNullException("ViewContext");

			var div = new TagBuilder("div");
			div.AddCssClass("alert");

			var divInner = new StringBuilder();
			divInner.Append(@"<a class='close' href='#' data-dismiss='alert'>×</a>");

			if (htmlAttributes != null)
				div.MergeAttributes(new RouteValueDictionary(htmlAttributes));

			if (bar != null)
			{
				// Get data from tempData.
				data = GetDataFor(@this.ViewContext, bar);
				// Set alert's type to div container.
				div.AddCssClass(bar.CssClassName);
			}

			if (!string.IsNullOrEmpty(message))
			{
				hidden = false;
				divInner.AppendFormat("<span>{0}</span>", message);
			}

			if (string.IsNullOrWhiteSpace(message) && data.Count() == 1)
			{
				hidden = false;
				divInner.AppendFormat("<span>{0}</span>", data[0]);
			}
			else if (data != null && data.Count() > 0 && includePropertyErrors)
			{
				hidden = false;
				var ul = new TagBuilder("ul");
				var ulInner = new StringBuilder();

				foreach (string item in data)
				{
					var li = new TagBuilder("li") { InnerHtml = item };
					ulInner.Append(li.ToString(TagRenderMode.Normal));
				}
				ul.InnerHtml = ulInner.ToString();
				divInner.Append(ul.ToString(TagRenderMode.Normal));
			}

			div.InnerHtml = divInner.ToString();
			if (hidden) div.Attributes["style"] = "display:none";
			return MvcHtmlString.Create(div.ToString(TagRenderMode.Normal));
		}

		private static void _AddMessages(TempDataDictionary tempData, BarData bar, IEnumerable<string> messages)
		{
			var key = GetTempDataKeyFor(bar);

			if (tempData == null) throw new ArgumentNullException("tempData");

			if (!tempData.ContainsKey(key))
			{
				tempData[key] = new List<string>();
			}

			var statusData = tempData[key] as IList<string>;
			messages.ForEach(x => statusData.Add(x));
			tempData.Keep(key); // Ensure list will be kept around..
		}

		private static void _AddMessage(TempDataDictionary tempData, BarData bar, string message, params object[] args)
		{
			_AddMessages(tempData, bar, new[] { args.Length > 0 ? string.Format(message, args) : message });
		}
		#endregion

		#region AlertSummary
		/// <summary>
		/// Status (Info+Success+Alert or Errors if any) summary
		/// </summary>
		/// <param name="this"></param>
		/// <param name="validationMessage"></param>
		/// <param name="includePropertyErrors"></param>
		/// <param name="htmlAttributes"></param>
		/// <returns></returns>
		public static MvcHtmlString AlertSummary(this HtmlHelper @this, string validationMessage = null, 
			bool includePropertyErrors = true, object htmlAttributes = null)
		{
			var bag = @this.ViewContext.TempData;

			if (!@this.ViewData.ModelState.IsValid)
			{
				var data = _ParseModelState(@this.ViewData.ModelState);
				_AddMessages(@this.ViewContext.TempData, Error, data);
			}

			// From highest priority level (error) to lower, guess which one should be shown. (pruiz)
			foreach (var sb in Alerts)
			{
				// If this bar has no messages, move on to the next.
				if (!(@this.ViewContext.TempData[GetTempDataKeyFor(sb)] as IList<string>).IfNotNull(x => x.Any()))
				{
					continue;
				}

				return _RenderStatusBar(@this, sb, validationMessage, includePropertyErrors, htmlAttributes);
			}

			// Empty div to be used by jQuery/Ajax
			return _RenderStatusBar(@this, null, null, false, null);
		}

		#endregion

		#region AddMessage methods..
		public static void AddSuccessMessage(this ControllerBase @this, string message, params object[] args)
		{
			_AddMessage(@this.TempData, Success, message, args);
		}

		public static void AddInfoMessage(this ControllerBase @this, string message, params object[] args)
		{
			_AddMessage(@this.TempData, Info, message, args);
		}
		public static void AddWarningMessage(this ControllerBase @this, string message, params object[] args)
		{
			_AddMessage(@this.TempData, Warning, message, args);
		}

		public static void AddErrorMessage(this ControllerBase @this, string message, params object[] args)
		{
			_AddMessage(@this.TempData, Error, message, args);
		}
		#endregion

	}
}
