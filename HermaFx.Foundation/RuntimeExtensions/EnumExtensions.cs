using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.CodeAnalysis;

namespace HermaFx
{
	/// <summary>
	/// Helpful helper class for all things with enumerations
	/// </summary>
	public static class EnumExtensions
	{
		/// <summary>
		/// Tries the parse enum.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="str">The STR.</param>
		/// <param name="value">The value.</param>
		/// <param name="caseSensitive">if set to <c>true</c> [case sensitive].</param>
		/// <returns></returns>
		public static bool TryParse<T>(string str, out T value, bool caseSensitive) 
			where T : struct
		{
			// Can't make this a type constraint...
			if (!typeof(T).IsEnum)
			{
				throw new ArgumentException("Type parameter must be an enum");
			}

			var names = Enum.GetNames(typeof(T));
			value = (Enum.GetValues(typeof(T)) as T[])[0];  // For want of a better default
			foreach (var name in names)
			{
				if (String.Equals(name, str, caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase))
				{
					value = (T)Enum.Parse(typeof(T), name);
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Parses the specified enum value - ignoring case.
		/// </summary>
		/// <param name="enumValue">The enum value.</param>
		/// <returns></returns>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
		public static T Parse<T>(string enumValue)
				where T : struct
		{
			return Parse<T>(enumValue, true);
		}

		/// <summary>
		/// Parses the specified enum value.
		/// </summary>
		/// <param name="enumValue">The enum value.</param>
		/// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
		/// <returns></returns>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
		public static T Parse<T>(string enumValue, bool ignoreCase)
				where T : struct
		{
			if (string.IsNullOrEmpty(enumValue))
				throw new ArgumentNullException("enumValue");

			enumValue = enumValue.Trim();

			Type t = typeof(T);

			if (!t.IsEnum)
				throw new ArgumentException("enumValue is not an Enum");

			return (T)Enum.Parse(t, enumValue, ignoreCase);
		}

		/// <summary>
		/// Parses the enum.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
		public static T ParseEnum<T>(this string value)
				where T : struct
		{
			if (string.IsNullOrEmpty(value))
				throw new ArgumentNullException("value");

			return ParseEnum<T>(value, false);
		}


		/// <summary>
		/// Parses the enum.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value">The value.</param>
		/// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
		/// <returns></returns>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
		public static T ParseEnum<T>(this string value, bool ignoreCase)
				where T : struct
		{
			if (string.IsNullOrEmpty(value))
				throw new ArgumentNullException("value");

			return Parse<T>(value, ignoreCase);
		}

		/// <summary>
		/// Gets the enum or default.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value">The value.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns></returns>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
		public static T GetEnumOrDefault<T>(this string value, T defaultValue)
				where T : struct
		{
			T tmp;

			if (string.IsNullOrEmpty(value))
				return defaultValue;

			if (TryParse<T>(value, out tmp, false))
				return tmp;

			return defaultValue;
		}
	}
}
