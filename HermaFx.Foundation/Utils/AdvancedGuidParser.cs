using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HermaFx;
using HermaFx.Text;

namespace HermaFx.Utils
{
	public class AdvancedGuidParser
	{
		private const int BASE64_WO_PADD_LENGTH = 22;
		private const int ZBASE32_LENGTH = 26;

		private static string NormalizeBase64Padding(string base64)
		{
			// Normlize base64 string adding the padding at end if not have it
			return base64.EndsWith("==") ? base64 : $"{base64}==";
		}

		#region From Base32

		public static Guid ParseFromZBase32String(string base32)
		{
			Guard.IsNotNullNorEmpty(base32, nameof(base32));
			Guard.Against<ArgumentException>(base32.Length != ZBASE32_LENGTH,
				$"Invalid base32 string length, expected {ZBASE32_LENGTH}");

			try
			{
				var bytes = new ZBase32Encoder().Decode(base32);
				return new Guid(bytes);
			}
			catch (ArgumentException e)
			{
				throw new ArgumentException($"Unable to parse ZBase32String '{base32}' to Guid", e);
			}

		}

		public static bool TryParseFromBase32String(string base32, out Guid result)
		{
			try
			{
				result = ParseFromZBase32String(base32);
				return true;
			}
			catch { }

			result = default(Guid);
			return false;
		}

		#endregion

		#region From Base64

		public static Guid ParseFromBase64String(string base64)
		{
			Guard.IsNotNullNorEmpty(base64, nameof(base64));

			try
			{
				var bytes = Convert.FromBase64String(NormalizeBase64Padding(base64));
				return new Guid(bytes);
			}
			catch (FormatException e)
			{
				throw new ArgumentException($"Invalid format of Base64String '{base64}'", e);
			}
			catch (ArgumentException e)
			{
				throw new ArgumentException($"Unable to parse Base64String '{base64}' to Guid", e);
			}
		}

		public static bool TryParseFromBase64String(string base64, out Guid result)
		{
			try
			{
				result = ParseFromBase64String(base64);
				return true;
			}
			catch { }

			result = default(Guid);
			return false;
		}

		#endregion

		#region Parser

		public static Guid Parse(string input)
		{
			Guard.IsNotNullNorEmpty(input, nameof(input));

			if (input.TrimEnd('=').Length == BASE64_WO_PADD_LENGTH)
			{
				return ParseFromBase64String(input);
			}
			else if (input.Length == ZBASE32_LENGTH)
			{
				return ParseFromZBase32String(input);
			}

			return Guid.Parse(input);
		}

		public static bool TryParse(string input, out Guid result)
		{
			try
			{
				result = Parse(input);
				return true;
			}
			catch { }

			result = default(Guid);
			return false;
		}

		#endregion

	}
}
