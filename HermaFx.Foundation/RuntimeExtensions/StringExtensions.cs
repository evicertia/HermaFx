using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HermaFx
{
	public static class StringExtensions
	{
		#region StartWith / EndWith extensions
		public static bool StartsWithInvariant(this string @this, string value)
		{
			return @this.StartsWith(value, StringComparison.InvariantCulture);
		}

		public static bool StartsWithInvariant(this string @this, string value, bool ignoreCase)
		{
			var mode = ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture;
			return @this.StartsWith(value, mode);
		}

		public static bool StartsWithOrdinal(this string @this, string value)
		{
			return @this.StartsWith(value, StringComparison.Ordinal);
		}

		public static bool StartsWithOrdinal(this string @this, string value, bool ignoreCase)
		{
			var mode = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
			return @this.StartsWith(value, mode);
		}

		public static bool EndsWithInvariant(this string @this, string value)
		{
			return @this.EndsWith(value, StringComparison.InvariantCulture);
		}

		public static bool EndsWithInvariant(this string @this, string value, bool ignoreCase)
		{
			var mode = ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture;
			return @this.EndsWith(value, mode);
		}

		public static bool EndsWithOrdinal(this string @this, string value)
		{
			return @this.EndsWith(value, StringComparison.Ordinal);
		}

		public static bool EndsWithOrdinal(this string @this, string value, bool ignoreCase)
		{
			var mode = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
			return @this.EndsWith(value, mode);
		}
		#endregion

		#region IsNullOrWhiteSpace & IfNotNullOrWhiteSpace
		/// <summary>
		/// Determines whether [is null, empty or white space] [the specified @this].
		/// </summary>
		/// <param name="this">The @this.</param>
		/// <returns>
		/// 	<c>true</c> if [is null or white space] [the specified @this]; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsNullOrWhiteSpace(this string @this)
		{
			if (string.IsNullOrEmpty(@this))
				return true;

			for (int i = 0; i < @this.Length; i++)
			{
				if (!char.IsWhiteSpace(@this[i]))
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Returns the result of evaluating the lambda expression 
		/// over item, if @this is not null or whitespace.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="U"></typeparam>
		/// <param name="@this">The @this.</param>
		/// <param name="lambda">The lambda.</param>
		/// <returns></returns>
		public static T IfNotNullOrWhiteSpace<T>(this string @this, Func<string, T> lambda)
		{
			if (@this.IsNullOrWhiteSpace())
			{
				return default(T);
			}
			return lambda(@this);
		}
		
		/// <summary>
		/// Exectues action over element if it is not null, empty nor whitespace string
		/// </summary>
		/// <param name="@this">The @this.</param>
		/// <param name="lambda">Action to apply.</param>
		/// <returns></returns>
		public static void IfNotNullOrWhiteSpace(this string @this, Action<string> lambda)
		{
			if (!@this.IsNullOrWhiteSpace())
			{
				return lambda(@this);
			}
		}
		#endregion

		public static string TrimOrDefault(this string @this)
		{
			if (@this == null)
				return null;

			return @this.Trim();
		}

		public static string ToHexString(this Byte[] @this)
		{
			if (@this == null) return null;

			var sb = new StringBuilder();
			foreach (byte b in @this)
			{
				sb.Append(b.ToString("x2").ToLower());
			}
			return sb.ToString();
		}

		/// <summary>
		/// Converts a hexadecimal string to a byte array. Used to convert encryption key values from the configuration.
		/// </summary>
		/// <param name="hexString">hexadecimal string to conver.</param>
		/// <returns><c>byte</c> array containing the converted hexadecimal string contents.</returns>
		public static byte[] AsByteArray(this string hexString)
		{
			Guard.IsNotNull(hexString, nameof(hexString));

			if (hexString == string.Empty)
				return new byte[0];

			Guard.Against<ArgumentOutOfRangeException>(hexString.Length % 2 != 0, "The specified argument {0} does not contain a valid length.", nameof(hexString));

			var length = hexString.Length / 2;
			var bytes = new byte[length];

			for (var i = 0; i < length; i++)
				bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);

			return bytes;
		}

		public static IEnumerable<string> Chunk(this string str, int chunkSize)
		{
			for (int i = 0; i < str.Length; i += chunkSize)
				yield return str.Substring(i, Math.Min(chunkSize, str.Length - i));
		}

		public static bool IsValidGuid(this string @this)
		{
			if (!string.IsNullOrEmpty(@this))
			{
				Guid guidValue = Guid.Empty;

				try
				{
					guidValue = new Guid(@this);
				}
				catch
				{
				}

				return guidValue != Guid.Empty;
			}

			return false;
		}

		public static string Format(this string @this, params object[] args)
		{
			if (@this == null) throw new ArgumentException("@this");
			return string.Format(@this, args);
		}

		#region FormatWith extensions
		public static string FormatWith(this string format, object source)
		{
			return FormatWith(format, null, source);
		}

		public static string FormatWith(this string format, IFormatProvider provider, object source)
		{
			if (format == null)
				throw new ArgumentNullException("format");

			List<object> values = new List<object>();
			string rewrittenFormat = Regex.Replace(format,
			  @"(?<start>\{)+(?<property>[\w\.\[\]]+)(?<format>:[^}]+)?(?<end>\})+",
			  delegate(Match m)
			  {
				  Group startGroup = m.Groups["start"];
				  Group propertyGroup = m.Groups["property"];
				  Group formatGroup = m.Groups["format"];
				  Group endGroup = m.Groups["end"];

				  values.Add((propertyGroup.Value == "0")
					? source
					: Eval(source, propertyGroup.Value));

				  int openings = startGroup.Captures.Count;
				  int closings = endGroup.Captures.Count;

				  return openings > closings || openings % 2 == 0
					 ? m.Value
					 : new string('{', openings) + (values.Count - 1)
					   + formatGroup.Value
					   + new string('}', closings);
			  },
			  RegexOptions.Compiled
			  | RegexOptions.CultureInvariant
			  | RegexOptions.IgnoreCase);

			return string.Format(provider, rewrittenFormat, values.ToArray());
		}

		private static object Eval(object source, string expression)
		{
			try
			{
				// TODO: Support subexpression on dictionary case.. (Prop.SubProp..)
				if (source is IDictionary<string, object>)
					return (source as IDictionary<string, object>)[expression];
				else
					return System.Web.UI.DataBinder.Eval(source, expression);
			}
			catch (System.Web.HttpException e)
			{
				throw new FormatException(null, e);
			}
		}
		#endregion

		#region IsBase64String
		private static readonly HashSet<char> _base64Characters = new HashSet<char>() { 
			'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 
			'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 
			'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 
			'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '+', '/', 
			'='
		};

		public static bool IsBase64String(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return false;
			}
			else if (value.Any(c => !_base64Characters.Contains(c)))
			{
				return false;
			}

			try
			{
				Convert.FromBase64String(value);
			}
			catch (FormatException)
			{
				return false;
			}

			return true;
		}
		#endregion
	}
}
