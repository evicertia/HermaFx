using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HermaFx.DataAnnotations
{
	public class RequiredIfHasFlagAttribute : RequiredIfAttribute
	{
		public RequiredIfHasFlagAttribute(string dependentProperty, object flags) : base(dependentProperty, Operator.HasFlag, flags) { }
	}
}
