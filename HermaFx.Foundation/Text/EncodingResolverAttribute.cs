using System;

namespace HermaFx.Text
{
	[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
	sealed class EncodingResolverAttribute : Attribute
	{
		readonly Type _resolver;

		public IEncodingResolver GetResolver()
		{
			return (IEncodingResolver)Activator.CreateInstance(_resolver);
		}

		// This is a positional argument
		public EncodingResolverAttribute(Type resolver)
		{
			Guard.IsNotNull(resolver, nameof(resolver));
			Guard.Against<ArgumentOutOfRangeException>(
				typeof(IEncodingResolver).IsAssignableFrom(resolver), 
				"Type {0} does not implement {1}", resolver.FullName, typeof(IEncodingResolver).FullName
			);

			_resolver = resolver;
		}
	}
}
