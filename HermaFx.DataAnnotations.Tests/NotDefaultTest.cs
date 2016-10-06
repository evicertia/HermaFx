
using NUnit.Framework;
using System;

namespace HermaFx.DataAnnotations
{
	[TestFixture]
	public class NotDefaultTest
	{
		private class Model
		{
			[NotDefault]
			public Guid Value { get; set; }
		}

		[Test]
		public void IsValid()
		{
			var model = new Model() { Value = Guid.NewGuid() };
			ExtendedValidator.EnsureIsValid(model);
		}

		[Test]
		public void IsNotValid()
		{
			var model = new Model() { Value = default(Guid) };
			Assert.Throws<AggregateValidationException>(() => ExtendedValidator.EnsureIsValid(model));
		}

		[Test]
		public void IsNotValidWithNulls()
		{
			var model = new Model() { };
			Assert.Throws<AggregateValidationException>(() => ExtendedValidator.EnsureIsValid(model));
		}

	}
}
