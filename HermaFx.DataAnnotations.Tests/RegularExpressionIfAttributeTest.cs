
using NUnit.Framework;
using System;

namespace HermaFx.DataAnnotations
{    
    [TestFixture]
    public class RegularExpressionIfAttributeTest
    {
        private class Model : ModelBase<RegularExpressionIfAttribute>
        {
            public bool Value1 { get; set; }

            [RegularExpressionIf("^ *(1[0-2]|0?[1-9]):[0-5][0-9] *(a|p|A|P)(m|M) *$", "Value1", true)]
            public string Value2 { get; set; }
        }

        [Test]
        public void IsValidTest()
        {
            var model = new Model() { Value1 = true, Value2 = "8:30 AM" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [Test]
        public void IsNotValidTest()
        {
            var model = new Model() { Value1 = true, Value2 = "not a time" };
            Assert.IsFalse(model.IsValid("Value2"));
        }    
    }
}
