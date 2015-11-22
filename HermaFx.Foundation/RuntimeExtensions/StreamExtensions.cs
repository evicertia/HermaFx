using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HermaFx
{
	public static class StreamExtensions
	{
		private const int _BufferSize = 32768;

		/// <summary>
		/// Reads the contents of the stream into a byte array.
		/// data is returned as a byte array. An IOException is
		/// thrown if any of the underlying IO calls fail.
		/// </summary>
		/// <param name="stream">The stream to read.</param>
		/// <returns>A byte array containing the contents of the stream.</returns>
		/// <exception cref="NotSupportedException">The stream does not support reading.</exception>
		/// <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occurs.</exception>
		public static byte[] ReadAllBytes(this Stream source)
		{
#if true
			using (var ms = new MemoryStream())
			{
				int count;
				byte[] buffer = new byte[_BufferSize];
				
				while ((count = source.Read(buffer, 0, buffer.Length)) > 0)
					ms.Write(buffer, 0, count);

				return ms.ToArray();
			}
#else
			// Taken from: http://geekswithblogs.net/sdorman/archive/2009/01/10/reading-all-bytes-from-a-stream.aspx

			long originalPosition = source.Position;
			source.Position = 0;

			try
			{
				byte[] readBuffer = new byte[_BufferSize];

				int totalBytesRead = 0;
				int bytesRead;

				while ((bytesRead = source.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
				{
					totalBytesRead += bytesRead;

					if (totalBytesRead == readBuffer.Length)
					{
						int nextByte = source.ReadByte();
						if (nextByte != -1)
						{
							byte[] temp = new byte[readBuffer.Length * 2];
							Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
							Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
							readBuffer = temp;
							totalBytesRead++;
						}
					}
				}

				byte[] buffer = readBuffer;
				if (readBuffer.Length != totalBytesRead)
				{
					buffer = new byte[totalBytesRead];
					Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
				}
				return buffer;
			}
			finally
			{
				source.Position = originalPosition;
			}
#endif
		}
	}
}
