using System;
using System.Reflection;
using System.Collections.Concurrent;

namespace HermaFx
{
	public static class TypeExtension
	{
		//a thread-safe way to hold default instances created at run-time
		private static ConcurrentDictionary<Type, object> typeDefaults = new ConcurrentDictionary<Type, object>();

		public static object GetDefault(this Type type)
		{
			Guard.IsNotNull(type, nameof(type));

			// If no Type was supplied, if the Type was a reference type, or if the Type was a System.Void, return null
			if (type == null || !type.IsValueType || type == typeof(void))
				return null;

			// If the supplied Type has generic parameters, its default value cannot be determined
			if (type.ContainsGenericParameters)
			{
				throw new ArgumentException($"The supplied value type <{type}> contains generic parameters, so the default value cannot be retrieved");
			}

			// If the Type is a primitive type, or if it is another publicly-visible value type (i.e. struct), return a 
			//  default instance of the value type
			if (type.IsPrimitive || !type.IsNotPublic)
			{
				try
				{
					return typeDefaults.GetOrAdd(type, t => Activator.CreateInstance(t));
				}
				catch (Exception e)
				{
					throw new ArgumentException(
					$"The Activator.CreateInstance method could not create a default instance of the supplied value type <{type}>\n\n"+
					$"Exception message:{e.Message}", e);
				}
			}

			// Fail with exception
			throw new ArgumentException($"The supplied value type <{type}> is not a publicly-visible type, so the default value cannot be retrieved");
		}

	}
}
