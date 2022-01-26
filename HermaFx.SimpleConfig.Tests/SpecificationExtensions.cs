using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;

namespace HermaFx.SimpleConfig.Tests
{
    public static class SpecificationExtensions
    {
        public static void ShouldHaveCount<T>(this IEnumerable<T> sequence, int expectedCount)
        {
            sequence.ShouldNotBeNull();
            var count = sequence.Count();
            count.ShouldEqual(expectedCount);
        }
    }
}
