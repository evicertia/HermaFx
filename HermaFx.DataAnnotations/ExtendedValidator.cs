using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HermaFx.DataAnnotations
{
	public interface IValidatable
	{
		List<ValidationResult> Validate();
		bool IsValid();
		void EnsureIsValid();
	}

	public static class ExtendedValidator
	{
		/// <summary>
		/// Determines whether the specified object is valid and return an list of ValidationResults.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <returns></returns>
		public static List<ValidationResult> Validate(object obj)
		{
			var context = new ValidationContext(obj, serviceProvider: null, items: null);
			var results = new List<ValidationResult>();
			Validator.TryValidateObject(obj, context, results, true);
			return results;
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
			var exceptions = Validate(obj).Select(x => new ValidationException(x.ErrorMessage));

			if (exceptions != null && exceptions.Any())
			{
				// FIXME: We should be passing more detailes here (like failed class type/name).
				throw new AggregateValidationException("Validation failed", exceptions);
			}
		}
	}
}