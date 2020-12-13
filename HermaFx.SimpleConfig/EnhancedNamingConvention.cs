using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace HermaFx.SimpleConfig
{
    /// <summary>
	/// A more opinionated naming convention which:
	///  - Takes DataAnnotations like DisplayName into account.
	///  - Avoids [if possible] using 'add/remove/etc' for collections, using the type-name instead.
	///  - Uses camelCase convention for attribute names.
	/// </summary>
    public class EnhancedNamingConvention : NamingConvention
    {
        public override string AddToCollectionElementName(Type collectionElementType, string propertyName)
        {
            var name = collectionElementType.Name;

            var disattr = collectionElementType.GetCustomAttribute<DisplayNameAttribute>();
            if (disattr != null && !string.IsNullOrEmpty(disattr.DisplayName))
            {
                return disattr.DisplayName;
            }

#if false
            var elname = collectionElementType.GetCustomAttribute<ElementNameAttribute>();
            if (elname != null && !string.IsNullOrEmpty(elname.Name))
            {
                return elname.Name;
            }
#endif
            return name;
        }

        public override string AttributeName(PropertyInfo propertyInfo)
        {
            var name = propertyInfo.Name;
            var ptype = propertyInfo.PropertyType;
            var disattr = propertyInfo.GetCustomAttribute<DisplayNameAttribute>();

            if (disattr != null && !string.IsNullOrEmpty(disattr.DisplayName))
            {
                return disattr.DisplayName;
            }

            if (ptype.IsInterface)
            {
                if (ptype.IsGenericType && ptype.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    if (!ptype.GenericTypeArguments[0].IsEnum)
                        return name;
                }
                else
                {
                    return name;
                }
            }

            if (String.IsNullOrEmpty(name) || Char.IsLower(name, 0))
            {
                return name;
            }

            return Char.ToLowerInvariant(name[0]) + name.Substring(1);
        }
    }
}
