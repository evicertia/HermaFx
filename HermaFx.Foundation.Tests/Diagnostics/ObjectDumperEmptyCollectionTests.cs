using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NUnit.Framework;

namespace HermaFx.Diagnostics
{
	[TestFixture]
	public class ObjectDumperEmptyCollectionTests
	{
		private string TextForEmptyCollectionOf(Type type)
		{
			return ObjectDumper.DumpItemBase.TextForCollectionOf(type, 0);
		}

		[Test]
		public void Should_return_correct_empty_array_string()
		{
			TextForEmptyCollectionOf(typeof(int[])).ShouldEqual("Int32[] (0 items)");
		}

		[Test]
		public void Should_return_correct_empty_collection_of_ints_string()
		{
			TextForEmptyCollectionOf(typeof(Collection<int>)).ShouldEqual("Collection<Int32> (0 items)");
		}
		[Test]
		public void Should_return_correct_empty_list_of_strings_string()
		{
			TextForEmptyCollectionOf(typeof(List<string>)).ShouldEqual("List<String> (0 items)");
		}

		[Test]
		public void Should_return_correct_empty_dictionary_of_int_object_string()
		{
			TextForEmptyCollectionOf(typeof(Dictionary<int, object>)).ShouldEqual("Dictionary<Int32, Object> (0 items)");
		}
	}
}