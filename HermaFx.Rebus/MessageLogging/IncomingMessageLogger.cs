using System;
using System.Collections.Generic;
using System.Text;

using HermaFx;
using HermaFx.Logging;

namespace Rebus.Configuration
{
	public class IncommingMessageLogger
	{
		#region Private memmbers

		private static ILog _logger = LogProvider.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		private LogLevel _logLevel;

		#endregion

		#region .ctor

		public IncommingMessageLogger(LogLevel logLevel)
		{
			_logLevel = logLevel;
		}

		#endregion

		#region Private methods

		private void Append(StringBuilder messageLog, string id, IDictionary<string, object> headers)
		{
			Guard.IsNotNull(messageLog, nameof(messageLog));

			if (headers == null || headers.Count == 0)
			{
				messageLog.Append("(message contains no headers)!!");
				return;
			}

			foreach (var header in headers)
				messageLog.AppendFormat("{0}: {1}; ", header.Key, header.Value);
		}

		private void Append(StringBuilder messageLog, string id, byte[] body, Encoding encoding)
		{
			Guard.IsNotNull(messageLog, nameof(messageLog));

			try
			{
				var messageBody = encoding.GetString(body);
				messageLog.Append(messageBody);
			}
			catch (Exception ex)
			{
				_logger.Log(_logLevel, () => "An error ocurred while decoding incoming transport message?!", ex);
			}
		}

		#endregion

		#region Public Log method

		public void Log(ReceivedTransportMessage message)
		{
			var encoding = MessageLoggerHelper.GetEncoding(message.Headers);

			if (encoding == null)
			{
				_logger.WarnFormat("Unable to guess encoding for incoming message with id '{0}'?!", message.Id);
				return;
			}

			_logger.Log(_logLevel, () =>
			{
				var messageLog = new StringBuilder();

				messageLog.AppendFormat("Incoming message ({0}) headers:", message.Id);
				Append(messageLog, message.Id, message.Headers);

				return messageLog.ToString();
			});

			_logger.Log(_logLevel, () =>
			{
				var messageLog = new StringBuilder();

				messageLog.AppendFormat("Incoming message ({0}) body:", message.Id);
				Append(messageLog, message.Id, message.Body, encoding);

				return messageLog.ToString();
			});

		}

		#endregion
	}
}