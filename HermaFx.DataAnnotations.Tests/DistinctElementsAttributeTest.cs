using System;
using System.Collections.Generic;
using System.Linq;
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

			[DistinctElements("Property1", ErrorMessage = "New error message")]
			public IEnumerable<_Item> ValuesErrorMessageChanged { get; set; }
		}

		public class ModelWithWrongProperty
		{
			[DistinctElements("Property")]
			public int Value { get; set; }
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
			Assert.DoesNotThrow(() => ExtendedValidator.EnsureIsValid(model));
		}

		[Test]
		public void IsNotValidTest()
		{
			//setup
			var model = new Model();

			model.Values = new List<Model._Item>()
			{
					new Model._Item() { Property1 = "hello", Property2 = "world" },
					new Model._Item() { Property1 = "hello", Property2 = "world" },
			};

			//asserts
			Assert.That(ExtendedValidator.IsValid(model), Is.False);
			var exception = Assert.Throws<AggregateValidationException>(
				() => ExtendedValidator.EnsureIsValid(model),
				"Object of type Model has some invalid values."
			);
			Assert.That(exception.ValidationResults.Count, Is.EqualTo(1));
			Assert.That(
				exception.ValidationResults.ElementAt(0).ErrorMessage,
				Is.EqualTo("The property Property1 must be unique in the Values collection.")
			);
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
			Assert.DoesNotThrow(() => ExtendedValidator.EnsureIsValid(model));
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
			var exception = Assert.Throws<AggregateValidationException>(
				() => ExtendedValidator.EnsureIsValid(model),
				"Object of type Model has some invalid values."
			);
			Assert.That(exception.ValidationResults.Count, Is.EqualTo(1));
			Assert.That(
				exception.ValidationResults.ElementAt(0).ErrorMessage,
				Is.EqualTo("The property Property1 must be unique in the Values collection.")
			);
		}

		[Test]
		public void IsNotValidAndErrorMessageIsChanged()
		{
			var model = new Model();

			model.ValuesErrorMessageChanged = new List<Model._Item>()
			{
					new Model._Item() { Property1 = "hello", Property2 = "world" },
					new Model._Item() { Property1 = "hello", Property2 = "world" },
			};

			Assert.That(ExtendedValidator.IsValid(model), Is.False);
			var exception = Assert.Throws<AggregateValidationException>(
				() => ExtendedValidator.EnsureIsValid(model),
				"Object of type Model has some invalid values."
			);
			Assert.That(exception.ValidationResults.Count, Is.EqualTo(1));
			Assert.That(
				exception.ValidationResults.ElementAt(0).ErrorMessage,
				Is.EqualTo("New error message")
			);
		}

		[Test]
		public void InvalidOperationWithWrongProperty()
		{
			var model = new ModelWithWrongProperty();
			model.Value = 1;

			var exception = Assert.Throws<InvalidOperationException>(
				() => ExtendedValidator.EnsureIsValid(model),
				"The property being validated must be an IEnumerable."
			);
		}
	}
}
