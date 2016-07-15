using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Specialized;
using System.Collections.Generic;

using NUnit.Framework;

using HermaFx.ComponentModel;
using HermaFx.DataAnnotations;

namespace HermaFx.Settings
{
	[TestFixture]
	public class AppSettingsTests
	{
		#region Test DTOs
		[Settings("A")]
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

		[Settings("B")]
		public interface B
		{
			string Beta { get; set; }
		}

		[Settings]
		public interface C
		{
			string Charlie { get; set; }
		}

		[Settings]
		public interface D
		{
			string Data { get; set; }
		}

		public interface Nested
		{
			string Inner { get; set; }
		}

		[Settings]
		public interface E
		{
			string Data { get; set; }

			[Settings, Required]
			Nested Nested { get; set; }
		}
		#endregion

		private static readonly NameValueCollection _dict = new NameValueCollection()
		{
			{ "A:Data", "Value" },
			{ "A:StringList", "a,b,c" },
			{ "A:IntList", "1,2,3,4" },
			{ "B:Beta", "SubValue" },
			{ typeof(C).Namespace + ":Charlie", "C-Value" },
			{ typeof(E).Namespace + ":Nested:Inner", "SubValue" }

		};

		[Test]
		public void BasicTest()
		{
			var obja = new SettingsAdapter().Create<A>(_dict);

			CollectionAssert.AreEquivalent(obja.StringList, new[] { "a", "b", "c" });
			CollectionAssert.AreEquivalent(obja.IntList, new[] { 1, 2, 3, 4 });
		}

		[Test]
		public void MixedTest()
		{
			var objb = new SettingsAdapter().Create<B>(_dict);
			var objc = new SettingsAdapter().Create<C>(_dict);
			var obje = new SettingsAdapter().Create<E>(_dict);

			Assert.AreEqual("SubValue", obje.Nested.Inner);
			Assert.AreEqual("C-Value", objc.Charlie);
		}

		[Test]
		public void NestedTest()
		{
			var dict = new NameValueCollection()
			{
				{ "B:Inner", "Invalid1" },
				{ "Other:Inner", "Invalid2" },
				{ typeof(E).Namespace + ":Nested:Inner", "SubValue" }
			};

			var obje = new SettingsAdapter().Create<E>(dict);
			Assert.AreEqual("SubValue", obje.Nested.Inner);
		}

	}
}
