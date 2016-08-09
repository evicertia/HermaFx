using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace HermaFx.DataAnnotations
{
	public static class ExtendedValidator
	{
		#region Non-Public Stuff
		private static readonly string ITEMS_KEY = typeof(ExtendedValidator).Name + "::Seen";
		private static readonly string VALIDATE_ALL_PROPERTIES_KEY = typeof(ExtendedValidator).Name + "::ValidateAllProperties";
		private static readonly IEnumerable<ValidationResult> _ZeroResults = new ValidationResult[0];

		private static HashSet<object> TryGetSeenHashFrom(ValidationContext context)
		{
			if (context.Items != null && !context.Items.ContainsKey(ITEMS_KEY))
			{
				context.Items[ITEMS_KEY] = new HashSet<object>();
			}

			return context.Items[ITEMS_KEY] as HashSet<object>;
		}

		private static IDictionary<object, object> CreateContextItems(bool validateAllProperties)
		{
			return new Dictionary<object, object>() {
				{ VALIDATE_ALL_PROPERTIES_KEY, validateAllProperties }
			};
		}

		private static IEnumerable<ValidationResult> ValidateClass(object value, ValidationContext context)
		{
			var results = new List<ValidationResult>();
			var attributes = value.GetType().GetCustomAttributes<ValidationAttribute>(true);
			Validator.TryValidateValue(value, context, results, attributes);

			return results;
		}

		private static IEnumerable<ValidationResult> ValidateProperties(object value, ValidationContext context, bool validateAllProperties)
		{
			if (value == null) return Enumerable.Empty<ValidationResult>();

			var type = value.GetType();
			var results = new List<ValidationResult>();

			// Now go through the properties and find any that are complex enough that they might
			// have their own validation requirements. Recurse into each value that we find.
			foreach (var property in type.GetProperties())
			{
				// Ignore properties with no getter.
				if (!property.CanRead)
					continue;

				// Ignore indexed properties as there is no way to know how to enumerate them on
				// their own. Classes should implement IEnumberable if they want these accessed
				// anyway.
				if (property.GetIndexParameters().Length > 0)
					continue;

				// Get the value assigned to the property and recurse into it
				var value2 = property.GetValue(value, null);
				var reqattrs = property.GetCustomAttributes<RequiredAttribute>();
				var newctx = new ValidationContext(value2 ?? "", context.Items)
				{
					DisplayName = context.DisplayName.IfNotNull(x => string.Join(":", x, property.Name)) ?? property.Name,
					MemberName = context.MemberName.IfNotNull(x => string.Join(".", x, property.Name)) ?? property.Name
				};

				// Let's first evaluate RequiredAttribute..
				results.AddRange(
					reqattrs
						.Select(x => x.GetValidationResult(value2, newctx))
						.Where(x => x != ValidationResult.Success)
				);

				// Now let's try nested validation...
				if (value2 != null)
				{
					if (validateAllProperties)
					{
						Validator.TryValidateObject(value2, newctx, results);
					}
					else if (property.GetCustomAttribute<ValidateObjectAttribute>() != null)
					{
						results.AddRange(ValidateRecursing(value2, newctx, validateAllProperties));
					}
				}
			}

			return results;
		}

		internal static IEnumerable<ValidationResult> ValidateRecursing(object value, ValidationContext context, bool validateAllProperties)
		{
			context = context ?? new ValidationContext(value);
			var seen = TryGetSeenHashFrom(context);

			//_Log.DebugFormat("Trying to validate {0}", value.ToString());
			if (value == null || seen.IfNotNull(x => x.Contains(value)))
			{
				return _ZeroResults;
			}

			var results = new List<ValidationResult>();

			if (value is string || value.GetType().IsValueType)
			{
				//throw new InvalidOperationException("ValidObject cannot be applied over string or value type properties.");
				return _ZeroResults;
			}
			if (value is IEnumerable)
			{
				var idx = 0;
				var valueAsEnumerable = value as IEnumerable;
				foreach (var item in valueAsEnumerable)
				{
					var idx2 = idx++;
					var newctx = new ValidationContext(item, null, context.Items)
					{
						DisplayName = context.DisplayName.IfNotNull(x => string.Format("{0}[{1}]", x, idx2)),
						MemberName = context.MemberName.IfNotNull(x => string.Format("{0}[{1}]", x, idx2))
					};
					if (validateAllProperties) results.AddRange(ValidateClass(item, newctx));
					results.AddRange(ValidateProperties(item, newctx, validateAllProperties));
				}
			}
			else
			{
				var newctx = new ValidationContext(value, null, context.Items)
				{
					DisplayName = context.DisplayName,
					MemberName = context.MemberName
				};
				if (validateAllProperties) results.AddRange(ValidateClass(value, newctx));
				results.AddRange(ValidateProperties(value, newctx, validateAllProperties));
			}

			return results;
		}
		#endregion

		/// <summary>
		/// Determines whether the specified object is valid and returns an list of ValidationResults.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <param name="validateAllProperties">
		/// If <c>true</c>, evaluates all the properties, otherwise just checks that
		/// ones marked with <see cref="RequiredAttribute"/> are not null.
		/// </param>
		/// <returns></returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public static IEnumerable<ValidationResult> Validate(object obj, bool validateAllProperties)
		{
			var items = CreateContextItems(validateAllProperties);
			var context = new ValidationContext(obj, serviceProvider: null, items: items);
			var results = new List<ValidationResult>();

			if (!validateAllProperties)
			{
				results.AddRange(ValidateRecursing(obj, context, validateAllProperties));
			}
			else
			{
				Validator.TryValidateObject(obj, context, results, validateAllProperties);
			}

			return AggregateValidationResult.Flatten(results);

		}

		/// <summary>
		/// Determines whether the specified object is valid and returns an list of ValidationResults.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <returns></returns>
		public static IEnumerable<ValidationResult> Validate(object obj)
		{
#if false
			var context = new ValidationContext(obj, serviceProvider: null, items: null);
			var results = new List<ValidationResult>();
			Validator.TryValidateObject(obj, context, results, true);

			return AggregateValidationResult.Flatten(results);
#else
			return Validate(obj, true);
#endif
		}

		/// <summary>
		/// Determines whether the specified object passes validation without errors.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <param name="validateAllProperties">
		/// If <c>true</c>, evaluates all the properties, otherwise just checks that
		/// ones marked with <see cref="RequiredAttribute"/> are not null.
		/// </param>
		/// <returns></returns>
		public static bool IsValid(object obj, bool validateAllProperties)
		{
			return !Validate(obj, validateAllProperties).Any();
		}

		/// <summary>
		/// Determines whether the specified object passes validation without errors.
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
		/// <param name="validateAllProperties">
		/// If <c>true</c>, evaluates all the properties, otherwise just checks that
		/// ones marked with <see cref="RequiredAttribute"/> are not null.
		/// </param>
		/// <exception cref="AggregateValidationException">Validation failed</exception>
		public static void EnsureIsValid(object obj, bool validateAllProperties)
		{
			var results = Validate(obj, validateAllProperties);

			if (results.Any() && results.First() != ValidationResult.Success)
			{
				var type = (obj ?? new object()).GetType();
				throw AggregateValidationException.CreateFor(type, results);
			}
		}

		/// <summary>
		/// Ensures the object is valid.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <exception cref="AggregateValidationException">Validation failed</exception>
		public static void EnsureIsValid(object obj)
		{
			EnsureIsValid(obj, true);
		}
	}
}