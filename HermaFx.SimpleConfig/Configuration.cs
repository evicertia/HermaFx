using System;

namespace HermaFx.SimpleConfig
{
    public class Configuration
    {        
        public static TInterface Get<TInterface>() where TInterface : class
        {
            return new ConfigurationSource().Get<TInterface>();
        }

        public static void WithNamingConvention(NamingConvention namingConvention)
        {
            if(namingConvention == null)
            {
                throw new ArgumentException("namingConvention must not be null","namingConvention");
            }
            NamingConvention.Current = namingConvention;
        }
    }
}
