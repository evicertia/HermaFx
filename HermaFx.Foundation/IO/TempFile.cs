using System;
using System.IO;
using System.Linq;
using System.Text;

namespace HermaFx.IO
{
	public class TempFile : IDisposable
	{
		public string FullPath { get; private set; }

		public TempFile()
		{
			this.FullPath = Path.GetTempFileName();
		}

		public TempFile(Stream data)
			: this()
		{
			using (var stream = File.OpenWrite(FullPath))
			{
				data.CopyTo(stream);
				stream.Flush();
			}
		}

		public override string ToString()
		{
			return FullPath;
		}

		public static implicit operator String(TempFile file)
		{
			return file.FullPath;
		}

		#region IDisposable Members

		~TempFile() { Dispose(false); }
		public void Dispose() { Dispose(true); }
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				GC.SuppressFinalize(this);
			}
			if (FullPath != null)
			{
				try { File.Delete(FullPath); }
				catch { } // best effort
				FullPath = null;
			}
		}


		#endregion
	}
}
