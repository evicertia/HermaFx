using System;

using HermaFx;
using HermaFx.Logging;

using Rebus.Configuration;

namespace Rebus
{
    public static class MessageRunningTimeExtensions
    {
		private static ILog _Log = LogProvider.GetCurrentClassLogger();
		private const string StopwatchKey = nameof(MessageRunningTimeExtensions) + "::Stopwatch";

        public static RebusConfigurer EnablePerMessageRunningTimeReporting(this RebusConfigurer configurer, TimeSpan threshold)
        {
            configurer.Events(e =>
            {
                e.MessageContextEstablished += (_, ctx) =>
                {
                    var sw = new System.Diagnostics.Stopwatch();

                    ctx.Items[StopwatchKey] = sw;
                    ctx.Disposed += sw.Stop;

                    sw.Start();
                };

                e.AfterMessage += (_, ex, msg) =>
                {
                    if (!MessageContext.HasCurrent)
                    {
                        _Log.Warn("Missing message context at AfterMessage event?!");
                        return;
                    }

                    var ctx = MessageContext.GetCurrent();
                    var tname = ctx.CurrentMessage?.GetType().Name;
                    var sw = ctx.Items.GetValueOrDefault(StopwatchKey, null) as System.Diagnostics.Stopwatch;

                    if (sw == null)
                    {
                        _Log.Warn("Missing stopwatch at AfterMessage event?!");
                        return;
                    }

					if (sw.Elapsed > threshold)
                        _Log.WarnFormat("Message processing took too long ({0}), for message of type '{1}' (threshold: {2})", sw.Elapsed, tname, threshold);
                    else
                        _Log.InfoFormat("Message processing took ({0}), for message of type '{1}'", sw.Elapsed, tname);
                };
            });
            return configurer;
        }

		public static TimeSpan? GetRunningTime(this IMessageContext context)
        {
            return (context.Items.GetValueOrDefault(StopwatchKey, null) as System.Diagnostics.Stopwatch)?.Elapsed;
        }
    }
}
