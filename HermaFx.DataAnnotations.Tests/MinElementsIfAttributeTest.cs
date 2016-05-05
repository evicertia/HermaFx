
using NUnit.Framework;
using System;

namespace HermaFx.DataAnnotations
{
	[TestFixture]
	public class MinElementsIfAttributeTest
	{
		private class Model : ModelBase<MinElementsIfAttribute>
		{
			public string Value1 { get; set; }

			[MinElementsIf(3, "Value1", "hello")]
			public string[] Value2 { get; set; }
		}

		private class ComplexModel : ModelBase<MinElementsIfAttribute>
		{
			public class SubModel
			{
				public string InnerValue { get; set; }
			}

			public SubModel Value1 { get; set; }

			[MinElementsIf(3, "Value1.InnerValue", "hello")]
			public string[] Value2 { get; set; }
		}

		[Test]
		public void IsValidTest()
		{
			var model = new Model() { Value1 = "hello", Value2 = new string[] { "hello", "hello2", "hello3" } };
			Assert.IsTrue(model.IsValid("Value2"));
		}

		[Test]
		public void IsValidTestComplex()
		{
			var model = new ComplexModel() {Value1 = new ComplexModel.SubModel() {InnerValue = "hello"}, Value2 = new string[] { "bla", "bla2", "bla3" } };
			Assert.IsTrue(model.IsValid("Value2"));
		}

		[Test]
		public void IsNotValidTest()
		{
			var model = new Model() { Value1 = "hello", Value2 = new string[] { "hello", "hello2" } };
			Assert.IsFalse(model.IsValid("Value2"));
		}

		[Test]
		public void IsNotValidTestComplex()
		{
			var model = new ComplexModel() { Value1 = new ComplexModel.SubModel() { InnerValue = "hello" }, Value2 = new string[] { "bla", "bla2" } };
			Assert.IsFalse(model.IsValid("Value2"));
		}

		[Test]
		public void IsValidWithValue2NullTest()
		{
			var model = new Model() { Value1 = "hello", Value2 = null };
			Assert.IsTrue(model.IsValid("Value2"));
		}

		[Test]
		public void IsNotRequiredToHaveMinElementsTest()
		{
			var model = new Model() { Value1 = "goodbye" };
			Assert.IsTrue(model.IsValid("Value2"));
		}

		[Test]
		public void IsNotRequiredToHaveMinElementsWithValue1NullTest()
		{
			var model = new Model() { Value1 = null };
			Assert.IsTrue(model.IsValid("Value2"));
		}
	}
}
