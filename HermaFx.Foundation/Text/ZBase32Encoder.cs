using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HermaFx.Text
{
	public class ZBase32Encoder : Base32Encoder
	{
		//zBase32 encoding table: See http://zooko.com/repos/z-base-32/base32/DESIGN
		private const string DEF_ENCODING_TABLE = "ybndrfg8ejkmcpqxot1uwisza345h769";
		private const char DEF_PADDING = '=';

		public ZBase32Encoder() : base(DEF_ENCODING_TABLE, DEF_PADDING) { }

		override public string Encode(byte[] input)
		{
			var encoded = base.Encode(input);
			return encoded.TrimEnd(DEF_PADDING);
		}

		override public byte[] Decode(string data)
		{
			//Guess the original data size
			int expectedOrigSize = Convert.ToInt32(Math.Floor(data.Length / 1.6));
			int expectedPaddedLength = 8 * Convert.ToInt32(Math.Ceiling(expectedOrigSize / 5.0));
			string base32Data = data.PadRight(expectedPaddedLength, DEF_PADDING).ToLower();

			return base.Decode(base32Data);
		}
	}
}
