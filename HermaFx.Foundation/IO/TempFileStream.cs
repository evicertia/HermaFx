using System;
using System.IO;

namespace HermaFx.IO
{
    public class TempFileStream : StreamWrapper
    {
        private string _path;

        public string FullPath => _path;

        public TempFileStream(FileShare sharing, FileMode fileMode, FileAccess fileAccess)
                : base(CreateInner(sharing, fileMode, fileAccess))
        {
            _path = (Stream as FileStream).Name;
        }

		public TempFileStream(FileShare sharing, FileMode fileMode)
			: this(sharing, fileMode, FileAccess.ReadWrite)
		{
		}

		public TempFileStream(FileShare sharing, FileAccess fileAccess)
			: this(sharing, FileMode.OpenOrCreate | FileMode.Truncate, fileAccess)
		{
		}

		public TempFileStream(FileMode fileMode, FileAccess fileAccess)
			: this(FileShare.None, fileMode, fileAccess)
		{
		}

		public TempFileStream(FileMode fileMode)
			: this(FileShare.None, fileMode, FileAccess.ReadWrite)
		{
		}

		public TempFileStream(FileAccess fileAccess)
			: this(FileShare.None, FileMode.OpenOrCreate | FileMode.Truncate, fileAccess)
		{
		}

		public TempFileStream(FileShare sharing)
				: base(CreateInner(sharing, FileMode.OpenOrCreate | FileMode.Truncate, FileAccess.ReadWrite))
		{
			_path = (Stream as FileStream).Name;
		}

		public TempFileStream()
            : this(FileShare.None, FileMode.OpenOrCreate | FileMode.Truncate, FileAccess.ReadWrite)
		{
		}

		private static Stream CreateInner(FileShare sharing, FileMode fileMode, FileAccess fileAccess)
        {
            return File.Open(Path.GetTempFileName(), fileMode, fileAccess, sharing);
        }

        public override string ToString()
        {
            return _path;
        }

        public static implicit operator String(TempFileStream file)
        {
            return file._path;
        }

        #region IDisposable Members

        ~TempFileStream() { Dispose(false); }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            ExceptionExtensions.Swallow(() => File.Delete(_path));
            _path = null;
        }

        #endregion
    }
}
