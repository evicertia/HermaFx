using System;

namespace HermaFx
{
	public static class ExceptionExtensions
	{
		/// <summary>
		/// Determines whether [the specified ex] is of type [E]
		/// </summary>
		/// <typeparam name="E"></typeparam>
		/// <param name="ex">The ex.</param>
		/// <param name="recursive">if set to <c>true</c> [recursive].</param>
		/// <returns>
		///   <c>true</c> if [is] [the specified ex]; otherwise, <c>false</c>.
		/// </returns>
		public static bool Is<E>(this Exception ex, bool recursive)
			where E : Exception
		{
			if (ex is E)
				return true;

			if (recursive)
			{
				if (ex.InnerException != null)
					return Is<E>(ex.InnerException, recursive);
			}
			else if (ex.GetBaseException() is E)
			{
				return true;
			}

			return false;
		}
		/// <summary>
		/// Determines whether [the specified ex] is of type [E]
		/// </summary>
		/// <typeparam name="E"></typeparam>
		/// <param name="ex">The ex.</param>
		/// <returns>
		///   <c>true</c> if [is] [the specified ex]; otherwise, <c>false</c>.
		/// </returns>
		public static bool Is<E>(this Exception ex)
			where E : Exception
		{
			return Is<E>(ex, false);
		}
		/// <summary>
		/// Gets the specified ex of type E.
		/// </summary>
		/// <typeparam name="E"></typeparam>
		/// <param name="ex">The ex.</param>
		/// <param name="recursive">if set to <c>true</c> [recursive].</param>
		/// <returns></returns>
		public static E Get<E>(this Exception ex, bool recursive)
			where E : Exception
		{
			if (ex is E)
				return ex as E;

			if (recursive)
			{
				if (ex.InnerException != null)
					return Get<E>(ex.InnerException, recursive);
			}
			else if (ex.GetBaseException() is E)
			{
				return ex.GetBaseException() as E;
			}

			return default(E);
		}
		/// <summary>
		/// Gets the specified ex of type E.
		/// </summary>
		/// <typeparam name="E"></typeparam>
		/// <param name="ex">The ex.</param>
		/// <returns></returns>
		public static E Get<E>(this Exception ex)
			where E : Exception
		{
			return Get<E>(ex, false);
		}
		/// <summary>
		/// Run the specified @delegate avoiding throwing any exception (by catching & ignoring them if any).
		/// </summary>
		/// <param name="delegate">The @delegate.</param>
		public static void Swallow(Action @delegate)
		{
			try
			{
				@delegate();
			}
			catch
			{
			}
		}
	}
}
