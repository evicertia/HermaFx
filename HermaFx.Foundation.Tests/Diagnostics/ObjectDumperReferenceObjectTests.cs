using System.Linq;
using NUnit.Framework;

namespace HermaFx.Diagnostics
{
	[TestFixture]
	public class ObjectDumperReferenceObjectTests
	{

		[Test]
		public void SimpleReferenceObject()
		{
			var foo = new { Foo = 1 };
			var dumpItemBase = ObjectDumper.ObjectTypeFactory.Create(foo);
			dumpItemBase.ShouldBeOfType(typeof(ObjectDumper.ReferenceObject));

			((ObjectDumper.ReferenceObject)dumpItemBase).Properties.Count().ShouldEqual(1);
		}

		[Test]
		public void SimpleRefWithCollectionProperty()
		{
			var foo = new { Foo = new int[0] };
			var dumpItemBase = (ObjectDumper.ReferenceObject)ObjectDumper.ObjectTypeFactory.Create(foo);
			dumpItemBase.Properties.Count().ShouldEqual(1);
		}
	}
}