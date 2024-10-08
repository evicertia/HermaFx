#if true
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace HermaFx.DataAnnotations
{
	[AttributeUsage(AttributeTargets.Property)]
	public class ValidateElementsUsingAttribute : ValidationAttribute
	{
		#region Private Properties
		private const string DefaultErrorMessage = "{0} has some invalid values.";
		#endregion

		#region Public Properties
		public Type MetadataType { get; private set; }
		public string Property { get; private set; }

		/// <summary>
		/// A flag indicating that the attribute requires a non-null <see cref=System.ComponentModel.DataAnnotations.ValidationContext /> to perform validation.
		/// </summary>
		public override bool RequiresValidationContext
		{
			get
			{
				return true;
			}
		}
		#endregion

		#region .ctor

		public ValidateElementsUsingAttribute(Type metadataType, string propertyName)
			: this(metadataType, propertyName, DefaultErrorMessage) { }

		public ValidateElementsUsingAttribute(Type metadataType, string propertyName, string errorMessage)
			: base(() => errorMessage)
		{
			MetadataType = metadataType;
			Property = propertyName;
		}

		#endregion

		protected virtual void CheckTargetType(object value, string memberName)
		{
			if (!(value is IEnumerable))
				throw new ValidationException($"Property {memberName} is not enumerable.");
		}

		protected virtual IEnumerable GetEnumerable(object value) => value as IEnumerable;

		private IEnumerable<ValidationResult> ValidateClass(object value, ValidationContext context, IEnumerable<ValidationAttribute> attributes)
		{
			var results = new List<ValidationResult>();
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
			if (value == null)
			{
				return ValidationResult.Success;
			}

			CheckTargetType(value, context.MemberName);

			var property = MetadataType.GetRuntimeProperties().SingleOrDefault(x => x.Name == Property);

			if (property == null)
			{
				throw new ValidationException("Metadata Object of type {0} is missing property: {1}".Format(MetadataType.FullName as object, Property));
			}

			var attributes = property.GetCustomAttributes<ValidationAttribute>(true);
			var results = new List<ValidationResult>();

			if (attributes.Any())
			{
				var idx = 0;
				var valueAsEnumerable = GetEnumerable(value);
				foreach (var item in valueAsEnumerable)
				{
					var idx2 = idx++;
					var newctx = new ValidationContext(item, null, context.Items)
					{
						DisplayName = context.DisplayName.IfNotNull(x => string.Format("{0}[{1}]", x, idx2)),
						MemberName = context.MemberName.IfNotNull(x => string.Format("{0}[{1}]", x, idx2))
					};
					results.AddRange(ValidateClass(item, newctx, attributes));
					//results.AddRange(ValidateProperties(item, newctx));
				}
			}

			return results.Count == 0 ?
				ValidationResult.Success :
				AggregateValidationResult.CreateFor(
					FormatErrorMessage(context.DisplayName.IsNullOrEmpty() ? context.MemberName : context.DisplayName), results);
		}
	}
}
#endif
