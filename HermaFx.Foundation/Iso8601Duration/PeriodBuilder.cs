using System;

namespace HermaFx.Iso8601Duration
{
	/// <summary>
	/// Code extracted/obtained from https://github.com/J0rgeSerran0/Iso8601Duration
	/// We took project to HermaFx to avoid dependencies on netstandard framework, which is the one used in the original library, and its causing some problems when referencing on MONO/UNIX.
	/// </summary>
	public class PeriodBuilder
	{
		private const string EXCEPTION_GENERIC_ERROR = "A general error has occurred";
		private const string EXCEPTION_PATTERN_NOT_VALID = "The pattern text is not valid";
		private const string EXCEPTION_PATTERN_SHOULD_START_BY_P = "The pattern text should start with P";

		private bool _isMonthBeforeTime = false;
		private bool _isTagTimeFound = false;

		private int CalculateDays(int years, int months, int days)
		{
			var daysInMonths = 0;
			var daysInRest = 0;

			if (months > Constants.MONTHS_PER_YEAR)
			{
				daysInMonths = Constants.DAYS_PER_YEAR * (months / Constants.MONTHS_PER_YEAR);
				daysInRest = months - ((months / Constants.MONTHS_PER_YEAR) * Constants.MONTHS_PER_YEAR);
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

			if (durationStruct.Seconds > Constants.SECONDS_PER_MINUTE)
			{
				int seconds = durationStruct.Seconds / Constants.SECONDS_PER_MINUTE;
				durationStruct.Minutes += seconds;
				durationStruct.Seconds -= seconds * Constants.SECONDS_PER_MINUTE;
			}

			if (durationStruct.Minutes > Constants.MINUTES_PER_HOUR)
			{
				int minutes = durationStruct.Minutes / Constants.MINUTES_PER_HOUR;
				durationStruct.Hours += minutes;
				durationStruct.Minutes -= minutes * Constants.MINUTES_PER_HOUR;
			}

			if (durationStruct.Hours > Constants.HOURS_PER_DAY)
			{
				int days = durationStruct.Hours / Constants.HOURS_PER_DAY;
				durationStruct.Days += days;
				durationStruct.Hours -= days * Constants.HOURS_PER_DAY;
			}

			if (durationStruct.Minutes > Constants.MINUTES_PER_HOUR)
			{
				int hours = durationStruct.Minutes / Constants.MINUTES_PER_HOUR;
				durationStruct.Hours += hours;
				durationStruct.Minutes -= hours * Constants.MINUTES_PER_HOUR;
			}

			if (durationStruct.Days > Constants.DAYS_PER_MONTH)
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

			pattern += (durationStruct.Years > 0 ? durationStruct.Years + Constants.TAG_YEARS : String.Empty);
			pattern += (durationStruct.Months > 0 ? durationStruct.Months + Constants.TAG_MONTHS : String.Empty);
			pattern += (durationStruct.Days > 0 ? durationStruct.Days + Constants.TAG_DAYS : String.Empty);

			var patternTime = String.Empty;
			patternTime += (durationStruct.Hours > 0 ? durationStruct.Hours + Constants.TAG_HOURS : String.Empty);
			patternTime += (durationStruct.Minutes > 0 ? durationStruct.Minutes + Constants.TAG_MINUTES : String.Empty);
			patternTime += (durationStruct.Seconds > 0 ? durationStruct.Seconds + Constants.TAG_SECONDS : String.Empty);

			if (!String.IsNullOrEmpty(patternTime))
			{
				pattern += Constants.TAG_TIME + patternTime;
			}

			return pattern;
		}

		private int GetValue(string tagPattern, ref string pattern)
		{
			var valueString = String.Empty;

			int index = pattern.IndexOf(tagPattern);

			if (index != -1)
			{
				if (!pattern.Substring(0, index).IsNumeric())
				{
					throw new Iso8601DurationException(EXCEPTION_PATTERN_NOT_VALID);
				}

				valueString = pattern.Substring(0, index);
				pattern = pattern.Substring(index);

				if (pattern.Substring(0, 1) == tagPattern)
				{
					pattern = pattern.Substring(1);
				}
				else
				{
					throw new Iso8601DurationException(EXCEPTION_PATTERN_NOT_VALID);
				}

				if (pattern.Length > 0 &&
					pattern.StartsWith(Constants.TAG_TIME))
				{
					pattern = pattern.Substring(1);
				}

				return Convert.ToInt32(valueString);
			}

			return 0;
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
			_isTagTimeFound = false;

			return ToString(ToTimeSpan(pattern));
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

		public DurationStruct ToDurationStruct(string pattern)
		{
			return ToDurationStruct(ToTimeSpan(pattern));
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
			return NormalizeDuration(GetPatternFromDurationStruct(durationStruct));
		}

		public TimeSpan ToTimeSpan(string pattern)
		{
			try
			{
				_isTagTimeFound = false;
				pattern = pattern.ToUpper().Trim();

				if (!pattern.StartsWith(Constants.TAG_PERIOD))
				{
					throw new Iso8601DurationException(EXCEPTION_PATTERN_SHOULD_START_BY_P);
				}

				pattern = pattern.Substring(1);

				if (pattern.Length == 0)
				{
					return new TimeSpan(0);
				}

				_isMonthBeforeTime = pattern.IsMonthBeforeTime();

				if (pattern.Length > 0 &&
					pattern.StartsWith(Constants.TAG_TIME))
				{
					_isTagTimeFound = true;
					pattern = pattern.Substring(1);
				}

				var durationStruct = new DurationStruct();

				if (!_isTagTimeFound)
				{
					durationStruct.Years = GetValue(Constants.TAG_YEARS, ref pattern);
					if (_isMonthBeforeTime) durationStruct.Months = GetValue(Constants.TAG_MONTHS, ref pattern);
					durationStruct.Days = GetValue(Constants.TAG_DAYS, ref pattern);
				}

				durationStruct.Hours = GetValue(Constants.TAG_HOURS, ref pattern);
				durationStruct.Minutes = GetValue(Constants.TAG_MINUTES, ref pattern);
				durationStruct.Seconds = GetValue(Constants.TAG_SECONDS, ref pattern);

				return ToTimeSpan(durationStruct);
			}
			catch (Exception ex)
			{
				throw new Iso8601DurationException(EXCEPTION_GENERIC_ERROR, ex);
			}
		}

		public TimeSpan ToTimeSpan(DurationStruct durationStruct)
		{
			durationStruct.Days = CalculateDays(durationStruct.Years, durationStruct.Months, durationStruct.Days);

			return new TimeSpan(durationStruct.Days, durationStruct.Hours, durationStruct.Minutes, durationStruct.Seconds);
		}

	}
}
