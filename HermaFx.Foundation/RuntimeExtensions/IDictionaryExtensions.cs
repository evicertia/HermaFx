using System;
using System.Collections.Generic;
using System.Linq;

namespace HermaFx
{
	public static class IDictionaryExtensions
	{
		private static object GetDefault(Type type)
		{
			if (type.IsValueType)
			{
				return Activator.CreateInstance(type);
			}
			return null;
		}

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
	}
}
