using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;

namespace HermaFx.Castle
{
	public static class InjectExtensions
	{
		private static readonly BindingFlags _bflags = BindingFlags.Public | BindingFlags.Instance;
		private static readonly ConcurrentDictionary<Type, PropertyInfo[]> _properties = new ConcurrentDictionary<Type, PropertyInfo[]>();

		/// <summary>
		/// Determines whether the specified type is a generic collection.
		/// </summary>
		/// <param name="type">The type.</param>
		private static bool IsGenericCollection(this Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			return type.GetInterfaces()
				.Where(x => x.IsGenericType)
				.Any(x => 
					x.GetGenericTypeDefinition() == typeof(IEnumerable<>)
				);
		}

		/// <summary>
		/// Gets the dependency properties for object.
		/// </summary>
		/// <param name="object">The object.</param>
		/// <returns></returns>
		private static IEnumerable<PropertyInfo> GetDependencyPropertiesFor(object @object)
		{
			return @object.GetType().GetProperties(_bflags);
		}

		/// <summary>
		/// Injects the dependencies.
		/// </summary>
		/// <param name="kernel">The container.</param>
		/// <param name="object">The object to configure.</param>
		public static void InjectDependencies(this IKernel kernel, object @object)
		{
			var resolveCollections = kernel.HasComponent(typeof(CollectionResolver));

			foreach (var info in GetDependencyPropertiesFor(@object))
			{
				var o = info.GetValue(@object, null);

				// skip the object is it already contains a value of any sort
				if (o != null) continue;
				else if (resolveCollections && info.PropertyType.IsGenericCollection())
				{
					o = kernel.ResolveAll(info.PropertyType.GetGenericArguments()[0]);
				}
				else if ((info.PropertyType.IsInterface) || (info.PropertyType.IsClass))
				{
					// try to resolve the related type if the component knows it
					if (kernel.HasComponent(info.PropertyType))
						o = kernel.Resolve(info.PropertyType);
				}

				if (o != null) info.SetValue(@object, o, null);
			}
		}

		/// <summary>
		/// Injects the dependencies.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <param name="object">The object.</param>
		public static void InjectDependencies(this IWindsorContainer container, object @object)
		{
			container.Kernel.InjectDependencies(@object);
		}

		/// <summary>
		/// Releases the dependencies.
		/// </summary>
		/// <param name="kernel">The kernel.</param>
		/// <param name="object">The object.</param>
		public static void ReleaseDependencies(this IKernel kernel, object @object)
		{
			var resolveCollections = kernel.HasComponent(typeof(CollectionResolver));

			foreach (var info in GetDependencyPropertiesFor(@object))
			{
				var o = info.GetValue(@object, null);

				if (o == null) continue;
				else if (resolveCollections && info.PropertyType.IsGenericCollection())
				{
					foreach (var obj in ((o as IEnumerable) ?? Enumerable.Empty<object>()))
					{
						kernel.ReleaseComponent(obj);
					}
				}
				else if ((info.PropertyType.IsInterface) || (info.PropertyType.IsClass))
				{
					kernel.ReleaseComponent(o);
				}
			}
		}

		/// <summary>
		/// Releases the dependencies.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <param name="object">The object.</param>
		public static void ReleaseDependencies(this IWindsorContainer container, object @object)
		{
			container.Kernel.ReleaseDependencies(@object);
		}
	}
}