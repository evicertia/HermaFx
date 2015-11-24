using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Reflection;

namespace HermaFx.DesignPatterns
{
	[Serializable]
	public abstract class EnhancedEnumType<L, T> : ReadOnlyCollection<T>
			where L : EnhancedEnumType<L, T>
	{
		private static T[] _entries = GetFields<L>();

		public EnhancedEnumType()
			: base(_entries)
		{
		}

		#region static helper methods..
		public static T[] GetFields<W>()
				where W : ReadOnlyCollection<T>
		{
			// Fill _entries list..
			var type = typeof(W);
			var bflags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Static;
			var fields = type.FindMembers(MemberTypes.Field, bflags, null, null);

			return fields.OfType<FieldInfo>()
					.Select(x => x.GetValue(null))
					.OfType<T>()
					.ToArray();
		}
		#endregion
	}

}
