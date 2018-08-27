using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;

namespace HermaFx.Diagnostics
{
	[TestFixture]
	public class ObjectDumperTests
	{
		[Test]
		public void Simple_Value_int()
		{
			1.DumpAsString().ShouldEqual("1");
		}

		[Test]
		public void Simple_Value_string()
		{
			"HELLO".DumpAsString().ShouldEqual("HELLO");
		}

		[Test]
		public void Simple_AnonymousType()
		{
			(new { Foo = 1 }).DumpAsString().Trace().ShouldEqual(
@"|----------------------------|
| <>f__AnonymousType0<Int32> |
|----------------------------|
| Foo | 1                    |
|----------------------------|
");
		}

		[Test]
		public void Simple_AnonymousType2()
		{
			(new { Foo = 1, Foo2 = "HELLO" }).DumpAsString().Trace().ShouldEqual(
@"|------------------------------------|
| <>f__AnonymousType1<Int32, String> |
|------------------------------------|
|  Foo | 1                           |
|------------------------------------|
| Foo2 | HELLO                       |
|------------------------------------|
");
		}

		[Test]
		public void Simple_AnonymousType_with_multiline_string()
		{
			(new
			{
				Foo = 1,
				Foo2 = new StringBuilder()
					.AppendLine("HELLO")
					.AppendLine("World")
					.AppendLine("How")
					.AppendLine("Are")
					.AppendLine("You")
					.ToString()
			}).DumpAsString().Trace().ShouldEqual(
@"|------------------------------------|
| <>f__AnonymousType1<Int32, String> |
|------------------------------------|
|  Foo | 1                           |
|------------------------------------|
| Foo2 | HELLO                       |
|      | World                       |
|      | How                         |
|      | Are                         |
|      | You                         |
|------------------------------------|
");
		}

		[Test]
		public void Simple_Nested_AnonymousType2()
		{
			(new { Foo = 1, Foo2 = new { Bar = 1.234m } }).DumpAsString().Trace().ShouldEqual(
@"|----------------------------------------------------------|
| <>f__AnonymousType1<Int32, <>f__AnonymousType2<Decimal>> |
|----------------------------------------------------------|
|  Foo | 1                                                 |
|----------------------------------------------------------|
| Foo2 | |------------------------------|                  |
|      | | <>f__AnonymousType2<Decimal> |                  |
|      | |------------------------------|                  |
|      | | Bar | 1.234                  |                  |
|      | |------------------------------|                  |
|----------------------------------------------------------|
");
		}


		[Test]
		public void Simple_Nested_AnonymousType_with_multiline_string()
		{
			(new
			{
				Foo = 1,
				Foo2 = new
				{
					Bar = new StringBuilder()
					.AppendLine("HELLO")
					.AppendLine("World")
					.AppendLine("How")
					.AppendLine("Are")
					.AppendLine("You")
					.ToString()
				}
			}).DumpAsString().Trace().ShouldEqual(
@"|---------------------------------------------------------|
| <>f__AnonymousType1<Int32, <>f__AnonymousType2<String>> |
|---------------------------------------------------------|
|  Foo | 1                                                |
|---------------------------------------------------------|
| Foo2 | |-----------------------------|                  |
|      | | <>f__AnonymousType2<String> |                  |
|      | |-----------------------------|                  |
|      | | Bar | HELLO                 |                  |
|      | |     | World                 |                  |
|      | |     | How                   |                  |
|      | |     | Are                   |                  |
|      | |     | You                   |                  |
|      | |-----------------------------|                  |
|---------------------------------------------------------|
");
		}

		[Test]
		public void Simple_empty_array()
		{
			(new int[0]).DumpAsString().Trace().ShouldEqual(
@"|-------------------|
| Int32[] (0 items) |
|-------------------|
");
		}


		[Test]
		public void Simple_array_one_item()
		{
			(new[] { 1 }).DumpAsString().Trace().ShouldEqual(
@"|-------------------|
| Int32[] (1 items) |
|-------------------|
| 1                 |
|-------------------|
");
		}

		[Test]
		public void Simple_Dictionary()
		{
			var dictionary = new Dictionary<int, string>
			{
				{1, "FOO"},
				{2, "FOOTee Doo"},
				{10, "FOO"},
				{100, "FOO"},
				{1000, "FOO"},
				{10000, "FOO"},
			};

			dictionary.DumpAsString().Trace();
		}


		[Test]
		public void Simple_collection_of_custom_objects()
		{
			var items = new[]
			{
				new {Name = "FOO", Value = 1m},
				new {Name = "Bar", Value = 1.45m},
			};

			items.DumpAsString().Trace().ShouldEqual(
				@"|-------------------------------------------|
| <>f__AnonymousType3`2[] (2 items)         |
|-------------------------------------------|
| |--------------------------------------|  |
| | <>f__AnonymousType3<String, Decimal> |  |
| |--------------------------------------|  |
| |  Name | FOO                          |  |
| |--------------------------------------|  |
| | Value | 1                            |  |
| |--------------------------------------|  |
|-------------------------------------------|
| |--------------------------------------|  |
| | <>f__AnonymousType3<String, Decimal> |  |
| |--------------------------------------|  |
| |  Name | Bar                          |  |
| |--------------------------------------|  |
| | Value | 1.45                         |  |
| |--------------------------------------|  |
|-------------------------------------------|
");
		}

		[Test]
		public void ComplexType()
		{
			ComplexTypeParent complexTypeParent = GetComplexTypeParent();
			complexTypeParent.DumpAsString().Trace().ShouldEqual(
@"|-------------------------------------------------------------------|
| ComplexTypeParent                                                 |
|-------------------------------------------------------------------|
|                  Name | SomeName                                  |
|-------------------------------------------------------------------|
|           ListOfItems | |------------------------|                |
|                       | | List<String> (3 items) |                |
|                       | |------------------------|                |
|                       | | a                      |                |
|                       | |------------------------|                |
|                       | | b                      |                |
|                       | |------------------------|                |
|                       | | c                      |                |
|                       | |------------------------|                |
|-------------------------------------------------------------------|
| SomeDictionaryOfStuff | |--------------------------------------|  |
|                       | | Dictionary<String, String> (3 items) |  |
|                       | |--------------------------------------|  |
|                       | | [a, 1]                               |  |
|                       | |--------------------------------------|  |
|                       | | [b, 10]                              |  |
|                       | |--------------------------------------|  |
|                       | | [c, 100]                             |  |
|                       | |--------------------------------------|  |
|-------------------------------------------------------------------|
|   ComplexChildObjects | |------------------------------------|    |
|                       | | List<ComplexChildObject> (3 items) |    |
|                       | |------------------------------------|    |
|                       | | |--------------------|             |    |
|                       | | | ComplexChildObject |             |    |
|                       | | |--------------------|             |    |
|                       | | |  Name | FOO        |             |    |
|                       | | |--------------------|             |    |
|                       | | | Value | 1.2        |             |    |
|                       | | |--------------------|             |    |
|                       | |------------------------------------|    |
|                       | | |--------------------|             |    |
|                       | | | ComplexChildObject |             |    |
|                       | | |--------------------|             |    |
|                       | | |  Name | Hello      |             |    |
|                       | | |--------------------|             |    |
|                       | | | Value | 10.2       |             |    |
|                       | | |--------------------|             |    |
|                       | |------------------------------------|    |
|                       | | |--------------------|             |    |
|                       | | | ComplexChildObject |             |    |
|                       | | |--------------------|             |    |
|                       | | |  Name | World      |             |    |
|                       | | |--------------------|             |    |
|                       | | | Value | 100.2      |             |    |
|                       | | |--------------------|             |    |
|                       | |------------------------------------|    |
|-------------------------------------------------------------------|
");
		}

		[Test]
		public void ComplexType_ListOf()
		{
			var items = new List<ComplexTypeParent>
			{
				GetComplexTypeParent(),
				GetComplexTypeParent(),
			};

			items.DumpAsString().Trace();
		}

		private ComplexTypeParent GetComplexTypeParent()
		{
			return new ComplexTypeParent
			{
				Name = "SomeName",
				ComplexChildObjects = new List<ComplexChildObject>
													 {
														 new ComplexChildObject {Name = "FOO", Value = 1.2m},
														 new ComplexChildObject {Name = "Hello", Value = 10.2m},
														 new ComplexChildObject {Name = "World", Value = 100.2m},
													 },
				ListOfItems = new List<string> { "a", "b", "c" },
				SomeDictionaryOfStuff = new Dictionary<string, string>
													   {
														   {"a", "1"},
														   {"b", "10"},
														   {"c", "100"},
													   }
			};
		}

		public class ComplexTypeParent
		{
			public string Name { get; set; }
			public IEnumerable<string> ListOfItems { get; set; }
			public IDictionary<string, string> SomeDictionaryOfStuff { get; set; }
			public List<ComplexChildObject> ComplexChildObjects { get; set; }
		}
		public class ComplexChildObject
		{
			public string Name { get; set; }
			public decimal Value { get; set; }
		}


	}

	public static class Extensions
	{
		public static string Trace(this string item)
		{
			System.Diagnostics.Trace.WriteLine(item);
			return item.Replace("\r", string.Empty);
		}
	}
}