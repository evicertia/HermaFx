using System;

namespace HermaFx.SimpleConfig
{
    public class Configuration
    {        
        public static TInterface Get<TInterface>()
            where TInterface : class
        {
            return new ConfigurationSource().Get<TInterface>();
        }

        public static TResult Get<TResult>(Type type)
            where TResult : class
        {
            return new ConfigurationSource().Get<TResult>(type);
        }

        public static object Get(Type type)
        {
            return new ConfigurationSource().Get(type);
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
