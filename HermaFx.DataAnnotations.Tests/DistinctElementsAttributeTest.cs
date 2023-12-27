
using System.Collections.Generic;
using NUnit.Framework;

namespace HermaFx.DataAnnotations
{
	[TestFixture]
	public class DistinctElementsAttributeTest
	{

		private class Model
		{
			#region Innter Type
			public class _Item
			{
				public string Property1 { get; set; }
				public string Property2 { get; set; }

			}
			#endregion

			[DistinctElements("Property1")]
			public IEnumerable<_Item> Values { get; set; }

		}

		[Test]
		public void IsValidTest()
		{
			var model = new Model();

			model.Values = new List<Model._Item>()
			{
					new Model._Item() { Property1 = "hello", Property2 = "world" },
					new Model._Item() { Property1 = "hi", Property2 = "world" },
			};

			Assert.That(ExtendedValidator.IsValid(model), Is.True);
		}

		[Test]
		public void IsNotValidTest()
		{
			var model = new Model();

			model.Values = new List<Model._Item>()
			{
					new Model._Item() { Property1 = "hello", Property2 = "world" },
					new Model._Item() { Property1 = "hello", Property2 = "world" },
			};

			Assert.That(ExtendedValidator.IsValid(model), Is.False);
		}

		[Test]
		public void IsValidWithNullTest()
		{
			var model = new Model();

			model.Values = new List<Model._Item>()
			{
					new Model._Item() { Property1 = "hello", Property2 = "world" },
					new Model._Item() { Property1 = null, Property2 = "world" },
			};

			Assert.That(ExtendedValidator.IsValid(model), Is.True);
		}

		[Test]
		public void IsNotValidWithNullTest()
		{
			var model = new Model();

			model.Values = new List<Model._Item>()
			{
					new Model._Item() { Property1 = null, Property2 = "world" },
					new Model._Item() { Property1 = null, Property2 = "world" },
			};

			Assert.That(ExtendedValidator.IsValid(model), Is.False);
		}

	}
}
