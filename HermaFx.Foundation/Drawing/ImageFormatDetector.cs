using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace HermaFx.Drawing
{
	/// <summary>
	/// Helper class which provides logic to guess the image format of a given stream or byte buffer.
	/// </summary>
	public static class ImageFormatDetector
	{
		public enum ImageFormat
		{
			Unknown = 0,
			Bmp,
			Jpeg,
			Gif,
			Tiff,
			Png
		}

		#region File Magics
		// see http://www.mikekunz.com/image_file_header.html
		private static readonly byte[] bmp = Encoding.ASCII.GetBytes("BM");     // BMP
		private static readonly byte[] gif = Encoding.ASCII.GetBytes("GIF");    // GIF
		private static readonly byte[] png = new byte[] { 137, 80, 78, 71 };    // PNG
		private static readonly byte[] tiff = new byte[] { 73, 73, 42 };         // TIFF
		private static readonly byte[] tiff2 = new byte[] { 77, 77, 42 };         // TIFF
		private static readonly byte[] jpeg = new byte[] { 255, 216, 255, 224 }; // jpeg
		private static readonly byte[] jpeg2 = new byte[] { 255, 216, 255, 225 }; // jpeg canon
		private static readonly IDictionary<ImageFormat, string> ImageFormat2MimeType = new Dictionary<ImageFormat, string>()
		{
			{ ImageFormat.Unknown, "application/octet-stream" },
			{ ImageFormat.Bmp, "image/bmp" },
			{ ImageFormat.Jpeg, "image/jpeg" },
			{ ImageFormat.Gif, "image/gif" },
			{ ImageFormat.Tiff, "image/tiff" },
			{ ImageFormat.Png, "image/png" }
		};
		#endregion

		/// <summary>
		/// Gets the image format.
		/// </summary>
		/// <param name="buffer">The buffer.</param>
		/// <returns></returns>
		public static ImageFormat GetImageFormat(byte[] buffer)
		{
			if (buffer.Length < 4)
				throw new ArgumentException("buffer should be of at least 4 bytes len.");

			if (bmp.SequenceEqual(buffer.Take(bmp.Length)))
				return ImageFormat.Bmp;

			else if (gif.SequenceEqual(buffer.Take(gif.Length)))
				return ImageFormat.Gif;

			else if (png.SequenceEqual(buffer.Take(png.Length)))
				return ImageFormat.Png;

			else if (tiff.SequenceEqual(buffer.Take(tiff.Length)))
				return ImageFormat.Tiff;

			else if (tiff2.SequenceEqual(buffer.Take(tiff2.Length)))
				return ImageFormat.Tiff;

			else if (jpeg.SequenceEqual(buffer.Take(jpeg.Length)))
				return ImageFormat.Jpeg;

			else if (jpeg2.SequenceEqual(buffer.Take(jpeg2.Length)))
				return ImageFormat.Jpeg;

			return ImageFormat.Unknown;
		}
		/// <summary>
		/// Gets the image format.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <returns></returns>
		public static ImageFormat GetImageFormat(Stream stream)
		{
			var buffer = new byte[4];
			stream.Read(buffer, 0, buffer.Length);

			return GetImageFormat(buffer);
		}
		/// <summary>
		/// Gets the mime-type of the byte buffer.
		/// </summary>
		/// <param name="buffer">The buffer.</param>
		/// <returns></returns>
		public static string GetMimeType(byte[] buffer)
		{
			return ImageFormat2MimeType[GetImageFormat(buffer)];
		}
		/// <summary>
		/// Gets the mime-type of the stream.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <returns></returns>
		public static string GetMimeType(Stream stream)
		{
			return ImageFormat2MimeType[GetImageFormat(stream)];
		}
	}
}
