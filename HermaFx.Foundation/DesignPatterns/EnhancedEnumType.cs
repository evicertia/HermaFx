using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Reflection;

namespace HermaFx.DesignPatterns
{
	[Serializable]
	public abstract class EnhancedEnumType<L, T> : ReadOnlyCollection<T>, IEnhancedEnumTypeDescriptor<T>
			where L : EnhancedEnumType<L, T>
	{
		#region Fields
		private static T[] _entries = GetFields<L>();
		#endregion

		#region .ctors
		static EnhancedEnumType()
		{
			Guard.Against<InvalidOperationException>(_entries.Length == 0, "EnhancedEnumType {0} has no elements?!", typeof(L).Name);
		}

		public EnhancedEnumType(Func<T, object> memberKeyGetter)
			: this(memberKeyGetter, null)
		{
		}

		public EnhancedEnumType(Func<T, object> memberKeyGetter, Func<T, object, bool> memberKeyMatcher)
			: base(_entries)
		{
			MemberKeyGetter = memberKeyGetter;
			MemberKeyMatcher = memberKeyMatcher ?? ((x, value) => memberKeyGetter(x) == value);
			MemberType = memberKeyGetter(_entries.First()).GetType();

			Guard.Against<InvalidOperationException>(
				_entries.Count() == _entries.Select(x => MemberKeyGetter(x)).Distinct().Count(),
				"One or more EnhancedEnum members have duplicated keys"
			);
		}
		#endregion

		#region static helper methods..
		private static T[] GetFields<W>()
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

		#region IEnhancedEnumTypeDescriptor<T> Members

		public Type MemberType { get; private set; }
		public Func<T, object> MemberKeyGetter { get; private set; }
		public Func<T, object, bool> MemberKeyMatcher { get; private set; }

		#endregion
	}

	public interface IEnhancedEnumTypeDescriptor<T>
	{
		Type MemberType { get; }
		Func<T, object> MemberKeyGetter { get; }
		Func<T, object, bool> MemberKeyMatcher { get; }
	}


}
