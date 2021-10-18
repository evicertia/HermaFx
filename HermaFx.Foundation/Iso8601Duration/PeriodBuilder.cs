using System;
using System.Text.RegularExpressions;

namespace HermaFx.Iso8601Duration
{
	/// <summary>
	/// Code extracted/obtained/modified from https://github.com/J0rgeSerran0/Iso8601Duration
	/// We took project to HermaFx to avoid dependencies on netstandard framework, which is the one used in the original library, and its causing some problems when referencing on MONO/UNIX.
	/// IMPORTANT: Use HermaFx.Iso8601Duration.DateTimeExtensions to perform DateTime calculations in order to avoid losing precission when converting the ISO6801 expression to TimeSpan.
	/// </summary>
	public class PeriodBuilder
	{
		private const string EXCEPTION_GENERIC_ERROR = "A general error has occurred";
		private const string EXCEPTION_PATTERN_NOT_VALID = "The pattern text is not valid";

		/**
		 * Regex Explain:
		 * ^P: Ensure patterns starts with P
		 * ((?<years>\d*)Y): Capture a group of digits named years for a expression that ends with Y.
		 *	Do the same with every group (years, months, weeks, days, hours, minutes and seconds)
		 * (T((?<hours>\d*)H)?...) : Time is set after the T group
		 *
		 * INFO: ISO8601 allows decimal digits on Duration expressions, but since we're using int we'll stick to natural numbers
		 */
		private const string DURATION_EXPRESSION_REGEX = @"^P((?<years>\d*)Y)?((?<months>\d*)M)?((?<weeks>\d*)W)?((?<days>\d*)D)?(T((?<hours>\d*)H)?((?<minutes>\d*)M)?((?<seconds>\d*)S)?)?$";

		private int CalculateDays(int years, int months, int days)
		{
			int daysInMonths;
			if (months > Constants.MONTHS_PER_YEAR)
			{
				daysInMonths = Constants.DAYS_PER_YEAR * (months / Constants.MONTHS_PER_YEAR);
				int daysInRest = months - ((months / Constants.MONTHS_PER_YEAR) * Constants.MONTHS_PER_YEAR);
				daysInMonths += daysInRest * Constants.DAYS_PER_MONTH;
			}
			else
			{
				daysInMonths = months * Constants.DAYS_PER_MONTH;
			}

			return (years * Constants.DAYS_PER_YEAR) + daysInMonths + days;
		}

		private string GetPatternFromDurationStruct(DurationStruct durationStruct)
		{
			string pattern = Constants.TAG_PERIOD;

			if (durationStruct.Seconds >= Constants.SECONDS_PER_MINUTE)
			{
				int seconds = durationStruct.Seconds / Constants.SECONDS_PER_MINUTE;
				durationStruct.Minutes += seconds;
				durationStruct.Seconds -= seconds * Constants.SECONDS_PER_MINUTE;
			}

			if (durationStruct.Minutes >= Constants.MINUTES_PER_HOUR)
			{
				int hours = durationStruct.Minutes / Constants.MINUTES_PER_HOUR;
				durationStruct.Hours += hours;
				durationStruct.Minutes -= hours * Constants.MINUTES_PER_HOUR;
			}

			if (durationStruct.Hours >= Constants.HOURS_PER_DAY)
			{
				int days = durationStruct.Hours / Constants.HOURS_PER_DAY;
				durationStruct.Days += days;
				durationStruct.Hours -= days * Constants.HOURS_PER_DAY;
			}

			if (durationStruct.Days >= Constants.DAYS_PER_MONTH)
			{
				int years = durationStruct.Days / Constants.DAYS_PER_YEAR;
				int days = durationStruct.Days - (years * Constants.DAYS_PER_YEAR);
				int months = (days / Constants.DAYS_PER_MONTH);

				durationStruct.Years += years;
				durationStruct.Months += months;
				durationStruct.Days -= (years * Constants.DAYS_PER_YEAR) + (months * Constants.DAYS_PER_MONTH);
			}

			if (durationStruct.Months >= Constants.MONTHS_PER_YEAR)
			{
				int years = durationStruct.Months / Constants.MONTHS_PER_YEAR;
				int months = durationStruct.Months - (years * Constants.MONTHS_PER_YEAR);

				durationStruct.Years += years;
				durationStruct.Months = months;
			}

			pattern += (durationStruct.Years > 0 ? durationStruct.Years + Constants.TAG_YEARS : string.Empty);
			pattern += (durationStruct.Months > 0 ? durationStruct.Months + Constants.TAG_MONTHS : string.Empty);
			pattern += (durationStruct.Days > 0 ? durationStruct.Days + Constants.TAG_DAYS : string.Empty);

			var patternTime = string.Empty;
			patternTime += (durationStruct.Hours > 0 ? durationStruct.Hours + Constants.TAG_HOURS : string.Empty);
			patternTime += (durationStruct.Minutes > 0 ? durationStruct.Minutes + Constants.TAG_MINUTES : string.Empty);
			patternTime += (durationStruct.Seconds > 0 ? durationStruct.Seconds + Constants.TAG_SECONDS : string.Empty);

			if (!string.IsNullOrEmpty(patternTime))
			{
				pattern += Constants.TAG_TIME + patternTime;
			}

			return pattern;
		}

		private DurationStruct TimeSpanToDurationStruct(TimeSpan timeSpan)
		{
			return new DurationStruct()
			{
				Days = timeSpan.Days,
				Hours = timeSpan.Hours,
				Minutes = timeSpan.Minutes,
				Seconds = timeSpan.Seconds
			};
		}

		public string NormalizeDuration(string pattern)
		{
			return ToString(ToDurationStruct(pattern));
		}

		private Tuple<int, int> CalculatePeriodValues(int initialValue, int constantValue)
		{
			var upperValue = initialValue / constantValue;
			var lowerValue = ((initialValue / constantValue) * constantValue);

			return new Tuple<int, int>(upperValue, lowerValue);
		}

		public DurationStruct ToDurationStruct(TimeSpan timeSpan)
		{
			try
			{
				var durationStruct = TimeSpanToDurationStruct(timeSpan);

				if (timeSpan.Days >= Constants.DAYS_PER_YEAR)
				{
					var periodValues = CalculatePeriodValues(timeSpan.Days, Constants.DAYS_PER_YEAR);
					durationStruct.Years = periodValues.Item1;
					durationStruct.Days -= periodValues.Item2;
				}

				if (durationStruct.Days >= Constants.DAYS_PER_MONTH)
				{
					var periodValues = CalculatePeriodValues(durationStruct.Days, Constants.DAYS_PER_MONTH);
					durationStruct.Months = periodValues.Item1;
					durationStruct.Days -= periodValues.Item2;
				}

				if (durationStruct.Months >= Constants.MONTHS_PER_YEAR)
				{
					var periodValues = CalculatePeriodValues(durationStruct.Months, Constants.MONTHS_PER_YEAR);
					durationStruct.Years += periodValues.Item1;
					durationStruct.Months -= periodValues.Item2;
				}

				return durationStruct;
			}
			catch (Exception ex)
			{
				throw new Iso8601DurationException(EXCEPTION_GENERIC_ERROR, ex);
			}
		}

		public string ToString(TimeSpan timeSpan)
		{
			try
			{
				if (timeSpan == TimeSpan.Zero)
				{
					return Constants.TAG_ZERO;
				}

				var durationStruct = TimeSpanToDurationStruct(timeSpan);

				return GetPatternFromDurationStruct(durationStruct);
			}
			catch (Exception ex)
			{
				throw new Iso8601DurationException(EXCEPTION_GENERIC_ERROR, ex);
			}
		}

		public string ToString(DurationStruct durationStruct)
		{
			return GetPatternFromDurationStruct(durationStruct);
		}

		public DurationStruct ToDurationStruct(string pattern)
		{
			if (pattern.IsNullOrEmpty())
				return new DurationStruct();

			if (pattern == Constants.TAG_PERIOD // ISO8601 doesn't allow "P" to indicate a TimeSpan of 0
				|| pattern.EndsWith(Constants.TAG_TIME)) // Ensure time pattern contains any data
				throw new Iso8601DurationException(EXCEPTION_PATTERN_NOT_VALID);

			try
			{
				var regex = new Regex(DURATION_EXPRESSION_REGEX);
				var match = regex.Match(pattern);
				if (!match.Success)
					throw new Iso8601DurationException(EXCEPTION_PATTERN_NOT_VALID);

				var years = match.Groups["years"];
				var months = match.Groups["months"];
				var weeks = match.Groups["weeks"];
				var days = match.Groups["days"];
				var hours = match.Groups["hours"];
				var minutes = match.Groups["minutes"];
				var seconds = match.Groups["seconds"];

				var durationStruct = new DurationStruct();
				if (years.Length > 0)
					durationStruct.Years = int.Parse(years.Value);
				if (months.Length > 0)
					durationStruct.Months = int.Parse(months.Value);
				// Convert Weeks to Days to avoid precission loss when doing calculations with DateTimes
				if (weeks.Length > 0)
					durationStruct.Days += int.Parse(weeks.Value) * Constants.DAYS_PER_WEEK;
				if (days.Length > 0)
					durationStruct.Days += int.Parse(days.Value);
				if (hours.Length > 0)
					durationStruct.Hours = int.Parse(hours.Value);
				if (minutes.Length > 0)
					durationStruct.Minutes = int.Parse(minutes.Value);
				if (seconds.Length > 0)
					durationStruct.Seconds = int.Parse(seconds.Value);

				return durationStruct;
			}
			catch (Exception ex)
			{
				throw new Iso8601DurationException(EXCEPTION_GENERIC_ERROR, ex);
			}
		}

		public TimeSpan ToTimeSpan(string pattern)
		{
			return ToTimeSpan(ToDurationStruct(pattern));
		}

		public TimeSpan ToTimeSpan(DurationStruct durationStruct)
		{
			durationStruct.Days = CalculateDays(durationStruct.Years, durationStruct.Months, durationStruct.Days);

			return new TimeSpan(durationStruct.Days, durationStruct.Hours, durationStruct.Minutes, durationStruct.Seconds);
		}
	}
}
