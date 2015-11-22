using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HermaFx
{
	public static class SysDate
	{
		public static Func<DateTime> NowDelegate { get; set; }
		public static Func<DateTime> UtcNowDelegate { get; set; }

		public static DateTime Now
		{
			get
			{
				if (NowDelegate == null)
					return DateTime.Now;

				return NowDelegate();
			}
		}


		public static DateTime UtcNow
		{
			get
			{
				if (UtcNowDelegate == null)
					return DateTime.UtcNow;

				return UtcNowDelegate();
			}
		}
	}
}
