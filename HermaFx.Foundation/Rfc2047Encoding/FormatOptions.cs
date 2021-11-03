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

/// <summary>
/// Format options for serializing various MimeKit objects.
/// </summary>
/// <remarks>
/// Represents the available options for formatting MIME messages
/// and entities when writing them to a stream.
/// </remarks>

namespace HermaFx.Rfc2047Encoding
{
	public class FormatOptions
	{
		internal const int MaximumLineLength = 998;

		internal const int MinimumLineLength = 60;

		private const int DefaultMaxLineLength = 78;

		private bool allowMixedHeaderCharsets;

		private bool international;

		private int maxLineLength;

		/// <summary>
		/// The default formatting options.
		/// </summary>
		/// <remarks>
		/// If a custom <see cref="T:MimeKit.FormatOptions" /> is not passed to methods such as
		/// <see cref="M:MimeKit.MimeMessage.WriteTo(MimeKit.FormatOptions,System.IO.Stream,System.Threading.CancellationToken)" />,
		/// the default options will be used.
		/// </remarks>
		public static readonly FormatOptions Default;

		/// <summary>
		/// Gets or sets the maximum line length used by the encoders. The encoders
		/// use this value to determine where to place line breaks.
		/// </summary>
		/// <remarks>
		/// Specifies the maximum line length to use when line-wrapping headers.
		/// </remarks>
		/// <value>The maximum line length.</value>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// <paramref name="value" /> is out of range. It must be between 60 and 998.
		/// </exception>
		/// <exception cref="T:System.InvalidOperationException">
		/// <see cref="F:MimeKit.FormatOptions.Default" /> cannot be changed.
		/// </exception>
		public int MaxLineLength
		{
			get
			{
				return maxLineLength;
			}
			set
			{
				if (this == Default)
				{
					throw new InvalidOperationException("The default formatting options cannot be changed.");
				}
				if (value < 60 || value > 998)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				maxLineLength = value;
			}
		}

		/// <summary>
		/// Gets or sets whether the new "Internationalized Email" formatting standards should be used.
		/// </summary>
		/// <remarks>
		/// <para>The new "Internationalized Email" format is defined by
		/// <a href="https://tools.ietf.org/html/rfc6530">rfc6530</a> and
		/// <a href="https://tools.ietf.org/html/rfc6532">rfc6532</a>.</para>
		/// <para>This feature should only be used when formatting messages meant to be sent via
		/// SMTP using the SMTPUTF8 extension (<a href="https://tools.ietf.org/html/rfc6531">rfc6531</a>)
		/// or when appending messages to an IMAP folder via UTF8 APPEND
		/// (<a href="https://tools.ietf.org/html/rfc6855">rfc6855</a>).</para>
		/// </remarks>
		/// <value><c>true</c> if the new internationalized formatting should be used; otherwise, <c>false</c>.</value>
		/// <exception cref="T:System.InvalidOperationException">
		/// <see cref="F:MimeKit.FormatOptions.Default" /> cannot be changed.
		/// </exception>
		public bool International
		{
			get
			{
				return international;
			}
			set
			{
				if (this == Default)
				{
					throw new InvalidOperationException("The default formatting options cannot be changed.");
				}
				international = value;
			}
		}

		/// <summary>
		/// Gets or sets whether the formatter should allow mixed charsets in the headers.
		/// </summary>
		/// <remarks>
		/// <para>When this option is enabled, the MIME formatter will try to use us-ascii and/or
		/// iso-8859-1 to encode headers when appropriate rather than being forced to use the
		/// specified charset for all encoded-word tokens in order to maximize readability.</para>
		/// <para>Unfortunately, mail clients like Outlook and Thunderbird do not treat
		/// encoded-word tokens individually and assume that all tokens are encoded using the
		/// charset declared in the first encoded-word token despite the specification
		/// explicitly stating that each encoded-word token should be treated independently.</para>
		/// <para>The Thunderbird bug can be tracked at
		/// <a href="https://bugzilla.mozilla.org/show_bug.cgi?id=317263">
		/// https://bugzilla.mozilla.org/show_bug.cgi?id=317263</a>.</para>
		/// </remarks>
		/// <value><c>true</c> if the formatter should be allowed to use us-ascii and/or iso-8859-1 when encoding headers; otherwise, <c>false</c>.</value>
		public bool AllowMixedHeaderCharsets
		{
			get
			{
				return allowMixedHeaderCharsets;
			}
			set
			{
				if (this == Default)
				{
					throw new InvalidOperationException("The default formatting options cannot be changed.");
				}
				allowMixedHeaderCharsets = value;
			}
		}

		static FormatOptions()
		{
			Default = new FormatOptions();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:MimeKit.FormatOptions" /> class.
		/// </summary>
		/// <remarks>
		/// Creates a new set of formatting options for use with methods such as
		/// <see cref="M:MimeKit.MimeMessage.WriteTo(System.IO.Stream,System.Threading.CancellationToken)" />.
		/// </remarks>
		public FormatOptions()
		{
			maxLineLength = 78;
			allowMixedHeaderCharsets = false;
			international = false;
		}

		/// <summary>
		/// Clones an instance of <see cref="T:MimeKit.FormatOptions" />.
		/// </summary>
		/// <remarks>
		/// Clones the formatting options.
		/// </remarks>
		/// <returns>An exact copy of the <see cref="T:MimeKit.FormatOptions" />.</returns>
		public FormatOptions Clone()
		{
			return new FormatOptions
			{
				maxLineLength = maxLineLength,
				allowMixedHeaderCharsets = allowMixedHeaderCharsets,
				international = international
			};
		}

		/// <summary>
		/// Get the default formatting options in a thread-safe way.
		/// </summary>
		/// <remarks>
		/// Gets the default formatting options in a thread-safe way.
		/// </remarks>
		/// <returns>The default formatting options.</returns>
		internal static FormatOptions CloneDefault()
		{
			lock (Default)
			{
				return Default.Clone();
			}
		}
	}
}
