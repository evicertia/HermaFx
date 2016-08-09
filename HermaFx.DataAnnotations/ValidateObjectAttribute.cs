using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using HermaFx;
using HermaFx.Reflection;

namespace HermaFx.DataAnnotations
{
	public class ValidateObjectAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext context)
		{
			var results = ExtendedValidator.ValidateRecursing(value, context);

			if (results.Count() != 0)
			{
				//_Log.DebugFormat("Validation failed for: {0} | {1} | {2}", validationContext.ObjectType, validationContext.DisplayName, validationContext.MemberName);

				return AggregateValidationResult.CreateFor(context, results);
			}

			return ValidationResult.Success;
		}
	}
}
