using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;

using Castle.Components.DictionaryAdapter;

namespace HermaFx.Settings
{
	public class SettingsAdapter
	{
		private DictionaryAdapterFactory _factory = new DictionaryAdapterFactory();

		private IDictionary CreateDictionary(NameValueCollection collection)
		{
			var result = new ListDictionary();
			collection.ToDictionary().ForEach(x => result.Add(x.Key, x.Value));
			return result;
		}

		public T Create<T>(NameValueCollection nameValues)
			where T : class
		{
			var dict = CreateDictionary(nameValues);
			var meta = _factory.GetAdapterMeta(typeof(T));
			var attr = typeof(T).GetCustomAttribute<SettingsAttribute>();
			var behavior = new SettingsBehavior(attr?.KeyPrefix, attr?.PrefixSeparator);
			var desc = new PropertyDescriptor(new[] { behavior });
			desc.AddBehavior(behavior);

			return (T)meta.CreateInstance(dict, desc);
		}
	}
}
