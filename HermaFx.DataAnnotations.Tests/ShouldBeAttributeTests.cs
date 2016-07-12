using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace HermaFx.DataAnnotations
{
	[TestFixture]
	public class ShouldBeAttributeTests
	{
		public class TestClass
		{
			[ShouldBe(true)]
			public bool TrueBoolean { get; set; }

			[ShouldBe(true)]
			public bool? TrueNullableBoolean { get; set; }

			[ShouldBe(false)]
			public bool FalseBoolean { get; set; }

			[ShouldBe(false)]
			public bool? FalseNullableBoolean { get; set; }

			public TestClass()
			{
				TrueBoolean = true;
				TrueNullableBoolean = true;

				FalseBoolean = false;
				FalseNullableBoolean = false;
			}
		}

		[Test]
		public void Basic_Tests()
		{
			var obj = new TestClass();
			ExtendedValidator.EnsureIsValid(obj);
		}

		[Test]
		public void Expect_Failures_When_Values_Do_Not_Match()
		{
			var obj = new TestClass()
			{
				TrueBoolean = false,
			};
			Assert.Throws<AggregateValidationException>(() => ExtendedValidator.EnsureIsValid(obj));
		}

		[Test]
		public void Ignore_Null_NotRequired_Properties()
		{
			var obj = new TestClass()
			{
				TrueNullableBoolean = null,
				FalseNullableBoolean = null
			};
			ExtendedValidator.EnsureIsValid(obj);
		}

	}
}
