using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace HermaFx.DataAnnotations
{
	public class ValidateObjectAttribute : ValidationAttribute
	{
		private static bool IsComplexType(Type type)
		{
			if (type == typeof(string))
				return false;

			// Complex object test borrowed from System.Web.Mvc.ModelMetaData.IsComplexType
			if (TypeDescriptor.GetConverter(type).CanConvertFrom(typeof(string)))
				return false;

			return true;
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			//_Log.DebugFormat("Trying to validate {0}", value.ToString());
			if (value == null)
			{
				return ValidationResult.Success;
			}

			var results = new List<ValidationResult>();

			if (value is IEnumerable)
			{
				var valueAsEnumerable = value as IEnumerable;
				foreach (var item in valueAsEnumerable)
				{
					var context = new ValidationContext(item, null, null);
					Validator.TryValidateObject(item, context, results, true);
				}
			}
			else
			{
				var context = new ValidationContext(value, null, null);
				Validator.TryValidateObject(value, context, results, true);
			}

			if (results.Count != 0)
			{
				//_Log.DebugFormat("Validation failed for: {0} | {1} | {2}", validationContext.ObjectType, validationContext.DisplayName, validationContext.MemberName);

				return AggregateValidationResult.CreateFor(validationContext, results);
			}

			return ValidationResult.Success;
		}
	}
}
