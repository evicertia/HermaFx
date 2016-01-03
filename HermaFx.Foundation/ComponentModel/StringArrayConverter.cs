using System;
using System.ComponentModel;
using System.Linq;
using System.Globalization;

namespace HermaFx.ComponentModel
{
	public class StringArrayConverter : ArrayConverter
	{
		public const string DEFAULT_SEPARATOR = ",";
		private string _separator;
		private StringSplitOptions _options;

		public StringArrayConverter(string separator, StringSplitOptions options)
		{
			_separator = separator;
			_options = options;
		}

		public StringArrayConverter()
			: this(DEFAULT_SEPARATOR, StringSplitOptions.None)
		{
		}

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
				return ((string)value).Split(new[] { _separator }, _options);
			}

			return base.ConvertFrom(context, culture, value);
		}
	}

	public class StringArrayConverter<T> : ArrayConverter
	{
		public const string DEFAULT_SEPARATOR = ",";
		private string _separator;
		private StringSplitOptions _options;
		private TypeConverter _converter = TypeDescriptor.GetConverter(typeof(T));

		public StringArrayConverter(string separator, StringSplitOptions options)
		{
			_separator = separator;
			_options = options;
		}

		public StringArrayConverter()
			: this(DEFAULT_SEPARATOR, StringSplitOptions.None)
		{
		}

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
				return ((string)value).Split(new[] { _separator }, _options)
					.Select(x => _converter.ConvertFrom(x))
					.Cast<T>()
					.ToArray();
			}

			return base.ConvertFrom(context, culture, value);
		}
	}
}
