using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Originally based on BSD Licensed code from NETfx project.
// See: http://netfx.codeplex.com/

namespace HermaFx
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

	/// <summary>
	/// Provides useful extension methods for DateTime type
	/// </summary>
	public static class DateTimeExtensions
	{
		/// <summary>
		/// Returns the date for the first day of the week for current thread culture.
		/// </summary>
		/// <param name="date">The date.</param>
		/// <returns></returns>
		public static DateTime StartOfWeek(this DateTime date)
		{
			var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
			return date.StartOfWeek(culture);
		}

		/// <summary>
		/// Return the date for the first day of the week for the specified culture.
		/// </summary>
		/// <param name="date">The date.</param>
		/// <param name="culture">The culture.</param>
		/// <returns></returns>
		public static DateTime StartOfWeek(this DateTime date, CultureInfo culture)
		{
			var firstDayOfWeek = culture.DateTimeFormat.FirstDayOfWeek;
			return date.StartOfWeek(firstDayOfWeek);
		}

		/// <summary>
		/// Return the date for the first day of the week.
		/// </summary>
		/// <param name="date">The date.</param>
		/// <param name="startOfWeek">The start of week.</param>
		/// <returns></returns>
		public static DateTime StartOfWeek (this DateTime date, DayOfWeek startOfWeek)
		{
			int diff = date.DayOfWeek - startOfWeek;

			if (diff < 0)
				diff += 7;

			return date.AddDays(-diff).Date;
		}
	}
}
