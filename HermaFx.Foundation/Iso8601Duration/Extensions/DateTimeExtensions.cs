using System;

namespace HermaFx.Iso8601Duration
{
	public static class DateTimeExtensions
	{
		public static DateTime Add(this DateTime date, string durationPattern)
		{
			var duration = new PeriodBuilder().ToDurationStruct(durationPattern);
			return date
				.AddYears(duration.Years)
				.AddMonths(duration.Months)
				.AddDays(duration.Days)
				.AddHours(duration.Hours)
				.AddMinutes(duration.Minutes)
				.AddSeconds(duration.Seconds);
		}

		public static DateTime Add(this DateTime date, DurationStruct duration)
		{
			return date
				.AddYears(duration.Years)
				.AddMonths(duration.Months)
				.AddDays(duration.Days)
				.AddHours(duration.Hours)
				.AddMinutes(duration.Minutes)
				.AddSeconds(duration.Seconds);
		}
	}
}
