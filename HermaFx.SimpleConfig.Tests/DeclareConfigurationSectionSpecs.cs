using System;
using System.Configuration;
using Machine.Specifications;

namespace HermaFx.SimpleConfig.Tests
{
    public class when_declaring_configuration_section
    {
        private Because b =
            () => section = ConfigurationManager.GetSection("DeclareAppConfiguration");

        private It should_read_configuration_section_properly =
            () => section.ShouldNotBeNull();
            
        private static object section;
    }


    public class when_getting_simple_value_from_declared_configuration
    {
        private Establish ctx =
            () => config = Configuration.Get<IDeclareAppConfiguration>();

        private It should_return_not_null_configuration_section_value =
            () => config.ShouldNotBeNull();

        private It should_read_int_value_properly =
            () => config.IntProperty.ShouldEqual(3);

        private It should_read_double_value_properly =
            () => config.DoubleProperty.ShouldEqual(5.2D);

        private static IDeclareAppConfiguration config;
    }

    public class DeclareAppConfiguration : ConfigurationSection<IDeclareAppConfiguration>
    {
    }

    public interface IDeclareAppConfiguration
    {
        int IntProperty { get; set; }
        double DoubleProperty { get; set; }
    }
}
