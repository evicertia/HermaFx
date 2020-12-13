using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace HermaFx.SimpleConfig
{
    internal static class ReflectionHelpers
    {
        private static readonly BindingFlags _privateFlags = BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.NonPublic;

        public static bool RunningOnMono
        {
            get
            {
                Type t = Type.GetType("Mono.Runtime");
                if (t != null)
                    return true;

                return false;
            }
        }

        internal static FieldInfo GetPrivateField<TType>(string fieldName)
        {
            return GetPrivateField(typeof(TType), fieldName);
        }

        internal static FieldInfo GetPrivateField(Type type, string fieldName)
        {
            return type.GetField(fieldName, _privateFlags);
        }

        internal static PropertyInfo GetPrivateProperty<TType>(string propertyName)
        {
            return typeof(TType).GetProperty(propertyName, _privateFlags);
        }

        internal static Action<TOwner, TFieldType> MakeSetterForPrivateField<TOwner, TFieldType>(string fieldName)
        {
            var field = GetPrivateField<TOwner>(fieldName);
            return field != null ? (obj, value) => field.SetValue(obj, value) : (Action<TOwner, TFieldType>)null;
        }

        internal static Action<TOwner, TFieldType> MakeSetterForPrivateProperty<TOwner, TFieldType>(string propertyName)
        {
            var property = GetPrivateProperty<TOwner>(propertyName);
            return property != null ? (obj, value) => property.SetValue(obj, value, null) : (Action<TOwner, TFieldType>)null;
        }

        public static bool IsGenericIEnumerable(this Type ptype, params Type[] arguments)
        {
            if (!ptype.IsGenericType) return false;

            var genericTypeDefinition = ptype.GetGenericTypeDefinition();
            if (genericTypeDefinition != typeof(IEnumerable<>)) return false;

            if (arguments != null && arguments.Length > 0)
            {
                return Enumerable.SequenceEqual(ptype.GetGenericArguments(), arguments);
            }

            return true;
        }

        public static bool IsGenericIEnumerable(this PropertyInfo property, params Type[] arguments)
        {
            return property.PropertyType.IsGenericIEnumerable();
        }
    }
}
