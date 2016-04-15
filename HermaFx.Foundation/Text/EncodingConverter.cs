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
		private static Lazy<string[]> _allEncodings = new Lazy<string[]>(() => Encoding.GetEncodings().Select(x => x.GetEncoding().BodyName.ToLowerInvariant()).ToArray());
		private static IEnumerable<string> AllEncodings => _allEncodings.Value;

		private IEncodingResolver _resolver;

		public EncodingConverter(Type resolver)
		{
			_resolver = (IEncodingResolver)Activator.CreateInstance(resolver);
		}

		public EncodingConverter()
		{

		}

		private IEncodingResolver GetResolver(ITypeDescriptorContext context)
		{
			var attr = context.PropertyDescriptor.Attributes.OfType<EncodingResolverAttribute>().SingleOrDefault();
			return attr != null ? attr.GetResolver() : _resolver;
		}

		private Encoding GetEncoding(ITypeDescriptorContext context, string name)
		{
			var resolver = GetResolver(context);

			if (resolver != null && !AllEncodings.Any(x => x == name.ToLowerInvariant()))
				return _resolver.GetEncoding(name);

			return Encoding.GetEncoding(name);
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
