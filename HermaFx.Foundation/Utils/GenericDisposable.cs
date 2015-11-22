using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HermaFx.Utils
{
	public class GenericDisposable : IDisposable
	{
		private readonly Action _disposeAction;

		public GenericDisposable(Action disposeAction)
		{
			_disposeAction = disposeAction;
		}

		public GenericDisposable(Action createAction, Action disposeAction)
		{
			createAction();
			_disposeAction = disposeAction;
		}

		public static GenericDisposable<T> Create<T>(Func<T> createAction, Action<T> disposeAction)
		{
			return new GenericDisposable<T>(createAction, disposeAction);
		}

		public static GenericDisposable For(Action createAction, Action disposeAction)
		{
			return new GenericDisposable(createAction, disposeAction);
		}

		public void Dispose()
		{
			if (_disposeAction != null)
				_disposeAction();
		}
	}

	public class GenericDisposable<T> : IDisposable
	{
		private readonly T _instance;
		private readonly Action<T> _disposeAction;

		public T Instance { get { return _instance; } }

		public GenericDisposable(Func<T> createAction, Action<T> disposeAction)
		{
			_instance = createAction();
			_disposeAction = disposeAction;
		}

		public void Dispose()
		{
			if (_disposeAction != null)
			{
				_disposeAction(_instance);
			}
		}
	}
}
