using System;
using System.Configuration;
using System.Linq;
using System.Reflection;

using HermaFx.SimpleConfig.BasicExtensions;

namespace HermaFx.SimpleConfig
{
    internal class ConfigurationElementForInterface<T> : ConfigurationElementForInterface
    {
        public ConfigurationElementForInterface():base(typeof(T))
        {
        }
    }

    internal class ConfigurationElementForInterface : ConfigurationElement, IConfigValue, IBaseValueProvider
    {
        private readonly Type _interfaceType;
        private readonly ClientValueResolver _clientValueResolver;
        private readonly ConfigurationElementRegistrar _registrar = ConfigurationElementRegistrar.Instance;

        protected ConfigurationElementForInterface(Type interfaceType)
        {
            _interfaceType = interfaceType;
            _clientValueResolver = new ClientValueResolver(this, _interfaceType);
        }

        public object Value(PropertyInfo property)
        {
            return _clientValueResolver.ClientValue(property);
        }

        public object this[PropertyInfo property]
        {
            get { return base[NamingConvention.Current.AttributeName(property)]; }
        }

        protected override void Init()
        {
            _registrar.Register(this, _interfaceType);

            base.Init();
        }

        protected override void Reset(ConfigurationElement parentElement)
        {
            // XXX: Avoid infinite loop on mono.
            if (!ReflectionHelpers.RunningOnMono)
                base.Reset(parentElement);
            }
        }
}
