#region LICENSE
// Source Code licensed under MS-PL.
// Derived from: MVC Foolproof Validation (http://foolproof.codeplex.com/)  
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HermaFx.DataAnnotations
{
	public class RequiredIfNotEmptyAttribute : ContingentValidationAttribute
	{
		public RequiredIfNotEmptyAttribute(string dependentProperty)
			: base(dependentProperty) { }

		public override bool IsValid(object value, object dependentValue, object container)
		{
			if (!string.IsNullOrEmpty((dependentValue ?? string.Empty).ToString().Trim()))
				return value != null && !string.IsNullOrEmpty(value.ToString().Trim());

			return true;
		}

		public override string DefaultErrorMessage
		{
			get { return "{0} is required due to {1} not being empty."; }
		}
	}
}
