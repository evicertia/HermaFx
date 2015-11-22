using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HermaFx.Adobe
{
	public enum PdfVersion
	{
		Unknown = 0,
		Pdf1_0,
		Pdf1_1,
		Pdf1_2,
		Pdf1_3,
		Pdf1_4,
		Pdf1_5,
		Pdf1_6,
		Pdf1_7
	}

	public static class PdfValidator
	{
		private static byte[] PDFBEGINBYTES = new byte[] { 0x25, 0x50, 0x44, 0x46, 0x2d }; // %PDF-
		private static byte[] PDF12ENDBYTES1 = new byte[] { 0x25, 0x25, 0x45, 0x4F, 0x46, 0x0D, 0x0A }; // %%EOF{CR}{EOL}
		private static byte[] PDF12ENDBYTES2 = new byte[] { 0x25, 0x25, 0x45, 0x4F, 0x46, 0x0D }; // %%EOF{CR}
		private static byte[] PDF13ENDBYTES1 = new byte[] { 0x25, 0x25, 0x45, 0x4F, 0x46, 0x20, 0x0A }; // %%EOF {EOL}
		private static byte[] PDF14ENDBYTES1 = new byte[] { 0x25, 0x25, 0x45, 0x4F, 0x46, 0x0A }; // %%EOF{EOL}
		private static byte[] PDF14ENDBYTES2 = new byte[] { 0x25, 0x25, 0x45, 0x4F, 0x46 }; // %%EOF
		private static byte[] PDF16ENDBYTES1 = new byte[] { 0x25, 0x25, 0x45, 0x4F, 0x46, 0x0D, 0x0A, 0x0A }; // %%EOF{CR}{NL}{NL}

		private static IEnumerable<byte[]> PDF_EOFS = new[]
		{
			PDF12ENDBYTES1, PDF12ENDBYTES2, PDF13ENDBYTES1, PDF14ENDBYTES1, PDF14ENDBYTES2, PDF16ENDBYTES1
		};

		public static bool IsValid(byte[] bytes, out PdfVersion version, out string errorMessage)
		{
			version = PdfVersion.Unknown;

			if (bytes == null)
				throw new ArgumentNullException("bytes");

			if (bytes.Length < 8)
			{
				errorMessage = "file to short.";
				return false;
			}

			// Check if file begins with '%PDF-'
			if (!bytes.Take(5).SequenceEqual(PDFBEGINBYTES))
			{
				errorMessage = "Missing start-of-file mark.";
				return false;
			}

			if (bytes[5] == 0x31 && bytes[6] == 0x2E && bytes[7] == 0x31) // version is 1.2 ?
			{
				version = PdfVersion.Pdf1_0;
			}
			else if (bytes[5] == 0x31 && bytes[6] == 0x2E && bytes[7] == 0x31) // version is 1.2 ?
			{
				version = PdfVersion.Pdf1_1;
			}
			else if (bytes[5] == 0x31 && bytes[6] == 0x2E && bytes[7] == 0x32) // version is 1.2 ?
			{
				version = PdfVersion.Pdf1_2;

				if (!bytes.Skip(bytes.Length - 7).SequenceEqual(PDF12ENDBYTES1)
					&& !bytes.Skip(bytes.Length - 6).SequenceEqual(PDF12ENDBYTES2))
				{
					errorMessage = "PDF-1.2 missing End-Of-File mark.";
					return false;
				}
			}
			else if (bytes[5] == 0x31 && bytes[6] == 0x2E && bytes[7] == 0x33) // version is 1.3 ?
			{
				version = PdfVersion.Pdf1_3;
			}
			else if (bytes[5] == 0x31 && bytes[6] == 0x2E && bytes[7] == 0x34) // version is 1.4 ?
			{
				version = PdfVersion.Pdf1_4;
			}
			else if (bytes[5] == 0x31 && bytes[6] == 0x2E && bytes[7] == 0x35) // version is 1.4 ?
			{
				version = PdfVersion.Pdf1_5;
			}
			else if (bytes[5] == 0x31 && bytes[6] == 0x2E && bytes[7] == 0x36) // version is 1.4 ?
			{
				version = PdfVersion.Pdf1_6;
			}
			else if (bytes[5] == 0x31 && bytes[6] == 0x2E && bytes[7] == 0x37) // version is 1.4 ?
			{
				version = PdfVersion.Pdf1_7;
			}
			else
			{
				errorMessage = "Unknown or unsupported PDF Version.";
				return false;
			}

			// Some PDF files have NULLs at the end, let's skip them.
			int lenWithOutPadding = bytes.Reverse().SkipWhile(x => x == 0x0).Count();

			if (!PDF_EOFS.Any(x => bytes.Take(lenWithOutPadding).Skip(lenWithOutPadding - x.Length).SequenceEqual(x)))
			{
				errorMessage = "Missing End-Of-File mark.";
				return false;
			}

			errorMessage = null;
			return true;
		}

		public static bool IsValid(byte[] bytes, out PdfVersion version)
		{
			string dummy;

			return IsValid(bytes, out version, out dummy);
		}

		public static bool IsValid(byte[] bytes)
		{
			string dummy;
			PdfVersion ver;

			return IsValid(bytes, out ver, out dummy);
		}
	}
}
