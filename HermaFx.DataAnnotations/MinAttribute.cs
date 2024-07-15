using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace HermaFx.DataAnnotations
{
	/// <summary>
	/// Can be used with parameter target..
	/// </summary>
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
	public class MinAttribute : ValidationAttribute
	{
		#region Fields

		private const string ERROR_MESSAGE = "The field {0} should be greater or equal than {1}";

		private readonly long _min;

		#endregion

		#region .ctors

		public MinAttribute(long min)
			: base(() => ERROR_MESSAGE)
		{
			_min = min;
		}

		#endregion

		public override bool IsValid(object value) //< Ported from NH.V
		{
			if (value == null) return true;

			try
			{
				return Convert.ToDouble(value) >= _min;
			}
			catch (InvalidCastException)
			{
				if (value is char)
					return Convert.ToInt32(value) >= _min;

				return false;
			}
			catch (Exception ex) when (ex is FormatException || ex is OverflowException)
			{
				return false;
			}
		}

		public override string FormatErrorMessage(string name)
			=> string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, _min);
	}
}
