using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

using FluentBootstrap.Buttons;
using FluentBootstrap.Forms;
using FluentBootstrap.Mvc;
using FluentBootstrap.Mvc.Internals;


namespace FluentBootstrap
{
	public static class ButtonExtensions
	{
#if false
		public static ComponentBuilder<TConfig, Button> ButtonFor<TModel, TType>(this MvcBootstrapHelper<TModel> bootstrap, Expression<Func<TModel, TType>> expression,
					   TType value, Action<ButtonSettings> customizer = null, object htmlAttributes = null)
		{
			return null;
		}

		public static ComponentBuilder<TConfig, Button> For<TConfig, TModel, TType>(
			this ComponentBuilder<TConfig, Button> builder, 
			Expression<Func<TModel, TType>> expression, TType value) 
			where TConfig : BootstrapConfig
		{
			var html = builder.GetHelper()
			var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);

			return builder.
		}

		private static ComponentBuilder<MvcBootstrapConfig<TModel>, ControlLabel> GetControlLabel<TComponent, TModel, TValue>(
			BootstrapHelper<MvcBootstrapConfig<TModel>, TComponent> helper, Expression<Func<TModel, TValue>> expression)
			where TComponent : Component
		{
			string expressionText = ExpressionHelper.GetExpressionText(expression);
			ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, helper.GetConfig().HtmlHelper.ViewData);
			string name = GetControlName(helper, expressionText);
			string label = GetControlLabel(metadata, expressionText);
			return new MvcBootstrapHelper<TModel>(helper.GetConfig().HtmlHelper).ControlLabel(label).For(TagBuilder.CreateSanitizedId(name));
		}

		public static ComponentBuilder<MvcBootstrapConfig<TModel>, FormButton> ButtonFor<TComponent, TModel, TValue>(
			this BootstrapHelper<MvcBootstrapConfig<TModel>, TComponent> helper, 
			Expression<Func<TModel, TValue>> expression, params string[] options) 
			where TComponent : Component, ICanCreate<FormButton>
		{
			var metadata = ModelMetadata.FromLambdaExpression(expression, helper.GetConfig().HtmlHelper.ViewData);
			string expressionText = ExpressionHelper.GetExpressionText(expression);
			var name = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(expressionText);
			string name = GetControlName(helper, expressionText);
			string label = GetControlLabel(metadata, expressionText);
			ComponentBuilder<MvcBootstrapConfig<TModel>, Select> builder = helper.Select(name, label);
			if (metadata.Model != null && !string.IsNullOrEmpty(name))
			{
				// Add the model value before adding options so they'll get selected on a match
				builder.GetComponent().ModelValue = metadata.Model.ToString();
			}
			return builder.AddOptions(options);
		}

		public static ComponentBuilder<TConfig, FormButton> Submit<TConfig, TComponent>(
			this BootstrapHelper<TConfig, TComponent> helper, 
			string text = "Submit", string label = null, object value = null)
			where TConfig : BootstrapConfig
			where TComponent : Component, ICanCreate<FormButton>
		{
			return new ComponentBuilder<TConfig, FormButton>(helper.Config, new FormButton(helper, ButtonType.Submit))
				.SetText(text)
				.SetControlLabel(label)
				.SetValue(value)
				.SetState(ButtonState.Primary);
		}

#endif

		private static string GetControlName(HtmlHelper helper, string expressionText)
		{
			return helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(expressionText);
		}

		private static string GetControlLabel(ModelMetadata metadata, string expressionText)
		{
			string label = metadata.DisplayName;
			if (label == null)
			{
				label = metadata.PropertyName;
				if (label == null)
				{
					char[] chrArray = new char[] { '.' };
					label = expressionText.Split(chrArray).Last<string>();
				}
			}
			return label;
		}

		public static ComponentBuilder<MvcBootstrapConfig<TModel>, TTag> For<TTag, TModel, TValue>(
			this ComponentBuilder<MvcBootstrapConfig<TModel>, TTag> builder, Expression<Func<TModel, TValue>> expression, TValue value)
			where TTag : FormButton
		{
			var config = (MvcBootstrapConfig<TModel>)builder.GetConfig();
			var html = config.GetHtmlHelper();
			var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
			string expressionText = ExpressionHelper.GetExpressionText(expression);
			string name = GetControlName(html, expressionText);
			string label = GetControlLabel(metadata, expressionText);

			return builder
				.SetName(name)
				.SetControlLabel(label)
				.SetValue(value);
		}

		public static ComponentBuilder<MvcBootstrapConfig<TModel>, FormButton> ButtonFor<TComponent, TModel, TValue>(
			this BootstrapHelper<MvcBootstrapConfig<TModel>, TComponent> bootstrap, Expression<Func<TModel, TValue>> expression, string text, TValue value)
			where TComponent : Component, ICanCreate<FormButton>
		{
			var config = (MvcBootstrapConfig<TModel>)bootstrap.GetConfig();
			var html = config.GetHtmlHelper();
			var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
			var expressionText = ExpressionHelper.GetExpressionText(expression);
			var name = GetControlName(html, expressionText);
			var label = GetControlLabel(metadata, expressionText);

			return bootstrap.FormButton(text: text, label: label, value: value);
		}
	}
}