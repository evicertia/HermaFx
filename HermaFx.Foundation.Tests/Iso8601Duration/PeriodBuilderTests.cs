using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace HermaFx.Iso8601Duration
{
	public class PeriodBuilderTests
	{
		private readonly PeriodBuilder _periodBuilder = null;

		public PeriodBuilderTests()
		{
			_periodBuilder = new PeriodBuilder();
		}

		private static IEnumerable<Tuple<string, string>> NormalizeDuration_FromPattern_ShouldBeExpectedPattern_DataProvider()
		{
			yield return new Tuple<string, string>("PT61S", "PT1M1S");
			yield return new Tuple<string, string>("PT61M", "PT1H1M");
			yield return new Tuple<string, string>("PT25H", "P1DT1H");
			yield return new Tuple<string, string>("PT58M61S", "PT59M1S");
			yield return new Tuple<string, string>("PT59M61S", "PT1H1S");
			yield return new Tuple<string, string>("PT23H59M61S", "P1DT1S");
			yield return new Tuple<string, string>("P31D", "P1M1D");
			yield return new Tuple<string, string>("P13M", "P1Y1M");
			yield return new Tuple<string, string>("P11M31D", "P1Y1D");
			yield return new Tuple<string, string>("P1Y", "P1Y");
			yield return new Tuple<string, string>("P1Y31D", "P1Y1M1D");
			yield return new Tuple<string, string>("P1Y11M31D", "P2Y1D");
			yield return new Tuple<string, string>("P1Y11M30DT23H59M60S", "P2Y1D");
			yield return new Tuple<string, string>("P1W", "P7D");
			yield return new Tuple<string, string>("P1W7D", "P14D");
			yield return new Tuple<string, string>("P5W", "P1M5D");
			yield return new Tuple<string, string>("P52W1D", "P1Y");
		}

		[Test, TestCaseSource(nameof(NormalizeDuration_FromPattern_ShouldBeExpectedPattern_DataProvider))]
		public void NormalizeDuration_FromPattern_ShouldBeExpectedPattern(Tuple<string, string> @case)
		{
			var returnedResult = _periodBuilder.NormalizeDuration(@case.Item1);
			Assert.AreEqual(@case.Item2, returnedResult);
		}

		private static IEnumerable<Tuple<DurationStruct, string>> ToString_FromDurationStruct_DataProvider()
		{
			yield return new Tuple<DurationStruct, string>(new DurationStruct() { Minutes = 120 }, "PT2H");
			yield return new Tuple<DurationStruct, string>(new DurationStruct() { Minutes = 1440 }, "P1D");
			yield return new Tuple<DurationStruct, string>(new DurationStruct() { Minutes = 518400 }, "P1Y");
			yield return new Tuple<DurationStruct, string>(new DurationStruct() { Seconds = 3600 }, "PT1H");
			yield return new Tuple<DurationStruct, string>(new DurationStruct() { Seconds = 86400 }, "P1D");
			yield return new Tuple<DurationStruct, string>(new DurationStruct() { Seconds = 31104000 }, "P1Y");
			yield return new Tuple<DurationStruct, string>(new DurationStruct() { Hours = 23, Minutes = 121 }, "P1DT1H1M");
			yield return new Tuple<DurationStruct, string>(new DurationStruct() { Hours = 23, Minutes = 61 }, "P1DT1M");
			yield return new Tuple<DurationStruct, string>(new DurationStruct() { Hours = 23, Minutes = 60 }, "P1D");
			yield return new Tuple<DurationStruct, string>(new DurationStruct() { Days = 30, Hours = 24 }, "P1M1D");
			yield return new Tuple<DurationStruct, string>(new DurationStruct() { Days = 31, Hours = 24 }, "P1M2D");
			yield return new Tuple<DurationStruct, string>(new DurationStruct() { Days = 31 }, "P1M1D");
			yield return new Tuple<DurationStruct, string>(new DurationStruct() { Days = 360, Hours = 23, Minutes = 60 }, "P1Y1D");
			yield return new Tuple<DurationStruct, string>(new DurationStruct() { Days = 365, Hours = 23, Minutes = 60 }, "P1Y1D");
			yield return new Tuple<DurationStruct, string>(new DurationStruct() { Months = 11, Days = 31 }, "P1Y1D");
			yield return new Tuple<DurationStruct, string>(new DurationStruct() { Years = 1, Months = 11, Days = 31 }, "P2Y1D");
			yield return new Tuple<DurationStruct, string>(new DurationStruct() { Years = 1, Months = 12 }, "P2Y");
			yield return new Tuple<DurationStruct, string>(new DurationStruct() { Years = 1, Months = 13 }, "P2Y1M");
			yield return new Tuple<DurationStruct, string>(new DurationStruct() { Months = 11, Days = 30, Hours = 23, Minutes = 59, Seconds = 60 }, "P1Y1D");
		}

		[Test, TestCaseSource(nameof(ToString_FromDurationStruct_DataProvider))]
		public void ToString_FromDurationStruct_ShouldBeExpectedPattern(Tuple<DurationStruct, string> item)
		{
			var returnedResult = _periodBuilder.ToString(item.Item1);
			Assert.AreEqual(item.Item2, returnedResult);
		}

		private static IEnumerable<Tuple<TimeSpan, string>> ToString_FromTimeSpan_DataProvider()
		{
			yield return new Tuple<TimeSpan, string>(new TimeSpan(0, 120, 0), "PT2H");
			yield return new Tuple<TimeSpan, string>(new TimeSpan(0, 1440, 0), "P1D");
			yield return new Tuple<TimeSpan, string>(new TimeSpan(0, 518400, 0), "P1Y");
			yield return new Tuple<TimeSpan, string>(new TimeSpan(0, 0, 3600), "PT1H");
			yield return new Tuple<TimeSpan, string>(new TimeSpan(0, 0, 86400), "P1D");
			yield return new Tuple<TimeSpan, string>(new TimeSpan(0, 0, 31104000), "P1Y");
			yield return new Tuple<TimeSpan, string>(new TimeSpan(23, 121, 0), "P1DT1H1M");
			yield return new Tuple<TimeSpan, string>(new TimeSpan(23, 61, 0), "P1DT1M");
			yield return new Tuple<TimeSpan, string>(new TimeSpan(23, 60, 0), "P1D");
			yield return new Tuple<TimeSpan, string>(new TimeSpan(30, 24, 0, 0), "P1M1D");
			yield return new Tuple<TimeSpan, string>(new TimeSpan(31, 24, 0, 0), "P1M2D");
			yield return new Tuple<TimeSpan, string>(new TimeSpan(31, 0, 0, 0), "P1M1D");
			yield return new Tuple<TimeSpan, string>(new TimeSpan(360, 23, 60, 0), "P1Y1D");
			yield return new Tuple<TimeSpan, string>(new TimeSpan(365, 23, 60, 0), "P1Y1D");
		}

		[Test, TestCaseSource(nameof(ToString_FromTimeSpan_DataProvider))]
		public void ToString_FromTimeSpan_ShouldBeExpectedPattern(Tuple<TimeSpan, string> item)
		{
			var returnedResult = _periodBuilder.ToString(item.Item1);
			Assert.AreEqual(item.Item2, returnedResult);
		}

		private static IEnumerable<Tuple<string, TimeSpan>> ToTimeSpan_FromPattern_DataProvider()
		{
			yield return new Tuple<string, TimeSpan>("PT2H", new TimeSpan(0, 120, 0));
			yield return new Tuple<string, TimeSpan>("PT1440M", new TimeSpan(0, 1440, 0));
			yield return new Tuple<string, TimeSpan>("PT518400M", new TimeSpan(0, 518400, 0));
			yield return new Tuple<string, TimeSpan>("PT1H", new TimeSpan(0, 0, 3600));
			yield return new Tuple<string, TimeSpan>("PT86400S", new TimeSpan(0, 0, 86400));
			yield return new Tuple<string, TimeSpan>("PT31104000S", new TimeSpan(0, 0, 31104000));
			yield return new Tuple<string, TimeSpan>("PT25H1M", new TimeSpan(23, 121, 0));
			yield return new Tuple<string, TimeSpan>("P1W", TimeSpan.FromDays(7));
			yield return new Tuple<string, TimeSpan>("P2W", TimeSpan.FromDays(14));
			yield return new Tuple<string, TimeSpan>("P1W1D", TimeSpan.FromDays(8));
			yield return new Tuple<string, TimeSpan>(string.Empty, TimeSpan.FromDays(0));
			yield return new Tuple<string, TimeSpan>(null, TimeSpan.FromDays(0));
		}

		[Test, TestCaseSource(nameof(ToTimeSpan_FromPattern_DataProvider))]
		public void ToTimeSpan_FromPattern_ShouldBeExpectedTimeSpan(Tuple<string, TimeSpan> item)
		{
			var returnedResult = _periodBuilder.ToTimeSpan(item.Item1);
			Assert.AreEqual(item.Item2, returnedResult);
		}

		private static IEnumerable<Tuple<DurationStruct, TimeSpan>> ToTimeSpan_FromDurationStruct_DataProvider()
		{
			yield return new Tuple<DurationStruct, TimeSpan>(new DurationStruct() { Minutes = 1440 }, new TimeSpan(0, 1440, 0));
			yield return new Tuple<DurationStruct, TimeSpan>(new DurationStruct() { Minutes = 518400 }, new TimeSpan(0, 518400, 0));
			yield return new Tuple<DurationStruct, TimeSpan>(new DurationStruct() { Seconds = 3600 }, new TimeSpan(0, 0, 3600));
			yield return new Tuple<DurationStruct, TimeSpan>(new DurationStruct() { Seconds = 86400 }, new TimeSpan(0, 0, 86400));
			yield return new Tuple<DurationStruct, TimeSpan>(new DurationStruct() { Hours = 23, Minutes = 121 }, new TimeSpan(23, 121, 0));
			yield return new Tuple<DurationStruct, TimeSpan>(new DurationStruct() { Seconds = 31104000 }, new TimeSpan(0, 0, 31104000));
			yield return new Tuple<DurationStruct, TimeSpan>(new DurationStruct() { Hours = 23, Minutes = 61 }, new TimeSpan(23, 61, 0));
			yield return new Tuple<DurationStruct, TimeSpan>(new DurationStruct() { Hours = 23, Minutes = 60 }, new TimeSpan(23, 60, 0));
			yield return new Tuple<DurationStruct, TimeSpan>(new DurationStruct() { Days = 30, Hours = 24 }, new TimeSpan(30, 24, 0, 0));
			yield return new Tuple<DurationStruct, TimeSpan>(new DurationStruct() { Days = 31, Hours = 24 }, new TimeSpan(31, 24, 0, 0));
			yield return new Tuple<DurationStruct, TimeSpan>(new DurationStruct() { Days = 31 }, new TimeSpan(31, 0, 0, 0));
			yield return new Tuple<DurationStruct, TimeSpan>(new DurationStruct() { Days = 360, Hours = 23, Minutes = 60 }, new TimeSpan(360, 23, 60, 0));
			yield return new Tuple<DurationStruct, TimeSpan>(new DurationStruct() { Days = 365, Hours = 23, Minutes = 60 }, new TimeSpan(366, 0, 0, 0));
		}

		[Test, TestCaseSource(nameof(ToTimeSpan_FromDurationStruct_DataProvider))]
		public void ToTimeSpan_FromDurationStruct_ShouldBeExpectedTimeSpan(Tuple<DurationStruct, TimeSpan> item)
		{
			var returnedResult = _periodBuilder.ToTimeSpan(item.Item1);
			Assert.AreEqual(item.Item2, returnedResult);
		}

		private static IEnumerable<Tuple<string, DurationStruct>> ToDurationStruct_FromPattern_DataProvider()
		{
			yield return new Tuple<string, DurationStruct>("PT1S", new DurationStruct() { Seconds = 1 });
			yield return new Tuple<string, DurationStruct>("PT1M", new DurationStruct() { Minutes = 1 });
			yield return new Tuple<string, DurationStruct>("PT2H", new DurationStruct() { Hours = 2 });
			yield return new Tuple<string, DurationStruct>("PT1H", new DurationStruct() { Hours = 1 });
			yield return new Tuple<string, DurationStruct>("P1D", new DurationStruct() { Days = 1 });
			yield return new Tuple<string, DurationStruct>("P1M", new DurationStruct() { Months = 1 });
			yield return new Tuple<string, DurationStruct>("P1Y", new DurationStruct() { Years = 1 });
			yield return new Tuple<string, DurationStruct>("P1DT1H1M", new DurationStruct() { Days = 1, Hours = 1, Minutes = 1 });
		}

		[Test, TestCaseSource(nameof(ToDurationStruct_FromPattern_DataProvider))]
		public void ToDurationStruct_FromPattern_ShouldBeExpectedDurationStruct(Tuple<string, DurationStruct> item)
		{
			var returnedResult = _periodBuilder.ToDurationStruct(item.Item1);
			Assert.AreEqual(item.Item2, returnedResult);
		}

		private static IEnumerable<Tuple<TimeSpan, DurationStruct>> ToDurationStruct_FromTimeSpan_DataProvider()
		{
			yield return new Tuple<TimeSpan, DurationStruct>(new TimeSpan(0, 0, 120), new DurationStruct() { Minutes = 2 });
			yield return new Tuple<TimeSpan, DurationStruct>(new TimeSpan(0, 120, 0), new DurationStruct() { Hours = 2 });
			yield return new Tuple<TimeSpan, DurationStruct>(new TimeSpan(0, 0, 1440), new DurationStruct() { Minutes = 24 });
			yield return new Tuple<TimeSpan, DurationStruct>(new TimeSpan(0, 518400, 0), new DurationStruct() { Years = 1 });
			yield return new Tuple<TimeSpan, DurationStruct>(new TimeSpan(0, 0, 3600), new DurationStruct() { Hours = 1 });
			yield return new Tuple<TimeSpan, DurationStruct>(new TimeSpan(0, 0, 86400), new DurationStruct() { Days = 1 });
			yield return new Tuple<TimeSpan, DurationStruct>(new TimeSpan(0, 0, 31104000), new DurationStruct() { Years = 1 });
			yield return new Tuple<TimeSpan, DurationStruct>(new TimeSpan(23, 121, 0), new DurationStruct() { Days = 1, Hours = 1, Minutes = 1 });
			yield return new Tuple<TimeSpan, DurationStruct>(new TimeSpan(23, 61, 0), new DurationStruct() { Days = 1, Minutes = 1 });
			yield return new Tuple<TimeSpan, DurationStruct>(new TimeSpan(23, 60, 0), new DurationStruct() { Days = 1 });
			yield return new Tuple<TimeSpan, DurationStruct>(new TimeSpan(30, 24, 0, 0), new DurationStruct() { Months = 1, Days = 1 });
			yield return new Tuple<TimeSpan, DurationStruct>(new TimeSpan(31, 24, 0, 0), new DurationStruct() { Months = 1, Days = 2 });
			yield return new Tuple<TimeSpan, DurationStruct>(new TimeSpan(31, 0, 0, 0), new DurationStruct() { Months = 1, Days = 1 });
			yield return new Tuple<TimeSpan, DurationStruct>(new TimeSpan(360, 23, 60, 0), new DurationStruct() { Years = 1, Days = 1 });
		}

		[Test, TestCaseSource(nameof(ToDurationStruct_FromTimeSpan_DataProvider))]
		public void ToDurationStruct_FromTimeSpan_ShouldBeExpectedDurationStruct(Tuple<TimeSpan, DurationStruct> item)
		{
			var returnedResult = _periodBuilder.ToDurationStruct(item.Item1);
			Assert.AreEqual(item.Item2, returnedResult);
		}

		private static IEnumerable<string> Invalid_Patterns_DataProvider()
		{
			yield return "P";
			yield return "PT";
			yield return "P1YT";
			yield return "P1H";
			yield return "P1S";
			yield return "PT1Y";
			yield return "PT1D";
			yield return "PT1Y1M";
			yield return "P1M1Y";
			yield return "P1D1Y";
			yield return "P1D1M";
			yield return "PT1S1M";
			yield return "PT1M1H";
		}

		[Test, TestCaseSource(nameof(Invalid_Patterns_DataProvider))]
		public void Invalid_Patterns(string pattern)
		{
			Assert.That(() => _periodBuilder.ToTimeSpan(pattern), Throws.InstanceOf<Iso8601DurationException>());
			Assert.That(() => _periodBuilder.ToDurationStruct(pattern), Throws.InstanceOf<Iso8601DurationException>());
		}

		#region DateTime extensions

		private static IEnumerable<Tuple<DateTime, DurationStruct, DateTime>> DateTime_Add_DurationStruct_DataProvider()
		{
			yield return new Tuple<DateTime, DurationStruct, DateTime>(new DateTime(2021, 01, 01), new DurationStruct() { Days = 1 }, new DateTime(2021, 01, 02));
			yield return new Tuple<DateTime, DurationStruct, DateTime>(new DateTime(2021, 01, 31), new DurationStruct() { Days = 1 }, new DateTime(2021, 02, 01));
			yield return new Tuple<DateTime, DurationStruct, DateTime>(new DateTime(2020, 12, 31), new DurationStruct() { Days = 1 }, new DateTime(2021, 01, 01));
			yield return new Tuple<DateTime, DurationStruct, DateTime>(new DateTime(2020, 02, 28), new DurationStruct() { Days = 1 }, new DateTime(2020, 02, 29));
			yield return new Tuple<DateTime, DurationStruct, DateTime>(new DateTime(2021, 02, 28), new DurationStruct() { Days = 1 }, new DateTime(2021, 03, 01));
			yield return new Tuple<DateTime, DurationStruct, DateTime>(new DateTime(2021, 01, 01), new DurationStruct() { Months = 1 }, new DateTime(2021, 02, 01));
			yield return new Tuple<DateTime, DurationStruct, DateTime>(new DateTime(2021, 01, 01), new DurationStruct() { Months = 1 }, new DateTime(2021, 02, 01));
			yield return new Tuple<DateTime, DurationStruct, DateTime>(new DateTime(2020, 02, 15), new DurationStruct() { Months = 1 }, new DateTime(2020, 03, 15));
			yield return new Tuple<DateTime, DurationStruct, DateTime>(new DateTime(2021, 02, 15), new DurationStruct() { Months = 1 }, new DateTime(2021, 03, 15));
			yield return new Tuple<DateTime, DurationStruct, DateTime>(new DateTime(2021, 01, 01), new DurationStruct() { Months = 12 }, new DateTime(2022, 01, 01));
			yield return new Tuple<DateTime, DurationStruct, DateTime>(new DateTime(2020, 02, 29), new DurationStruct() { Months = 12 }, new DateTime(2021, 02, 28));
			yield return new Tuple<DateTime, DurationStruct, DateTime>(new DateTime(2020, 01, 01), new DurationStruct() { Years = 1 }, new DateTime(2021, 01, 01));
			yield return new Tuple<DateTime, DurationStruct, DateTime>(new DateTime(2020, 02, 29), new DurationStruct() { Years = 1 }, new DateTime(2021, 02, 28));
			yield return new Tuple<DateTime, DurationStruct, DateTime>(new DateTime(2020, 02, 29, 01, 00, 00), new DurationStruct() { Hours = 1 }, new DateTime(2020, 02, 29, 02, 00, 00));
			yield return new Tuple<DateTime, DurationStruct, DateTime>(new DateTime(2020, 02, 29, 23, 00, 00), new DurationStruct() { Hours = 1 }, new DateTime(2020, 03, 01, 00, 00, 00));
			yield return new Tuple<DateTime, DurationStruct, DateTime>(new DateTime(2021, 02, 28, 23, 00, 00), new DurationStruct() { Hours = 1 }, new DateTime(2021, 03, 01, 00, 00, 00));
			yield return new Tuple<DateTime, DurationStruct, DateTime>(new DateTime(2021, 12, 31, 23, 00, 00), new DurationStruct() { Hours = 1 }, new DateTime(2022, 01, 01, 00, 00, 00));
			yield return new Tuple<DateTime, DurationStruct, DateTime>(new DateTime(2020, 02, 29, 01, 00, 00), new DurationStruct() { Minutes = 30 }, new DateTime(2020, 02, 29, 01, 30, 00));
			yield return new Tuple<DateTime, DurationStruct, DateTime>(new DateTime(2020, 02, 29, 23, 30, 00), new DurationStruct() { Minutes = 30 }, new DateTime(2020, 03, 01, 00, 00, 00));
			yield return new Tuple<DateTime, DurationStruct, DateTime>(new DateTime(2021, 02, 28, 23, 30, 00), new DurationStruct() { Minutes = 30 }, new DateTime(2021, 03, 01, 00, 00, 00));
			yield return new Tuple<DateTime, DurationStruct, DateTime>(new DateTime(2021, 12, 31, 23, 30, 00), new DurationStruct() { Minutes = 30 }, new DateTime(2022, 01, 01, 00, 00, 00));
			yield return new Tuple<DateTime, DurationStruct, DateTime>(new DateTime(2020, 02, 29, 01, 00, 00), new DurationStruct() { Seconds = 10 }, new DateTime(2020, 02, 29, 01, 00, 10));
			yield return new Tuple<DateTime, DurationStruct, DateTime>(new DateTime(2020, 02, 29, 23, 59, 50), new DurationStruct() { Seconds = 10 }, new DateTime(2020, 03, 01, 00, 00, 00));
			yield return new Tuple<DateTime, DurationStruct, DateTime>(new DateTime(2021, 02, 28, 23, 59, 50), new DurationStruct() { Seconds = 10 }, new DateTime(2021, 03, 01, 00, 00, 00));
			yield return new Tuple<DateTime, DurationStruct, DateTime>(new DateTime(2021, 12, 31, 23, 59, 50), new DurationStruct() { Seconds = 10 }, new DateTime(2022, 01, 01, 00, 00, 00));
		}

		[Test, TestCaseSource(nameof(DateTime_Add_DurationStruct_DataProvider))]
		public void DateTime_Add_DurationStruct(Tuple<DateTime, DurationStruct, DateTime> testcase)
		{
			var initial = testcase.Item1;
			var duration = testcase.Item2;
			var expected = testcase.Item3;

			var actual = initial.Add(duration);

			Assert.That(actual, Is.EqualTo(expected));
		}

		private static IEnumerable<Tuple<DateTime, string, DateTime>> DateTime_Add_DurationPattern_DataProvider()
		{
			yield return new Tuple<DateTime, string, DateTime>(new DateTime(2021, 01, 01), "P1D", new DateTime(2021, 01, 02));
			yield return new Tuple<DateTime, string, DateTime>(new DateTime(2021, 01, 31), "P1D", new DateTime(2021, 02, 01));
			yield return new Tuple<DateTime, string, DateTime>(new DateTime(2020, 12, 31), "P1D", new DateTime(2021, 01, 01));
			yield return new Tuple<DateTime, string, DateTime>(new DateTime(2020, 02, 28), "P1D", new DateTime(2020, 02, 29));
			yield return new Tuple<DateTime, string, DateTime>(new DateTime(2021, 02, 28), "P1D", new DateTime(2021, 03, 01));
			yield return new Tuple<DateTime, string, DateTime>(new DateTime(2020, 02, 28), "P31D", new DateTime(2020, 03, 30));
			yield return new Tuple<DateTime, string, DateTime>(new DateTime(2021, 02, 28), "P31D", new DateTime(2021, 03, 31));
			yield return new Tuple<DateTime, string, DateTime>(new DateTime(2020, 02, 28), "P365D", new DateTime(2021, 02, 27));
			yield return new Tuple<DateTime, string, DateTime>(new DateTime(2021, 02, 28), "P365D", new DateTime(2022, 02, 28));
			yield return new Tuple<DateTime, string, DateTime>(new DateTime(2021, 01, 01), "P1W", new DateTime(2021, 01, 08));
			yield return new Tuple<DateTime, string, DateTime>(new DateTime(2021, 01, 28), "P1W", new DateTime(2021, 02, 04));
			yield return new Tuple<DateTime, string, DateTime>(new DateTime(2021, 02, 22), "P1W", new DateTime(2021, 03, 01));
			yield return new Tuple<DateTime, string, DateTime>(new DateTime(2020, 02, 24), "P1W", new DateTime(2020, 03, 02));
			yield return new Tuple<DateTime, string, DateTime>(new DateTime(2021, 01, 01), "P1M", new DateTime(2021, 02, 01));
			yield return new Tuple<DateTime, string, DateTime>(new DateTime(2021, 01, 01), "P1M", new DateTime(2021, 02, 01));
			yield return new Tuple<DateTime, string, DateTime>(new DateTime(2020, 02, 15), "P1M", new DateTime(2020, 03, 15));
			yield return new Tuple<DateTime, string, DateTime>(new DateTime(2021, 02, 15), "P1M", new DateTime(2021, 03, 15));
			yield return new Tuple<DateTime, string, DateTime>(new DateTime(2021, 01, 01), "P12M", new DateTime(2022, 01, 01));
			yield return new Tuple<DateTime, string, DateTime>(new DateTime(2020, 02, 29), "P12M", new DateTime(2021, 02, 28));
			yield return new Tuple<DateTime, string, DateTime>(new DateTime(2020, 01, 01), "P1Y", new DateTime(2021, 01, 01));
			yield return new Tuple<DateTime, string, DateTime>(new DateTime(2020, 02, 29), "P1Y", new DateTime(2021, 02, 28));
			yield return new Tuple<DateTime, string, DateTime>(new DateTime(2020, 02, 29, 01, 00, 00), "PT1H", new DateTime(2020, 02, 29, 02, 00, 00));
			yield return new Tuple<DateTime, string, DateTime>(new DateTime(2020, 02, 29, 23, 00, 00), "PT1H", new DateTime(2020, 03, 01, 00, 00, 00));
			yield return new Tuple<DateTime, string, DateTime>(new DateTime(2021, 02, 28, 23, 00, 00), "PT1H", new DateTime(2021, 03, 01, 00, 00, 00));
			yield return new Tuple<DateTime, string, DateTime>(new DateTime(2021, 12, 31, 23, 00, 00), "PT1H", new DateTime(2022, 01, 01, 00, 00, 00));
			yield return new Tuple<DateTime, string, DateTime>(new DateTime(2020, 02, 29, 01, 00, 00), "PT30M", new DateTime(2020, 02, 29, 01, 30, 00));
			yield return new Tuple<DateTime, string, DateTime>(new DateTime(2020, 02, 29, 23, 30, 00), "PT30M", new DateTime(2020, 03, 01, 00, 00, 00));
			yield return new Tuple<DateTime, string, DateTime>(new DateTime(2021, 02, 28, 23, 30, 00), "PT30M", new DateTime(2021, 03, 01, 00, 00, 00));
			yield return new Tuple<DateTime, string, DateTime>(new DateTime(2021, 12, 31, 23, 30, 00), "PT30M", new DateTime(2022, 01, 01, 00, 00, 00));
			yield return new Tuple<DateTime, string, DateTime>(new DateTime(2020, 02, 29, 01, 00, 00), "PT10S", new DateTime(2020, 02, 29, 01, 00, 10));
			yield return new Tuple<DateTime, string, DateTime>(new DateTime(2020, 02, 29, 23, 59, 50), "PT10S", new DateTime(2020, 03, 01, 00, 00, 00));
			yield return new Tuple<DateTime, string, DateTime>(new DateTime(2021, 02, 28, 23, 59, 50), "PT10S", new DateTime(2021, 03, 01, 00, 00, 00));
			yield return new Tuple<DateTime, string, DateTime>(new DateTime(2021, 12, 31, 23, 59, 50), "PT10S", new DateTime(2022, 01, 01, 00, 00, 00));
			yield return new Tuple<DateTime, string, DateTime>(new DateTime(2020, 01, 01, 01, 00, 00), "P1DT1H", new DateTime(2020, 01, 02, 02, 00, 00));
			yield return new Tuple<DateTime, string, DateTime>(new DateTime(2021, 01, 01, 00, 00, 00), "P1Y11M30DT23H59M60S", new DateTime(2023, 01, 01, 00, 00, 00));
			yield return new Tuple<DateTime, string, DateTime>(new DateTime(2020, 01, 01, 00, 00, 00), "P1Y11M30DT23H59M60S", new DateTime(2022, 01, 01, 00, 00, 00));
		}

		[Test, TestCaseSource(nameof(DateTime_Add_DurationPattern_DataProvider))]
		public void DateTime_Add_DurationPattern(Tuple<DateTime, string, DateTime> testcase)
		{
			var initial = testcase.Item1;
			var duration = testcase.Item2;
			var expected = testcase.Item3;

			var actual = initial.Add(duration);

			Assert.That(actual, Is.EqualTo(expected));
		}

		#endregion
	}
}
