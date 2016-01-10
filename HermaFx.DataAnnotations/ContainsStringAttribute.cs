using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HermaFx.DataAnnotations
{
	/// <summary>
	/// Attribute to ensure a string contains the provided value
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	sealed public class ContainsStringAttribute : ValidationAttribute
	{
		readonly IEnumerable<String> _literals;

		public ContainsStringAttribute(params string[] literals)
		{
			_literals = literals;
		}

		public override bool IsValid(object value)
		{
			// only the [Required] attribute means value can't be null
			if (value == null) return true;

			var toValidate = value.ToString();
			foreach (var literal in _literals)
			{
				if (!toValidate.Contains(literal))
				{
					return false;
				}
			}
			return true;
		}
	}
}
