using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HermaFx.DataAnnotations
{
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public class RequiredNotNullAttribute : RequiredAttribute
	{
		public RequiredNotNullAttribute()
		{
			this.AllowEmptyStrings = false;
		}
	}
}
