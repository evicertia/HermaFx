﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using NUnit.Framework;

namespace HermaFx.DataAnnotations
{
	[TestFixture]
	public class ValidateElementsUsingTests
	{
		public class TestDtoMetadata
		{
			[MinLength(5)]
			private string StringProperty { get; set; }
		}

		public class TestDto
		{
			[ValidateElementsUsing(typeof(TestDtoMetadata), "StringProperty")]
			public IEnumerable<string> StringList { get; set; }
		}

		public class BadTypeDto
		{
			[ValidateElementsUsing(typeof(TestDtoMetadata), "StringProperty")]
			public TestDto TestProperty { get; set; }
		}

		[Test]
		public void BasicTest()
		{
			var @null = new TestDto()
			{
				StringList = null
			};
			var empty = new TestDto()
			{
				StringList = new string[] { }
			};
			var good = new TestDto()
			{
				StringList = new[] { "abcde" }
			};
			var bad = new TestDto()
			{
				StringList = new[] { "a" }
			};

			ExtendedValidator.EnsureIsValid(@null);
			ExtendedValidator.EnsureIsValid(empty);
			ExtendedValidator.EnsureIsValid(good);
			Assert.Throws<AggregateValidationException>(() => ExtendedValidator.EnsureIsValid(bad));
		}

		[Test]
		public void ValidateElementsMandatoryContext()
		{
			var good = new TestDto()
			{
				StringList = new[] { "abcde" }
			};

			var results = new List<ValidationResult>();
			Assert.Throws<ArgumentNullException>(() => Validator.TryValidateObject(good, null, results));
		}

		[Test]
		public void BadTypeNullTest()
		{
			var ignore = new BadTypeDto()
			{
				TestProperty = null
			};

			var bad = new BadTypeDto()
			{
				TestProperty = new TestDto()
				{
					StringList = new[] { "aaaaaa" }
				}
			};

			ExtendedValidator.EnsureIsValid(ignore);
			Assert.Throws<ValidationException>(() => ExtendedValidator.EnsureIsValid(bad));
		}

	}
}
