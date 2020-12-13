using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Machine.Specifications;

namespace HermaFx.SimpleConfig.Tests
{
    public class when_getting_nested_collections_with_required_attributes : observations_for_getting_configuration_section_with_custom_naming_convention<IPublishableConcepts, ConceptsNamingConvention>
    {
        private It should_properly_read_child_elements =
            () => section.Concepts.ShouldHaveCount(1);

        private It should_properly_read_required_value =
            () => section.Concepts.First().UniqueId.ShouldEqual("first");

        private It should_properly_read_one_volume =
            () => section.Volumes.ShouldHaveCount(1);

        private It should_properly_read_volume_matcher =
            () => section.Volumes.First().Matcher.ShouldEqual("kot");

    }

    public class ConceptsNamingConvention : NamingConvention
    {
        public override string AddToCollectionElementName(System.Type collectionElementType, string propertyName)
        {
            return collectionElementType.Name.Substring(1);
        }
    }


    public interface IPublishableConcepts
    {
        IEnumerable<IConcept> Concepts { get; set; }
        
        IEnumerable<IVolume> Volumes { get; set; }
    }

    public interface IVolume
    {
        [Required]
        string Matcher { get; set; }
    }

    public interface IConcept
    {
        [Required]
        string UniqueId { get; set; }
    }

    public class PublishableConcepts : ConfigurationSection<IPublishableConcepts> {}
}
