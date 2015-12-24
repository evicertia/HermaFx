using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace HermaFx.DataAnnotations
{
	[TestFixture]
	public class MinElementsTests
	{
		public class TestClass
		{
			[MinElements(0)]
			public IEnumerable<int> ZeroElements { get; set; }

			[MinElements(1)]
			public IEnumerable<int> OneElements { get; set; }

			[MinElements(5)]
			public IEnumerable<int> FiveElements { get; set; }
		}

		[Test]
		public void NullElementsTest()
		{
			var obj = new TestClass();
			ExtendedValidator.EnsureIsValid(obj);
		}

		[Test]
		public void ZeroElementsWithEmptyArrayValidates()
		{
			ExtendedValidator.EnsureIsValid(new TestClass()
			{
				ZeroElements = new int[] { }
			});
		}

		[Test]
		public void ZeroElementsWithNonEmptyArrayValidates()
		{
			ExtendedValidator.EnsureIsValid(new TestClass()
			{
				ZeroElements = new int[] { 1 }
			});
		}

		[Test]
		public void ZeroElementsWithNonEmptyListValidates()
		{
			ExtendedValidator.EnsureIsValid(new TestClass()
			{
				ZeroElements = (new int[] { 1, }).ToList()
			});
		}

		[Test]
		public void ZeroElementsWithNonEmptyIEnumerableValidates()
		{
			ExtendedValidator.EnsureIsValid(new TestClass()
			{
				ZeroElements = Enumerable.Range(0, 1)
			});
		}

		[Test]
		public void OneElementsWithEmptyArrayDoesNotValidate()
		{
			Assert.Throws<AggregateValidationException>(() =>
			{
				ExtendedValidator.EnsureIsValid(new TestClass()
				{
					OneElements = new int[] { }
				});
			});
		}

		[Test]
		public void OneElementsWithEmptyListDoesNotValidate()
		{
			Assert.Throws<AggregateValidationException>(() =>
			{
				ExtendedValidator.EnsureIsValid(new TestClass()
				{
					OneElements = (new int[] { }).ToList()
				});
			});
		}

		[Test]
		public void OneElementsWithEmptyIEnumerableDoesNotValidate()
		{
			Assert.Throws<AggregateValidationException>(() =>
			{
				ExtendedValidator.EnsureIsValid(new TestClass()
				{
					OneElements = Enumerable.Range(0, 0)
				});
			});
		}

		[Test]
		public void OneElementsWithSingleItemValidates()
		{
			ExtendedValidator.EnsureIsValid(new TestClass()
			{
				OneElements = new int[] { 1 }
			});
		}

		[Test]
		public void OneElementsWithTwoItemsValidates()
		{
			ExtendedValidator.EnsureIsValid(new TestClass()
			{
				OneElements = new int[] { 1, 2 }
			});
		}

		[Test]
		public void FiveElementsWithFourItemDoesNotValidate()
		{
			Assert.Throws<AggregateValidationException>(() =>
			{
				ExtendedValidator.EnsureIsValid(new TestClass()
				{
					FiveElements = new int[] { 1, 2, 3, 4 }
				});
			});
		}

		[Test]
		public void FiveElementsWithFiveItemValidates()
		{
			ExtendedValidator.EnsureIsValid(new TestClass()
			{
				FiveElements = new int[] { 1, 2, 3, 4, 5 }
			});
		}

		[Test]
		public void FiveElementsWithSixItemsValidates()
		{
			ExtendedValidator.EnsureIsValid(new TestClass()
			{
				OneElements = new int[] { 1, 2, 3, 4, 5, 6 }
			});
		}

	}
}
