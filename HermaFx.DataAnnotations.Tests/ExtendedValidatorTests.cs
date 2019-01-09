using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

namespace HermaFx.DataAnnotations
{
	[TestFixture]
	public class ExtendedValidatorTests
	{
		public class InnerDto
		{
			[Required]
			public string Field { get; set; }

			[MaxLength(10)]
			public string Field2 { get; set; }
		}

		public class OutterDto
		{
			[Required, MinLength(5)]
			public string AString { get; set; }

			[MaxLength(10)]
			public string BString { get; set; }

			[ValidateObject]
			public InnerDto Inner { get; set; }

			public bool? NullableBool { get; set; }

			[ValidateObject, Required]
			public IEnumerable<InnerDto> InnerList { get; set; }
		}

		private static OutterDto BuildDto(bool valid)
		{
			return new OutterDto()
			{
				AString = valid ? "SomeValue" : null,
				Inner = new InnerDto()
				{
					Field = valid ? "InnerField" : null
				},
				InnerList = new List<InnerDto>()
				{
					new InnerDto()
					{
						Field = valid ? "InnerListField" : null
					}
				}
			};
		}

		[Test]
		public void ValidObjectPassesValidation()
		{
			var dto = BuildDto(true);

			Assert.That(ExtendedValidator.IsValid(dto), Is.True);

		}

		[Test]
		public void InvalidObjectFailsValidation()
		{
			var dto = BuildDto(false);

			var result = ExtendedValidator.Validate(dto);

			Assert.IsNotNull(result);
			Assert.That(result, Has.Length.EqualTo(3));
			Assert.That(result, Has.Exactly(1).Property("MemberNames").Contains("AString"));
			Assert.That(result, Has.Exactly(1).Property("MemberNames").Contains("Inner.Field"));
			Assert.That(result, Has.Exactly(1).Property("MemberNames").Contains("InnerList[0].Field"));
		}

		[Test]
		public void EnsuresIsValidReturnsValidationErrors()
		{
			var dto = BuildDto(false);

			var ex = Assert.Throws<AggregateValidationException>(() =>
			{
				ExtendedValidator.EnsureIsValid(dto);
			});

			Assert.That(ex.ValidationExceptions.ToArray(), Has.Length.EqualTo(3));

			Assert.That(ex.ValidationResult, Is.TypeOf<AggregateValidationResult>());

			var result = (ex.ValidationResult as AggregateValidationResult).Results.ToArray();

			Assert.That(result, Has.Length.EqualTo(3));
			Assert.That(result, Has.Exactly(1).Property("MemberNames").Contains("AString"));
			Assert.That(result, Has.Exactly(1).Property("MemberNames").Contains("Inner.Field"));
			Assert.That(result, Has.Exactly(1).Property("MemberNames").Contains("InnerList[0].Field"));
		}

		[Test]
		public void ValidObjectPassesRequiredOnlyValidation()
		{
			var dto = BuildDto(true);

			Assert.That(ExtendedValidator.IsValid(dto, false), Is.True);
		}

		[Test]
		public void InvalidObjectFailsRequiredOnlyValidation()
		{
			var dto = BuildDto(false);

			var result = ExtendedValidator.Validate(dto, false);

			Assert.IsNotNull(result);
			Assert.That(result, Has.Length.EqualTo(3));
			Assert.That(result, Has.Exactly(1).Property("MemberNames").Contains("AString"));
			Assert.That(result, Has.Exactly(1).Property("MemberNames").Contains("Inner.Field"));
			Assert.That(result, Has.Exactly(1).Property("MemberNames").Contains("InnerList[0].Field"));
		}

		[Test]
		public void InvalidObjectFailsRequiredOnlyValidation2()
		{
			var dto = BuildDto(false);
			dto.AString = "abc"; // Try to fire MinLengthAttribute..

			var result = ExtendedValidator.Validate(dto, false);

			Assert.IsNotNull(result);
			Assert.That(result, Has.Length.EqualTo(2));
			//Assert.That(result, Has.Exactly(1).Property("MemberNames").Contains("AString"));
			Assert.That(result, Has.Exactly(1).Property("MemberNames").Contains("Inner.Field"));
			Assert.That(result, Has.Exactly(1).Property("MemberNames").Contains("InnerList[0].Field"));
		}


		[Test]
		public void InvalidObjectFailsMaxLengthInnerDto()
		{
			var dto = BuildDto(true);
			dto.Inner.Field = null; // Try to fire RequiredAttribute..
			dto.BString = "123456789012345";  // Try to fire MaxLengthAttribute..
			dto.Inner.Field2 = "123456789012345";  // Try to fire MaxLengthAttribute..

			var result = ExtendedValidator.Validate(dto);

			Assert.IsNotNull(result);
			Assert.That(result, Has.Length.EqualTo(3));
			Assert.That(result, Has.Exactly(1).Property("MemberNames").Contains("BString"));
			Assert.That(result, Has.Exactly(1).Property("MemberNames").Contains("Inner.Field"));
			Assert.That(result, Has.Exactly(1).Property("MemberNames").Contains("Inner.Field2"));
		}
	}
}