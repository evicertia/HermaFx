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
                return
                    new ConcreteConfiguration((IConfigValue)obj).ClientValue(
                        _sourceInterfaceType.GetProperty(property.Name).PropertyType);
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
