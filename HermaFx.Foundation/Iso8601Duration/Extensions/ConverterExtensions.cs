using System;

namespace HermaFx.Iso8601Duration
{
	public static class ConverterExtensions
	{
		public static bool IsNumeric(this string input)
		{
			int number;
			return int.TryParse(input, out number);
		}

		public static bool IsMonthBeforeTime(this string pattern)
		{
			int mOccurrences = pattern.Split(Constants.TAG_MONTHS.ToCharArray()).Length - 1;
			int indexOfT = pattern.IndexOf(Constants.TAG_TIME);
			int indexOfM = pattern.IndexOf(Constants.TAG_MONTHS);

			if ((indexOfT > indexOfM) ||
			   (indexOfT == -1 && indexOfM != -1 && mOccurrences != 0)) return true;

			return false;
		}
	}
}
