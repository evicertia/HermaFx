using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

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
		/// <param name="source">The source.</param>
		/// <param name="initialCapacity">The initial array capacity.</param>
		/// <returns>
		/// A byte array containing the contents of the stream.
		/// </returns>
		/// <remarks>
		/// Specifying an initialCapacity allows reducing the amount of double copying 
		/// and multiple reallocations of internal temporal buffers.
		/// </remarks>
		/// <exception cref="NotSupportedException">The stream does not support reading.</exception>
		/// <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occurs.</exception>
		public static byte[] ReadAllBytes(this Stream source, int initialCapacity)
		{
			Guard.Against<ArgumentOutOfRangeException>(initialCapacity < 0, "initialCapacity");

			using (var ms = new MemoryStream(initialCapacity > 0 ? initialCapacity : _BufferSize))
			{
				int count, total = 0;
				byte[] buffer = new byte[_BufferSize];

				while ((count = source.Read(buffer, 0, buffer.Length)) > 0)
				{
					ms.Write(buffer, 0, count);
					total += count;
				}

				// Try to avoid double copying within memory stream.
				if (ms.GetBuffer().Length == total)
				{
					return ms.GetBuffer();
				}

				return ms.ToArray();
			}
		}

		/// <summary>
		/// Reads the contents of the stream into a byte array.
		/// data is returned as a byte array. An IOException is
		/// thrown if any of the underlying IO calls fail.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <returns>
		/// A byte array containing the contents of the stream.
		/// </returns>
		/// <exception cref="NotSupportedException">The stream does not support reading.</exception>
		/// <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occurs.</exception>
		public static byte[] ReadAllBytes(this Stream source)
		{
			return ReadAllBytes(source, 0);
		}

		/// <summary>
		/// Copy the contents of a stream to a destination.
		/// thrown if it is cancelled during the process.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <exception cref="OperationCanceledException">Occurs in a thread when an operation being executed by that thread is cancelled.</exception>
		public static void CopyTo(this Stream @this, Stream destination, int bufferSize, CancellationToken cancellationToken)
		{
			var buffer = new byte[bufferSize];
			int count;

			while ((count = @this.Read(buffer, 0, buffer.Length)) != 0)
			{
				if (cancellationToken.IsCancellationRequested)
					throw new OperationCanceledException(string.Format("{0}: Operation cancelled... aborting request!", nameof(CopyTo)));

				destination.Write(buffer, 0, count);
			}

			destination.Flush();
		}
	}
}
