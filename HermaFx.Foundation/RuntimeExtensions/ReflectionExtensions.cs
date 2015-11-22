using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace HermaFx
{
	public static class ReflectionExtensions
	{
		public static T[] GetCustomAttributes<T>(this FieldInfo field)
			where T : Attribute
		{
			if (field == null) throw new ArgumentNullException("field");

			return field.GetCustomAttributes(typeof(T), false) as T[];
		}

		public static T[] GetCustomAttributes<T>(this FieldInfo field, bool inherit)
			where T : Attribute
		{
			if (field == null) throw new ArgumentNullException("field");

			return field.GetCustomAttributes(typeof(T), inherit) as T[];
		}

		public static string GetFriendlyTypeName(this Type type, bool includeNamespace = true)
		{
			if (type.IsGenericParameter)
			{
				return type.Name;
			}

			if (!type.IsGenericType)
			{
				return includeNamespace ? type.FullName : type.Name;
			}

			var builder = new System.Text.StringBuilder();
			var name = type.Name;
			var index = name.IndexOf("`");
			if (includeNamespace) builder.AppendFormat("{0}.", type.Namespace);
			builder.AppendFormat("{0}", name.Substring(0, index));
			builder.Append('<');
			var first = true;
			foreach (var arg in type.GetGenericArguments())
			{
				if (!first)
				{
					builder.Append(',');
				}
				builder.Append(GetFriendlyTypeName(arg, includeNamespace));
				first = false;
			}
			builder.Append('>');
			return builder.ToString();
		}
	}
}
