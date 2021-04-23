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
			CopyStreamToFullPath(data);
		}
		public TempFile(string path)
		{
			Guard.Against<IOException>(File.Exists(path),
				string.Format("Path {0} provided for TempFile already exists. TempFile cannot be created?!", path));

			var directoryName = Path.GetDirectoryName(path);
			Guard.Against<DirectoryNotFoundException>(!Directory.Exists(directoryName),
				string.Format("Directory provided {0} for creating TempFile does not exists?!", directoryName));

			// XXX: We create empty file to "reserve" path. Then dispose FileStream
			File.Create(path).Dispose();
			this.FullPath = path;
		}

		public TempFile(string path, Stream data)
			: this(path)
		{
			CopyStreamToFullPath(data);
		}

		#region Private Methods
		private void CopyStreamToFullPath(Stream data)
		{
			using (var stream = File.OpenWrite(FullPath))
			{
				data.CopyTo(stream);
				stream.Flush();
			}
		}
		#endregion

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
