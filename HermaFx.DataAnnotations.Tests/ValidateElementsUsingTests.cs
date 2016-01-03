using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace HermaFx.DataAnnotations
{
	[TestFixture]
	public class ValidateElementsUsingTests
	{
		public class TestDtoMetadata
		{
			[MinLength(5)]
			string StringProperty { get; set; }
		}

		public class TestDto
		{
			[ValidateElementsUsing(typeof(TestDtoMetadata), "StringProperty")]
			public IEnumerable<string> StringList { get; set; }
		}

		[Test]
		public void BasicTest()
		{
			var good = new TestDto()
			{
				StringList = new[] { "abcde" }
			};
			var bad = new TestDto()
			{
				StringList = new[] { "a" }
			};

			ExtendedValidator.EnsureIsValid(good);
			Assert.Throws<AggregateValidationException>(() => ExtendedValidator.EnsureIsValid(bad));
		}

	}
}
