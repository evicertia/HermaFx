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
#if false
		private static readonly string ITEMS_KEY = typeof(ValidateObjectAttribute).Name + "::Seen";

		private static HashSet<object> TryGetSeenHashFrom(ValidationContext context)
		{
			if (context.Items != null && !context.Items.ContainsKey(ITEMS_KEY))
			{
				context.Items[ITEMS_KEY] = new HashSet<object>();
			}

			return context.Items[ITEMS_KEY] as HashSet<object>;
		}

		private IEnumerable<ValidationResult> ValidateClass(object value, ValidationContext context)
		{
			var results = new List<ValidationResult>();
			var attributes = value.GetType().GetCustomAttributes<ValidationAttribute>(true);
			Validator.TryValidateValue(value, context, results, attributes);

			return results;
		}

		private IEnumerable<ValidationResult> ValidateProperties(object value, ValidationContext context)
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
				var reqattr = property.GetCustomAttribute<RequiredAttribute>();
				var newctx = new ValidationContext(value2 ?? "", context.Items)
				{
					DisplayName = context.DisplayName.IfNotNull(x => string.Join(":", x, property.Name)),
					MemberName = context.MemberName.IfNotNull(x => string.Join(".", x, property.Name))
				};

				// Let's first evaluate RequiredAttribute..
				if (reqattr != null)
				{
					var attr = property.GetCustomAttribute<RequiredAttribute>();
					var res = attr.GetValidationResult(value2, newctx);
					if (res != ValidationResult.Success) results.Add(res);
				}

				if (value2 != null)
				{
					Validator.TryValidateObject(value2, newctx, results);
				}
			}

			return results;
		}

		protected override ValidationResult IsValid(object value, ValidationContext context)
		{
			context = context ?? new ValidationContext(value);
			var seen = TryGetSeenHashFrom(context);

			//_Log.DebugFormat("Trying to validate {0}", value.ToString());
			if (value == null || seen.IfNotNull(x => x.Contains(value)))
			{
				return ValidationResult.Success;
			}

			var results = new List<ValidationResult>();

			if (value is string || value.GetType().IsValueType)
			{
				//throw new InvalidOperationException("ValidObject cannot be applied over string or value type properties.");
				return ValidationResult.Success;
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
					results.AddRange(ValidateClass(item, newctx));
					results.AddRange(ValidateProperties(item, newctx));
				}
			}
			else
			{
				var newctx = new ValidationContext(value, null, context.Items)
				{
					DisplayName = context.DisplayName,
					MemberName = context.MemberName
				};
				results.AddRange(ValidateClass(value, newctx));
				results.AddRange(ValidateProperties(value, newctx));
			}

			if (results.Count != 0)
			{
				//_Log.DebugFormat("Validation failed for: {0} | {1} | {2}", validationContext.ObjectType, validationContext.DisplayName, validationContext.MemberName);

				return AggregateValidationResult.CreateFor(context, results);
			}

			return ValidationResult.Success;
		}
#endif
		protected override ValidationResult IsValid(object value, ValidationContext context)
		{
			var results = ExtendedValidator.ValidateRecursing(value, context, true);

			if (results.Count() != 0)
			{
				//_Log.DebugFormat("Validation failed for: {0} | {1} | {2}", validationContext.ObjectType, validationContext.DisplayName, validationContext.MemberName);

				return AggregateValidationResult.CreateFor(context, results);
			}

			return ValidationResult.Success;
		}
	}
}
