/*MimeKit is Copyright (C) 2012-2018 Xamarin Inc. and is licensed under the MIT license:

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.*/
using System;
using System.Collections.Generic;
using System.Text;

namespace HermaFx.Rfc2047Encoding
{
	internal static class CharsetUtils
	{
		public static readonly IEqualityComparer<string> OrdinalIgnoreCase = new OptimizedOrdinalIgnoreCaseComparer();
		private static readonly Dictionary<string, int> aliases;
		public static readonly Encoding Latin1;
		public static readonly Encoding UTF8;

		static CharsetUtils()
		{
			int gb2312;

#if NET461_OR_GREATER || NETSTANDARD
			Encoding.RegisterProvider (CodePagesEncodingProvider.Instance);
#endif

			try
			{
				Latin1 = Encoding.GetEncoding(28591, new EncoderExceptionFallback(), new DecoderExceptionFallback());
			}
			catch (NotSupportedException)
			{
				// Note: Some ASP.NET web hosts such as GoDaddy's Windows environment do not have
				// iso-8859-1 support, they only have the built-in text encodings, so we need to
				// hack around it by using an alternative encoding.

				// Try to use Windows-1252 if it is available...
				Latin1 = Encoding.GetEncoding(1252, new EncoderExceptionFallback(), new DecoderExceptionFallback());
			}

			// Note: Encoding.UTF8.GetString() replaces invalid bytes with a unicode '?' character,
			// so we use our own UTF8 instance when using GetString() if we do not want it to do that.
			UTF8 = Encoding.GetEncoding(65001, new EncoderExceptionFallback(), new DecoderExceptionFallback());

			aliases = new Dictionary<string, int>(OrdinalIgnoreCase);

			AddAliases(aliases, 65001, -1, "utf-8", "utf8");

			// ANSI_X3.4-1968 is used on some systems and should be
			// treated the same as US-ASCII.
			AddAliases(aliases, 20127, -1, "ansi_x3.4-1968");

			// ANSI_X3.110-1983 is another odd-ball charset that appears
			// every once in a while and seems closest to ISO-8859-1.
			AddAliases(aliases, 28591, -1, "ansi_x3.110-1983", "latin1");

			// Macintosh aliases
			AddAliases(aliases, 10000, -1, "macintosh");
			AddAliases(aliases, 10079, -1, "x-mac-icelandic");

			// Korean charsets (aliases for euc-kr)
			// 'upgrade' ks_c_5601-1987 to euc-kr since it is a superset
			AddAliases(aliases, 51949, -1,
				"ks_c_5601-1987",
				"ksc-5601-1987",
				"ksc-5601_1987",
				"ksc-5601",
				"5601",
				"ks_c_5861-1992",
				"ksc-5861-1992",
				"ksc-5861_1992",
				"euckr-0",
				"euc-kr");

			// Chinese charsets (aliases for big5)
			AddAliases(aliases, 950, -1, "big5", "big5-0", "big5-hkscs", "big5.eten-0", "big5hkscs-0");

			// Chinese charsets (aliases for gb2312)
			gb2312 = AddAliases(aliases, 936, -1, "gb2312", "gb-2312", "gb2312-0", "gb2312-80", "gb2312.1980-0");

			// Chinese charsets (euc-cn and gbk not supported on Mono)
			// https://bugzilla.mozilla.org/show_bug.cgi?id=844082 seems to suggest falling back to gb2312.
			AddAliases(aliases, 51936, gb2312, "euc-cn", "gbk-0", "x-gbk", "gbk");

			// Chinese charsets (hz-gb-2312 not suported on Mono)
			AddAliases(aliases, 52936, gb2312, "hz-gb-2312", "hz-gb2312");

			// Chinese charsets (aliases for gb18030)
			AddAliases(aliases, 54936, -1, "gb18030-0", "gb18030");

			// Japanese charsets (aliases for euc-jp)
			AddAliases(aliases, 51932, -1, "eucjp-0", "euc-jp", "ujis-0", "ujis");

			// Japanese charsets (aliases for Shift_JIS)
			AddAliases(aliases, 932, -1, "shift_jis", "jisx0208.1983-0", "jisx0212.1990-0", "pck");

			// Note from http://msdn.microsoft.com/en-us/library/system.text.encoding.getencodings.aspx
			// Encodings 50220 and 50222 are both associated with the name "iso-2022-jp", but they
			// are not identical. Encoding 50220 converts half-width Katakana characters to
			// full-width Katakana characters, whereas encoding 50222 uses a shift-in/shift-out
			// sequence to encode half-width Katakana characters. The display name for encoding
			// 50222 is "Japanese (JIS-Allow 1 byte Kana - SO/SI)" to distinguish it from encoding
			// 50220, which has the display name "Japanese (JIS)".
			//
			// If your application requests the encoding name "iso-2022-jp", the .NET Framework
			// returns encoding 50220. However, the encoding that is appropriate for your application
			// will depend on the preferred treatment of the half-width Katakana characters.
			AddAliases(aliases, 50220, -1, "iso-2022-jp");
		}

		private class InvalidByteCountFallback : DecoderFallback
		{
			private class InvalidByteCountFallbackBuffer : DecoderFallbackBuffer
			{
				private readonly InvalidByteCountFallback fallback;

				private const string replacement = "?";

				private bool invalid;

				private int current;

				public override int Remaining
				{
					get
					{
						if (!invalid)
						{
							return 0;
						}
						return "?".Length - current;
					}
				}

				public InvalidByteCountFallbackBuffer(InvalidByteCountFallback fallback)
				{
					this.fallback = fallback;
				}

				public override bool Fallback(byte[] bytesUnknown, int index)
				{
					fallback.InvalidByteCount++;
					invalid = true;
					current = 0;
					return true;
				}

				public override char GetNextChar()
				{
					if (!invalid)
					{
						return '\0';
					}
					if (current == "?".Length)
					{
						return '\0';
					}
					return "?"[current++];
				}

				public override bool MovePrevious()
				{
					if (current == 0)
					{
						return false;
					}
					current--;
					return true;
				}

				public override void Reset()
				{
					invalid = false;
					current = 0;
					base.Reset();
				}
			}

			public int InvalidByteCount
			{
				get;
				private set;
			}

			public override int MaxCharCount => 1;

			public InvalidByteCountFallback()
			{
				Reset();
			}

			public void Reset()
			{
				InvalidByteCount = 0;
			}

			public override DecoderFallbackBuffer CreateFallbackBuffer()
			{
				return new InvalidByteCountFallbackBuffer(this);
			}
		}

		private static bool ProbeCharset(int codepage)
		{
			try
			{
				Encoding.GetEncoding(codepage);
				return true;
			}
			catch
			{
				return false;
			}
		}

		private static int AddAliases(Dictionary<string, int> dict, int codepage, int fallback, params string[] names)
		{
			int num = ProbeCharset(codepage) ? codepage : fallback;
			for (int i = 0; i < names.Length; i++)
			{
				dict.Add(names[i], num);
			}
			return num;
		}

		public static string GetMimeCharset(Encoding encoding)
		{
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			switch (encoding.CodePage)
			{
				case 932:
					return "iso-2022-jp";
				case 949:
					return "euc-kr";
				case 50220:
					return "iso-2022-jp";
				case 50221:
					return "iso-2022-jp";
				case 50225:
					return "euc-kr";
				default:
					return encoding.WebName.ToLowerInvariant();
			}
		}

		public static string GetMimeCharset(string charset)
		{
			try
			{
				return GetMimeCharset(GetEncoding(charset));
			}
			catch (NotSupportedException)
			{
				return charset;
			}
		}

		private static int ParseIsoCodePage(string charset)
		{
			if (charset.Length < 5)
			{
				return -1;
			}
			int num = charset.IndexOfAny(new char[2]
			{
			'-',
			'_'
			});
			if (num == -1)
			{
				num = charset.Length;
			}
			int result;
			if (!int.TryParse(charset.Substring(0, num), out result))
			{
				return -1;
			}
			if (result == 10646)
			{
				return 1201;
			}
			if (num + 2 > charset.Length)
			{
				return -1;
			}
			string text = charset.Substring(num + 1);
			int result2;
			switch (result)
			{
				case 8859:
					if (!int.TryParse(text, out result2))
					{
						return -1;
					}
					if (result2 <= 0 || (result2 > 9 && result2 < 13) || result2 > 15)
					{
						return -1;
					}
					return result2 + 28590;
				case 2022:
					switch (text.ToLowerInvariant())
					{
						case "jp":
							return 50220;
						case "kr":
							return 50225;
						default:
							return -1;
					}
				default:
					return -1;
			}
		}

		internal static int ParseCodePage(string charset)
		{
			int result;
			if (charset.StartsWith("windows", StringComparison.OrdinalIgnoreCase))
			{
				int num = 7;
				if (num == charset.Length)
				{
					return -1;
				}
				if (charset[num] == '-' || charset[num] == '_')
				{
					if (num + 1 == charset.Length)
					{
						return -1;
					}
					num++;
				}
				if (num + 2 < charset.Length && charset[num] == 'c' && charset[num + 1] == 'p')
				{
					num += 2;
				}
				if (int.TryParse(charset.Substring(num), out result))
				{
					return result;
				}
			}
			else if (charset.StartsWith("ibm", StringComparison.OrdinalIgnoreCase))
			{
				int num = 3;
				if (num == charset.Length)
				{
					return -1;
				}
				if (charset[num] == '-' || charset[num] == '_')
				{
					num++;
				}
				if (int.TryParse(charset.Substring(num), out result))
				{
					return result;
				}
			}
			else if (charset.StartsWith("iso", StringComparison.OrdinalIgnoreCase))
			{
				int num = 3;
				if (num == charset.Length)
				{
					return -1;
				}
				if (charset[num] == '-' || charset[num] == '_')
				{
					num++;
				}
				if ((result = ParseIsoCodePage(charset.Substring(num))) != -1)
				{
					return result;
				}
			}
			else if (charset.StartsWith("cp", StringComparison.OrdinalIgnoreCase))
			{
				int num = 2;
				if (num == charset.Length)
				{
					return -1;
				}
				if (charset[num] == '-' || charset[num] == '_')
				{
					num++;
				}
				if (int.TryParse(charset.Substring(num), out result))
				{
					return result;
				}
			}
			else if (charset.Equals("latin1", StringComparison.OrdinalIgnoreCase))
			{
				return 28591;
			}
			return -1;
		}

		public static int GetCodePage(string charset)
		{
			if (charset == null)
			{
				throw new ArgumentNullException("charset");
			}

			int value;
			lock (aliases)
			{
				if (aliases.TryGetValue(charset, out value))
				{
					return value;
				}
				value = ParseCodePage(charset);
				if (value == -1)
				{
					try
					{
						Encoding encoding = Encoding.GetEncoding(charset);
						value = encoding.CodePage;
						if (!aliases.ContainsKey(encoding.WebName))
						{
							aliases[encoding.WebName] = value;
						}
					}
					catch
					{
						value = -1;
					}
				}
				else
				{
					try
					{
						Encoding encoding = Encoding.GetEncoding(value);
						if (!aliases.ContainsKey(encoding.WebName))
						{
							aliases[encoding.WebName] = value;
						}
					}
					catch
					{
						value = -1;
					}
				}
				if (aliases.ContainsKey(charset))
				{
					return value;
				}
				aliases[charset] = value;
				return value;
			}
		}

		public static Encoding GetEncoding(string charset, string fallback)
		{
			if (charset == null)
			{
				throw new ArgumentNullException("charset");
			}
			if (fallback == null)
			{
				throw new ArgumentNullException("fallback");
			}
			int codePage = GetCodePage(charset);
			if (codePage == -1)
			{
				throw new NotSupportedException($"The '{charset}' encoding is not supported.");
			}
			EncoderReplacementFallback encoderFallback = new EncoderReplacementFallback(fallback);
			DecoderReplacementFallback decoderFallback = new DecoderReplacementFallback(fallback);
			return Encoding.GetEncoding(codePage, encoderFallback, decoderFallback);
		}

		public static Encoding GetEncoding(string charset)
		{
			int codePage;
			if ((codePage = GetCodePage(charset)) == -1)
			{
				throw new NotSupportedException($"The '{charset}' encoding is not supported.");
			}
			try
			{
				return Encoding.GetEncoding(codePage);
			}
			catch (Exception innerException)
			{
				throw new NotSupportedException($"The '{charset}' encoding is not supported.", innerException);
			}
		}

		public static Encoding GetEncoding(int codepage, string fallback)
		{
			if (fallback == null)
			{
				throw new ArgumentNullException("fallback");
			}
			EncoderReplacementFallback encoderFallback = new EncoderReplacementFallback(fallback);
			DecoderReplacementFallback decoderFallback = new DecoderReplacementFallback(fallback);
			return Encoding.GetEncoding(codepage, encoderFallback, decoderFallback);
		}

		public static Encoding GetEncoding(int codepage)
		{
			return Encoding.GetEncoding(codepage);
		}

		internal static char[] ConvertToUnicode(ParserOptions options, byte[] input, int startIndex, int length, out int charCount)
		{
			InvalidByteCountFallback invalidByteCountFallback = new InvalidByteCountFallback();
			Encoding charsetEncoding = options.CharsetEncoding;
			int num = int.MaxValue;
			int num2 = 0;
			char[] array = null;
			int codepage = -1;
			int[] array2 = (charsetEncoding == null || charsetEncoding.CodePage == 65001 || charsetEncoding.CodePage == 28591) ? new int[2]
			{
			65001,
			28591
			} : new int[3]
			{
			65001,
			charsetEncoding.CodePage,
			28591
			};
			Decoder decoder;
			for (int i = 0; i < array2.Length; i++)
			{
				decoder = Encoding.GetEncoding(array2[i], new EncoderReplacementFallback("?"), invalidByteCountFallback).GetDecoder();
				int charCount2 = decoder.GetCharCount(input, startIndex, length, flush: true);
				if (invalidByteCountFallback.InvalidByteCount < num)
				{
					num = invalidByteCountFallback.InvalidByteCount;
					num2 = charCount2;
					codepage = array2[i];
					if (num == 0)
					{
						break;
					}
				}
				invalidByteCountFallback.Reset();
			}
			decoder = GetEncoding(codepage, "?").GetDecoder();
			array = new char[num2];
			charCount = decoder.GetChars(input, startIndex, length, array, 0, flush: true);
			return array;
		}

		public static string ConvertToUnicode(ParserOptions options, byte[] buffer, int startIndex, int length)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (startIndex < 0 || startIndex > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}
			if (length < 0 || length > buffer.Length - startIndex)
			{
				throw new ArgumentOutOfRangeException("length");
			}
			int charCount;
			return new string(ConvertToUnicode(options, buffer, startIndex, length, out charCount), 0, charCount);
		}

		internal static char[] ConvertToUnicode(Encoding encoding, byte[] input, int startIndex, int length, out int charCount)
		{
			Decoder decoder = encoding.GetDecoder();
			char[] array = new char[decoder.GetCharCount(input, startIndex, length, flush: true)];
			charCount = decoder.GetChars(input, startIndex, length, array, 0, flush: true);
			return array;
		}

		internal static char[] ConvertToUnicode(ParserOptions options, int codepage, byte[] input, int startIndex, int length, out int charCount)
		{
			Encoding encoding = null;
			if (codepage != -1)
			{
				try
				{
					encoding = GetEncoding(codepage);
				}
				catch (NotSupportedException)
				{
				}
				catch (ArgumentException)
				{
				}
			}
			if (encoding == null)
			{
				return ConvertToUnicode(options, input, startIndex, length, out charCount);
			}
			return ConvertToUnicode(encoding, input, startIndex, length, out charCount);
		}

		public static string ConvertToUnicode(Encoding encoding, byte[] buffer, int startIndex, int length)
		{
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (startIndex < 0 || startIndex > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}
			if (length < 0 || length > buffer.Length - startIndex)
			{
				throw new ArgumentOutOfRangeException("length");
			}
			int charCount;
			return new string(ConvertToUnicode(encoding, buffer, startIndex, length, out charCount), 0, charCount);
		}
	}
}
