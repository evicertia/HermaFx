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
				return ((string)value).Split(new[] { _separator }, _options).Select(x => x?.Trim()).ToArray();
			}

			return base.ConvertFrom(context, culture, value);
		}
	}

	public class StringArrayConverter<T> : ArrayConverter
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

		protected virtual TypeConverter GetConverter()
		{
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
					.Select(x => x?.Trim())
					.Select(x => converter.ConvertFrom(x))
					.Cast<T>()
					.ToArray();
			}

			return base.ConvertFrom(context, culture, value);
		}
	}

	public class StringArrayConverter<T, C> : StringArrayConverter<T>
		where C : TypeConverter
	{
		public StringArrayConverter(string separator, StringSplitOptions options)
			: base(separator, options)
		{
		}

		public StringArrayConverter()
			: this(DEFAULT_SEPARATOR, StringSplitOptions.None)
		{
		}

		protected override TypeConverter GetConverter()
		{
			var converter = typeof(C);
			var args = new[] { typeof(Type) };
			var ctor = converter.GetConstructor(args);

			// XXX: Converters may have a .ctor accepting destination type as parameter. (pruiz)
			return (ctor != null ?
				TypeDescriptor.CreateInstance(null, converter, args, new object[] { typeof(T) })
				: TypeDescriptor.CreateInstance(null, converter, null, null))
				as TypeConverter;
		}
	}
}
