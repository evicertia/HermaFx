#region LICENSE
// Source Code licensed under MS-PL.
// Derived from: MVC Foolproof Validation (http://foolproof.codeplex.com/)  
# endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HermaFx.DataAnnotations
{
	public enum Operator
	{
		EqualTo,
		NotEqualTo,
		GreaterThan,
		LessThan,
		GreaterThanOrEqualTo,
		LessThanOrEqualTo,
		RegExMatch,
		NotRegExMatch
	}
}