using System;
using System.Linq;
using System.Collections.Generic;

namespace HermaFx
{
	public static class IDictionaryExtensions
	{
		public static IDictionary<K, V> Clone<K,V>(this IDictionary<K, V> source)
		{
			if (source == null) throw new ArgumentNullException("source");

			return source.ToDictionary(x => x.Key, x => x.Value);
		}

		public static V GetValueOrDefault<K, V>(this IDictionary<K, V> source, K key, V @default)
		{
			if (source == null) throw new ArgumentNullException("source");

			return source.ContainsKey(key) ? source[key] : @default;
		}

		public static V GetValueOrDefault<K, V>(this IDictionary<K, V> source, K key)
		{
			return source.GetValueOrDefault(key, default(V));
		}

		public static V GetValueOrDefault<K, V>(this IReadOnlyDictionary<K, V> source, K key, V @default)
		{
			if (source == null) throw new ArgumentNullException("source");

			return source.ContainsKey(key) ? source[key] : @default;
		}

		public static V GetValueOrDefault<K, V>(this IReadOnlyDictionary<K, V> source, K key)
		{
			return source.GetValueOrDefault(key, default(V));
		}

		public static void Add<K, V>(this IDictionary<K, V> source, IDictionary<K, V> dictionary)
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source));

			if (dictionary == null)
				throw new ArgumentNullException(nameof(dictionary));

			foreach (var item in dictionary)
			{
				if (source.ContainsKey(item.Key))
					throw new ArgumentException("Source already contains the key {0}.".Format((object)item.Key));

				source.Add(item.Key, item.Value);
			}
		}
	}
}