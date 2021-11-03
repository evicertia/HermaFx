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
/// Incrementally decodes content encoded with the quoted-printable encoding.
/// </summary>
/// <remarks>
/// Quoted-Printable is an encoding often used in MIME to textual content outside
/// of the ASCII range in order to ensure that the text remains intact when sent
/// via 7bit transports such as SMTP.
/// </remarks>
///

namespace HermaFx.Rfc2047Encoding
{
	public class QuotedPrintableDecoder : IMimeDecoder
	{
		private enum QpDecoderState : byte
		{
			PassThrough,
			EqualSign,
			SoftBreak,
			DecodeByte
		}

		private readonly bool rfc2047;

		private QpDecoderState state;

		private byte saved;

		/// <summary>
		/// Gets the encoding.
		/// </summary>
		/// <remarks>
		/// Gets the encoding that the decoder supports.
		/// </remarks>
		/// <value>The encoding.</value>
		public ContentEncoding Encoding => ContentEncoding.QuotedPrintable;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:MimeKit.Encodings.QuotedPrintableDecoder" /> class.
		/// </summary>
		/// <remarks>
		/// Creates a new quoted-printable decoder.
		/// </remarks>
		/// <param name="rfc2047">
		/// <c>true</c> if this decoder will be used to decode rfc2047 encoded-word payloads; <c>false</c> otherwise.
		/// </param>
		public QuotedPrintableDecoder(bool rfc2047)
		{
			this.rfc2047 = rfc2047;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:MimeKit.Encodings.QuotedPrintableDecoder" /> class.
		/// </summary>
		/// <remarks>
		/// Creates a new quoted-printable decoder.
		/// </remarks>
		public QuotedPrintableDecoder()
			: this(rfc2047: false)
		{
		}

		/// <summary>
		/// Clone the <see cref="T:MimeKit.Encodings.QuotedPrintableDecoder" /> with its current state.
		/// </summary>
		/// <remarks>
		/// Creates a new <see cref="T:MimeKit.Encodings.QuotedPrintableDecoder" /> with exactly the same state as the current decoder.
		/// </remarks>
		/// <returns>A new <see cref="T:MimeKit.Encodings.QuotedPrintableDecoder" /> with identical state.</returns>
		public IMimeDecoder Clone()
		{
			return new QuotedPrintableDecoder(rfc2047)
			{
				state = state,
				saved = saved
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
		/// see <see cref="M:MimeKit.Encodings.QuotedPrintableDecoder.EstimateOutputLength(System.Int32)" />.</para>
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
				switch (state)
				{
					case QpDecoderState.PassThrough:
						while (ptr3 < ptr)
						{
							byte* intPtr6 = ptr3;
							ptr3 = intPtr6 + 1;
							byte b = *intPtr6;
							if (b == 61)
							{
								state = QpDecoderState.EqualSign;
								break;
							}
							if (rfc2047 && b == 95)
							{
								byte* intPtr7 = ptr2;
								ptr2 = intPtr7 + 1;
								*intPtr7 = 32;
							}
							else
							{
								byte* intPtr8 = ptr2;
								ptr2 = intPtr8 + 1;
								*intPtr8 = b;
							}
						}
						break;
					case QpDecoderState.EqualSign:
						{
							byte* intPtr9 = ptr3;
							ptr3 = intPtr9 + 1;
							byte b = *intPtr9;
							if (b.IsXDigit())
							{
								state = QpDecoderState.DecodeByte;
								saved = b;
								break;
							}
							switch (b)
							{
								case 61:
									{
										byte* intPtr12 = ptr2;
										ptr2 = intPtr12 + 1;
										*intPtr12 = 61;
										break;
									}
								case 13:
									state = QpDecoderState.SoftBreak;
									break;
								case 10:
									state = QpDecoderState.PassThrough;
									break;
								default:
									{
										state = QpDecoderState.PassThrough;
										byte* intPtr10 = ptr2;
										ptr2 = intPtr10 + 1;
										*intPtr10 = 61;
										byte* intPtr11 = ptr2;
										ptr2 = intPtr11 + 1;
										*intPtr11 = b;
										break;
									}
							}
							break;
						}
					case QpDecoderState.SoftBreak:
						{
							state = QpDecoderState.PassThrough;
							byte* intPtr13 = ptr3;
							ptr3 = intPtr13 + 1;
							byte b = *intPtr13;
							if (b != 10)
							{
								byte* intPtr14 = ptr2;
								ptr2 = intPtr14 + 1;
								*intPtr14 = 61;
								byte* intPtr15 = ptr2;
								ptr2 = intPtr15 + 1;
								*intPtr15 = 13;
								byte* intPtr16 = ptr2;
								ptr2 = intPtr16 + 1;
								*intPtr16 = b;
							}
							break;
						}
					case QpDecoderState.DecodeByte:
						{
							byte* intPtr = ptr3;
							ptr3 = intPtr + 1;
							byte b = *intPtr;
							if (b.IsXDigit())
							{
								saved = saved.ToXDigit();
								b = b.ToXDigit();
								byte* intPtr2 = ptr2;
								ptr2 = intPtr2 + 1;
								*intPtr2 = (byte)((saved << 4) | b);
							}
							else
							{
								byte* intPtr3 = ptr2;
								ptr2 = intPtr3 + 1;
								*intPtr3 = 61;
								byte* intPtr4 = ptr2;
								ptr2 = intPtr4 + 1;
								*intPtr4 = saved;
								byte* intPtr5 = ptr2;
								ptr2 = intPtr5 + 1;
								*intPtr5 = b;
							}
							state = QpDecoderState.PassThrough;
							break;
						}
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
		/// see <see cref="M:MimeKit.Encodings.QuotedPrintableDecoder.EstimateOutputLength(System.Int32)" />.</para>
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
		/// <para>Use the <see cref="M:MimeKit.Encodings.QuotedPrintableDecoder.EstimateOutputLength(System.Int32)" /> method to properly determine the
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
			state = QpDecoderState.PassThrough;
			saved = 0;
		}
	}
}
