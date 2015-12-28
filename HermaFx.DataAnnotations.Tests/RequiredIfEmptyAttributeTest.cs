
using NUnit.Framework;
using System;

namespace HermaFx.DataAnnotations
{
    [TestFixture]
    public class RequiredIfEmptyAttributeTest
    {
        private class Model : ModelBase<RequiredIfEmptyAttribute>
        {
            public string Value1 { get; set; }

            [RequiredIfEmpty("Value1")]
            public string Value2 { get; set; }
        }

        [Test]
        public void IsValidTest()
        {
            var model = new Model() { Value1 = "", Value2 = "hello" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [Test]
        public void IsValidWithValue1NullTest()
        {
            var model = new Model() { Value1 = null, Value2 = "hello" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [Test]
        public void IsNotRequiredTest()
        {
            var model = new Model() { Value1 = "hello", Value2 = "" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [Test]
        public void IsNotRequiredWithValue2NullTest()
        {
            var model = new Model() { Value1 = "hello", Value2 = null };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [Test]
        public void IsNotValidTest()
        {
            var model = new Model() { Value1 = "", Value2 = "" };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        [Test]
        public void IsNotValidWithvalue1NullTest()
        {
            var model = new Model() { Value1 = null, Value2 = "" };
            Assert.IsFalse(model.IsValid("Value2"));
        }    
    }
}
