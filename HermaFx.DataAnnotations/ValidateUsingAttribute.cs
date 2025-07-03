#if true
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace HermaFx.DataAnnotations
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class ValidateUsingAttribute : ValidationAttribute
	{
		private readonly RequiredAttribute _requiredValidation;
		private readonly IEnumerable<ValidationAttribute> _validationAttributes;

		public static MemberTypes DefaultMemberTypes { get; } = MemberTypes.Property;
		public static BindingFlags DefaultBindingFlags { get; } = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;

		public Type MetadataType { get; }
		public string TargetName { get; }
		public MemberTypes MemberTypes { get; }
		public BindingFlags BindingFlags { get; }


		protected virtual MemberInfo GetMember(string name)
		{
			var member = MetadataType.GetMember(name, BindingFlags).SingleOrDefault(x => MemberTypes.HasFlag(x.MemberType));
			return member;
		}

		protected virtual IEnumerable<ValidationAttribute> GetValidationAttributes(Type type, string name)
		{
			var member = GetMember(name)
				?? throw new ValidationException($"Metadata Object of type {MetadataType.FullName} is missing member: {TargetName}");

			return member.GetCustomAttributes<ValidationAttribute>(true);
		}

		public ValidateUsingAttribute(Type container, string target, BindingFlags bindingFlags, MemberTypes memberTypes)
		{
			MetadataType = container.ThrowIfNull(nameof(container));
			TargetName = target.ThrowIfNullOrWhiteSpace(nameof(target));
			BindingFlags = bindingFlags;
			MemberTypes = memberTypes;

			var attributes = GetValidationAttributes(container, target);

			_requiredValidation = attributes.OfType<RequiredAttribute>().SingleOrDefault(); // RequiredAttribute can not be multiple
			_validationAttributes = attributes.Where(x => x != _requiredValidation);
		}

		public ValidateUsingAttribute(Type container, string target)
			: this(container, target, DefaultBindingFlags, DefaultMemberTypes)
		{
		}

		protected override ValidationResult IsValid(object value, ValidationContext context)
		{
			// check RequiredAttribute and throw if not success
			if (_requiredValidation != null
				&& _requiredValidation.GetValidationResult(value, context) is ValidationResult requiredResult
				&& requiredResult != ValidationResult.Success)
				return requiredResult;

			// Rest of validation attributes and aggregate results
			var results = _validationAttributes
				.Select(x => x.GetValidationResult(value, context))
				.Where(x => x != ValidationResult.Success)
				.ToArray();

			if (results.Length == 0)
				return ValidationResult.Success;
			return AggregateValidationResult.CreateFor(context, results);
		}
	}
}
#endif
