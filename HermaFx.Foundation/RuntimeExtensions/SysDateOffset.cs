using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HermaFx
{
	public static class SysDateOffset
	{
		public static Func<DateTimeOffset> NowDelegate { get; set; }
		public static Func<DateTimeOffset> UtcNowDelegate { get; set; }

		public static DateTimeOffset Now
		{
			get
			{
				if (NowDelegate == null)
					return DateTimeOffset.Now;

				return NowDelegate();
			}
		}

		public static DateTimeOffset UtcNow
		{
			get
			{
				if (UtcNowDelegate == null)
					return DateTimeOffset.UtcNow;

				return UtcNowDelegate();
			}
		}
	}
}
