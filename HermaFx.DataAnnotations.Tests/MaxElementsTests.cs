using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace HermaFx.DataAnnotations
{
	[TestFixture]
	public class MaxElementsTests
	{
		public class TestModel
		{
			[MaxElements(0)]
			public IEnumerable<int> ZeroElements { get; set; }

			[MaxElements(1)]
			public IEnumerable<int> OneElements { get; set; }

			[MaxElements(5)]
			public IEnumerable<int> FiveElements { get; set; }
		}

		[Test]
		public void NullElementsTest()
		{
			var obj = new TestModel();
			ExtendedValidator.EnsureIsValid(obj);
		}

		[Test]
		public void ZeroElementsWithEmptyArrayValidates()
		{
			ExtendedValidator.EnsureIsValid(new TestModel
			{
				ZeroElements = new int[] { }
			});
		}

		[Test]
		public void ZeroElementsWithNonEmptyArrayDoesNotValidate()
		{
			Assert.Throws<AggregateValidationException>(() =>
			{
				ExtendedValidator.EnsureIsValid(new TestModel
				{
					ZeroElements = new int[] { 1 }
				});
			});
		}

		[Test]
		public void ZeroElementsWithNonEmptyListDoesNotValidate()
		{
			Assert.Throws<AggregateValidationException>(() =>
			{
				ExtendedValidator.EnsureIsValid(new TestModel
				{
					ZeroElements = (new int[] { 1, }).ToList()
				});
			});
		}

		[Test]
		public void ZeroElementsWithNonEmptyIEnumerableDoesNotValidate()
		{
			Assert.Throws<AggregateValidationException>(() =>
			{
				ExtendedValidator.EnsureIsValid(new TestModel
				{
					ZeroElements = Enumerable.Range(0, 1)
				});
			});
		}

		[Test]
		public void OneElementsWithEmptyArrayValidates()
		{
			ExtendedValidator.EnsureIsValid(new TestModel
			{
				OneElements = new int[] { }
			});
		}

		[Test]
		public void OneElementsWithEmptyListValidates()
		{
			ExtendedValidator.EnsureIsValid(new TestModel
			{
				OneElements = (new int[] { }).ToList()
			});
		}

		[Test]
		public void OneElementsWithEmptyIEnumerableValidates()
		{
			ExtendedValidator.EnsureIsValid(new TestModel
			{
				OneElements = Enumerable.Range(0, 0)
			});
		}

		[Test]
		public void OneElementsWithSingleItemValidates()
		{
			ExtendedValidator.EnsureIsValid(new TestModel
			{
				OneElements = new int[] { 1 }
			});
		}

		[Test]
		public void OneElementsWithTwoItemsDoesNotValidate()
		{
			Assert.Throws<AggregateValidationException>(() =>
			{
				ExtendedValidator.EnsureIsValid(new TestModel
				{
					OneElements = new int[] { 1, 2 }
				});
			});
		}

		[Test]
		public void FiveElementsWithFourItemValidates()
		{
			ExtendedValidator.EnsureIsValid(new TestModel
			{
				FiveElements = new int[] { 1, 2, 3, 4 }
			});
		}

		[Test]
		public void FiveElementsWithFiveItemValidates()
		{
			ExtendedValidator.EnsureIsValid(new TestModel
			{
				FiveElements = new int[] { 1, 2, 3, 4, 5 }
			});
		}

		[Test]
		public void FiveElementsWithSixItemsDoesNotValidate()
		{
			Assert.Throws<AggregateValidationException>(() =>
			{
				ExtendedValidator.EnsureIsValid(new TestModel
				{
					OneElements = new int[] { 1, 2, 3, 4, 5, 6 }
				});
			});
		}

	}
}
