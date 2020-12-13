using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;

namespace HermaFx.SimpleConfig.Tests
{
    public abstract class observations_for_getting_configuration_section<T> where T : class
    {
        Because of =
            () => section = Configuration.Get<T>();

        protected static T section;
    }

    public abstract class observations_for_getting_configuration_section_with_custom_naming_convention<TSection,TConvention> 
    : observations_for_getting_configuration_section<TSection> 
        where TSection : class
        where TConvention : NamingConvention, new()
    {
        private Establish context =
            () => Configuration.WithNamingConvention(new TConvention());

        private Cleanup after_each = () =>
            Configuration.WithNamingConvention(new NamingConvention());

    }

    public abstract class observations_for_getting_configuration_section_with_custom_naming_convention <T>
        : observations_for_getting_configuration_section_with_custom_naming_convention<T, CustomNamingConverntion> where T : class
    {
        
    }
    
    public class CustomNamingConverntion : NamingConvention
        {
            public override string AddToCollectionElementName(Type collectionElementType, string propertyName)
            {
                if (IsCustomProperty(collectionElementType, propertyName))
                {
                    return "addCustom";    
                }
                return "add";
            }

            private static bool IsCustomProperty(Type collectionElementType, string propertyName)
            {
                return collectionElementType == typeof(IChangedNamesConfiguration)
                       && propertyName == "CustomChildren";
            }

            public override string RemoveFromCollectionElementName(Type collectionElementType, string propertyName)
            {
                if (IsCustomProperty(collectionElementType,propertyName))
                {
                    return "removeCustom";
                }
                return "remove";
            }

            public override string ClearCollectionElementName(Type collectionElementType, string propertyName)
            {
                if (IsCustomProperty(collectionElementType, propertyName))
                {
                    return "clearCustom";
                }
                return "clear";
            }

        }


    public class when_reading_child_config_section_that_is_a_collection
        : observations_for_getting_configuration_section_with_custom_naming_convention<IContainingCollectionConfigSection>
    {
        private It should_read_child_section =
            () => section.Children.ShouldNotBeNull();

        private It should_have_proper_number_of_child_elements =
            () => section.Children.ShouldHaveCount(2);

        private It should_create_empty_collection_for_not_specified_elements =
            () => section.EmptyButNotNull.ShouldNotBeNull();

        private It should_order_child_elements_as_they_appear_in_the_configuration_file =
            () =>
                {
                    section.Children.First().IntProperty.ShouldEqual(1);
                    section.Children.Last().IntProperty.ShouldEqual(2);
                };

    }


    public class when_reading_child_config_section_that_is_a_collection_with_changed_add_remove_clear_names
         : observations_for_getting_configuration_section_with_custom_naming_convention<IContainingCollectionConfigSection>
    {
        It should_read_custom_children_section_properly =
            () => section.CustomChildren.ShouldHaveCount(3);
    }


    public interface IContainingCollectionConfigSection
    {
        IEnumerable<IDeclareAppConfiguration> Children { get; set; }
        IEnumerable<IChangedNamesConfiguration> CustomChildren { get; set; }
        IEnumerable<IDeclareAppConfiguration> EmptyButNotNull { get; set; }
    }

    public interface IChangedNamesConfiguration : IDeclareAppConfiguration
    {
    }

    public class ContainingCollectionConfigSection : ConfigurationSection<IContainingCollectionConfigSection>
    {
    }
}
