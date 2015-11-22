using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit;
using NUnit.Framework;

namespace HermaFx.Text
{
	[TestFixture]
	public class Base32EncodingTests : EncodingTests
	{
		[Test]
		public void VariableLengthTests()
		{
			base.VariableLengthTests(new Base32Encoder());
		}

		[Test]
		public void WordListTests()
		{
			base.WordListTests(new Base32Encoder());
		}
	}
}
