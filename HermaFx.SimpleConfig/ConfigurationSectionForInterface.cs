using System;
using System.Configuration;
using System.Xml;
using System.Linq;
using System.Reflection;

namespace HermaFx.SimpleConfig
{
    public class ConfigurationSectionForInterface : ConfigurationSection, IConfigValue, IBaseValueProvider
    {
        private readonly ClientValueResolver _clientValueResolver;
        private readonly Type _interfaceType;
        private readonly ConfigurationElementRegistrar _registrar = ConfigurationElementRegistrar.Instance;
        private bool _sectionDeserialized = false;

        public ConfigurationSectionForInterface(Type interfaceType)
        {
            _interfaceType = interfaceType;
            _clientValueResolver = new ClientValueResolver(this, InterfaceType);

            // XXX: Mono does not call 'Init' on ConfigurationSection instances. :(
            if (ReflectionHelpers.RunningOnMono) Init();
        }


        public object this[PropertyInfo property]
        {
            get { return base[NamingConvention.Current.AttributeName(property)]; }
        }

        public Type InterfaceType
        {
            get { return _interfaceType; }
        }

        public object Value(PropertyInfo property)
        {
            return _clientValueResolver.ClientValue(property);
        }

        protected override void Init()
        {
            _registrar.Register(this, _interfaceType);

            base.Init();
        }

        protected override void DeserializeSection(XmlReader reader)
        {
            base.DeserializeSection(reader);
            _sectionDeserialized = true;
        }

        protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
        {
            // XXX: Mono does load standalone config files
            //		as a two step process. So during the first
            //		deserializeElement call we should avoid 
            //		processing XmlReader contents.
            if (ReflectionHelpers.RunningOnMono
                && !string.IsNullOrEmpty(SectionInformation.ConfigSource)
                && !_sectionDeserialized)
            {
                return;
            }

            base.DeserializeElement(reader, serializeCollectionKey);

            // XXX: Mono has a bug were it does not correctly
            //        asserts IsRequired on section's attributes.
            if (ReflectionHelpers.RunningOnMono)
            {
                var missingProperties = ElementInformation.Properties
                    .OfType<PropertyInformation>()
                    .Where(x => x.IsRequired && x.Value == null)
                    .Distinct()
                    .Select(x => x.Name);

                if (missingProperties.Any())
                {
                    OnRequiredPropertyNotFound(missingProperties.First());
                }
            }
        }
    }
}
