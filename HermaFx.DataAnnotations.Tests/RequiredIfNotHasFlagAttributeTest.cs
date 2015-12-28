using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace HermaFx.DataAnnotations
{
	[TestFixture]
	public class RequiredIfNotHasFlagAttributeTest
	{
		[Flags]
		public enum AnEnum
		{
			A = (1<<0),
			B = (1<<1),
			C = (1<<2)
		}

		class Model : ModelBase<RequiredIfNotHasFlagAttribute>
		{
			public AnEnum Value1 { get; set; }

			[RequiredIfNotHasFlag("Value1", (AnEnum.A | AnEnum.B))]
			public string Value2 { get; set; }
		}

		[Test]
		public void IsValidTest()
		{
			var model = new Model() { Value1 = AnEnum.C, Value2 = "hello" };
			Assert.IsTrue(model.IsValid("Value2"));
		}

		[Test]
		public void IsValid2Test()
		{
			var model = new Model() { Value1 = AnEnum.B, Value2 = null };
			Assert.IsTrue(model.IsValid("Value2"));
		}

		[Test]
		public void IsNotValidTest()
		{
			var model = new Model() { Value1 = AnEnum.C, Value2 = null };
			Assert.IsFalse(model.IsValid("Value2"));
		}
	}
}
