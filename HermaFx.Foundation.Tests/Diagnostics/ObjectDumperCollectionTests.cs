using System.Linq;
using NUnit.Framework;

namespace HermaFx.Diagnostics
{
    public class ObjectDumperCollectionTests
    {
        [Test]
        public void CollectionWithItems()
        {
            var items = new[] { 1, 2, 3 };
			var dumpItemBase = (ObjectDumper.CollectionObject) ObjectDumper.ObjectTypeFactory.Create(items);
            dumpItemBase.Children.Count().ShouldEqual(3);
        }
    }
}