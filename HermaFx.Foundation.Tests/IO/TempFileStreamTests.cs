using System;
using System.IO;

using NUnit.Framework;

namespace HermaFx.IO
{
	[TestFixture]
	public class TempFileStreamTests
	{
		[Test]
		public void CanCreateTempFileStream()
		{
			var obj = new TempFileStream();

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
		public void CanWriteToTempFileStream()
		{
			var data = new byte[] { 0x1, 0x2, 0x3, 0x4, 0x5 };

			using (var obj = new TempFileStream(FileShare.ReadWrite))
			{
				obj.Write(data, 0, data.Length);
				obj.Flush();

				// I am using Open() instead of File.OpenRead or File.ReadAllBytes()
				// case we need to pass FileShare.ReadWrite for this to work on win32.
				using (var stream = File.Open(obj.FullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
				{
					var bytes = stream.ReadAllBytes();
					Assert.That(bytes, Is.EquivalentTo(data));
				}
			}
		}
	}
}
