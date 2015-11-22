using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit;
using NUnit.Framework;

namespace HermaFx.Text
{
	[TestFixture]
	public class ZBase32EncodingTests : EncodingTests
	{
		[Test]
		public void VariableLengthTests()
		{
			base.VariableLengthTests(new ZBase32Encoder());
		}

		[Test]
		public void WordListTests()
		{
			base.WordListTests(new ZBase32Encoder());
		}
	}
}
