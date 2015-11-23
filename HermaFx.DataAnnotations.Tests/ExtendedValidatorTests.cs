using System;
using System.ComponentModel.DataAnnotations;

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
            [ValidateObject]
            public InnerDto Inner { get; set;  }
        }

        [Test]
        public void ValidatesInnerDto()
        {
            var dto = new OutterDto()
            {
                Inner = new InnerDto()
                {
                    Field = null
                }
            };

            var result = ExtendedValidator.Validate(dto);

            Assert.IsNotNull(result);

        }
    }
}