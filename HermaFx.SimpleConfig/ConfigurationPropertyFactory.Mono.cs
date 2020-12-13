using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace HermaFx.SimpleConfig
{
    internal abstract partial class ConfigurationPropertyFactory
    {
        private class MonoFactory : ConfigurationPropertyFactory, IConfigurationPropertyFactory
        {
            private static Action<ConfigurationProperty, ConfigurationCollectionAttribute> _collectionAttributesSetter = 
                ReflectionHelpers.MakeSetterForPrivateProperty<ConfigurationProperty, ConfigurationCollectionAttribute>("CollectionAttribute");

            public ConfigurationProperty Collection(PropertyInfo propertyInfo, Type elementType)
            {
                var propertyName = propertyInfo.Name;
                var collectionType = typeof(ConfigurationElementCollectionForInterface<>).MakeGenericType(elementType);
                var namingConvention = NamingConvention.Current;
                var property = new ConfigurationProperty(propertyName,
                                                         collectionType,
                                                         GetDefaultValue(propertyInfo),
                                                         null,
                                                         GetValidator(propertyInfo),
                                                         GetOptions(propertyInfo));

                _collectionAttributesSetter(property, new ConfigurationCollectionAttribute(elementType)
                {
                    AddItemName = namingConvention.AddToCollectionElementName(elementType, propertyName),
                    RemoveItemName = namingConvention.RemoveFromCollectionElementName(elementType, propertyName),
                    ClearItemsName = namingConvention.ClearCollectionElementName(elementType, propertyName)
                });

                return property;
            }
        }
    }
}
