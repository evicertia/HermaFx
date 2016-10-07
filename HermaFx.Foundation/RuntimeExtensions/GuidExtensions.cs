using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HermaFx.Text;

namespace HermaFx
{
	public static class GuidExtensions
	{
		public static bool TryParse(string input, out Guid result)
		{
			try
			{
				result = new Guid(input);
				return true;
			}
			catch { }

			result = default(Guid);
			return false;
		}

		public static string ToBase32String(this Guid guid)
		{
			return new ZBase32Encoder().Encode(guid.ToByteArray());
		}

		public static string ToBase64String(this Guid guid)
		{
			return Convert.ToBase64String(guid.ToByteArray());
		}
	}
}
