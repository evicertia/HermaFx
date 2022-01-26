using System;
using System.Collections;
using System.Configuration;
using System.Linq;
using System.Reflection;

using SC = System.Configuration;

namespace HermaFx.SimpleConfig
{
    internal abstract partial class ConfigurationElementRegistrar
    {
        private class MonoRegistrar : ConfigurationElementRegistrar
        {
            private static readonly Type ElementMapType = ReflectElementMapType();
            private static readonly Hashtable ElementMaps = RetrieveElementMapsHashtable();
            private static readonly Func<Type, object> ElementMapCreator = x => Activator.CreateInstance(ElementMapType, new[] { x });
            private static readonly FieldInfo ElementMapPropertiesAccesor = CreateElementMapPropertiesAccessor();
            private static readonly Func<ConfigurationElement, ElementInformation> ElementInformationConstructor = CreateElementInformationConstructor();
            private static readonly FieldInfo ElementInformationAccessor = CreateElementInformationAccessor();

            private static Type ReflectElementMapType()
            {
                var cet = typeof(ConfigurationElement);
                return cet.Assembly.GetType(cet.Namespace + ".ElementMap");
            }

            private static Hashtable RetrieveElementMapsHashtable()
            {
                return ElementMapType.GetField("elementMaps", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) as Hashtable;
            }

            private static FieldInfo CreateElementMapPropertiesAccessor()
            {
                return ElementMapType.GetField("properties", BindingFlags.Instance | BindingFlags.NonPublic);
            }

            private static Func<ConfigurationElement, ElementInformation> CreateElementInformationConstructor()
            {
                var bflags = BindingFlags.NonPublic | BindingFlags.Instance;
                return x => Activator.CreateInstance(typeof(ElementInformation), bflags, null, new object[] { x, null }, null) as ElementInformation;
            }

            private static FieldInfo CreateElementInformationAccessor()
            {
                return typeof(ConfigurationElement).GetField("elementInfo", BindingFlags.NonPublic | BindingFlags.Instance);
            }

            protected override void Register(ConfigurationElement element, params ConfigurationProperty[] configurationProperties)
            {
                lock (ElementMaps)
                {
                    var ownerType = element.GetType();
                    var map = ElementMaps[ownerType];
                    if (map == null)
                    {
                        map = ElementMaps[ownerType] = ElementMapCreator(ownerType);
                    }
                    var properties = ElementMapPropertiesAccesor.GetValue(map) as SC.ConfigurationPropertyCollection;
                    if (properties == null)
                    {
                        properties = new SC.ConfigurationPropertyCollection();
                    }
                    ElementMapPropertiesAccesor.SetValue(map, properties);

                    configurationProperties.ToList().ForEach(x => properties.Add(x));

                    var einfo = ElementInformationConstructor(element);
                    ElementInformationAccessor.SetValue(element, einfo);
                }
            }
        }
    }
}
