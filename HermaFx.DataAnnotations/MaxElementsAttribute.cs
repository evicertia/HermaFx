using System;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HermaFx.DataAnnotations
{
	public class MaxElementsAttribute : ValidationAttribute
	{
		private const string _defaultErrorMessage = "There should be at most {0} elements.";

		private uint _elements;
		public uint Count { get { return _elements; } }

		public MaxElementsAttribute(uint elements)
			: base(() => GetErrorMessage(elements))
		{
			_elements = elements;
		}

		private static string GetErrorMessage(uint elements)
		{
			return string.Format(_defaultErrorMessage, elements);
		}

		public override bool IsValid(object value)
		{
			if (value == null) return true;
			if (value is Array) return (value as Array).Length <= _elements;
			if (value is ICollection) return (value as ICollection).Count <= _elements;
			if (value is IEnumerable) return (value as IEnumerable).OfType<object>().Count() <= _elements;

			return false;
		}
	}
}
