using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

using NUnit.Framework;

namespace HermaFx.RuntimeExtensions
{
	public class DateTimeExtensionsTests
	{
		#region Start of Week tests
		[Test]
		public void Check_Date_from_StartOfWeek_Is_Correct()
		{
			var date1 = new DateTime(2017, 1, 1);
			var date2 = new DateTime(2017, 1, 2);

			Assert.That(date1.StartOfWeek(CultureInfo.GetCultureInfo("es")) == new DateTime(2016, 12, 26), "#1");
			Assert.That(date2.StartOfWeek(CultureInfo.GetCultureInfo("es")) == new DateTime(2017, 1, 2), "#2");
			Assert.That(date1.StartOfWeek(DayOfWeek.Monday) == new DateTime(2016, 12, 26), "#3");
			Assert.That(date2.StartOfWeek(DayOfWeek.Monday) == new DateTime(2017, 1, 2), "#4");
		}

		[Test]
		public void Check_Date_from_StartOfWeek_Is_Incorrect()
		{
			var date1 = new DateTime(2017, 1, 2);
			var date2 = new DateTime(2017, 1, 1);

			Assert.That(date1.StartOfWeek(CultureInfo.GetCultureInfo("es")) != new DateTime(2016, 12, 26), "#1");
			Assert.That(date2.StartOfWeek(CultureInfo.GetCultureInfo("es")) != new DateTime(2017, 1, 2), "#2");
			Assert.That(date1.StartOfWeek(DayOfWeek.Monday) != new DateTime(2016, 12, 26), "#3");
			Assert.That(date2.StartOfWeek(DayOfWeek.Monday) != new DateTime(2017, 1, 2), "#4");
		}
		#endregion

		#region End of Week tests
		[Test]
		public void Check_Date_from_EndOfWeek_Is_Correct()
		{
			var date1 = new DateTime(2017, 1, 1);
			var date2 = new DateTime(2017, 1, 2);

			Assert.That(date1.EndOfWeek(CultureInfo.GetCultureInfo("es")) == new DateTime(2017, 1, 1), "#1");
			Assert.That(date2.EndOfWeek(CultureInfo.GetCultureInfo("es")) == new DateTime(2017, 1, 8), "#2");
			Assert.That(date1.EndOfWeek(DayOfWeek.Sunday) == new DateTime(2017, 1, 1), "#3");
			Assert.That(date2.EndOfWeek(DayOfWeek.Sunday) == new DateTime(2017, 1, 8), "#4");
		}

		[Test]
		public void Check_Date_from_EndOfWeek_Is_Incorrect()
		{
			var date1 = new DateTime(2017, 1, 2);
			var date2 = new DateTime(2017, 1, 1);

			Assert.That(date1.EndOfWeek(CultureInfo.GetCultureInfo("es")) != new DateTime(2017, 1, 1), "#1");
			Assert.That(date2.EndOfWeek(CultureInfo.GetCultureInfo("es")) != new DateTime(2017, 1, 8), "#2");
			Assert.That(date1.EndOfWeek(DayOfWeek.Sunday) != new DateTime(2017, 1, 1), "#3");
			Assert.That(date2.EndOfWeek(DayOfWeek.Sunday) != new DateTime(2017, 1, 8), "#4");
		}
		#endregion
	}
}