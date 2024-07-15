using System;
using System.Linq;

namespace HermaFx
{
	public static class GenericExtensions
	{
		/// <summary>
		/// Tests wether source is part of the specified list.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">The source.</param>
		/// <param name="list">The list.</param>
		/// <returns></returns>
		public static bool In<T>(this T source, params T[] list)
		{
			if (null == source) throw new ArgumentNullException("source");
			return list.Contains(source);
		}

		/// <summary>
		/// Returns the result of evaluating the lambda expression 
		/// over item, if item is not null.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="U"></typeparam>
		/// <param name="item">The item.</param>
		/// <param name="lambda">The lambda.</param>
		/// <returns></returns>
		public static void IfNotNull<U>(this U item, Action<U> lambda)
			where U : class
		{
			if (item != null) lambda(item);
		}

		/// <summary>
		/// Returns the result of evaluating the lambda expression 
		/// over item, if item is not null.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="U"></typeparam>
		/// <param name="item">The item.</param>
		/// <param name="lambda">The lambda.</param>
		/// <returns></returns>
		public static T IfNotNull<T, U>(this U item, Func<U, T> lambda)
			where U : class
		{
			if (item == null)
			{
				return default(T);
			}
			return lambda(item);
		}

		/// <summary>
		/// Returns the result of evaluating the lambda expression 
		/// over item, if item is not null.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="U"></typeparam>
		/// <param name="item">The item.</param>
		/// <param name="lambda">The lambda.</param>
		/// <returns></returns>
		public static T IfNotNull<T, U>(this U item, Func<U, T> lambda, T @default)
			where U : class
		{
			if (item == null)
			{
				return @default;
			}
			return lambda(item);
		}

		/// <summary>
		/// Throws if null.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">The source.</param>
		/// <returns></returns>
		public static T ThrowIfNull<T>(this T source)
			where T : class
		{
			if (source == null) throw new ArgumentNullException();
			return source;
		}

		/// <summary>
		/// Throws if null.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">The source.</param>
		/// <param name="paramName">Name of the param.</param>
		/// <returns></returns>
		public static T ThrowIfNull<T>(this T source, string paramName)
			where T : class
		{
			if (source == null) throw new ArgumentNullException(paramName);
			return source;
		}

		/// <summary>
		/// Returns the result of evaluating the lambda expression 
		/// over item, if (nullable) item has value.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="U"></typeparam>
		/// <param name="item">The item.</param>
		/// <param name="lambda">The lambda.</param>
		/// <returns></returns>
		public static T IfHasValue<T, U>(this Nullable<U> item, Func<U, T> lambda)
			where U : struct
		{
			if (!item.HasValue)
			{
				return default(T);
			}
			return lambda(item.Value);
		}

		/// <summary>
		/// Throws if source is null.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">The source.</param>
		/// <param name="message">Name of the param.</param>
		/// <returns></returns>
		public static T ThrowIfNull<T>(this T source, string paramName, string message, params object[] args)
			where T : class
		{
			if (source == null)
			{
				message = string.Format(message, args);
				throw new ArgumentNullException(paramName, message);
			}

			return source;
		}
	}
}
