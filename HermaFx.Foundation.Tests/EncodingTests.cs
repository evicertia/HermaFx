using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit;
using NUnit.Framework;

namespace HermaFx.Text
{
	[TestFixture]
	public abstract class EncodingTests
	{
		public void VariableLengthTests(Base32Encoder encoder)
		{
			//Create strings of length from 1 to 26 and see whether we can encode and decode successfully.
			var originalText = String.Empty;
			var byteConverter = new ASCIIEncoding();

			for (int i = 0x41; i <= 0x5A; i++)
			{
				originalText += Convert.ToChar(i);
				var encodedText = encoder.Encode(byteConverter.GetBytes(originalText));
				var decodedText = byteConverter.GetString(encoder.Decode(encodedText));
				TestContext.WriteLine("Original: {0}, Encoded: {1}, Decoded: {2}", originalText, encodedText, decodedText);
				Assert.IsTrue(originalText == decodedText, "Original and decoded text must match.");
			}
		}

		public void WordListTests(Base32Encoder encoder)
		{
			var wordList = Properties.Resources.Wordlist.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
			var byteConverter = new ASCIIEncoding();

			foreach (string originalText in wordList)
			{
				var encodedText = encoder.Encode(byteConverter.GetBytes(originalText));
				var decodedText = byteConverter.GetString(encoder.Decode(encodedText));
				TestContext.WriteLine("Original: {0}, Encoded: {1}, Decoded: {2}", originalText, encodedText, decodedText);
				Assert.IsTrue(originalText == decodedText, "Original and decoded text must match.");
			}
		}
	}
}
