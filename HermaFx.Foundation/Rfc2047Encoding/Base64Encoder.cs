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

/// <summary>
/// Incrementally encodes content using the base64 encoding.
/// </summary>
/// <remarks>
/// Base64 is an encoding often used in MIME to encode binary content such
/// as images and other types of multi-media to ensure that the data remains
/// intact when sent via 7bit transports such as SMTP.
/// </remarks>

namespace HermaFx.Rfc2047Encoding
{
	public class Base64Encoder : IMimeEncoder
	{
		private static readonly byte[] base64_alphabet = new byte[64]
		{
		65,
		66,
		67,
		68,
		69,
		70,
		71,
		72,
		73,
		74,
		75,
		76,
		77,
		78,
		79,
		80,
		81,
		82,
		83,
		84,
		85,
		86,
		87,
		88,
		89,
		90,
		97,
		98,
		99,
		100,
		101,
		102,
		103,
		104,
		105,
		106,
		107,
		108,
		109,
		110,
		111,
		112,
		113,
		114,
		115,
		116,
		117,
		118,
		119,
		120,
		121,
		122,
		48,
		49,
		50,
		51,
		52,
		53,
		54,
		55,
		56,
		57,
		43,
		47
		};

		private readonly int quartetsPerLine;

		private readonly bool rfc2047;

		private int quartets;

		private byte saved1;

		private byte saved2;

		private byte saved;

		/// <summary>
		/// Gets the encoding.
		/// </summary>
		/// <remarks>
		/// Gets the encoding that the encoder supports.
		/// </remarks>
		/// <value>The encoding.</value>
		public ContentEncoding Encoding => ContentEncoding.Base64;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:MimeKit.Encodings.Base64Encoder" /> class.
		/// </summary>
		/// <remarks>
		/// Creates a new base64 encoder.
		/// </remarks>
		/// <param name="rfc2047"><c>true</c> if this encoder will be used to encode rfc2047 encoded-word payloads; <c>false</c> otherwise.</param>
		/// <param name="maxLineLength">The maximum number of octets allowed per line (not counting the CRLF). Must be between <c>60</c> and <c>998</c> (inclusive).</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// <paramref name="maxLineLength" /> is not between <c>60</c> and <c>998</c> (inclusive).
		/// </exception>
		internal Base64Encoder(bool rfc2047, int maxLineLength = 72)
		{
			if (maxLineLength < 60 || maxLineLength > 998)
			{
				throw new ArgumentOutOfRangeException("maxLineLength");
			}
			quartetsPerLine = maxLineLength / 4;
			this.rfc2047 = rfc2047;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:MimeKit.Encodings.Base64Encoder" /> class.
		/// </summary>
		/// <remarks>
		/// Creates a new base64 encoder.
		/// </remarks>
		/// <param name="maxLineLength">The maximum number of octets allowed per line (not counting the CRLF). Must be between <c>60</c> and <c>998</c> (inclusive).</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// <paramref name="maxLineLength" /> is not between <c>60</c> and <c>998</c> (inclusive).
		/// </exception>
		public Base64Encoder(int maxLineLength = 72)
			: this(rfc2047: false, maxLineLength: 72)
		{
		}

		/// <summary>
		/// Clone the <see cref="T:MimeKit.Encodings.Base64Encoder" /> with its current state.
		/// </summary>
		/// <remarks>
		/// Creates a new <see cref="T:MimeKit.Encodings.Base64Encoder" /> with exactly the same state as the current encoder.
		/// </remarks>
		/// <returns>A new <see cref="T:MimeKit.Encodings.Base64Encoder" /> with identical state.</returns>
		public IMimeEncoder Clone()
		{
			return new Base64Encoder(rfc2047, quartetsPerLine * 4)
			{
				quartets = quartets,
				saved1 = saved1,
				saved2 = saved2,
				saved = saved
			};
		}

		/// <summary>
		/// Estimates the length of the output.
		/// </summary>
		/// <remarks>
		/// Estimates the number of bytes needed to encode the specified number of input bytes.
		/// </remarks>
		/// <returns>The estimated output length.</returns>
		/// <param name="inputLength">The input length.</param>
		public int EstimateOutputLength(int inputLength)
		{
			if (rfc2047)
			{
				return (inputLength + 2) / 3 * 4;
			}
			int num = quartetsPerLine * 4 + 1;
			int num2 = quartetsPerLine * 3;
			return (inputLength + 2) / num2 * num + num;
		}

		private void ValidateArguments(byte[] input, int startIndex, int length, byte[] output)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (startIndex < 0 || startIndex > input.Length)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}
			if (length < 0 || length > input.Length - startIndex)
			{
				throw new ArgumentOutOfRangeException("length");
			}
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			if (output.Length < EstimateOutputLength(length))
			{
				throw new ArgumentException("The output buffer is not large enough to contain the encoded input.", "output");
			}
		}

		private unsafe int Encode(byte* input, int length, byte* output)
		{
			if (length == 0)
			{
				return 0;
			}
			int num = length;
			byte* ptr = output;
			byte* ptr2 = input;
			if (length + saved > 2)
			{
				byte* ptr3 = ptr2 + length - 2;
				int num2;
				if (saved >= 1)
				{
					num2 = saved1;
				}
				else
				{
					byte* intPtr = ptr2;
					ptr2 = intPtr + 1;
					num2 = *intPtr;
				}
				int num3 = num2;
				int num4;
				if (saved >= 2)
				{
					num4 = saved2;
				}
				else
				{
					byte* intPtr2 = ptr2;
					ptr2 = intPtr2 + 1;
					num4 = *intPtr2;
				}
				int num5 = num4;
				byte* intPtr3 = ptr2;
				ptr2 = intPtr3 + 1;
				int num6 = *intPtr3;
				while (true)
				{
					byte* intPtr4 = ptr;
					ptr = intPtr4 + 1;
					*intPtr4 = base64_alphabet[num3 >> 2];
					byte* intPtr5 = ptr;
					ptr = intPtr5 + 1;
					*intPtr5 = base64_alphabet[(num5 >> 4) | ((num3 & 3) << 4)];
					byte* intPtr6 = ptr;
					ptr = intPtr6 + 1;
					*intPtr6 = base64_alphabet[((num5 & 0xF) << 2) | (num6 >> 6)];
					byte* intPtr7 = ptr;
					ptr = intPtr7 + 1;
					*intPtr7 = base64_alphabet[num6 & 0x3F];
					if (!rfc2047 && ++quartets >= quartetsPerLine)
					{
						byte* intPtr8 = ptr;
						ptr = intPtr8 + 1;
						*intPtr8 = 10;
						quartets = 0;
					}
					if (ptr2 >= ptr3)
					{
						break;
					}
					byte* intPtr9 = ptr2;
					ptr2 = intPtr9 + 1;
					num3 = *intPtr9;
					byte* intPtr10 = ptr2;
					ptr2 = intPtr10 + 1;
					num5 = *intPtr10;
					byte* intPtr11 = ptr2;
					ptr2 = intPtr11 + 1;
					num6 = *intPtr11;
				}
				num = 2 - (int)(ptr2 - ptr3);
				saved = 0;
			}
			if (num > 0)
			{
				if (saved == 0)
				{
					saved = (byte)num;
					byte* intPtr12 = ptr2;
					ptr2 = intPtr12 + 1;
					saved1 = *intPtr12;
					if (num == 2)
					{
						saved2 = *ptr2;
					}
					else
					{
						saved2 = 0;
					}
				}
				else
				{
					byte* intPtr13 = ptr2;
					ptr2 = intPtr13 + 1;
					saved2 = *intPtr13;
					saved = 2;
				}
			}
			return (int)(ptr - output);
		}

		/// <summary>
		/// Encodes the specified input into the output buffer.
		/// </summary>
		/// <remarks>
		/// <para>Encodes the specified input into the output buffer.</para>
		/// <para>The output buffer should be large enough to hold all of the
		/// encoded input. For estimating the size needed for the output buffer,
		/// see <see cref="M:MimeKit.Encodings.Base64Encoder.EstimateOutputLength(System.Int32)" />.</para>
		/// </remarks>
		/// <returns>The number of bytes written to the output buffer.</returns>
		/// <param name="input">The input buffer.</param>
		/// <param name="startIndex">The starting index of the input buffer.</param>
		/// <param name="length">The length of the input buffer.</param>
		/// <param name="output">The output buffer.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <para><paramref name="input" /> is <c>null</c>.</para>
		/// <para>-or-</para>
		/// <para><paramref name="output" /> is <c>null</c>.</para>
		/// </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// <paramref name="startIndex" /> and <paramref name="length" /> do not specify
		/// a valid range in the <paramref name="input" /> byte array.
		/// </exception>
		/// <exception cref="T:System.ArgumentException">
		/// <para><paramref name="output" /> is not large enough to contain the encoded content.</para>
		/// <para>Use the <see cref="M:MimeKit.Encodings.Base64Encoder.EstimateOutputLength(System.Int32)" /> method to properly determine the
		/// necessary length of the <paramref name="output" /> byte array.</para>
		/// </exception>
		public unsafe int Encode(byte[] input, int startIndex, int length, byte[] output)
		{
			ValidateArguments(input, startIndex, length, output);
			fixed (byte* ptr = input)
			{
				fixed (byte* output2 = output)
				{
					return Encode(ptr + startIndex, length, output2);
				}
			}
		}

		private unsafe int Flush(byte* input, int length, byte* output)
		{
			byte* ptr = output;
			if (length > 0)
			{
				ptr += Encode(input, length, output);
			}
			if (saved >= 1)
			{
				int num = saved1;
				int num2 = saved2;
				byte* intPtr = ptr;
				ptr = intPtr + 1;
				*intPtr = base64_alphabet[num >> 2];
				byte* intPtr2 = ptr;
				ptr = intPtr2 + 1;
				*intPtr2 = base64_alphabet[(num2 >> 4) | ((num & 3) << 4)];
				if (saved == 2)
				{
					byte* intPtr3 = ptr;
					ptr = intPtr3 + 1;
					*intPtr3 = base64_alphabet[(num2 & 0xF) << 2];
				}
				else
				{
					byte* intPtr4 = ptr;
					ptr = intPtr4 + 1;
					*intPtr4 = 61;
				}
				byte* intPtr5 = ptr;
				ptr = intPtr5 + 1;
				*intPtr5 = 61;
			}
			if (!rfc2047)
			{
				byte* intPtr6 = ptr;
				ptr = intPtr6 + 1;
				*intPtr6 = 10;
			}
			Reset();
			return (int)(ptr - output);
		}

		/// <summary>
		/// Encodes the specified input into the output buffer, flushing any internal buffer state as well.
		/// </summary>
		/// <remarks>
		/// <para>Encodes the specified input into the output buffer, flusing any internal state as well.</para>
		/// <para>The output buffer should be large enough to hold all of the
		/// encoded input. For estimating the size needed for the output buffer,
		/// see <see cref="M:MimeKit.Encodings.Base64Encoder.EstimateOutputLength(System.Int32)" />.</para>
		/// </remarks>
		/// <returns>The number of bytes written to the output buffer.</returns>
		/// <param name="input">The input buffer.</param>
		/// <param name="startIndex">The starting index of the input buffer.</param>
		/// <param name="length">The length of the input buffer.</param>
		/// <param name="output">The output buffer.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <para><paramref name="input" /> is <c>null</c>.</para>
		/// <para>-or-</para>
		/// <para><paramref name="output" /> is <c>null</c>.</para>
		/// </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// <paramref name="startIndex" /> and <paramref name="length" /> do not specify
		/// a valid range in the <paramref name="input" /> byte array.
		/// </exception>
		/// <exception cref="T:System.ArgumentException">
		/// <para><paramref name="output" /> is not large enough to contain the encoded content.</para>
		/// <para>Use the <see cref="M:MimeKit.Encodings.Base64Encoder.EstimateOutputLength(System.Int32)" /> method to properly determine the
		/// necessary length of the <paramref name="output" /> byte array.</para>
		/// </exception>
		public unsafe int Flush(byte[] input, int startIndex, int length, byte[] output)
		{
			ValidateArguments(input, startIndex, length, output);
			fixed (byte* ptr = input)
			{
				fixed (byte* output2 = output)
				{
					return Flush(ptr + startIndex, length, output2);
				}
			}
		}

		/// <summary>
		/// Resets the encoder.
		/// </summary>
		/// <remarks>
		/// Resets the state of the encoder.
		/// </remarks>
		public void Reset()
		{
			quartets = 0;
			saved1 = 0;
			saved2 = 0;
			saved = 0;
		}
	}
}
