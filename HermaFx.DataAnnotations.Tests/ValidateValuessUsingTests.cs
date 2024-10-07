using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using NUnit.Framework;

namespace HermaFx.DataAnnotations
{
	[TestFixture]
	public class ValidateValuessUsingTests
	{
		public class TestDtoMetadata
		{
			[Regex(@"^[a-z]{4,10}$")]
			private string ValueProperty { get; set; }
		}

		public class TestDto
		{
			[ValidateValuesUsing(typeof(TestDtoMetadata), "ValueProperty")]
			public Dictionary<string, string> TestProperty { get; set; }
		}

		public class BadTypeDto
		{
			[ValidateValuesUsing(typeof(TestDtoMetadata), "ValueProperty")]
			public TestDto TestProperty { get; set; }
		}

		[Test]
		public void BasicTest()
		{
			var @null = new TestDto()
			{
				TestProperty = null
			};
			var empty = new TestDto()
			{
				TestProperty = new Dictionary<string, string>()
			};
			var good = new TestDto()
			{
				TestProperty = new Dictionary<string, string>(new[] { new KeyValuePair<string, string>("dummy", "good") })
			};
			var bad = new TestDto()
			{
				TestProperty = new Dictionary<string, string>(new[] { new KeyValuePair<string, string>("dummy", "bad") })
			};

			ExtendedValidator.EnsureIsValid(good);
			Assert.Throws<AggregateValidationException>(() => ExtendedValidator.EnsureIsValid(bad));
		}

		[Test]
		public void ValidateElementsMandatoryContext()
		{
			var good = new TestDto()
			{
				TestProperty = new Dictionary<string, string>(new[] { new KeyValuePair<string, string>("dummy", "success") })
			};

			var results = new List<ValidationResult>();
			Assert.Throws<ArgumentNullException>(() => Validator.TryValidateObject(good, null, results));
		}

		[Test]
		public void ValidateBadTypeTest()
		{
			var ignore = new BadTypeDto()
			{
				TestProperty = null
			};

			var bad = new BadTypeDto()
			{
				TestProperty = new TestDto()
				{
					TestProperty = new Dictionary<string, string>(new[] { new KeyValuePair<string, string>("ïgnore", "ïgnore") })
				}
			};

			ExtendedValidator.EnsureIsValid(ignore);
			Assert.Throws<ValidationException>(() => ExtendedValidator.EnsureIsValid(bad));
		}
	}
}
