using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HermaFx.DataAnnotations
{
	public class MaxElementsIfHasFlagAttribute : MaxElementsIfAttribute
	{
		public MaxElementsIfHasFlagAttribute(uint elements, string dependentProperty, object flags) : base(elements, dependentProperty, Operator.HasFlag, flags) { }
	}
}
