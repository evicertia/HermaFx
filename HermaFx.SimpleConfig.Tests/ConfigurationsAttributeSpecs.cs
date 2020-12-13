using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using Machine.Specifications;

namespace HermaFx.SimpleConfig.Tests
{
    public class when_reading_configuration_with_required_attribute_that_is_not_Present
    {
        private It should_throw_configuration_error_exception =
            () => Catch.Exception(() => Configuration.Get<ISectionWithRequiredAttribute>())
                      .ShouldBeOfType(typeof (ConfigurationErrorsException));
    }

    public class when_reading_configuration_with_default_attribute_setting
    {
        private Because b =
            () => section = Configuration.Get<ISectionWithDefaultAttribute>();

        private It should_use_default_value_if_attribute_is_not_present =
            () => section.DefaultStandardAttribute.ShouldEqual("default attribute");

        private It should_use_default_value_from_custom_attribute_if_not_present =
            () => section.DefaultCustomAttribute.ShouldEqual("custom attribute");

        private It should_use_value_setted_in_config_if_is_provided =
            () => section.SettedInConfig.ShouldEqual("setted in config");

        private static ISectionWithDefaultAttribute section;
    }

    public interface ISectionWithDefaultAttribute
    {
        [TestDefault]
        string DefaultCustomAttribute { get; set; }

        [Default(DefaultValue = "default attribute")]
        string DefaultStandardAttribute { get; set; }

        [DefaultValue("should be overrided")]
        string SettedInConfig { get; set; }
        
    }

    public class TestDefaultAttribute : DefaultAttribute
    {
        public override object Default()
        {
            return "custom attribute";
        }
    }

    public interface ISectionWithRequiredAttribute
    {
        [Required]
        string RequiredButNotPresent { get; set; }
    }

    public class SectionWithRequiredAttribute : ConfigurationSection<ISectionWithRequiredAttribute>
    {
    }

    public class SectionWithDefaultAttribute : ConfigurationSection<ISectionWithDefaultAttribute>
    {
    }
}
