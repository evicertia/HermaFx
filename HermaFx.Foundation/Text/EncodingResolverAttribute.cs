using System;
using System.Text;
using System.Linq;

namespace HermaFx.Text
{
	[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
	public class EncodingResolverAttribute : Attribute, IEncodingResolver
	{
		private readonly Type _resolver;
		private readonly Encoding[] _encodings;

		public EncodingResolverAttribute(Type resolver)
		{
			Guard.IsNotNull(resolver, nameof(resolver));
			Guard.Against<ArgumentOutOfRangeException>(
				typeof(IEncodingResolver).IsAssignableFrom(resolver), 
				"Type {0} does not implement {1}", resolver.FullName, typeof(IEncodingResolver).FullName
			);

			_resolver = resolver;
		}

		public EncodingResolverAttribute(params Type[] encodings)
		{
			Guard.IsNotNull(encodings, nameof(encodings));
			Guard.Against<ArgumentOutOfRangeException>(
				encodings.Any(x => !typeof(Encoding).IsAssignableFrom(x)),
				"Some passed objects do not implement {0}", typeof(Encoding).FullName
			);

			_encodings = encodings.Select(x => (Encoding)Activator.CreateInstance(x)).ToArray();
		}

		public Encoding GetEncoding(string name)
		{
			Guard.IsNotNullNorWhitespace(name, nameof(name));
			return _encodings.Single(x => x.BodyName.ToLowerInvariant() == name.ToLowerInvariant());
		}

		public IEncodingResolver GetResolver()
		{
			return _encodings != null ? this : (IEncodingResolver)Activator.CreateInstance(_resolver);
		}
	}
}
