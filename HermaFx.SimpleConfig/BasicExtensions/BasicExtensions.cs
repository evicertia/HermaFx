using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace HermaFx.SimpleConfig.BasicExtensions
{

    internal static class BasicExtensions
    {
        internal static string PropertyName(this MethodInfo methodInfo)
        {
            return methodInfo.Name.Substring(4);
        }

        public static PropertyInfo GetPropertyInfo(this MethodInfo method)
        {
            if (!method.IsSpecialName) return null;

            var type = method.DeclaringType;

            return type.GetProperty(method.Name.Substring(4), BindingFlags.Instance | BindingFlags.Public);
        }
		public static bool HasAttribute<TAttribute>(this MemberInfo memberInfo)
		{
#if NETSTANDARD1_0
            return memberInfo
                .CustomAttributes
                .Any(a => a.AttributeType == typeof(TAttribute));
#else
			return memberInfo
				.GetCustomAttributes(typeof(TAttribute), inherit: false)
				.Any();
#endif
		}

		internal static void Each<T>(this IEnumerable<T> sequence, Action<T> job)
        {
            foreach (var obj in sequence)
            {
                job(obj);
            }
        }

        internal static string ToFormat(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        internal static bool IsNullOrEmptyOrWhiteSpace(this string value)
        {
            return string.IsNullOrEmpty(value) || value.IsWhiteSpace();
        }

        private static bool IsWhiteSpace(this string value)
        {
            return value.Any(Char.IsWhiteSpace);
        }
    }
}
