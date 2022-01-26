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

namespace HermaFx.Rfc2047Encoding
{
	internal static class ByteExtensions
	{
		private const string AtomSafeCharacters = "!#$%&'*+-/=?^_`{|}~";

		private const string AttributeSpecials = "*'%";

		private const string CommentSpecials = "()\\\r";

		private const string DomainSpecials = "[]\\\r \t";

		private const string EncodedWordSpecials = "()<>@,;:\"/[]?.=_";

		private const string EncodedPhraseSpecials = "!*+-/=_";

		private const string Specials = "()<>@,;:\\\".[]";

		internal const string TokenSpecials = "()<>@,;:\\\"/[]?=";

		private const string Whitespace = " \t\r\n";

		private static readonly CharType[] table;

		private static void RemoveFlags(string values, CharType bit)
		{
			for (int i = 0; i < values.Length; i++)
			{
				table[(byte)values[i]] &= (CharType)(ushort)(~(uint)bit);
			}
		}

		private static void SetFlags(string values, CharType bit, CharType bitcopy, bool remove)
		{
			if (remove)
			{
				for (int i = 0; i < 128; i++)
				{
					table[i] |= bit;
				}
				for (int i = 0; i < values.Length; i++)
				{
					table[values[i]] &= (CharType)(ushort)(~(uint)bit);
				}
				return;
			}
			for (int i = 0; i < values.Length; i++)
			{
				table[values[i]] |= bit;
			}
			if (bitcopy == CharType.None)
			{
				return;
			}
			for (int i = 0; i < 256; i++)
			{
				if ((table[i] & bitcopy) != 0)
				{
					table[i] |= bit;
				}
			}
		}

		static ByteExtensions()
		{
			table = new CharType[256];
			for (int i = 0; i < 256; i++)
			{
				if (i < 127)
				{
					if (i < 32)
					{
						table[i] |= CharType.IsControl;
					}
					if (i > 32)
					{
						table[i] |= CharType.IsAttrChar;
					}
					if ((i >= 33 && i <= 60) || (i >= 62 && i <= 126) || i == 32)
					{
						table[i] |= (CharType.IsEncodedWordSafe | CharType.IsQuotedPrintableSafe);
					}
					if ((i >= 48 && i <= 57) || (i >= 97 && i <= 122) || (i >= 65 && i <= 90))
					{
						table[i] |= (CharType.IsAtom | CharType.IsEncodedPhraseSafe);
					}
					if ((i >= 48 && i <= 57) || (i >= 97 && i <= 102) || (i >= 65 && i <= 70))
					{
						table[i] |= CharType.IsXDigit;
					}
					table[i] |= CharType.IsAscii;
				}
				else
				{
					if (i == 127)
					{
						table[i] |= CharType.IsAscii;
					}
					else
					{
						table[i] |= CharType.IsAtom;
					}
					table[i] |= CharType.IsControl;
				}
			}
			table[9] |= (CharType.IsBlank | CharType.IsQuotedPrintableSafe);
			table[32] |= (CharType.IsBlank | CharType.IsSpace);
			SetFlags(" \t\r\n", CharType.IsWhitespace, CharType.None, remove: false);
			SetFlags("!#$%&'*+-/=?^_`{|}~", CharType.IsAtom, CharType.None, remove: false);
			SetFlags("()<>@,;:\\\"/[]?=", CharType.IsTokenSpecial, CharType.IsControl, remove: false);
			SetFlags("()<>@,;:\\\".[]", CharType.IsSpecial, CharType.None, remove: false);
			SetFlags("[]\\\r \t", CharType.IsDomainSafe, CharType.None, remove: true);
			RemoveFlags("()<>@,;:\\\".[]", CharType.IsAtom);
			RemoveFlags("()<>@,;:\"/[]?.=_", CharType.IsEncodedWordSafe);
			RemoveFlags("*'%()<>@,;:\\\"/[]?=", CharType.IsAttrChar);
			SetFlags("!*+-/=_", CharType.IsEncodedPhraseSafe, CharType.None, remove: false);
		}

		public static bool IsAsciiAtom(this byte c)
		{
			return (table[c] & CharType.IsAsciiAtom) == CharType.IsAsciiAtom;
		}

		public static bool IsAtom(this byte c)
		{
			return (table[c] & CharType.IsAtom) != CharType.None;
		}

		public static bool IsAttr(this byte c)
		{
			return (table[c] & CharType.IsAttrChar) != CharType.None;
		}

		public static bool IsBlank(this byte c)
		{
			return (table[c] & CharType.IsBlank) != CharType.None;
		}

		public static bool IsCtrl(this byte c)
		{
			return (table[c] & CharType.IsControl) != CharType.None;
		}

		public static bool IsDomain(this byte c)
		{
			return (table[c] & CharType.IsDomainSafe) != CharType.None;
		}

		public static bool IsQpSafe(this byte c)
		{
			return (table[c] & CharType.IsQuotedPrintableSafe) != CharType.None;
		}

		public static bool IsToken(this byte c)
		{
			return (table[c] & (CharType.IsControl | CharType.IsTokenSpecial | CharType.IsWhitespace)) == CharType.None;
		}

		public static bool IsType(this byte c, CharType type)
		{
			return (table[c] & type) != CharType.None;
		}

		public static bool IsWhitespace(this byte c)
		{
			return (table[c] & CharType.IsWhitespace) != CharType.None;
		}

		public static bool IsXDigit(this byte c)
		{
			return (table[c] & CharType.IsXDigit) != CharType.None;
		}

		public static byte ToXDigit(this byte c)
		{
			if (c >= 65)
			{
				if (c >= 97)
				{
					return (byte)(c - 87);
				}
				return (byte)(c - 55);
			}
			return (byte)(c - 48);
		}
	}
}
