using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace HermaFx.DataAnnotations
{
	[AttributeUsage(AttributeTargets.Property)]
	public class ValidateKeysUsingAttribute : ValidateElementsUsingAttribute
	{
		#region .ctor

		public ValidateKeysUsingAttribute(Type metadataType, string propertyName)
			: base(metadataType, propertyName)
		{
		}

		public ValidateKeysUsingAttribute(Type metadataType, string propertyName, string errorMessage)
			: base(metadataType, propertyName, errorMessage)
		{
		}

		public ValidateKeysUsingAttribute(Type metadataType, string propertyName, BindingFlags bindingFlags, MemberTypes memberTypes)
			: base(metadataType, propertyName, bindingFlags, memberTypes)
		{
		}

		public ValidateKeysUsingAttribute(Type metadataType, string propertyName, BindingFlags bindingFlags, MemberTypes memberTypes, string errorMessage)
			: base(metadataType, propertyName, bindingFlags, memberTypes, errorMessage)
		{
		}

		#endregion

		protected override void CheckTargetType(object value, string memberName)
		{
			if (!(value is IDictionary))
				throw new ValidationException($"Property {memberName} is not dictionary.".Format(memberName));
		}

		protected override IEnumerable GetEnumerable(object value) => (value as IDictionary).Keys;

	}
}
