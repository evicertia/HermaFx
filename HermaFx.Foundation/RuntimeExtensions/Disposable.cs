﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HermaFx
{
	public class Disposable : IDisposable
	{
		private bool _disposed;
		private readonly Action _disposeAction;

		public Disposable(Action disposeAction)
		{
			_disposeAction = disposeAction;
		}

		public Disposable(Action createAction, Action disposeAction)
		{
			createAction();
			_disposeAction = disposeAction;
		}

		public static Disposable<T> Create<T>(Func<T> createAction, Action<T> disposeAction)
		{
			return new Disposable<T>(createAction, disposeAction);
		}

		public static Disposable<T> Using<T>(T instance, Action<T> disposeAction)
		{
			return new Disposable<T>(() => instance, disposeAction);
		}

		public static Disposable For(Action disposeAction)
		{
			return new Disposable(disposeAction);
		}

		public static Disposable For(Action createAction, Action disposeAction)
		{
			return new Disposable(createAction, disposeAction);
		}


		public void Dispose()
		{
			if (!_disposed && _disposeAction != null)
			{
				_disposed = true;
				_disposeAction();
			}
		}
	}

	public class Disposable<T> : IDisposable
	{
		private bool _disposed;
		private readonly T _instance;
		private readonly Action<T> _disposeAction;

		public T Instance { get { return _instance; } }

		public Disposable(Func<T> createAction, Action<T> disposeAction)
		{
			_instance = createAction();
			_disposeAction = disposeAction;
		}

		public void Dispose()
		{
			if (!_disposed && _disposeAction != null)
			{
				_disposed = true;
				_disposeAction(_instance);
			}
		}
	}
}
