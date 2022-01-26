using System;
using System.IO;

// Originally taken from: https://github.com/peteraritchie/StreamWrapper/blob/master/StreamWrapper/StreamWrapper.cs

namespace HermaFx.IO
{
	public class StreamWrapper : Stream
	{
		protected readonly Stream Stream;

		public StreamWrapper(Stream stream)
		{
			Stream = stream;
		}

		public override void Flush()
		{
			Stream.Flush();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return Stream.Seek(offset, origin);
		}

		public override void SetLength(long value)
		{
			Stream.SetLength(value);
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			return Stream.Read(buffer, offset, count);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			Stream.Write(buffer, offset, count);
		}

		public override bool CanRead
		{
			get { return Stream.CanRead; }
		}

		public override bool CanSeek
		{
			get { return Stream.CanSeek; }
		}

		public override bool CanWrite
		{
			get { return Stream.CanWrite; }
		}

		public override long Length
		{
			get { return Stream.Length; }
		}

		public override long Position
		{
			get { return Stream.Position; }
			set { Stream.Position = value; }
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			Stream.Dispose();
		}
	}
}