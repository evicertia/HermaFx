using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using Machine.Specifications;

namespace HermaFx.SimpleConfig.Tests
{
    public class when_reading_configuration_with_validators_that_are_not_met
    {
        private Because b =
            () => exception = Catch.Exception(() => Configuration.Get<ISectionWithValidators>());

        private It should_throw_configuration_error_exception =
            () => exception.ShouldBeOfType<ConfigurationErrorsException>();

        private It should_have_message_with_field_name_in_exception_message =
            () => exception.Message.ShouldContain("ToLong");

        private static Exception exception;
    }

    public interface ISectionWithValidators
    {
        [StringLength(3)]
        string ToLong { get; set; }

        [Range(1, 256), Default(DefaultValue = (ushort)1)]
        ushort NotDeclared { get; set; }

        [Range(1, 256)]
        ushort? NullableNotDeclared { get; set; }
    }

    public class SectionWithValidators : ConfigurationSection<ISectionWithValidators>
    {
    }
}
