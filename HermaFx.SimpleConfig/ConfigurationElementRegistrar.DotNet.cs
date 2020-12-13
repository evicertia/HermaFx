using System.Collections;
using System.Configuration;
using System.Linq;
using System.Reflection;

using SC = System.Configuration;

namespace HermaFx.SimpleConfig
{
    internal partial class ConfigurationElementRegistrar
    {
        private class DotNetRegistrar : ConfigurationElementRegistrar
        {
            private static readonly Hashtable PropertyBagAccessor = CreatePropertyBagAccessor();

            private static Hashtable CreatePropertyBagAccessor()
            {
                var propertyBagField = typeof(ConfigurationElement).GetField("s_propertyBags", BindingFlags.Static | BindingFlags.NonPublic);

                var value = (Hashtable)propertyBagField.GetValue(null);
                if (value == null)
                {
                    value = new Hashtable();
                    propertyBagField.SetValue(null, value);
                }
                return value;
            }

            protected override void Register(ConfigurationElement element, params ConfigurationProperty[] configurationProperties)
            {
                var ownerType = element.GetType();
                var properties = (SC.ConfigurationPropertyCollection)PropertyBagAccessor[ownerType];
                if (properties == null)
                {
                    properties = new SC.ConfigurationPropertyCollection();
                    PropertyBagAccessor[ownerType] = properties;
                }
                configurationProperties.ToList().ForEach(x => properties.Add(x));
            }
        }
    }
}
