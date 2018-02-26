using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit;
using NUnit.Framework;

namespace HermaFx.Functional
{
	public class PatternMatcherTests
	{
		[Test]
		public void PatternMatcher_With_Output()
		{
			//Arrange
			int valueToMatch = 5;
			PatternMatcher<int> patterMatch = new PatternMatcher<int>().
				Case<string>(str => Int32.Parse(str)).
				Case<double>(dble => Convert.ToInt32(dble)).
				Case<int>(integer => integer).
				Default(x => { throw new NotImplementedException(); });

			//Act
			int patternMatcherResult = patterMatch.Match(valueToMatch);

			//Assert
			Assert.AreEqual(valueToMatch, patternMatcherResult);
		}

		[Test]
		public void PatternMatcher_Without_Output()
		{
			//Assert
			List<int> integers = new List<int>();
			List<string> strings = new List<string>();
			List<double> floats = new List<double>();
			PatternMatcher patternMatcher = new PatternMatcher().
				Case<string>(str => strings.Add(str)).
				Case<double>(dbl => floats.Add(dbl)).
				Case<int>(integer => integers.Add(integer)).
				Default(x => { throw new NotImplementedException(); });

			//Act
			patternMatcher.Match(10);
			patternMatcher.Match("string");
			patternMatcher.Match(3.14);

			//Assert
			Assert.AreEqual(1, integers.Count());
			Assert.AreEqual(1, strings.Count());
			Assert.AreEqual(1, floats.Count());
		}
	}
}
