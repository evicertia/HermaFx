using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

//using Common.Logging;

namespace HermaFx.Threading
{
	internal abstract class ReaderWriterLockSlimDisposable : IDisposable
	{
		protected ReaderWriterLockSlim _Locks;


		public ReaderWriterLockSlimDisposable(ReaderWriterLockSlim locks)
		{
			_Locks = locks;
		}


		public abstract void Dispose();
	}

	internal class ReaderWriterLockSlimReadDisposable : ReaderWriterLockSlimDisposable
	{
		//private static readonly global::Common.Logging.ILog _Log = global::Common.Logging.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public ReaderWriterLockSlimReadDisposable(ReaderWriterLockSlim locks)
			: base(locks)
		{
			//if (_Log.IsDebugEnabled) _Log.DebugFormat("Entering.. ({0})", _Locks.GetHashCode());
			_Locks.EnterUpgradeableReadLock();
		}


		public override void Dispose()
		{
			//if (_Log.IsDebugEnabled) _Log.DebugFormat("Exiting.. ({0})", _Locks.GetHashCode());
			_Locks.ExitUpgradeableReadLock();
		}
	}


	internal class ReaderWriterLockSlimReadOnlyDisposable : ReaderWriterLockSlimDisposable
	{
		//private static readonly global::Common.Logging.ILog _Log = global::Common.Logging.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public ReaderWriterLockSlimReadOnlyDisposable(ReaderWriterLockSlim locks)
			: base(locks)
		{
			//if (_Log.IsDebugEnabled) _Log.DebugFormat("Entering.. ({0})", _Locks.GetHashCode());
			_Locks.EnterReadLock();
		}


		public override void Dispose()
		{
			//if (_Log.IsDebugEnabled) _Log.DebugFormat("Exiting.. ({0})", _Locks.GetHashCode());
			_Locks.ExitReadLock();
		}
	}


	internal class ReaderWriterLockSlimWriteDisposable : ReaderWriterLockSlimDisposable
	{
		//private static readonly global::Common.Logging.ILog _Log = global::Common.Logging.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public ReaderWriterLockSlimWriteDisposable(ReaderWriterLockSlim locks)
			: base(locks)
		{
			//if (_Log.IsDebugEnabled) _Log.DebugFormat("Entering.. ({0})", _Locks.GetHashCode());
			_Locks.EnterWriteLock();
		}


		public override void Dispose()
		{
			//if (_Log.IsDebugEnabled) _Log.DebugFormat("Exiting.. ({0})", _Locks.GetHashCode());
			_Locks.ExitWriteLock();
		}
	}

	public static class ReadWriterLockSlimExtensions
	{
		public static IDisposable GetReadOnlyLock(this ReaderWriterLockSlim @lock)
		{
			return new ReaderWriterLockSlimReadOnlyDisposable(@lock);
		}

		public static IDisposable GetReadLock(this ReaderWriterLockSlim @lock)
		{
			return new ReaderWriterLockSlimReadDisposable(@lock);
		}

		public static IDisposable GetWriteLock(this ReaderWriterLockSlim @lock)
		{
			return new ReaderWriterLockSlimWriteDisposable(@lock);
		}

	}
}
