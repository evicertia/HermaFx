using System;
using System.IO;

namespace HermaFx.IO
{
    public class TempFileStream : StreamWrapper
    {
        private string _path;

        public string FullPath => _path;

        public TempFileStream(FileShare sharing)
                : base(CreateInner(sharing))
        {
            _path = (Stream as FileStream).Name;
        }

        public TempFileStream()
            : this(FileShare.None)
		{
		}

        private static Stream CreateInner(FileShare sharing)
        {
            return File.Open(Path.GetTempFileName(), FileMode.OpenOrCreate | FileMode.Truncate, FileAccess.ReadWrite, sharing);
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
