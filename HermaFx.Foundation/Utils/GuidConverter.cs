using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HermaFx;
using HermaFx.Text;

namespace HermaFx.Utils
{
	public class GuidConverter
	{
		private const int ZBASE32_LENGTH = 26;
		private const int ZBASE64_LENGTH = 24;

		private static ZBase32Encoder _encoder = new ZBase32Encoder();

		#region Base32

		public Guid FromBase32String(string base32)
		{
			Guard.IsNotNullNorEmpty(base32, nameof(base32));
			Guard.Against<ArgumentException>(base32.Length != ZBASE32_LENGTH,
				$"Invalid base32 string length, expected {ZBASE32_LENGTH}");

			var bytes = _encoder.Decode(base32);

			return new Guid(bytes);
		}

		public bool TryFromBase32String(string base32, out Guid result)
		{
			try
			{
				result = FromBase32String(base32);
				return true;
			}
			catch { }

			result = default(Guid);
			return false;
		}

		#endregion

		#region Base64

		public Guid FromBase64String(string base64)
		{
			Guard.IsNotNullNorEmpty(base64, nameof(base64));

			base64 = base64.EndsWith("==") ? base64 : $"{base64}==";

			Guard.Against<ArgumentException>(base64.Length != ZBASE64_LENGTH,
				$"Invalid base64 string length, expected {ZBASE64_LENGTH}");

			var bytes = Convert.FromBase64String(base64);

			return new Guid(bytes);
		}

		public bool TryFromBase64String(string base64, out Guid result)
		{
			try
			{
				result = FromBase64String(base64);
				return true;
			}
			catch { }

			result = default(Guid);
			return false;
		}

		#endregion


		public Guid Parse(string input)
		{
			Guard.IsNotNullNorEmpty(input, nameof(input));

			var length = input.Length;

			if (length == ZBASE64_LENGTH)
			{
				return FromBase64String(input);
			}
			else if (length > ZBASE64_LENGTH && length <= ZBASE32_LENGTH)
			{
				return FromBase32String(input);
			}

			return Guid.Parse(input);
		}

		public bool TryParse(string input, out Guid result)
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

	}
}
