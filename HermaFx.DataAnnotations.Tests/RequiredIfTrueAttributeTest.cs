
using NUnit.Framework;
using System;
using System.ComponentModel.DataAnnotations;

namespace HermaFx.DataAnnotations
{
    [TestFixture]
    public class RequiredIfTrueAttributeTest
    {
        private class Model : ModelBase<RequiredIfTrueAttribute>
        {
            public bool? Value1 { get; set; }

            [RequiredIfTrue("Value1")]
            public string Value2 { get; set; }
        }

        [Test]
        public void IsValidTest()
        {
            var model = new Model() { Value1 = true, Value2 = "hello" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [Test]
        public void IsNotValidTest()
        {
            var model = new Model() { Value1 = true, Value2 = "" };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        [Test]
        public void IsNotValidWithValue2NullTest()
        {
            var model = new Model() { Value1 = true, Value2 = null };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        [Test]
        public void IsNotRequiredTest()
        {
            var model = new Model() { Value1 = false, Value2 = "" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [Test]
        public void IsNotRequiredWithValue1NullTest()
        {
            var model = new Model() { Value1 = null, Value2 = "" };
            Assert.IsTrue(model.IsValid("Value2"));
        }
    }
}
