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
		public const string DEFAULT_TIMEOUT = "00:01:00";
		private TimeSpan _timeout = TimeSpan.Parse(DEFAULT_TIMEOUT);

		public TimeSpan Timeout
		{
			get { return _timeout; }
		}

		public TimeoutAttribute()
		{
		}

		public TimeoutAttribute(string expression)
		{
			_timeout = TimeSpan.Parse(expression);
		}

	}
}
