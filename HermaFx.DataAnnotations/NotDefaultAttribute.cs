using System;
using System.ComponentModel.DataAnnotations;

namespace HermaFx.DataAnnotations
{
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class NotDefaultAttribute : ValidationAttribute
	{
		private const string _defaultErrorMessage = "The field {0} requires a non-default value";

		public NotDefaultAttribute()
				: base(() => _defaultErrorMessage)
		{
		}

		public override bool IsValid(object value)
		{
			if (value == null) return true;
			if (!value.GetType().IsValueType) return true;

			return !object.Equals(value, value.GetType().GetDefault());
		}
	}
}
