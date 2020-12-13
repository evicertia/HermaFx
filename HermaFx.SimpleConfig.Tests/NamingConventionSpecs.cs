using System;
using Machine.Specifications;

namespace HermaFx.SimpleConfig.Tests
{
    public class when_setting_naming_convention_to_null_value_ 
    {
        It should_throw_invalid_argument_exception =
            () => Catch.Exception(()=> Configuration.WithNamingConvention(null))
            .ShouldBeOfType<ArgumentException>();
    }

    public class when_setting_naming_convention_to_convention_that_returns_null_values 
    {
        Because of =
            () => Configuration.WithNamingConvention(new ReturningNullValuesConvention());

        It should_use_default_convention_value_instead_of_nulls =
            () => NamingConvention.Current.AddToCollectionElementName(null,null)
                    .ShouldEqual("add");

        It should_use_default_convention_value_instead_of_empty_strings =
            () => NamingConvention.Current.RemoveFromCollectionElementName(null,null)
                    .ShouldEqual("remove");

        It should_use_real_convent_value_for_valid_names =
            () => NamingConvention.Current.ClearCollectionElementName(null,null)
                .ShouldEqual("customClearElementName");

        It should_use_default_convention_value_instead_of_whitespace_strings =
            () => NamingConvention.Current.SectionNameByInterfaceType(typeof(IDeclareAppConfiguration))
                    .ShouldEqual("DeclareAppConfiguration");

        private class ReturningNullValuesConvention : NamingConvention
        {
            public override string AddToCollectionElementName(Type collectionElementType, string propertyName)
            {
                return null;
            }

            public override string RemoveFromCollectionElementName(Type collectionElementType, string propertyName)
            {
                return string.Empty;
            }

            public override string ClearCollectionElementName(Type collectionElementType, string propertyName)
            {
                return "customClearElementName";
            }

            public override string SectionNameByInterfaceType(Type interfaceType)
            {
                return "   ";
            }            
        }
    }
}
