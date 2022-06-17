using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Reflection;

namespace HermaFx.SimpleConfig
{
    internal abstract partial class ConfigurationPropertyFactory
    {
        private class DotNetFactory : ConfigurationPropertyFactory, IConfigurationPropertyFactory
        {
            private static readonly Action<ConfigurationProperty, string> SetAddElementName = null;
            private static readonly Action<ConfigurationProperty, string> SetRemoveElementName = null;
            private static readonly Action<ConfigurationProperty, string> SetClearElementName = null;

            static DotNetFactory()
            {
				SetAddElementName
					= ReflectionHelpers.MakeSetterForPrivateField<ConfigurationProperty, string>("_addElementName")						// NetFX
					?? ReflectionHelpers.MakeSetterForPrivateField<ConfigurationProperty, string>("<AddElementName>k__BackingField");	// .NET5+
                SetRemoveElementName
					= ReflectionHelpers.MakeSetterForPrivateField<ConfigurationProperty, string>("_removeElementName")					// NetFX
					?? ReflectionHelpers.MakeSetterForPrivateField<ConfigurationProperty, string>("<RemoveElementName>k__BackingField");// .NET5+
				SetClearElementName
					= ReflectionHelpers.MakeSetterForPrivateField<ConfigurationProperty, string>("_clearElementName")					// NetFX
					?? ReflectionHelpers.MakeSetterForPrivateField<ConfigurationProperty, string>("<ClearElementName>k__BackingField");	// .NET5+
			}

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

                SetAddElementName(property, namingConvention.AddToCollectionElementName(elementType, propertyName));
                SetRemoveElementName(property, namingConvention.RemoveFromCollectionElementName(elementType, propertyName));
                SetClearElementName(property, namingConvention.ClearCollectionElementName(elementType, propertyName));

                return property;
            }

        }
    }
}
