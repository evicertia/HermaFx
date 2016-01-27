using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Originally based on BSD Licensed code from NETfx project.
// See: http://netfx.codeplex.com/

namespace HermaFx.RuntimeExtensions
{
	/// <summary>
	/// Provides conversion of DateTime and DateTimeOffset into an epoch-relative number value (total seconds).
	/// See Unix Epoch in Wikipedia for more information on why this might be needed. 
	/// Typical uses include using this simplified representation as an expiration time for a token, password or verification code.
	/// </summary>
	public static class DateTimeEpochExtensions
	{
		/// <summary>
		/// Converts the given date value to epoch/unix time.
		/// </summary>
		/// <param name="dateTime" this="true">The data to convert</param>
		public static long ToUnixTime(this DateTime dateTime)
		{
			var date = dateTime.ToUniversalTime();
			var ts = date - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

			return Convert.ToInt64(ts.TotalSeconds);
		}

		/// <summary>
		/// Converts the given date value to epoch/unix time.
		/// </summary>
		/// <param name="dateTime" this="true">The data to convert</param>
		public static long ToUnixTime(this DateTimeOffset dateTime)
		{
			var date = dateTime.ToUniversalTime();
			var ts = date - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

			return Convert.ToInt64(ts.TotalSeconds);
		}

		/// <summary>
		/// Converts the given epoch time to a <see cref="DateTime"/> with <see cref="DateTimeKind.Utc"/> kind.
		/// </summary>
		/// <param name="secondsSince1970" this="true">The seconds to convert</param>
		public static DateTime ToDateTime(this long secondsSince1970)
		{
			return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(secondsSince1970);
		}

		/// <summary>
		/// Converts the given epoch time to a UTC <see cref="DateTimeOffset"/>.
		/// </summary>
		/// <param name="secondsSince1970" this="true">The seconds to convert</param>
		public static DateTimeOffset ToDateTimeOffset(this long secondsSince1970)
		{
			return new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero).AddSeconds(secondsSince1970);
		}
	}
}
