using System;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HermaFx.DataAnnotations
{
	public class MaxElementsIfAttribute : ContingentValidationAttribute
	{
		public Operator Operator { get; private set; }
		public object DependentValue { get; private set; }
		private MaxElementsAttribute MaxElements { get; set; }
		/// <summary>
		/// Gets or sets a flag indicating whether the attribute should allow empty strings.
		/// </summary>
		public bool AllowEmptyStrings { get; set; }
		protected OperatorMetadata Metadata { get; private set; }

		public MaxElementsIfAttribute(uint elements, string dependentProperty, Operator @operator, object dependentValue)
			: base(dependentProperty)
		{
			Operator = @operator;
			DependentValue = dependentValue;
			Metadata = OperatorMetadata.Get(Operator);
			MaxElements = new MaxElementsAttribute(elements);
		}

		public MaxElementsIfAttribute(uint elements, string dependentProperty, object dependentValue)
			: this(elements, dependentProperty, Operator.EqualTo, dependentValue) { }

		public override string FormatErrorMessage(string name)
		{
			if (string.IsNullOrEmpty(ErrorMessageResourceName) && string.IsNullOrEmpty(ErrorMessage))
				ErrorMessage = DefaultErrorMessage;

			return MaxElements.ErrorMessage;
		}

		public override string ClientTypeName
		{
			get { return "MaxElementsIf"; }
		}

		protected override IEnumerable<KeyValuePair<string, object>> GetClientValidationParameters()
		{
			return base.GetClientValidationParameters()
				.Union(new[] {
					new KeyValuePair<string, object>("Operator", Operator.ToString()),
					new KeyValuePair<string, object>("DependentValue", DependentValue),
					new KeyValuePair<string, object>("Elements", MaxElements.Count),
				});
		}

		public override bool IsValid(object value, object dependentValue, object container)
		{
			if (Metadata.IsValid(dependentValue, DependentValue))
				return MaxElements.IsValid(value);

			return true;
		}

		public override string DefaultErrorMessage
		{
			get { return MaxElements.ErrorMessage; }
		}
	}
}
