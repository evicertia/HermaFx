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
/// Incrementally encodes content using a variation of the quoted-printable encoding
/// that is specifically meant to be used for rfc2047 encoded-word tokens.
/// </summary>
/// <remarks>
/// The Q-Encoding is an encoding often used in MIME to encode textual content outside
/// of the ASCII range within an rfc2047 encoded-word token in order to ensure that
/// the text remains intact when sent via 7bit transports such as SMTP.
/// </remarks>

namespace HermaFx.Rfc2047Encoding
{
	public class QEncoder : IMimeEncoder
	{
		private static readonly byte[] hex_alphabet = new byte[16]
		{
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
		65,
		66,
		67,
		68,
		69,
		70
		};

		private readonly CharType mask;

		/// <summary>
		/// Gets the encoding.
		/// </summary>
		/// <remarks>
		/// Gets the encoding that the encoder supports.
		/// </remarks>
		/// <value>The encoding.</value>
		public ContentEncoding Encoding => ContentEncoding.QuotedPrintable;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:MimeKit.Encodings.QEncoder" /> class.
		/// </summary>
		/// <remarks>
		/// Creates a new rfc2047 quoted-printable encoder.
		/// </remarks>
		/// <param name="mode">The rfc2047 encoding mode.</param>
		public QEncoder(QEncodeMode mode)
		{
			mask = ((mode == QEncodeMode.Phrase) ? CharType.IsEncodedPhraseSafe : CharType.IsEncodedWordSafe);
		}

		/// <summary>
		/// Clone the <see cref="T:MimeKit.Encodings.QEncoder" /> with its current state.
		/// </summary>
		/// <remarks>
		/// Creates a new <see cref="T:MimeKit.Encodings.QEncoder" /> with exactly the same state as the current encoder.
		/// </remarks>
		/// <returns>A new <see cref="T:MimeKit.Encodings.QEncoder" /> with identical state.</returns>
		public IMimeEncoder Clone()
		{
			return new QEncoder((mask != CharType.IsEncodedPhraseSafe) ? QEncodeMode.Text : QEncodeMode.Phrase);
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
			return inputLength * 3;
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
			byte* ptr = input + length;
			byte* ptr2 = output;
			byte* ptr3 = input;
			while (ptr3 < ptr)
			{
				byte* intPtr = ptr3;
				ptr3 = intPtr + 1;
				byte b = *intPtr;
				switch (b)
				{
					case 32:
						{
							byte* intPtr3 = ptr2;
							ptr2 = intPtr3 + 1;
							*intPtr3 = 95;
							continue;
						}
					default:
						if (b.IsType(mask))
						{
							byte* intPtr2 = ptr2;
							ptr2 = intPtr2 + 1;
							*intPtr2 = b;
							continue;
						}
						break;
					case 95:
						break;
				}
				byte* intPtr4 = ptr2;
				ptr2 = intPtr4 + 1;
				*intPtr4 = 61;
				byte* intPtr5 = ptr2;
				ptr2 = intPtr5 + 1;
				*intPtr5 = hex_alphabet[(b >> 4) & 0xF];
				byte* intPtr6 = ptr2;
				ptr2 = intPtr6 + 1;
				*intPtr6 = hex_alphabet[b & 0xF];
			}
			return (int)(ptr2 - output);
		}

		/// <summary>
		/// Encodes the specified input into the output buffer.
		/// </summary>
		/// <remarks>
		/// <para>Encodes the specified input into the output buffer.</para>
		/// <para>The output buffer should be large enough to hold all of the
		/// encoded input. For estimating the size needed for the output buffer,
		/// see <see cref="M:MimeKit.Encodings.QEncoder.EstimateOutputLength(System.Int32)" />.</para>
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
		/// <para>Use the <see cref="M:MimeKit.Encodings.QEncoder.EstimateOutputLength(System.Int32)" /> method to properly determine the
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
		/// see <see cref="M:MimeKit.Encodings.QEncoder.EstimateOutputLength(System.Int32)" />.</para>
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
		/// <para>Use the <see cref="M:MimeKit.Encodings.QEncoder.EstimateOutputLength(System.Int32)" /> method to properly determine the
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
		}
	}
}
