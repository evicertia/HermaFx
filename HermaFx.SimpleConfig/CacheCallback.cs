using System;

namespace HermaFx.SimpleConfig
{
    internal class CacheCallback<TKey,TValue>
        where TValue : class 
    {
        private readonly Func<TKey, TValue> _valueResolver;
        private readonly Cache<TKey, TValue> _cache = new Cache<TKey, TValue>();

        public CacheCallback(Func<TKey,TValue> valueResolver)
        {
            _valueResolver = valueResolver;
        }

        public TValue Get(TKey key)
        {
            TValue value = _cache.Get(key);
            if(value == null)
            {
                value = _valueResolver(key);
                _cache.Insert(key, value, CacheStrategy.Permanent);
            }
            return value;
        }
    }
}
