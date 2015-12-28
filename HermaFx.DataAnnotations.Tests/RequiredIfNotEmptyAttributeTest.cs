
using NUnit.Framework;
using System;

namespace HermaFx.DataAnnotations
{
    [TestFixture]
    public class RequiredIfNotEmptyAttributeTest
    {
        private class Model : ModelBase<RequiredIfNotEmptyAttribute>
        {
            public string Value1 { get; set; }

            [RequiredIfNotEmpty("Value1")]
            public string Value2 { get; set; }
        }

        [Test]
        public void IsValidTest()
        {
            var model = new Model() { Value1 = "hello", Value2 = "hello" };
            Assert.IsTrue(model.IsValid("Value2"));
        }
        
        [Test]
        public void IsNotRequiredTest()
        {
            var model = new Model() { Value1 = "", Value2 = "" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [Test]
        public void IsNotRequiredWithValue1NullTest()
        {
            var model = new Model() { Value1 = null, Value2 = null };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [Test]
        public void IsNotValidTest()
        {
            var model = new Model() { Value1 = "hello", Value2 = "" };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        [Test]
        public void IsNotValidWithvalue2NullTest()
        {
            var model = new Model() { Value1 = "hello", Value2 = null };
            Assert.IsFalse(model.IsValid("Value2"));
        }    
    }
}
