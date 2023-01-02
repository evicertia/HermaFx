using System;
using System.Linq;
using System.Text;

using HermaFx.Logging;

using Rebus;
using Rebus.Messages;
using Rebus.Serialization.Json;

using RHeaders = Rebus.Shared.Headers;

namespace HermaFx.Rebus
{
	/// <summary>
	/// This is an enhanced JSON serializer which has backwards compatibility with Rebus' standard
	/// JsonSerializer, but add some improvements/optimizations.
	/// </summary>
	// TODO: Add more missing features like..
	//		- Type resolving / remmapping mechanism.
	//		- Avoid unneded use of $type by Newtonsoft.
	//		- Allow configurationg of Enum serialization mode (w/ back/forward compatibility on deserialization)
	public class EnhancedJsonSerializer : ISerializeMessages
	{
		private static ILog _logger = LogProvider.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public static class Headers
		{
			public const string Serializer = "serializer";
			public const string SerializerVersion = "serializer-version"; //< Header intended for backwards/forwards compatibility
			public const string MessageType = "message-type";
		}
		private const int SerializerVersion = 230101;

		private readonly JsonMessageSerializer _inner;

		public EnhancedJsonSerializer(JsonMessageSerializer inner)
		{
			//  XXX: Right now we use Rebus' JsonSerializer underneath, but at some point we should be using
			// Newtonsoft directly (internalizing it as we do with other conflicting dependences).
			_inner = inner.ThrowIfNull(nameof(inner));

			_inner.SpecifyEncoding(Encoding.UTF8); //< Our default encoding should be UTF-8
			// TODO: Serialize EnumsAsString by default (unless bitflags?)
		}

		/// <summary>
		/// Gets an assembly-qualified name without the version and public key token stuff
		/// </summary>
		private static string GetMinimalAssemblyQualifiedName(Type type)
		{
			return string.Format("{0},{1}", type.FullName, type.Assembly.GetName().Name);
		}

		private static string TryGetSingleMessageType(Message message)
		{
			return message.Messages?.Count() != 1
				? null
				: GetMinimalAssemblyQualifiedName(message.Messages.First().GetType());
		}

		private static string GetMessageTypes(Message message)
		{
			var types = message.Messages?.Select(x => GetMinimalAssemblyQualifiedName(x?.GetType()));
			return string.Join(";", types ?? Enumerable.Empty<string>());
		}

		private void TrySetHeader(TransportMessageToSend message, string key, object value)
		{
			if (message.Headers.ContainsKey(RHeaders.MessageTypes))
			{
				_logger.WarnFormat("Not overwritting header '{0}' already present on TransportMessageToSend from serializer {1}?!", key, _inner?.GetType().Name);
				return;
			}

			if (value == null)
			{
				return; //< Skip setting headers w/ empty values..
			}

			message.Headers[key] = value;
		}

		public Message Deserialize(ReceivedTransportMessage transportMessage)
		{
			return _inner.Deserialize(transportMessage);
		}

		public TransportMessageToSend Serialize(Message message)
		{
			var result = _inner.Serialize(message);

			// Augment serialized message with extra headers which can be used during
			// deserialization for optimizatins of future backwards compatibility cases.

			TrySetHeader(result, Headers.Serializer, nameof(EnhancedJsonSerializer));
			TrySetHeader(result, Headers.SerializerVersion, SerializerVersion);
			TrySetHeader(result, Headers.MessageType, TryGetSingleMessageType(message));
			TrySetHeader(result, RHeaders.MessageTypes, GetMessageTypes(message));

			return result;
		}
	}
}