using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Machine.Specifications;

namespace HermaFx.SimpleConfig.Tests
{
    public class when_reading_configuration_section_defined_as_class
    {
        private Because b =
            () =>
            {
                Configuration.WithNamingConvention(new GetNameFromDisplayConvention());
                section = Configuration.Get<SimpleConfiguration>();
            };

        private It should_return_not_empty_configuration =
            () => section.ShouldNotBeNull();

        private It should_be_able_to_read_properies =
            () =>
                {
                    section.Name.ShouldEqual("SimpleName");
                    section.Count.ShouldEqual(42);
                };

        private It should_be_able_to_read_property_as_simple_type_as_long_as_there_is_a_converter_from_string =
            () => section.Matcher.ToString().ShouldEqual("123abc456");


        private It should_be_able_to_call_simple_properties =
            () => section.NameAndCount.ShouldEqual("SimpleName42");

        private It should_be_able_to_call_simple_virutal_properties =
            () => section.NameAndCountVirtual.ShouldEqual("SimpleName42");

        private It should_read_child_class_configuration_properly =
            () => section.Child.ShouldNotBeNull();

        private It Should_have_enum1_with_default_value =
            () => section.Enum1.ShouldEqual(EnumWithDefault.Default);

        private It Should_have_enum2_with_other_value =
            () => section.Enum2.ShouldEqual(EnumWithDefault.Other);

        private It Should_have_enum3_with_default_value =
            () => section.Enum3.ShouldEqual((EnumWithNoDefault)0);

        private It Should_have_enum4_with_second_value =
            () => section.Enum4.ShouldEqual(EnumWithNoDefault.Second);

        private It Should_have_number_with_value_from_config =
            () => section.Number.ShouldEqual((ushort)6);

        private It Should_have_another_number_with_default_value =
            () => section.AnotherNumber.ShouldEqual((ushort)0);

        private Establish ctx =
            () =>
                {
                    TypeDescriptor.AddAttributes(typeof (Regex),
                                                 new TypeConverterAttribute(typeof (RegexFromStringConverter)));
                };

        private static SimpleConfiguration section;
    }

    internal class RegexFromStringConverter : TypeConverter
    {
        public override bool  CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof (string);
        }

        public override object  ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            return new Regex((string) value);
        }
    }

    public enum EnumWithDefault
    {
        Default = 0,
        Other
    }

    public enum EnumWithNoDefault
    {
        First = 1,
        Second = 2
    }

    public class SimpleConfiguration
    {
        [Display(Name = "name")]
        public virtual string Name { get; set; }
        public virtual int Count { get; set; }

        public virtual SimpleConfiguration Child { get; set; }

        public virtual Regex Matcher { get; set; }

        public virtual EnumWithDefault Enum1 { get; set; }
        [Default(DefaultValue = EnumWithDefault.Other)]
        public virtual EnumWithDefault Enum2 { get; set; }

        public virtual EnumWithNoDefault Enum3 { get; set; }
        [Default(DefaultValue = EnumWithNoDefault.Second)]
        public virtual EnumWithNoDefault Enum4 { get; set; }

        [Display(Name = "number"), Range(1, 256)]
        public virtual ushort Number { get; set; }

        public virtual ushort AnotherNumber { get; set; }

        public string NameAndCount
        {
            get { return Name + Count; }
        }

        public virtual string NameAndCountVirtual
        {
            get { return NameAndCount; }
        }
    }

    public class GetNameFromDisplayConvention : NamingConvention
    {
        public override string AttributeName(PropertyInfo propertyInfo)
        {
            var attr = propertyInfo.GetCustomAttributes(false).OfType<DisplayAttribute>().SingleOrDefault();

            if (attr != null && !string.IsNullOrEmpty(attr.Name))
            {
                return attr.Name;
            }

            return base.AttributeName(propertyInfo);
        }
    }

    public class SimpleConfigurationSection : ConfigurationSection<SimpleConfiguration>
    {
    }
}
