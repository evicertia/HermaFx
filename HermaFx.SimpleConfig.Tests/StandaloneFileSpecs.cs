using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Machine.Specifications;


namespace HermaFx.SimpleConfig.Tests
{
	[Tags("standalone_file")]
	public class when_reading_configuration_section_from_standalone_file
	{
		private Because b =
			() =>
			{
				Configuration.WithNamingConvention(new GetNameFromDisplayConvention());
				section = Configuration.Get<StandaloneConfiguration>();
			};

		private It should_not_return_empty_configuration =
			() => section.ShouldNotBeNull();

		private It should_be_able_to_read_properies =
			() =>
			{
				section.Id.ShouldEqual("Main");
				section.Value.ShouldEqual(42);
			};
		private static StandaloneConfiguration section;
	}

	public class StandaloneConfiguration
	{
		[Display(Name = "id"), Required]
		public virtual string Id { get; set; }
		public virtual int Value { get; set; }
	}

	public class StandaloneConfigurationSection : ConfigurationSection<StandaloneConfiguration>
	{
	}
}
