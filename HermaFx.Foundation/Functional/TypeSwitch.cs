using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HermaFx.Functional
{
	// See: http://stackoverflow.com/questions/298976/c-sharp-is-there-a-better-alternative-than-this-to-switch-on-type
	public static class TypeSwitch
	{
		#region Switch<TSource>
		public static Switch<TSource> On<TSource>(TSource value)
		{
			return new Switch<TSource>(value);
		}

		public class Switch<TSource>
		{
			private TSource value;
			private bool handled = false;

			internal Switch(TSource value)
			{
				this.value = value;
			}

			public Switch<TSource> Case<TTarget>(Action<TTarget> action)
				where TTarget : TSource
			{
				if (!handled)
				{
					var sourceType = value.GetType();
					var targetType = typeof(TTarget);
					if (targetType.IsAssignableFrom(sourceType))
					{
						action((TTarget)value);
						handled = true;
					}
				}

				return this;
			}

			public Switch<TSource, TResult> Returning<TResult>()
			{
				return new Switch<TSource, TResult>(value);
			}

			public void Default(Action<TSource> action)
			{
				if (!handled)
					action(value);
			}
		}
		#endregion

		#region Switch<TSource, TResult>
		public static Switch<TSource, TResult> On<TSource, TResult>(TSource value)
		{
			return new Switch<TSource, TResult>(value);
		}

		public class Switch<TSource, TResult>
		{
			private TSource value;
			private TResult result;
			private bool handled = false;

			public TResult Result { get { return result; } }

			internal Switch(TSource value)
			{
				this.value = value;
			}

			public Switch<TSource, TResult> Case<TTarget>(Func<TTarget, TResult> action)
				where TTarget : TSource
			{
				if (!handled)
				{
					var sourceType = value.GetType();
					var targetType = typeof(TTarget);
					if (targetType.IsAssignableFrom(sourceType))
					{
						result = action((TTarget)value);
						handled = true;
					}
				}

				return this;
			}

			public TResult Default(Func<TSource, TResult> action)
			{
				if (!handled)
					result = action(value);

				return Result;
			}
		}
		#endregion
	}
}
