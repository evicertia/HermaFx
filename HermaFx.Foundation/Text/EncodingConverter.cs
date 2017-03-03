using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace HermaFx.Text
{
	public class EncodingConverter : TypeConverter
	{
		#region DefaultEncodingResolver
		public class DefaultEncodingResolver : IEncodingResolver
		{
			private readonly EncoderFallback _encoderFallback;
			private readonly DecoderFallback _decoderFallback;

			public DefaultEncodingResolver()
			{
			}

			public DefaultEncodingResolver(EncoderFallback encoderFallback, DecoderFallback decoderFallback)
			{
				Guard.IsNotNull(encoderFallback, nameof(encoderFallback));
				Guard.IsNotNull(decoderFallback, nameof(decoderFallback));

				_encoderFallback = encoderFallback;
				_decoderFallback = decoderFallback;
			}

			public virtual Encoding GetEncoding(string name)
			{
				Guard.IsNotNullNorWhitespace(name, nameof(name));

				return _encoderFallback == null ? Encoding.GetEncoding(name)
					: Encoding.GetEncoding(name, _encoderFallback, _decoderFallback);
			}
		}
		#endregion

		private readonly IEncodingResolver _resolver;

		#region .ctor
		protected EncodingConverter(IEncodingResolver resolver)
		{
			Guard.IsNotNull(resolver, nameof(resolver));

			_resolver = resolver;
		}

		public EncodingConverter()
			: this(new DefaultEncodingResolver())
		{
		}
		#endregion

		protected virtual Encoding GetEncoding(ITypeDescriptorContext context, string name)
		{
			return _resolver.GetEncoding(name);
		}

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			Guard.IsNotNull(sourceType, nameof(sourceType));

			return sourceType == typeof(string) || typeof(Encoding).IsAssignableFrom(sourceType);
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			Guard.IsNotNull(destinationType, nameof(destinationType));

			return destinationType == typeof(string) || typeof(Encoding).IsAssignableFrom(destinationType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			Guard.IsNotNull(value, nameof(value));
			Guard.Against<NotSupportedException>(!CanConvertFrom(context, value.GetType()), "Cannot convert from value: {0}", value);

			return value is Encoding ? value : GetEncoding(context, value as string);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			Guard.Against<NotSupportedException>(!CanConvertTo(context, destinationType), "Cannot convert to {0}", destinationType.FullName);

			var encoding = value as Encoding;
			if (encoding != null)
			{
				return destinationType == typeof(string) ? (object)encoding.BodyName : encoding;
			}

			var name = value as string;
			if (name != null)
			{
				return destinationType == typeof(string) ? value : GetEncoding(context, name);
			}

			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
