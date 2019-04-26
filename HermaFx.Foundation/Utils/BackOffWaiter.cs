using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

//
// Code originally based on mookid's BackoffHelper from Rebus.
//

namespace HermaFx.Utils
{
	/// <summary>
	/// I can help you wait, especially if you want to wait e.g. for some kind of increasing amount of time.
	/// </summary>
	public class BackoffWaiter
	{
		private readonly TimeSpan[] backoffTimes;
		private long currentIndex;
		/// <summary>
		/// So we can replace it with something else e.g. during tests
		/// </summary>
		internal Action<TimeSpan> waitAction = timeToWait => Thread.Sleep(timeToWait);

		public bool LoggingDisabled { get; set; }

		public BackoffWaiter(params TimeSpan[] backoffTimes)
		{
			Guard.IsNotNull(backoffTimes, nameof(backoffTimes));

			var timeSpans = backoffTimes.ToArray();
			if (timeSpans.Length == 0)
			{
				throw new ArgumentException("Backoff helper must be initialized with at least one time span!", "backoffTimes");
			}
			if (timeSpans.Where(t => t <= TimeSpan.FromSeconds(0)).Any())
			{
				throw new ArgumentException(
					string.Format(
						"Backoff helper must be initialized with only positive time spans - the following time spans were given: {0}",
						string.Join(", ", timeSpans)), "backoffTimes");
			}
			this.backoffTimes = timeSpans;
		}


		public BackoffWaiter(params uint[] backoffSeconds)
			: this(backoffSeconds?.Select(x => TimeSpan.FromSeconds(x)).ToArray())
		{
		}

		/// <summary>
		/// Resets the backoff helper which means that waiting will start over from the beginning of the sequence of wait times
		/// </summary>
		public void Reset()
		{
			Interlocked.Exchange(ref currentIndex, 0);
		}

		/// <summary>
		/// Waits the time specified next in the sequence
		/// </summary>
		public void Wait()
		{
			Wait(_ => { });
		}

		/// <summary>
		/// Waits the time specified next in the sequence, invoking the callback with the time that will be waited
		/// </summary>
		public void Wait(Action<TimeSpan> howLongTheWaitWillLast)
		{
			var effectiveIndex = Math.Min(Interlocked.Read(ref currentIndex), backoffTimes.Length - 1);
			var timeToWait = backoffTimes[effectiveIndex];

#if false
			if (!LoggingDisabled)
			{
				log.Debug("Waiting {0}", timeToWait);
			}
#endif

			howLongTheWaitWillLast(timeToWait);
			waitAction(timeToWait);

			Interlocked.Increment(ref currentIndex);
		}
	}
}