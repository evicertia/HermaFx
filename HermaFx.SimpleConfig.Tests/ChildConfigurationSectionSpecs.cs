using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Machine.Specifications;

namespace HermaFx.SimpleConfig.Tests
{
    public class when_declaring_configuration_section_that_has_complex_property
    {
        private Establish context =
            () => Configuration.WithNamingConvention(new _NamingConvention());

        private Because b =
            () => value = Configuration.Get<ISectionWithComplexProperty>();

        private It should_create_complex_property =
            () => value.ComplexProperty.ShouldNotBeNull();

        private It should_read_complex_property_of_complex_property =
            () => value.ComplexProperty.Simple.ShouldNotBeNull();

        private It should_read_simple_values_of_complex_property =
            () => value.ComplexProperty.UriProperty.AbsoluteUri.ShouldEqual("http://google.pl/");

        private It should_have_a_default_instance_for_simple_property =
            () => value.SimpleProperty.ShouldNotBeNull();

        private It should_work_with_nested_properties_with_default_values =
            () => value.SimpleProperty.Dummy.ShouldEqual("something");

        private It should_contain_one_child_item =
            () => value.CollectionProperty.ShouldHaveCount(1);

        private It should_containe_one_child_item_with_value1 =
            () => value.CollectionProperty.First().Value1.ShouldEqual("one");

        private It should_containe_one_child_item_with_default_value2 =
            () => value.CollectionProperty.First().Value2.ShouldEqual("two");

        private It should_have_a_child_within_child_item =
            () => value.CollectionProperty.First().Child.ShouldNotBeNull();

        private It should_have_a_child_with_defaults_within_child_item =
            () => value.CollectionProperty.First().Child.Dummy.ShouldEqual("something");

        private Cleanup clean =
            () => Configuration.WithNamingConvention(new NamingConvention());

        private static ISectionWithComplexProperty value;

        public class _NamingConvention : NamingConvention
        {
            public override string AttributeName(PropertyInfo propertyInfo)
            {
                if (propertyInfo.Name == "SimpleProperty") return "simpleProperty";
                if (propertyInfo.Name == "Value2") return "value2";
                return base.AttributeName(propertyInfo);
            }
        }


    }

    public class  SectionWithComplexProperty : ConfigurationSection<ISectionWithComplexProperty>
    {
        
    }

    public interface ISectionWithComplexProperty
    {
        IComplexConfigSection ComplexProperty { get; set; }
        ISimpleChildSection SimpleProperty { get; set; }
        IEnumerable<ISimpleChildItem> CollectionProperty { get; set; }
    }

    public interface IComplexConfigSection
    {
        IDeclareAppConfiguration Simple { get; set; }
        Uri UriProperty { get; set; }
    }

    public interface ISimpleChildSection
    {
        [Default(DefaultValue = "something")]
        string Dummy { get; set; }
    }

    public interface ISimpleChildItem
    {
        string Value1 { get; set; }

        [Default(DefaultValue = "two")]
        string Value2 { get; set; }

        ISimpleChildSection Child { get; set; }
    }
}
