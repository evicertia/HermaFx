using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace HermaFx.DataAnnotations
{
	[AttributeUsage(AttributeTargets.Property)]
	public class DistinctElementsAttribute : ValidationAttribute
	{
		public string UniqueProperty { get; }
		public Type ElementType { get; }
		public DistinctElementsAttribute(string property)
		{
			ErrorMessage = "The property {0} must be unique in the {1} collection.";
			UniqueProperty = property;
		}

		private bool IsGenericEnumerable(Type type)
		{
			return type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
		}

		private IEnumerable<Type> GetGenericEnumerableTypes(Type type)
		{
			return type.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>)).ToArray();
		}

		protected override ValidationResult IsValid(object value, ValidationContext context)
		{
			if (value is null)
			{
				return ValidationResult.Success;
			}
			// If value is IEnumerate, check if the property being validate is unique in the collection
			else if (!IsGenericEnumerable(value.GetType()))
			{
				throw new InvalidOperationException("The property being validated must be an IEnumerable.");
			}

			var ifaces = GetGenericEnumerableTypes(value.GetType())
				.Where(x => ElementType == null || x.GetGenericArguments()[0].GetType() == ElementType);

			if (ifaces.Count() > 1)
			{
				throw new InvalidOperationException("The property being validated implements more than one generic IEnumerable<>, please use ElementType to indicate intended type.");
			}
			else if (ifaces.Count() == 0)
			{
				throw new InvalidOperationException("Unable to guess element type, maybe an incorrect ElementType was specified?");
			}

			var type = ElementType ?? ifaces.First().GetGenericArguments()[0];

			Guard.Against<InvalidOperationException>(
				type.GetProperty(UniqueProperty) == null,
				"ElementType ({0}) does not contains a property named {1}?", type, UniqueProperty
			);

			var collection = ((IEnumerable)value).Cast<object>().ToArray();
			var property = type.GetProperty(UniqueProperty);

			if (collection.Select(x => property.GetValue(x)).Distinct().Count() == collection.Count())
			{
				return ValidationResult.Success;
			}

			return new ValidationResult(FormatErrorMessage(context.MemberName));
		}
		public override string FormatErrorMessage(string name)
		{
			return string.Format(ErrorMessageString, UniqueProperty, name);
		}
	}
}
