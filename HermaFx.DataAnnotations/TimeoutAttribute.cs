using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HermaFx.DataAnnotations
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class TimeoutAttribute : Attribute
	{
		private TimeSpan _timeout;

		public TimeSpan Timeout
		{
			get { return _timeout; }
		}

		public TimeoutAttribute()
		{
		}

		public TimeoutAttribute(string expression)
		{
			Guard.IsNotNullNorWhitespace(expression, "expression");

			_timeout = TimeSpan.Parse(expression);
		}

	}
}
