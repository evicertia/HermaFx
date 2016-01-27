using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HermaFx
{
	public static class IEnumerableExtensions
	{
		/// <summary>
		/// Call action delegate for each member of col.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="col">The col.</param>
		/// <param name="action">The action.</param>
		public static void ForEach<T>(this IEnumerable<T> col, Action<T> action)
		{
			if (col == null) throw new ArgumentNullException("col");
			if (action == null) throw new ArgumentNullException("action");

			foreach (var item in col)
			{
				action(item);
			}
		}

		/// <summary>
		/// Call action delegate for each member of col.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="col">The col.</param>
		/// <param name="action">The action.</param>
		public static void ForEachIfNotNull<T>(this IEnumerable<T> col, Action<T> action)
		{
			if (action == null) throw new ArgumentNullException("action");

			if (col == null) return;

			foreach (var item in col)
			{
				action(item);
			}
		}

		/// <summary>
		/// Call action delegate for each member of col, passing the index of the current entry on col.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="col">The col.</param>
		/// <param name="action">The action.</param>
		public static int ForEach<T>(this IEnumerable<T> list, Action<int, T> action)
		{
			if (list == null) throw new ArgumentNullException("list");
			if (action == null) throw new ArgumentNullException("action");

			var index = 0;

			foreach (var elem in list)
				action(index++, elem);

			return index;
		}

		/// <summary>
		/// Call action delegate for each member of col, passing the index of the current entry on col.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="col">The col.</param>
		/// <param name="action">The action.</param>
		public static int ForEachIfNotNull<T>(this IEnumerable<T> list, Action<int, T> action)
		{
			if (action == null) throw new ArgumentNullException("action");

			if (list == null) return 0;

			var index = 0;

			foreach (var elem in list)
				action(index++, elem);

			return index;
		}

		/// <summary>
		/// Determines whether [is null or empty] [the specified list].
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list">The list.</param>
		/// <returns>
		/// 	<c>true</c> if [is null or empty] [the specified list]; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsNullOrEmpty<T>(this IEnumerable<T> list)
		{
			if (list == null) return true;

			return !list.Any();
		}

		/// <summary>
		/// Returns an empty enumeration if the <paramref name="source"/> is null. 
		/// Otherwise, returns the <paramref name="source"/>.
		/// </summary>
		/// <nuget id="netfx-System.Collections.Generic.IEnumerable.EmptyIfNull" />
		/// <param name="source" this="true">The enumerable to check if it's null</param>
		public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> source)
		{
			if (source == null)
				return Enumerable.Empty<T>();

			return source;
		}
	}
}
