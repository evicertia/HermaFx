using System;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;

using HermaFx;

namespace HermaFx.DataAnnotations
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class NotDefaultAttribute : ValidationAttribute
	{
		private const string _defaultErrorMessage = "The field {0} requires a non-default value";

		public NotDefaultAttribute()
				: base(string.Format(_defaultErrorMessage))
		{
		}

		public override bool IsValid(object value)
		{
			if (value == null) return true;
			if (!value.GetType().IsValueType) return true;

			return value == value.GetType().GetDefaultValue();
		}
	}
}
