using System;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HermaFx.DataAnnotations
{
	public class MinElementsIfAttribute : RequiredIfAttribute
	{
		private const string _defaultErrorMessage = "There should be at least {0} elements.";

		public uint Elements { get; set; }

		public MinElementsIfAttribute(uint elements, string dependentProperty, Operator @operator, object dependentValue)
			: base(dependentProperty, @operator, dependentValue)
		{
			Elements = elements;
		}

		public MinElementsIfAttribute(uint elements, string dependentProperty, object dependentValue)
			: this(elements, dependentProperty, Operator.EqualTo, dependentValue) { }

		public override bool IsValid(object value, object dependentValue, object container)
		{
			if (Metadata.IsValid(dependentValue, DependentValue))
			{
				if (value == null) return false;
				if (value is Array) return (value as Array).Length >= Elements;
				if (value is ICollection) return (value as ICollection).Count >= Elements;
				if (value is IEnumerable) return (value as IEnumerable).OfType<object>().Count() >= Elements;
			}

			return true;
		}

		protected override IEnumerable<KeyValuePair<string, object>> GetClientValidationParameters()
		{
			return base.GetClientValidationParameters()
				.Union(new[] {
					new KeyValuePair<string, object>("Elements", Elements),
				});
		}

		public override string FormatErrorMessage(string name)
		{
			if (string.IsNullOrEmpty(ErrorMessageResourceName) && string.IsNullOrEmpty(ErrorMessage))
				ErrorMessage = DefaultErrorMessage;

			return string.Format(ErrorMessageString, name, DependentProperty, DependentValue, Elements);
		}

		public override string DefaultErrorMessage
		{
			get { return String.Format("There should be at least {0} elements.", Elements); }
		}
	}
}
