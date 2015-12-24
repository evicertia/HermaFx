using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace HermaFx.DataAnnotations
{
	/// <summary>
	/// Just like RegularExpressionAttribute, but allowing multiple instances.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true)]
	public class RegexAttribute : RegularExpressionAttribute
	{
		public RegexAttribute(string pattern)
			: base(pattern)
		{
		}
	}
}
