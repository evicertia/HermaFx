using System;
using System.Reflection;

namespace HermaFx.SimpleConfig
{
    internal class ClientValueResolver
    {
        private readonly CacheCallback<PropertyInfo,object> _cachedValues;
        private readonly IBaseValueProvider _source;
        private readonly Type _sourceInterfaceType;

        public ClientValueResolver(IBaseValueProvider source, Type sourceInterfaceType)
        {
            _cachedValues = new CacheCallback<PropertyInfo, object>(ClientValueCreate);
            _source = source;
            _sourceInterfaceType = sourceInterfaceType;
        }

        public object ClientValue(PropertyInfo property)
        {
            return _cachedValues.Get(property);
        }

        private object ClientValueCreate(PropertyInfo property)
        {
            var obj = _source[property];
            if (obj is IConfigValue)
            {
                var accessor = _sourceInterfaceType.GetProperty(property.Name);
                if (accessor == null) //< ie. IEnumerable<> defined on impl interface?
                {
                    foreach (var iface in _sourceInterfaceType.GetInterfaces())
                    {
                        accessor = iface.GetProperty(property.Name);
                        if (accessor != null) break;
                    }

                    if (accessor == null)
                        throw new ArgumentOutOfRangeException($"Could not load accessor for property {property.Name}");
                }

                return new ConcreteConfiguration((IConfigValue)obj).ClientValue(accessor.PropertyType);
            }
            else if (obj == null && property.PropertyType.IsValueType)
            {
                return Activator.CreateInstance(property.PropertyType);
            }
            else if (obj == null && property.IsGenericIEnumerable())
            {
                return Array.CreateInstance(property.PropertyType.GetGenericArguments()[0], 0);
            }
            return obj;
        }
    }
}
