using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HermaFx.DataAnnotations
{
	public class RequiredIfNotHasFlagAttribute : RequiredIfAttribute
	{
		public RequiredIfNotHasFlagAttribute(string dependentProperty, object flags) : base(dependentProperty, Operator.NotHasFlag, flags) { }
	}
}
