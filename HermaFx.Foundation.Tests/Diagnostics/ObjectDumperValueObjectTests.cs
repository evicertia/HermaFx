using System;
using NUnit.Framework;

namespace HermaFx.Diagnostics
{
	[TestFixture]
	public class ObjectDumperValueObjectTests
	{
		[Test]
		[TestCase(typeof(ObjectDumper.ValueObject), 1, "1")]
		[TestCase(typeof(ObjectDumper.ValueObject), "FOO", "FOO")]
		[TestCase(typeof(ObjectDumper.ValueObject), null, ObjectDumper.DumpItemBase.NullValue)]
		public void ValueTests(Type expectedType, object itemValue, string expectedTextValue)
		{
			var dumpItem = ObjectDumper.ObjectTypeFactory.Create(itemValue);
			dumpItem.ShouldBeOfType(expectedType);

			dumpItem.Value.ShouldEqual(expectedTextValue);
		}
	}
}