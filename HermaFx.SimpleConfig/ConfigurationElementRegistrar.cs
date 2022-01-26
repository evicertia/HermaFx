using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace HermaFx.SimpleConfig
{
    internal abstract partial class ConfigurationElementRegistrar
    {
        private static readonly IDictionary<Type, int> _alreadyRegistered = new Dictionary<Type, int>();
        private static ConfigurationElementRegistrar _instance = CreateInstance();

        public static ConfigurationElementRegistrar Instance { get { return _instance; } }

        private static ConfigurationElementRegistrar CreateInstance()
        {
            return ReflectionHelpers.RunningOnMono ? new MonoRegistrar() as ConfigurationElementRegistrar : new DotNetRegistrar();
        }

        private static bool ClassAlreadyRegistered(Type type)
        {
            int num = 0;

            lock (_alreadyRegistered)
            {
                if (_alreadyRegistered.ContainsKey(type))
                {
                    num = _alreadyRegistered[type];
                }

                _alreadyRegistered[type] = num++;
            }

            return num > 1;
         }

        protected abstract void Register(ConfigurationElement element, params ConfigurationProperty[] configurationProperties);

        public void Register(ConfigurationElement element, Type @interface)
        {
            var type = element.GetType();

            if (ClassAlreadyRegistered(type))
                return;

            Register(element, new ConfigurationPropertyCollection(@interface, type).ToArray());
        }
    }
}
