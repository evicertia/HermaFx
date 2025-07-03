using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace HermaFx.DataAnnotations
{
	[AttributeUsage(AttributeTargets.Property)]
	public class ValidateValuesUsingAttribute : ValidateElementsUsingAttribute
	{
		#region .ctor

		public ValidateValuesUsingAttribute(Type metadataType, string propertyName)
			: base(metadataType, propertyName)
		{
		}

		public ValidateValuesUsingAttribute(Type metadataType, string propertyName, string errorMessage)
			: base(metadataType, propertyName, errorMessage)
		{
		}

		public ValidateValuesUsingAttribute(Type metadataType, string propertyName, BindingFlags bindingFlags, MemberTypes memberTypes)
			: base(metadataType, propertyName, bindingFlags, memberTypes)
		{
		}

		public ValidateValuesUsingAttribute(Type metadataType, string propertyName, BindingFlags bindingFlags, MemberTypes memberTypes, string errorMessage)
			: base(metadataType, propertyName, bindingFlags, memberTypes, errorMessage)
		{
		}

		#endregion

		protected override void CheckTargetType(object value, string memberName)
		{
			if (!(value is IDictionary))
				throw new ValidationException($"Property {memberName} is not dictionary.".Format(memberName));
		}

		protected override IEnumerable GetEnumerable(object value) => (value as IDictionary).Values;

	}
}
