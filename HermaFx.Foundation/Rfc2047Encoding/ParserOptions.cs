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
using System.Reflection;
using System.Text;

/// <summary>
/// Parser options as used by <see cref="T:MimeKit.MimeParser" /> as well as various Parse and TryParse methods in MimeKit.
/// </summary>
/// <remarks>
/// <see cref="T:MimeKit.ParserOptions" /> allows you to change and/or override default parsing options used by methods such
/// as <see cref="M:MimeKit.MimeMessage.Load(MimeKit.ParserOptions,System.IO.Stream,System.Threading.CancellationToken)" /> and others.
/// </remarks>

namespace HermaFx.Rfc2047Encoding
{
	public class ParserOptions
	{
		/// <summary>
		/// The default parser options.
		/// </summary>
		/// <remarks>
		/// If a <see cref="T:MimeKit.ParserOptions" /> is not supplied to <see cref="T:MimeKit.MimeParser" /> or other Parse and TryParse
		/// methods throughout MimeKit, <see cref="F:MimeKit.ParserOptions.Default" /> will be used.
		/// </remarks>
		public static readonly ParserOptions Default = new ParserOptions();

		/// <summary>
		/// Gets or sets the compliance mode that should be used when decoding rfc2047 encoded words.
		/// </summary>
		/// <remarks>
		/// In general, you'll probably want this value to be <see cref="F:MimeKit.RfcComplianceMode.Loose" />
		/// (the default) as it allows maximum interoperability with existing (broken) mail clients
		/// and other mail software such as sloppily written perl scripts (aka spambots).
		/// </remarks>
		/// <value>The RFC compliance mode.</value>
		public RfcComplianceMode Rfc2047ComplianceMode
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the charset encoding to use as a fallback for 8bit headers.
		/// </summary>
		/// <remarks>
		/// <see cref="M:MimeKit.Utils.Rfc2047.DecodeText(MimeKit.ParserOptions,System.Byte[])" /> and
		/// <see cref="M:MimeKit.Utils.Rfc2047.DecodePhrase(MimeKit.ParserOptions,System.Byte[])" />
		/// use this charset encoding as a fallback when decoding 8bit text into unicode. The first
		/// charset encoding attempted is UTF-8, followed by this charset encoding, before finally
		/// falling back to iso-8859-1.
		/// </remarks>
		/// <value>The charset encoding.</value>
		public Encoding CharsetEncoding
		{
			get;
			set;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:MimeKit.ParserOptions" /> class.
		/// </summary>
		/// <remarks>
		/// By default, new instances of <see cref="T:MimeKit.ParserOptions" /> enable rfc2047 work-arounds
		/// (which are needed for maximum interoperability with mail software used in the wild)
		/// and do not respect the Content-Length header value.
		/// </remarks>
		public ParserOptions()
		{
			Rfc2047ComplianceMode = RfcComplianceMode.Loose;
			CharsetEncoding = CharsetUtils.UTF8;
		}

		/// <summary>
		/// Clones an instance of <see cref="T:MimeKit.ParserOptions" />.
		/// </summary>
		/// <remarks>
		/// Clones a set of options, allowing you to change a specific option
		/// without requiring you to change the original.
		/// </remarks>
		/// <returns>An identical copy of the current instance.</returns>
		public ParserOptions Clone()
		{
			ParserOptions parserOptions = new ParserOptions();
			parserOptions.Rfc2047ComplianceMode = Rfc2047ComplianceMode;
			parserOptions.CharsetEncoding = CharsetEncoding;
			return parserOptions;
		}
	}
}
