using System;
using System.IO;

using NUnit.Framework;

namespace HermaFx.IO
{
	[TestFixture]
	public class TempFileTests
	{
		[Test]
		public void CanCreateTempFile()
		{
			var obj = new TempFile();

			Assert.That(obj, Is.Not.Null, "#1");
			Assert.That(obj.FullPath, Is.Not.Null.Or.Empty, "#2");
			Assert.That(obj.ToString(), Is.Not.Null.Or.Empty, "#3");
			Assert.That((string)obj, Is.Not.Null.Or.Empty, "#4");
			Assert.That(obj.ToString(), Is.EqualTo(obj.FullPath), "#5");
			Assert.That((string)obj, Is.EqualTo(obj.FullPath), "#6");

			Assert.That(File.Exists(obj.FullPath), Is.True, "#7");

			Assert.DoesNotThrow(() => obj.Dispose(), "#E1");
			Assert.DoesNotThrow(() => obj.Dispose(), "#E2"); //< XXX: Try it twice..
		}

		[Test]
		public void CanCreateTempFileFromStream()
		{
			var data = new byte[] { 0x1, 0x2, 0x3, 0x4, 0x5 };

			using (var ms = new MemoryStream(data))
			using (var obj = new TempFile(ms))
			{
				Assert.That(File.ReadAllBytes(obj.FullPath), Is.EquivalentTo(data));
			}
		}
	}
}
