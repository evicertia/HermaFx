using System;
using System.Linq;
using System.Collections.Generic;

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

		/// <summary>
		/// Shuffle the specified source.
		/// </summary>
		/// <returns>The shuffle.</returns>
		/// <param name="source">Source.</param>
		/// <param name="rng">A rng instance to use as randomness source.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng)
		{
			T[] elements = source.ToArray();
			// Note i > 0 to avoid final pointless iteration
			for (int i = elements.Length - 1; i >= 0; i--)
			{
				// Swap element "i" with a random earlier element it (or itself)
				// ... except we don't really need to swap it fully, as we can
				// return it immediately, and afterwards it's irrelevant.
				int swapIndex = rng.Next(i + 1);
				yield return elements[swapIndex];
				elements[swapIndex] = elements[i];
				// we don't actually perform the swap, we can forget about the
				// swapped element because we already returned it.
			}
		}

		/// <summary>
		/// Shuffle the specified source.
		/// </summary>
		/// <returns>The shuffle.</returns>
		/// <param name="source">Source.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
		{
			return source.Shuffle(new Random());
		}

		/// <summary>
		/// Split the elements of a sequence into chunks of size at most <paramref name="size"/>.
		/// Taken from: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Linq/src/System/Linq/Chunk.cs
		/// </summary>
		/// <remarks>
		/// Every chunk except the last will be of size <paramref name="size"/>.
		/// The last chunk will contain the remaining elements and may be of a smaller size.
		/// </remarks>
		/// <param name="source">
		/// An <see cref="IEnumerable{T}"/> whose elements to chunk.
		/// </param>
		/// <param name="size">
		/// Maximum size of each chunk.
		/// </param>
		/// <typeparam name="TSource">
		/// The type of the elements of source.
		/// </typeparam>
		/// <returns>
		/// An <see cref="IEnumerable{T}"/> that contains the elements the input sequence split into chunks of size <paramref name="size"/>.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="source"/> is null.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <paramref name="size"/> is below 1.
		/// </exception>
		public static IEnumerable<T[]> Chunk<T>(this IEnumerable<T> source, int size)
		{
			Guard.IsNotNull(source, nameof(source));
			Guard.Against<ArgumentOutOfRangeException>(size < 1, nameof(size));

			using (IEnumerator<T> e = source.GetEnumerator())
			{
				if (e.MoveNext())
				{
					var chunkBuilder = new List<T>();
					while (true)
					{
						do
						{
							chunkBuilder.Add(e.Current);
						}
						while (chunkBuilder.Count < size && e.MoveNext());

						yield return chunkBuilder.ToArray();

						if (chunkBuilder.Count < size || !e.MoveNext())
						{
							yield break;
						}

						chunkBuilder.Clear();
					}
				}
			}
		}
	}
}
