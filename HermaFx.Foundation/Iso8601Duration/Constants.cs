namespace HermaFx.Iso8601Duration
{
	public sealed class Constants
	{
		public const string ZERO = "0";

		public const string TAG_PERIOD = "P";
		public const string TAG_TIME = "T";

		public const string TAG_YEARS = "Y";
		public const string TAG_MONTHS = "M";
		public const string TAG_DAYS = "D";

		public const string TAG_HOURS = "H";
		public const string TAG_MINUTES = "M";
		public const string TAG_SECONDS = "S";

		public const string TAG_ZERO = TAG_PERIOD + TAG_TIME + ZERO + TAG_SECONDS;

		public const int DAYS_PER_YEAR = 365;
		public const int DAYS_PER_MONTH = 30;
		public const int DAYS_PER_WEEK = 7;
		public const int MONTHS_PER_YEAR = 12;

		public const int HOURS_PER_DAY = 24;
		public const int MINUTES_PER_HOUR = 60;
		public const int SECONDS_PER_MINUTE = 60;
	}
}
