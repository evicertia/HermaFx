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
		}

		public class OutterDto
		{
			[Required]
			public string AString { get; set; }

			[ValidateObject]
			public InnerDto Inner { get; set; }

			[ValidateObject, Required]
			public IEnumerable<InnerDto> InnerList { get; set; }
		}

		[Test]
		public void ValidatesInnerDto()
		{
			var dto = new OutterDto()
			{
				AString = null,
				Inner = new InnerDto()
				{
					Field = null
				},
				InnerList = new List<InnerDto>()
				{
					new InnerDto()
					{
						Field = null
					}
				}
			};

			var result = ExtendedValidator.Validate(dto);

			Assert.IsNotNull(result);
			Assert.That(result, Has.Length.EqualTo(3));
			Assert.That(result, Has.Exactly(1).Property("MemberNames").Contains("AString"));
			Assert.That(result, Has.Exactly(1).Property("MemberNames").Contains("Inner.Field"));
			Assert.That(result, Has.Exactly(1).Property("MemberNames").Contains("InnerList[0].Field"));
		}
	}
}