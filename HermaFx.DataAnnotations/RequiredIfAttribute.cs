#region LICENSE
// Source Code licensed under MS-PL.
// Derived from: MVC Foolproof Validation (http://foolproof.codeplex.com/)
# endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HermaFx.DataAnnotations
{
	public class RequiredIfAttribute : ContingentValidationAttribute
	{
		public Operator Operator { get; private set; }
		public object DependentValue { get; private set; }
		/// <summary>
		/// Gets or sets a flag indicating whether the attribute should allow empty strings.
		/// </summary>
		public bool AllowEmptyStrings { get; set; }
		protected OperatorMetadata Metadata { get; private set; }

		public RequiredIfAttribute(string dependentProperty, Operator @operator, object dependentValue)
			: base(dependentProperty)
		{
			Operator = @operator;
			DependentValue = dependentValue;
			Metadata = OperatorMetadata.Get(Operator);
		}

		public RequiredIfAttribute(string dependentProperty, object dependentValue)
			: this(dependentProperty, Operator.EqualTo, dependentValue) { }

		public override string FormatErrorMessage(string name)
		{
			if (string.IsNullOrEmpty(ErrorMessageResourceName) && string.IsNullOrEmpty(ErrorMessage))
				ErrorMessage = DefaultErrorMessage;

			return string.Format(ErrorMessageString, name, DependentProperty, DependentValue);
		}

		public override string ClientTypeName
		{
			get { return "RequiredIf"; }
		}

		protected override IEnumerable<KeyValuePair<string, object>> GetClientValidationParameters()
		{
			return base.GetClientValidationParameters()
				.Union(new[] {
					new KeyValuePair<string, object>("Operator", Operator.ToString()),
					new KeyValuePair<string, object>("DependentValue", DependentValue)
				});
		}

		public override bool IsValid(object value, object dependentValue, object container)
		{
			if (Metadata.IsValid(dependentValue, DependentValue))
			{
				if (value == null) return false;
				if (value is string) return AllowEmptyStrings ? true : (value as string).Length > 0;
			}

			return true;
		}

		public override string DefaultErrorMessage
		{
			get { return "{0} is required due to {1} being " + Metadata.ErrorMessage + " {2}"; }
		}
	}
}
