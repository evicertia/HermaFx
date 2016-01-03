using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Castle.Components.DictionaryAdapter;

using NUnit.Framework;

using HermaFx.ComponentModel;
using HermaFx.DataAnnotations;

namespace HermaFx.Castle.DictionaryAdapter
{
	[TestFixture]
	public class AppSettingsTests
	{
		#region Test DTOs
		[AppSettings("A")]
		public interface A
		{
			string Data { get; set; }
			[DefaultValue(10)]
			uint Number { get; set; }

			[TypeConverter(typeof(StringArrayConverter))]
			string[] StringList { get; set; }

			[MinElements(2), MaxElements(4)]
			[TypeConverter(typeof(StringArrayConverter<int>))]
			IEnumerable<int> IntList { get; set; }
		}

		[AppSettings("B")]
		public interface B
		{
			string Data { get; set; }
		}

		[AppSettings]
		public interface C
		{
			string Data { get; set; }
		}

		[AppSettings]
		public interface D
		{
			string Data { get; set; }
		}

		[AppSettings]
		public interface E
		{
			string Data { get; set; }

			[Required]
			B Other { get; set; }
		}
		#endregion

		private static readonly NameValueCollection _dict = new NameValueCollection()
		{
			{ "A:Data", "Value" },
			{ "A:StringList", "a,b,c" },
			{ "A:IntList", "1,2,3,4" },
			{ "B:Data", "SubValue" },
			{ typeof(C).Namespace + ":Data", "Value" },
			{ typeof(E).Namespace + ":Other:Data", "SubValue" }

		};

		[Test]
		public void BasicTest()
		{
			var obja = new DictionaryAdapterFactory().GetAdapter<A>(_dict);

			CollectionAssert.AreEquivalent(obja.StringList, new[] { "a", "b", "c" });
			CollectionAssert.AreEquivalent(obja.IntList, new[] { 1, 2, 3, 4 });

			var objb = new DictionaryAdapterFactory().GetAdapter<B>(_dict);
			var objc = new DictionaryAdapterFactory().GetAdapter<C>(_dict);
			var obje = new DictionaryAdapterFactory().GetAdapter<E>(_dict);

			Assert.AreEqual(obje.Other.Data, "SubValue");
		}

	}
}
