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
/// Incrementally decodes content encoded with the base64 encoding.
/// </summary>
/// <remarks>
/// Base64 is an encoding often used in MIME to encode binary content such
/// as images and other types of multi-media to ensure that the data remains
/// intact when sent via 7bit transports such as SMTP.
/// </remarks>

namespace HermaFx.Rfc2047Encoding
{
	public class Base64Decoder : IMimeDecoder
	{
		private static readonly byte[] base64_rank = new byte[256]
		{
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		62,
		255,
		255,
		255,
		63,
		52,
		53,
		54,
		55,
		56,
		57,
		58,
		59,
		60,
		61,
		255,
		255,
		255,
		0,
		255,
		255,
		255,
		0,
		1,
		2,
		3,
		4,
		5,
		6,
		7,
		8,
		9,
		10,
		11,
		12,
		13,
		14,
		15,
		16,
		17,
		18,
		19,
		20,
		21,
		22,
		23,
		24,
		25,
		255,
		255,
		255,
		255,
		255,
		255,
		26,
		27,
		28,
		29,
		30,
		31,
		32,
		33,
		34,
		35,
		36,
		37,
		38,
		39,
		40,
		41,
		42,
		43,
		44,
		45,
		46,
		47,
		48,
		49,
		50,
		51,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255,
		255
		};

		private uint saved;

		private byte bytes;

		private byte npad;

		/// <summary>
		/// Gets the encoding.
		/// </summary>
		/// <remarks>
		/// Gets the encoding that the decoder supports.
		/// </remarks>
		/// <value>The encoding.</value>
		public ContentEncoding Encoding => ContentEncoding.Base64;

		/// <summary>
		/// Clone the <see cref="T:MimeKit.Encodings.Base64Decoder" /> with its current state.
		/// </summary>
		/// <remarks>
		/// Creates a new <see cref="T:MimeKit.Encodings.Base64Decoder" /> with exactly the same state as the current decoder.
		/// </remarks>
		/// <returns>A new <see cref="T:MimeKit.Encodings.Base64Decoder" /> with identical state.</returns>
		public IMimeDecoder Clone()
		{
			return new Base64Decoder
			{
				saved = saved,
				bytes = bytes,
				npad = npad
			};
		}

		/// <summary>
		/// Estimates the length of the output.
		/// </summary>
		/// <remarks>
		/// Estimates the number of bytes needed to decode the specified number of input bytes.
		/// </remarks>
		/// <returns>The estimated output length.</returns>
		/// <param name="inputLength">The input length.</param>
		public int EstimateOutputLength(int inputLength)
		{
			return inputLength + 3;
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
				throw new ArgumentException("The output buffer is not large enough to contain the decoded input.", "output");
			}
		}

		/// <summary>
		/// Decodes the specified input into the output buffer.
		/// </summary>
		/// <remarks>
		/// <para>Decodes the specified input into the output buffer.</para>
		/// <para>The output buffer should be large enough to hold all of the
		/// decoded input. For estimating the size needed for the output buffer,
		/// see <see cref="M:MimeKit.Encodings.Base64Decoder.EstimateOutputLength(System.Int32)" />.</para>
		/// </remarks>
		/// <returns>The number of bytes written to the output buffer.</returns>
		/// <param name="input">A pointer to the beginning of the input buffer.</param>
		/// <param name="length">The length of the input buffer.</param>
		/// <param name="output">A pointer to the beginning of the output buffer.</param>
		public unsafe int Decode(byte* input, int length, byte* output)
		{
			byte* ptr = input + length;
			byte* ptr2 = output;
			byte* ptr3 = input;
			while (ptr3 < ptr)
			{
				byte[] array = base64_rank;
				byte* intPtr = ptr3;
				ptr3 = intPtr + 1;
				byte b = array[*intPtr];
				if (b == byte.MaxValue)
				{
					continue;
				}
				saved = ((saved << 6) | b);
				bytes++;
				if (bytes == 4)
				{
					byte* intPtr2 = ptr2;
					ptr2 = intPtr2 + 1;
					*intPtr2 = (byte)((saved >> 16) & 0xFF);
					byte* intPtr3 = ptr2;
					ptr2 = intPtr3 + 1;
					*intPtr3 = (byte)((saved >> 8) & 0xFF);
					byte* intPtr4 = ptr2;
					ptr2 = intPtr4 + 1;
					*intPtr4 = (byte)(saved & 0xFF);
					saved = 0u;
					bytes = 0;
					if (npad > 0)
					{
						ptr2 -= (int)npad;
						npad = 0;
					}
				}
			}
			int num = 0;
			while (ptr3 > input && num < 2)
			{
				ptr3--;
				if (base64_rank[*ptr3] != byte.MaxValue)
				{
					if (*ptr3 != 61 || ptr2 <= output)
					{
						break;
					}
					if (bytes == 0)
					{
						ptr2--;
					}
					else if (npad < 2)
					{
						npad++;
					}
					num++;
				}
			}
			return (int)(ptr2 - output);
		}

		/// <summary>
		/// Decodes the specified input into the output buffer.
		/// </summary>
		/// <remarks>
		/// <para>Decodes the specified input into the output buffer.</para>
		/// <para>The output buffer should be large enough to hold all of the
		/// decoded input. For estimating the size needed for the output buffer,
		/// see <see cref="M:MimeKit.Encodings.Base64Decoder.EstimateOutputLength(System.Int32)" />.</para>
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
		/// <para>Use the <see cref="M:MimeKit.Encodings.Base64Decoder.EstimateOutputLength(System.Int32)" /> method to properly determine the
		/// necessary length of the <paramref name="output" /> byte array.</para>
		/// </exception>
		public unsafe int Decode(byte[] input, int startIndex, int length, byte[] output)
		{
			ValidateArguments(input, startIndex, length, output);
			fixed (byte* ptr = input)
			{
				fixed (byte* output2 = output)
				{
					return Decode(ptr + startIndex, length, output2);
				}
			}
		}

		/// <summary>
		/// Resets the decoder.
		/// </summary>
		/// <remarks>
		/// Resets the state of the decoder.
		/// </remarks>
		public void Reset()
		{
			saved = 0u;
			bytes = 0;
			npad = 0;
		}
	}
}
