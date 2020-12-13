using System;
using System.Configuration;

using HermaFx.SimpleConfig.BasicExtensions;

namespace HermaFx.SimpleConfig
{
    public class ConfigurationSource
    {
        private static readonly CacheCallback<SectionIdentity, object> _cachedConfigs =
            new CacheCallback<SectionIdentity, object>(GetValueForKey);

        public ConfigurationSource()
        {
        }

        private static object GetValueForKey(SectionIdentity sectionIdentity)
        {
            var concreteConfiguration = new ConcreteConfiguration(sectionIdentity.Section);
            return concreteConfiguration.ClientValue(sectionIdentity.Type);
        }

        public object Get(Type type)
        {
            var sectionName = NamingConvention.Current.SectionNameByInterfaceOrClassType(type);
            var section = ConfigurationManager.GetSection(sectionName);

            if (section == null)
            {
                throw new ConfigurationErrorsException("There is no section named {0}".ToFormat(sectionName));
            }
            return _cachedConfigs.Get(new SectionIdentity(sectionName, type, (ConfigurationSectionForInterface)section));
        }

        public TResult Get<TResult>(Type type) where TResult : class
        {
            return (TResult)Get(type);
        }

        public TInterface Get<TInterface>() where TInterface : class
        {
            return (TInterface)Get(typeof(TInterface));
        }

    }
}
