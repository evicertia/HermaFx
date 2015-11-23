using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace HermaFx.DataAnnotations
{
	[AttributeUsage(AttributeTargets.Property)]
	public abstract class ContingentValidationAttribute : ModelAwareValidationAttribute
	{
		#region Properties
		public override bool RequiresValidationContext
		{
			get
			{
				return base.RequiresValidationContext;
			}
		}

		public string DependentProperty { get; private set; }
		#endregion

#if false // TODO: Bring client side stuff from foolprof. (pruiz)
		static ContingentValidationAttribute()
		{
			ClientSideRegistry.RegisterAll();
		}
#endif

		public ContingentValidationAttribute(string dependentProperty)
		{
			DependentProperty = dependentProperty;
		}

		public override sealed bool IsValid(object value)
		{
			return base.IsValid(value);
		}

		protected override sealed ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if (validationContext == null) throw new ArgumentNullException("validationContext");

			if (IsValid(value, validationContext.ObjectInstance))
				return ValidationResult.Success;

			string[] memberNames = validationContext.MemberName != null ? new string[] { validationContext.MemberName } : null;
			return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName), memberNames);
		}

		public override string FormatErrorMessage(string name)
		{
			if (string.IsNullOrEmpty(ErrorMessageResourceName) && string.IsNullOrEmpty(ErrorMessage))
				ErrorMessage = DefaultErrorMessage;

			return string.Format(ErrorMessageString, name, DependentProperty);
		}

		public override string DefaultErrorMessage
		{
			get { return "{0} is invalide due to {1}."; }
		}

		private object GetDependentPropertyValue(object container)
		{
			var currentType = container.GetType();
			var value = container;

			foreach (string propertyName in DependentProperty.Split('.'))
			{
				var property = currentType.GetProperty(propertyName);
				value = property.GetValue(value, null);
				currentType = property.PropertyType;
			}

			return value;
		}

		protected override IEnumerable<KeyValuePair<string, object>> GetClientValidationParameters()
		{
			return base.GetClientValidationParameters()
				.Union(new[] { new KeyValuePair<string, object>("DependentProperty", DependentProperty) });
		}

		public bool IsValid(object value, object container)
		{
			return IsValid(value, GetDependentPropertyValue(container), container);
		}

		public abstract bool IsValid(object value, object dependentValue, object container);
	}
}