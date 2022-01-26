using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace HermaFx.Rfc2047Encoding
{
	[TestFixture]
	public class Rfc2047Tests
	{
		[Test]
		public void EncodeTextWithDefatultFormatOptions()
		{
			var text = "50304_2201_2021_Contract_Main_2021-11-02.pdf";
			var encodingOptions = new FormatOptions();

			var encodedString = Rfc2047.EncodeText(encodingOptions, Encoding.UTF8, text);
			var decodedString = Rfc2047.DecodeText(encodedString);

			Assert.AreEqual(text, decodedString);
		}

		[Test]
		public void EncodeTextWithInternationalFormatOptions()
		{
			var text = "50304_2201_2021_Contract_Main_2021-11-03.pdf";
			var encodingOptionsWithInternational = new FormatOptions { International = true, MaxLineLength = 988, AllowMixedHeaderCharsets = false };

			var encodedString = Rfc2047.EncodeText(encodingOptionsWithInternational, Encoding.UTF8, text);
			var decodedString = Rfc2047.DecodeText(encodedString);

			Assert.AreEqual(text, decodedString);
		}


		[Test]
		public void EncodeTextWithAllowMixedHeaderCharsetsFormatOptions()
		{
			var text = "50304_2201_2021_Contract_Main_2021-11-04.pdf";
			var encodingOptionsWithAllowMixedHeaderCharsets = new FormatOptions { International = false, MaxLineLength = 988, AllowMixedHeaderCharsets = true };

			var encodedString = Rfc2047.EncodeText(encodingOptionsWithAllowMixedHeaderCharsets, Encoding.UTF8, text);
			var decodedString = Rfc2047.DecodeText(encodedString);

			Assert.AreEqual(text, decodedString);
		}
	}
}
