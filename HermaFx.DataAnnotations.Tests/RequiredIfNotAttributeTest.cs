
using NUnit.Framework;
using System;

namespace HermaFx.DataAnnotations
{
    [TestFixture]
    public class RequiredIfNotAttributeTest
    {
        private class Model : ModelBase<RequiredIfNotAttribute>
        {
            public string Value1 { get; set; }

            [RequiredIfNot("Value1", "hello")]
            public string Value2 { get; set; }
        }

        [Test]
        public void IsValidTest()
        {
            var model = new Model() { Value1 = "goodbye", Value2 = "hello" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [Test]
        public void IsNotValidTest()
        {
            var model = new Model() { Value1 = "goodbye", Value2 = "" };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        [Test]
        public void IsNotValidWithValue2NullTest()
        {
            var model = new Model() { Value1 = "goodbye", Value2 = null };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        [Test]
        public void IsNotRequiredTest()
        {
            var model = new Model() { Value1 = "hello" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [Test]
        public void IsRequiredWithValue1NullTest()
        {
            var model = new Model() { Value1 = null };
            Assert.IsFalse(model.IsValid("Value2"));
        }
    }
}
