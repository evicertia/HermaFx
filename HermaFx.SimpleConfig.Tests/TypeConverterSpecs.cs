using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Globalization;
using Machine.Specifications;

namespace HermaFx.SimpleConfig.Tests
{
	public class when_declaring_configuration_properties_with_type_converters
	{
		private Because b =
			() => value = Configuration.Get<ISectionWithConverters>();

		private It should_have_enum_array_from_config =
			() => value.EnumArray.ShouldContainOnly(AnEnum.Value2, AnEnum.Value3);

		private It should_have_enum_enumerable_from_config =
			() => value.EnumEnumerable.ShouldContainOnly(AnEnum.Value2, AnEnum.Value3);

		private It should_have_bitmask_enum_from_config =
			() => value.BitmaskEnum.ShouldEqual(BitmaskEnum.Value1 | BitmaskEnum.Value3);

		private It should_have_a_default_enum_array =
			() => value.DefaultEnumArray.ShouldBeEmpty();

		private It should_have_a_default_enum_enumerable =
			() => value.DefaultEnumEnumerable.ShouldBeEmpty();

		private static ISectionWithConverters value;
	}

	public enum AnEnum
	{
		Value1 = 0,
		Value2,
		Value3
	}

	[Flags]
	public enum BitmaskEnum
	{
		Value1 = (1<<0),
		Value2 = (1<<1),
		Value3 = (1<<2)
	}

	public class AnEnumArrayConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string))
			{
				return true;
			}

			return base.CanConvertFrom(context, sourceType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string s = value as string;

			if (!string.IsNullOrWhiteSpace(s))
			{
				// Converts from sightly modified enum member names.
				var values = ((string)value).Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
				values = values.Select(x => x.Replace("Two", "2").Replace("Three", "3")).ToArray();
				return values.Select(x => (AnEnum)Enum.Parse(typeof(AnEnum), x)).ToArray();
			}

			return base.ConvertFrom(context, culture, value);
		}
	}

	public class SectionWithConverters : ConfigurationSection<ISectionWithConverters>
	{

	}

	public interface ISectionWithConverters
	{
		[TypeConverter(typeof(AnEnumArrayConverter))]
		AnEnum[] EnumArray { get; set; }
		[TypeConverter(typeof(AnEnumArrayConverter))]
		IEnumerable<AnEnum> EnumEnumerable { get; set; }

		[TypeConverter(typeof(EnumConverter))]
		BitmaskEnum BitmaskEnum { get; set; }

		AnEnum[] DefaultEnumArray { get; set; }
		IEnumerable<AnEnum> DefaultEnumEnumerable { get; set; }
	}
}
