using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HermaFx
{
	public static class LinqExtensions
	{
		/// <summary>
		/// Converts source to an array containing output of selector delegate.
		/// </summary>
		/// <typeparam name="TSource">The type of the source.</typeparam>
		/// <typeparam name="TResult">The type of the result.</typeparam>
		/// <param name="source">The source.</param>
		/// <param name="selector">The selector.</param>
		/// <returns></returns>
		public static TResult[] ToArray<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
		{
			return source.Select(selector).ToArray();
		}
	}
}
