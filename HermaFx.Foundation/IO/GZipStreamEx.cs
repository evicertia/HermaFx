using System;
using System.IO;
using System.IO.Compression;

namespace HermaFx.IO
{
	public class GZipStreamEx : Stream
	{
		#region constructor / cleanup

		public GZipStreamEx(Stream inputStream, uint internalBufferSize, CompressionLevel compressionLevel = CompressionLevel.Fastest)
		{
			try
			{
				_inputStream = inputStream;
				_outputStream = new MemoryStream((int)internalBufferSize);
				_gZipStream = new GZipStream(_outputStream, compressionLevel, true);
			}
			catch
			{
				Cleanup();
				throw;
			}
		}

		private void Cleanup()
		{
			_gZipStream?.Dispose();
			_outputStream?.Dispose();
			_inputStream?.Dispose();
		}

		#endregion

		#region private variables

		private bool _endOfInputStreamReached = false;

		private readonly Stream _inputStream;
		private readonly MemoryStream _outputStream;
		private readonly GZipStream _gZipStream;

		#endregion

		#region stream overrides

		public override bool CanRead => true;

		public override bool CanSeek => false;

		public override bool CanWrite => false;

		public override long Length => 0;

		public override long Position
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		public override void Flush()
		{
			throw new NotSupportedException();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			// 4k buffer size
			var bufLength = count <= 4096 ? count : count / 4;
			var buf = new byte[bufLength];

			while ((_outputStream.Length < count) && !_endOfInputStreamReached)
			{
				var readCount = _inputStream.Read(buf, 0, buf.Length);
				if (readCount == 0)
				{
					_endOfInputStreamReached = true;
					_gZipStream.Flush();
					_gZipStream.Dispose(); // because Flush() does not actually flush...
					break;
				}
				else
				{
					_gZipStream.Write(buf, 0, readCount);
				}
			}

			_outputStream.Position = 0;

			var bytesRead = _outputStream.Read(buffer, offset, count);

			if (_outputStream.Length > bytesRead)
			{
				// First we calculate how many bytes we should move to beggining.
				var _remainingBytes = _outputStream.Length - bytesRead;
				// We copy those remaining bytes to buf byte array.
				_outputStream.Read(buf, 0, (int)_remainingBytes);
				// We truncate outputStream to 0 length and set position to 0
				_outputStream.SetLength(0);
				// Then we copy those bytes to the 0 position for stream
				_outputStream.Write(buf, 0, (int)_remainingBytes);
			}
			else
			{
				// We truncate outputStream to 0 length and set position to 0
				_outputStream.SetLength(0);
			}

			return bytesRead;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
				Cleanup();
		}
		#endregion
	}
}