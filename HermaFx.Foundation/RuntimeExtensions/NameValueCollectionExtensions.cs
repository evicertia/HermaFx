using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HermaFx
{
	public static class NameValueCollectionExtensions
	{
		public static IDictionary<string, string> ToDictionary(this NameValueCollection collection)
		{
			return collection.AllKeys.ToDictionary(x => x, x => collection[x]);
		}

		public static IDictionary<string, TValue> ToDictionary<TValue>(this NameValueCollection collection, Func<string, TValue> converter)
		{
			Guard.IsNotNull(converter, nameof(converter));

			return collection.AllKeys.ToDictionary(x => x, x => converter(collection[x]));
		}
	}
}
