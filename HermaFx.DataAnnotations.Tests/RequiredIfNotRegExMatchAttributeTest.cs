
using NUnit.Framework;
using System;

namespace HermaFx.DataAnnotations
{    
    [TestFixture]
    public class RequiredIfNotRegExMatchAttributeTest
    {
        private class Model : ModelBase<RequiredIfNotRegExMatchAttribute>
        {
            public string Value1 { get; set; }

            [RequiredIfNotRegExMatch("Value1", "^ *(1[0-2]|0?[1-9]):[0-5][0-9] *(a|p|A|P)(m|M) *$")]
            public string Value2 { get; set; }
        }

        [Test]
        public void IsValidTest()
        {
            var model = new Model() { Value1 = "not a time", Value2 = "hello" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [Test]
        public void IsNotValidTest()
        {
            var model = new Model() { Value1 = "not a time", Value2 = "" };
            Assert.IsFalse(model.IsValid("Value2"));
        }
    }
}
