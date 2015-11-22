using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace HermaFx
{
	public static partial class Guard
	{
		private static string GetParameterName(Expression reference)
		{
			var lambda = reference as LambdaExpression;
			var member = lambda.Body as MemberExpression;

			return member.Member.Name;
		}

		private static string FormatMessage(string message, params object[] args)
		{
			return args != null ? string.Format(message, args) : message;
		}

		/// <summary>
		/// Ensures the given <paramref name="value"/> is not null.
		/// Throws <see cref="ArgumentNullException"/> otherwise.
		/// </summary>
		public static void IsNotNull<T>(Expression<Func<T>> reference, T value, string message = null, params object[] args)
		{
			if (value == null)
			{
				message = message == null ? GetParameterName(reference) : message;
				Guard.IsNotNull(value, message, args);
			}
		}

		/// <summary>
		/// Ensures the given <paramref name="value"/> is not default.
		/// Throws <see cref="ArgumentNullException"/> otherwise.
		/// </summary>
		public static void IsNotDefault<T>(Expression<Func<T>> reference, T value, string message = null, params object[] args)
			where T : struct
		{
			if (object.Equals(value, default(T)))
			{
				message = message == null ? GetParameterName(reference) : message;
				Guard.IsNotDefault(value, message, args);
			}
		}

		/// <summary>
		/// Ensures the given string <paramref name="value"/> is not null nor default.
		/// Throws <see cref="ArgumentNullException"/> in the first case, or
		/// <see cref="ArgumentException"/> in the latter.
		/// </summary>
		public static void IsNotNullNorDefault<T>(Expression<Func<T>> reference, T value, string message = null, params object[] args)
		{
			if (value == null || object.Equals(value, default(T)))
			{
				message = message == null ? GetParameterName(reference) : message;
				Guard.IsNotNullNorDefault(value, message, args);
			}
		}

		/// <summary>
		/// Ensures the given string <paramref name="value"/> is not null or empty.
		/// Throws <see cref="ArgumentNullException"/> in the first case, or
		/// <see cref="ArgumentException"/> in the latter.
		/// </summary>
		public static void IsNotNullNorEmpty(Expression<Func<string>> reference, string value, string message = null, params object[] args)
		{
			if (string.IsNullOrEmpty(value))
			{
				message = message == null ? GetParameterName(reference) : message;
				Guard.IsNotNullNorEmpty(value, message, args);
			}
		}

		/// <summary>
		/// Ensures the given string <paramref name="value"/> is not null or whitespace.
		/// Throws <see cref="ArgumentNullException"/> in the first case, or
		/// <see cref="ArgumentException"/> in the latter.
		/// </summary>
		public static void IsNotNullNorWhitespace(Expression<Func<string>> reference, string value, string message = null, params object[] args)
		{
#if NET_4_0
			if (string.IsNullOrEmpty(value))
#else
			if (StringExtensions.IsNullOrWhiteSpace(value))
#endif
			{
				message = message == null ? GetParameterName(reference) : message;
				Guard.IsNotNullNorWhitespace(value, message, args);
			}
		}

	}
}
