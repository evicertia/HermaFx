using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace HermaFx.Utils
{
	public class AdvancedGuidGeneratorTests
	{
		[Test]
		public void BasicCombGuidGeneration()
		{
			var id = AdvancedGuidGenerator.GenerateComb();
			Guid.Parse(id.ToString());
		}
	}
}
