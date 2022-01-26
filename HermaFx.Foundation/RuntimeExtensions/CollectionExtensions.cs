using System;
using System.Collections;

namespace HermaFx
{
	public static class CollectionExtensions
	{
		/// <summary>
		/// Throws if source is null.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">The source.</param>
		/// <param name="message">Name of the param.</param>
		/// <returns></returns>
		public static T ThrowIfEmpty<T>(this T source, string paramName, string message, params object[] args)
			where T : ICollection
		{
			if (source != null && source.Count == 0)
			{
				message = string.Format(message, args);
				throw new ArgumentException(paramName, message);
			}

			return source;
		}
	}
}