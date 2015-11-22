using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit;
using NUnit.Framework;

namespace HermaFx
{
	[TestFixture]
	public class GuardTests
	{
		[Test]
		public void IsNotDefault()
		{
			Guard.IsNotDefault(1, "#A-0");
			Guard.IsNotDefault(-1, "#A-1");
			Guard.IsNotDefault(0.1, "#A-2");
			Guard.IsNotDefault(-0.1, "#A-3");
			Guard.IsNotDefault(DateTime.Now, "#A-4");
			Guard.IsNotDefault(Guid.NewGuid(), "#A-5");

			Assert.Throws<ArgumentNullException>(() => Guard.IsNotDefault(0, ""), "#B-0");
			Assert.Throws<ArgumentNullException>(() => Guard.IsNotDefault(default(int), ""), "#B-1");
			Assert.Throws<ArgumentNullException>(() => Guard.IsNotDefault(default(DateTime), ""), "#B-2");
			Assert.Throws<ArgumentNullException>(() => Guard.IsNotDefault(Guid.Empty, ""), "#B-3");
		}

		[Test]
		public void IsNotNullNorDefault()
		{
			string emptyString = "";
			string nonEmptyString = "hi";
			string nullString = null;

			Guard.IsNotNullNorDefault(1, "#A-0");
			Guard.IsNotNullNorDefault(-1, "#A-1");
			Guard.IsNotNullNorDefault(0.1, "#A-2");
			Guard.IsNotNullNorDefault(-0.1, "#A-3");
			Guard.IsNotNullNorDefault(DateTime.Now, "#A-4");
			Guard.IsNotNullNorDefault(emptyString, "#A-5");
			Guard.IsNotNullNorDefault(nonEmptyString, "#A-6");
			Guard.IsNotNullNorDefault(Guid.NewGuid(), "#A-7");

			Assert.Throws<ArgumentNullException>(() => Guard.IsNotNullNorDefault(0, ""), "#B-0");
			Assert.Throws<ArgumentNullException>(() => Guard.IsNotNullNorDefault(default(int), ""), "#B-1");
			Assert.Throws<ArgumentNullException>(() => Guard.IsNotNullNorDefault(default(DateTime), ""), "#B-2");
			Assert.Throws<ArgumentNullException>(() => Guard.IsNotNullNorDefault(nullString, ""), "#B-3");
			Assert.Throws<ArgumentNullException>(() => Guard.IsNotNullNorDefault(Guid.Empty, ""), "#B-4");
		}

		[Test]
		public void IsNotNull_Using_Expressions()
		{
			string nullString = null;
			string emptyString = "";

			Guard.IsNotNull(() => emptyString, emptyString);

			Assert.Throws<ArgumentNullException>(() => Guard.IsNotNull(() => nullString, nullString), "#EX-1");
		}
	}
}
