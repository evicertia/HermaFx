using System;
using System.ComponentModel.DataAnnotations;

using NUnit.Framework;

namespace HermaFx.DataAnnotations
{
    [TestFixture]
    public class EqualToAttributeTest
    {
        private class Model : ModelBase<EqualToAttribute>
        {
            public string Value1 { get; set; }

            [EqualTo("Value1")]
            public string Value2 { get; set; }
        }

        private class ModelWithPassOnNull : ModelBase<EqualToAttribute>
        {
            public string Value1 { get; set; }

            [EqualTo("Value1", PassOnNull = true)]
            public string Value2 { get; set; }
        }

        [Test]
        public void IsValid()
        {
            var model = new Model() { Value1 = "hello", Value2 = "hello" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [Test]
        public void IsNotValid()
        {
            var model = new Model() { Value1 = "hello", Value2 = "goodbye" };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        [Test]
        public void IsValidWithNulls()
        {
            var model = new Model() { };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [Test]
        public void IsNotValidWithValue1Null()
        {
            var model = new Model() { Value2 = "hello" };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        [Test]
        public void IsNotValidWithValue2Null()
        {
            var model = new Model() { Value1 = "hello" };
            Assert.IsFalse(model.IsValid("Value2"));
        }    

        [Test]
        public void IsValidWithValue1Null()
        {
            var model = new ModelWithPassOnNull() { Value2 = "hello" };
            Assert.IsTrue(model.IsValid("Value2"));            
        }

        [Test]
        public void IsValidWithValue2Null()
        {
            var model = new ModelWithPassOnNull() { Value1 = "hello" };
            Assert.IsTrue(model.IsValid("Value2"));
        }    
    }
}
