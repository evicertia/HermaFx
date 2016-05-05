#region LICENSE
// Source Code licensed under MS-PL.
// Derived from: MVC Foolproof Validation (http://foolproof.codeplex.com/)  
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HermaFx.DataAnnotations
{
	public class MinElementsIfNotAttribute : MinElementsIfAttribute
	{
		public MinElementsIfNotAttribute(uint elements, string dependentProperty, object dependentValue) : base(elements, dependentProperty, Operator.NotEqualTo, dependentValue) { }
	}
}
