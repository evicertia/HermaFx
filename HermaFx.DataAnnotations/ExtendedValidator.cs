using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HermaFx.DataAnnotations
{
	public static class ExtendedValidator
	{
		/// <summary>
		/// Determines whether the specified object is valid and return an list of ValidationResults.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <returns></returns>
		public static IEnumerable<ValidationResult> Validate(object obj)
		{
			var context = new ValidationContext(obj, serviceProvider: null, items: null);
			var results = new List<ValidationResult>();
			Validator.TryValidateObject(obj, context, results, true);

			return AggregateValidationResult.Flatten(results);
		}

		/// <summary>
		/// Determines whether the specified object has exception.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <returns></returns>
		public static bool IsValid(object obj)
		{
			return !Validate(obj).Any();
		}

		/// <summary>
		/// Ensures the object is valid.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <exception cref="AggregateValidationException">Validation failed</exception>
		public static void EnsureIsValid(object obj)
		{
			var results = Validate(obj);

			if (results.Any() || results.First() != ValidationResult.Success)
			{
				var type = (obj ?? new object()).GetType();
				throw AggregateValidationException.CreateFor(type, results);
			}
		}
	}
}