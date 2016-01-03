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
		private Type _converter;

		public StringArrayConverter(string separator, StringSplitOptions options, Type converter)
		{
			_separator = separator;
			_options = options;
			_converter = converter;
		}

		public StringArrayConverter(string separator, StringSplitOptions options)
			: this(separator, options, null)
		{
		}

		public StringArrayConverter(Type converter)
			: this(DEFAULT_SEPARATOR, StringSplitOptions.None)
		{
			_converter = converter;
		}

		public StringArrayConverter()
			: this(DEFAULT_SEPARATOR, StringSplitOptions.None)
		{
		}

		private TypeConverter GetConverter()
		{
			if (_converter != null)
			{
				return (TypeConverter)Activator.CreateInstance(_converter);
			}
				
			return TypeDescriptor.GetConverter(typeof(T));
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
				var converter = GetConverter();
				return ((string)value).Split(new[] { _separator }, _options)
					.Select(x => converter.ConvertFrom(x))
					.Cast<T>()
					.ToArray();
			}

			return base.ConvertFrom(context, culture, value);
		}
	}
}
