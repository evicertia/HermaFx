using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HermaFx.Iso8601Duration
{
	public class PeriodBuilderTests
	{
		private PeriodBuilder _periodBuilder = null;

		public PeriodBuilderTests()
		{
			this._periodBuilder = new PeriodBuilder();
		}

		#region DataProviders

		public class NormalizeDuration_FromPattern_ShouldBeExpectedPattern_DataProvider : IEnumerable
		{
			public IEnumerator GetEnumerator()
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
			}
		}
		#endregion

		[TestCaseSource(typeof(NormalizeDuration_FromPattern_ShouldBeExpectedPattern_DataProvider))]
		public void NormalizeDuration_FromPattern_ShouldBeExpectedPattern(Tuple<string, string> @case)
		{
			var returnedResult = this._periodBuilder.NormalizeDuration(@case.Item1);
			Assert.AreEqual(@case.Item2, returnedResult);
		}

		[Test]
		public void ToString_FromDurationStruct_ShouldBeExpectedPattern()
		{
			var values = new List<Tuple<DurationStruct, string>>();
			values.Add(new Tuple<DurationStruct, string>(new DurationStruct() { Minutes = 120 }, "PT2H"));
			values.Add(new Tuple<DurationStruct, string>(new DurationStruct() { Minutes = 1440 }, "P1D"));
			values.Add(new Tuple<DurationStruct, string>(new DurationStruct() { Minutes = 518400 }, "P1Y"));
			values.Add(new Tuple<DurationStruct, string>(new DurationStruct() { Seconds = 3600 }, "PT1H"));
			values.Add(new Tuple<DurationStruct, string>(new DurationStruct() { Seconds = 86400 }, "P1D"));
			values.Add(new Tuple<DurationStruct, string>(new DurationStruct() { Seconds = 31104000 }, "P1Y"));
			values.Add(new Tuple<DurationStruct, string>(new DurationStruct() { Hours = 23, Minutes = 121 }, "P1DT1H1M"));
			values.Add(new Tuple<DurationStruct, string>(new DurationStruct() { Hours = 23, Minutes = 61 }, "P1DT1M"));
			values.Add(new Tuple<DurationStruct, string>(new DurationStruct() { Hours = 23, Minutes = 60 }, "P1D"));
			values.Add(new Tuple<DurationStruct, string>(new DurationStruct() { Days = 30, Hours = 24 }, "P1M1D"));
			values.Add(new Tuple<DurationStruct, string>(new DurationStruct() { Days = 31, Hours = 24 }, "P1M2D"));
			values.Add(new Tuple<DurationStruct, string>(new DurationStruct() { Days = 31 }, "P1M1D"));
			values.Add(new Tuple<DurationStruct, string>(new DurationStruct() { Days = 360, Hours = 23, Minutes = 60 }, "P1Y1D"));
			values.Add(new Tuple<DurationStruct, string>(new DurationStruct() { Days = 365, Hours = 23, Minutes = 60 }, "P1Y1D"));
			values.Add(new Tuple<DurationStruct, string>(new DurationStruct() { Months = 11, Days = 31 }, "P1Y1D"));
			values.Add(new Tuple<DurationStruct, string>(new DurationStruct() { Years = 1, Months = 11, Days = 31 }, "P2Y1D"));
			values.Add(new Tuple<DurationStruct, string>(new DurationStruct() { Years = 1, Months = 12 }, "P2Y"));
			values.Add(new Tuple<DurationStruct, string>(new DurationStruct() { Years = 1, Months = 13 }, "P2Y1M"));
			values.Add(new Tuple<DurationStruct, string>(new DurationStruct() { Months = 11, Days = 30, Hours = 23, Minutes = 59, Seconds = 60 }, "P1Y1D"));

			foreach (var item in values)
			{
				var returnedResult = this._periodBuilder.ToString(item.Item1);
				Assert.AreEqual(item.Item2, returnedResult);
			}
		}

		[Test]
		public void ToString_FromTimeSpan_ShouldBeExpectedPattern()
		{
			var values = new List<Tuple<TimeSpan, string>>();
			values.Add(new Tuple<TimeSpan, string>(new TimeSpan(0, 120, 0), "PT2H"));
			values.Add(new Tuple<TimeSpan, string>(new TimeSpan(0, 1440, 0), "P1D"));
			values.Add(new Tuple<TimeSpan, string>(new TimeSpan(0, 518400, 0), "P1Y"));
			values.Add(new Tuple<TimeSpan, string>(new TimeSpan(0, 0, 3600), "PT1H"));
			values.Add(new Tuple<TimeSpan, string>(new TimeSpan(0, 0, 86400), "P1D"));
			values.Add(new Tuple<TimeSpan, string>(new TimeSpan(0, 0, 31104000), "P1Y"));
			values.Add(new Tuple<TimeSpan, string>(new TimeSpan(23, 121, 0), "P1DT1H1M"));
			values.Add(new Tuple<TimeSpan, string>(new TimeSpan(23, 61, 0), "P1DT1M"));
			values.Add(new Tuple<TimeSpan, string>(new TimeSpan(23, 60, 0), "P1D"));
			values.Add(new Tuple<TimeSpan, string>(new TimeSpan(30, 24, 0, 0), "P1M1D"));
			values.Add(new Tuple<TimeSpan, string>(new TimeSpan(31, 24, 0, 0), "P1M2D"));
			values.Add(new Tuple<TimeSpan, string>(new TimeSpan(31, 0, 0, 0), "P1M1D"));
			values.Add(new Tuple<TimeSpan, string>(new TimeSpan(360, 23, 60, 0), "P1Y1D"));
			values.Add(new Tuple<TimeSpan, string>(new TimeSpan(365, 23, 60, 0), "P1Y1D"));

			foreach (var item in values)
			{
				var returnedResult = this._periodBuilder.ToString(item.Item1);
				Assert.AreEqual(item.Item2, returnedResult);
			}
		}

		[Test]
		public void ToTimeSpan_FromPattern_ShouldBeExpectedTimeSpan()
		{
			var values = new List<Tuple<string, TimeSpan>>();
			values.Add(new Tuple<string, TimeSpan>("PT2H", new TimeSpan(0, 120, 0)));
			values.Add(new Tuple<string, TimeSpan>("PT1440M", new TimeSpan(0, 1440, 0)));
			values.Add(new Tuple<string, TimeSpan>("PT518400M", new TimeSpan(0, 518400, 0)));
			values.Add(new Tuple<string, TimeSpan>("PT1H", new TimeSpan(0, 0, 3600)));
			values.Add(new Tuple<string, TimeSpan>("PT86400S", new TimeSpan(0, 0, 86400)));
			values.Add(new Tuple<string, TimeSpan>("PT31104000S", new TimeSpan(0, 0, 31104000)));
			values.Add(new Tuple<string, TimeSpan>("PT25H1M", new TimeSpan(23, 121, 0)));

			foreach (var item in values)
			{
				var returnedResult = this._periodBuilder.ToTimeSpan(item.Item1);
				Assert.AreEqual(item.Item2, returnedResult);
			}
		}

		[Test]
		public void ToTimeSpan_FromDurationStruct_ShouldBeExpectedTimeSpan()
		{
			var values = new List<Tuple<DurationStruct, TimeSpan>>();
			values.Add(new Tuple<DurationStruct, TimeSpan>(new DurationStruct() { Minutes = 1440 }, new TimeSpan(0, 1440, 0)));
			values.Add(new Tuple<DurationStruct, TimeSpan>(new DurationStruct() { Minutes = 518400 }, new TimeSpan(0, 518400, 0)));
			values.Add(new Tuple<DurationStruct, TimeSpan>(new DurationStruct() { Seconds = 3600 }, new TimeSpan(0, 0, 3600)));
			values.Add(new Tuple<DurationStruct, TimeSpan>(new DurationStruct() { Seconds = 86400 }, new TimeSpan(0, 0, 86400)));
			values.Add(new Tuple<DurationStruct, TimeSpan>(new DurationStruct() { Hours = 23, Minutes = 121 }, new TimeSpan(23, 121, 0)));
			values.Add(new Tuple<DurationStruct, TimeSpan>(new DurationStruct() { Seconds = 31104000 }, new TimeSpan(0, 0, 31104000)));
			values.Add(new Tuple<DurationStruct, TimeSpan>(new DurationStruct() { Hours = 23, Minutes = 61 }, new TimeSpan(23, 61, 0)));
			values.Add(new Tuple<DurationStruct, TimeSpan>(new DurationStruct() { Hours = 23, Minutes = 60 }, new TimeSpan(23, 60, 0)));
			values.Add(new Tuple<DurationStruct, TimeSpan>(new DurationStruct() { Days = 30, Hours = 24 }, new TimeSpan(30, 24, 0, 0)));
			values.Add(new Tuple<DurationStruct, TimeSpan>(new DurationStruct() { Days = 31, Hours = 24 }, new TimeSpan(31, 24, 0, 0)));
			values.Add(new Tuple<DurationStruct, TimeSpan>(new DurationStruct() { Days = 31 }, new TimeSpan(31, 0, 0, 0)));
			values.Add(new Tuple<DurationStruct, TimeSpan>(new DurationStruct() { Days = 360, Hours = 23, Minutes = 60 }, new TimeSpan(360, 23, 60, 0)));
			values.Add(new Tuple<DurationStruct, TimeSpan>(new DurationStruct() { Days = 365, Hours = 23, Minutes = 60 }, new TimeSpan(366, 0, 0, 0)));

			foreach (var item in values)
			{
				var returnedResult = this._periodBuilder.ToTimeSpan(item.Item1);
				Assert.AreEqual(item.Item2, returnedResult);
			}
		}

		[Test]
		public void ToDurationStruct_FromPattern_ShouldBeExpectedDurationStruct()
		{
			var values = new List<Tuple<string, DurationStruct>>();
			values.Add(new Tuple<string, DurationStruct>("PT1S", new DurationStruct() { Seconds = 1 }));
			values.Add(new Tuple<string, DurationStruct>("PT1M", new DurationStruct() { Minutes = 1 }));
			values.Add(new Tuple<string, DurationStruct>("PT2H", new DurationStruct() { Hours = 2 }));
			values.Add(new Tuple<string, DurationStruct>("PT1H", new DurationStruct() { Hours = 1 }));
			values.Add(new Tuple<string, DurationStruct>("P1D", new DurationStruct() { Days = 1 }));
			values.Add(new Tuple<string, DurationStruct>("P1M", new DurationStruct() { Months = 1 }));
			values.Add(new Tuple<string, DurationStruct>("P1Y", new DurationStruct() { Years = 1 }));
			values.Add(new Tuple<string, DurationStruct>("P1DT1H1M", new DurationStruct() { Days = 1, Hours = 1, Minutes = 1 }));

			foreach (var item in values)
			{
				var returnedResult = this._periodBuilder.ToDurationStruct(item.Item1);
				Assert.AreEqual(item.Item2, returnedResult);
			}
		}

		[Test]
		public void ToDurationStruct_FromTimeSpan_ShouldBeExpectedDurationStruct()
		{
			var values = new List<Tuple<TimeSpan, DurationStruct>>();
			values.Add(new Tuple<TimeSpan, DurationStruct>(new TimeSpan(0, 0, 120), new DurationStruct() { Minutes = 2 }));
			values.Add(new Tuple<TimeSpan, DurationStruct>(new TimeSpan(0, 120, 0), new DurationStruct() { Hours = 2 }));
			values.Add(new Tuple<TimeSpan, DurationStruct>(new TimeSpan(0, 0, 1440), new DurationStruct() { Minutes = 24 }));
			values.Add(new Tuple<TimeSpan, DurationStruct>(new TimeSpan(0, 518400, 0), new DurationStruct() { Years = 1 }));
			values.Add(new Tuple<TimeSpan, DurationStruct>(new TimeSpan(0, 0, 3600), new DurationStruct() { Hours = 1 }));
			values.Add(new Tuple<TimeSpan, DurationStruct>(new TimeSpan(0, 0, 86400), new DurationStruct() { Days = 1 }));
			values.Add(new Tuple<TimeSpan, DurationStruct>(new TimeSpan(0, 0, 31104000), new DurationStruct() { Years = 1 }));
			values.Add(new Tuple<TimeSpan, DurationStruct>(new TimeSpan(23, 121, 0), new DurationStruct() { Days = 1, Hours = 1, Minutes = 1 }));
			values.Add(new Tuple<TimeSpan, DurationStruct>(new TimeSpan(23, 61, 0), new DurationStruct() { Days = 1, Minutes = 1 }));
			values.Add(new Tuple<TimeSpan, DurationStruct>(new TimeSpan(23, 60, 0), new DurationStruct() { Days = 1 }));
			values.Add(new Tuple<TimeSpan, DurationStruct>(new TimeSpan(30, 24, 0, 0), new DurationStruct() { Months = 1, Days = 1 }));
			values.Add(new Tuple<TimeSpan, DurationStruct>(new TimeSpan(31, 24, 0, 0), new DurationStruct() { Months = 1, Days = 2 }));
			values.Add(new Tuple<TimeSpan, DurationStruct>(new TimeSpan(31, 0, 0, 0), new DurationStruct() { Months = 1, Days = 1 }));
			values.Add(new Tuple<TimeSpan, DurationStruct>(new TimeSpan(360, 23, 60, 0), new DurationStruct() { Years = 1, Days = 1 }));
			values.Add(new Tuple<TimeSpan, DurationStruct>(new TimeSpan(365, 23, 60, 0), new DurationStruct() { Years = 1, Days = 1 }));

			foreach (var item in values)
			{
				var returnedResult = this._periodBuilder.ToDurationStruct(item.Item1);
				Assert.AreEqual(item.Item2, returnedResult);
			}
		}

	}
}
