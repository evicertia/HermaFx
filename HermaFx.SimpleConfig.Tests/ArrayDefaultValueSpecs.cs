using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using Machine.Specifications;

using HermaFx.ComponentModel;

namespace HermaFx.SimpleConfig.Tests
{
	// Reproduces HERMA-15243: a TimeSpan[] property using StringArrayConverter<TimeSpan>
	// and a string [DefaultValue] comes back empty when the attribute is omitted.
	[Subject("HERMA-15243")]
	public class when_reading_array_property_with_default_value_not_present_in_config
		: observations_for_getting_configuration_section_with_custom_naming_convention<ISectionWithArrayDefaults, ConceptsNamingConvention>
	{
		private static readonly TimeSpan[] Defaults = new[] {
			TimeSpan.FromSeconds(30),
			TimeSpan.FromMinutes(1),
			TimeSpan.FromMinutes(2),
			TimeSpan.FromMinutes(5),
			TimeSpan.FromMinutes(10),
			TimeSpan.FromMinutes(30),
		};

		private It should_read_both_rules =
			() => section.Rules.ShouldHaveCount(2);

		private It should_use_explicit_value_when_present_in_rule =
			() => section.Rules.Single(x => x.Id == "explicit").BackoffIntervals
				.ShouldEqual(new[] { TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10) });

		private It should_use_default_value_when_omitted_in_rule =
			() => section.Rules.Single(x => x.Id == "omitted").BackoffIntervals.ShouldEqual(Defaults);

		private It should_use_default_value_when_omitted_in_section =
			() => section.BackoffIntervals.ShouldEqual(Defaults);
	}

	public interface ISectionWithArrayDefaults
	{
		[TypeConverter(typeof(StringArrayConverter<TimeSpan>))]
		[DefaultValue("00:00:30,00:01:00,00:02:00,00:05:00,00:10:00,00:30:00")]
		TimeSpan[] BackoffIntervals { get; set; }

		IEnumerable<IRule> Rules { get; set; }
	}

	public interface IRule
	{
		string Id { get; set; }

		[TypeConverter(typeof(StringArrayConverter<TimeSpan>))]
		[DefaultValue("00:00:30,00:01:00,00:02:00,00:05:00,00:10:00,00:30:00")]
		TimeSpan[] BackoffIntervals { get; set; }
	}

	public class SectionWithArrayDefaults : ConfigurationSection<ISectionWithArrayDefaults>
	{
	}
}
