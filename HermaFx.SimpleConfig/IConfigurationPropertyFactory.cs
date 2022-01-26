using System;
using System.Configuration;
using System.Reflection;

namespace HermaFx.SimpleConfig
{
    internal interface IConfigurationPropertyFactory
    {
        //ConfigurationProperty Section(Type type);
        ConfigurationProperty Class(PropertyInfo pi);
        ConfigurationProperty Collection(PropertyInfo propertyInfo, Type elementType);
        ConfigurationProperty Interface(PropertyInfo pi);
        ConfigurationProperty Simple(PropertyInfo pi);
    }
}
