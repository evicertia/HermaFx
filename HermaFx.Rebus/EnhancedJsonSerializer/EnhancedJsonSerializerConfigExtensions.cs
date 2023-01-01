using System;

using Rebus;
using Rebus.Configuration;
using Rebus.Serialization.Json;

namespace HermaFx.Rebus
{
	public static class EnhancedJsonSerializerConfigExtensions
	{ 
		/// <summary>
		/// Configures Rebus to use <see cref="EnhancedJsonSerializer"/> to serialize messages. A <see cref="EnhancedJsonSerializer"/>
		/// object is returned, which can be used to configure details around how the JSON serialization should work
		/// </summary>
		public static EnhancedJsonSerializerOptions UseEnhancedJsonSerializer(this RebusSerializationConfigurer @this)
		{
			var inner = @this.UseJsonSerializer();
			var serializer = new EnhancedJsonSerializer(@this.Backbone.SerializeMessages as JsonMessageSerializer);
			@this.Use(serializer);
			return new EnhancedJsonSerializerOptions(serializer, inner);
		}
	}
}

