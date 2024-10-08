using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using NUnit.Framework;

namespace HermaFx.DataAnnotations
{
	[TestFixture]
	public class ValidateKeysUsingTests
	{
		public class TestDtoMetadata
		{
			[Regex(@"^[a-z]{4,10}$")]
			private string KeyProperty { get; set; }
		}

		public class TestDto
		{
			[ValidateKeysUsing(typeof(TestDtoMetadata), "KeyProperty")]
			public Dictionary<string, string> TestProperty { get; set; }
		}

		public class BadTypeDto
		{
			[ValidateKeysUsing(typeof(TestDtoMetadata), "KeyProperty")]
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
				TestProperty = new Dictionary<string, string>(new[] { new KeyValuePair<string, string>("good", "dummy") })
			};
			var bad = new TestDto()
			{
				TestProperty = new Dictionary<string, string>(new[] { new KeyValuePair<string, string>("bad", "dummy") })
			};

			ExtendedValidator.EnsureIsValid(good);
			Assert.Throws<AggregateValidationException>(() => ExtendedValidator.EnsureIsValid(bad));
		}

		[Test]
		public void ValidateElementsMandatoryContext()
		{
			var good = new TestDto()
			{
				TestProperty = new Dictionary<string, string>(new[] { new KeyValuePair<string, string>("success", "dummy") })
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
