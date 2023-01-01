using System;
using System.Text;

using Rebus.Configuration;
using Rebus.Serialization.Json;

namespace HermaFx.Rebus
{
	public class EnhancedJsonSerializerOptions
	{
		private readonly JsonSerializationOptions _inner;
		private readonly EnhancedJsonSerializer _serializer;

		internal EnhancedJsonSerializerOptions(EnhancedJsonSerializer serializer, JsonSerializationOptions inner)
		{
			_inner = inner.ThrowIfNull(nameof(inner));
			_serializer = serializer.ThrowIfNull(nameof(serializer));
		}

		/// <summary>
		/// Adds a function that will determine how a given type is turned into a <see cref="TypeDescriptor"/>.
		/// Return null if the function has no opinion about this particular type, allowing other functions and
		/// ultimately the default JSON serializer's opinion to be used.
		/// </summary>
		public EnhancedJsonSerializerOptions AddNameResolver(Func<Type, TypeDescriptor> resolve)
		{
			_inner.AddNameResolver(resolve);
			return this;
		}

		/// <summary>
		/// Adds a function that will determine how a given <see cref="TypeDescriptor"/> is turned into a .NET type.
		/// Return null if the function has no opinion about this particular type descriptor, allowing other functions and
		/// ultimately the default JSON serializer's opinion to be used.
		/// </summary>
		public EnhancedJsonSerializerOptions AddTypeResolver(Func<TypeDescriptor, Type> resolve)
		{
			_inner.AddTypeResolver(resolve);
			return this;
		}

		/// <summary>
		/// Overrides the default UTF-7 encoding and uses the specified encoding instead when serializing. The used encoding
		/// is put in a header, so you don't necessarily need to specify the same encoding in order to be able to deserialize
		/// properly.
		/// </summary>
		public EnhancedJsonSerializerOptions SpecifyEncoding(Encoding encoding)
		{
			if (encoding == null) throw new ArgumentNullException("encoding");
			_inner.SpecifyEncoding(encoding);
			return this;
		}

		/// <summary>
		/// Configure the serializer to serialize the enums as string.
		/// </summary>
		public EnhancedJsonSerializerOptions SerializeEnumAsStrings(bool camelCaseText)
		{
			_inner.SerializeEnumAsStrings(camelCaseText);
			return this;
		}
	}
}

