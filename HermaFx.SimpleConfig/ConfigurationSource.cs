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

        public TInterface Get<TInterface>() where TInterface : class
        {
            var sectionName = NamingConvention.Current.SectionNameByIntefaceOrClassType(typeof(TInterface));            
            var section = ConfigurationManager.GetSection(sectionName);

            if (section == null)
            {
                throw new ConfigurationErrorsException("There is no section named {0}".ToFormat(sectionName));
            }
            return (TInterface)_cachedConfigs.Get(new SectionIdentity(sectionName, typeof(TInterface), 
                (ConfigurationSectionForInterface)section));
        }
    }
}
