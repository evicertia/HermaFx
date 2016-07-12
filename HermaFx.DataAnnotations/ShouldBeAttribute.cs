using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HermaFx.DataAnnotations
{
	/// <summary>
	/// Attribute to ensure a string contains the provided value
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	sealed public class ShouldBeAttribute : ValidationAttribute
	{
		private const string _defaultErrorMessage = "Value should match {0}";
		readonly object _expected;

		public ShouldBeAttribute(object expected)
			: base(string.Format(_defaultErrorMessage, expected))
		{
			_expected = expected;
		}

		public override bool IsValid(object value)
		{
			// only the [Required] attribute means value can't be null
			if (value == null) return true;

			// TODO: Try to convert-to/convert-from if value 
			//		 and _expected types doesn't match.
			return Object.Equals(value, _expected);
		}
	}
}
