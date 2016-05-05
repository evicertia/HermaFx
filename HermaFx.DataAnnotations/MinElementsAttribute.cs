using System;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HermaFx.DataAnnotations
{
	public class MinElementsAttribute : ValidationAttribute
	{
		private const string _defaultErrorMessage = "There should be at least {0} elements.";

		public uint Elements;

		public MinElementsAttribute(uint elements)
			: base(() => GetErrorMessage(elements))
		{
			Elements = elements;
		}

		private static string GetErrorMessage(uint elements)
		{
			return string.Format(_defaultErrorMessage, elements);
		}

		public override bool IsValid(object value)
		{
			if (value == null) return true;
			if (value is Array) return (value as Array).Length >= Elements;
			if (value is ICollection) return (value as ICollection).Count >= Elements;
			if (value is IEnumerable) return (value as IEnumerable).OfType<object>().Count() >= Elements;

			return false;
		}
	}
}
