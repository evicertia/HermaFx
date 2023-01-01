using System;
using System.Collections.Generic;
using System.Text;

using HermaFx;
using HermaFx.Logging;

namespace Rebus.Configuration
{
	public class OutgoingMessageLogger: ISendMessages
	{
		#region Private memmbers

		private static ILog _logger = LogProvider.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		private LogLevel _logLevel;
		private ISendMessages _sender;

		#endregion

		#region .ctor

		public OutgoingMessageLogger(ISendMessages sender, LogLevel logLevel)
		{
			Guard.IsNotNull(sender, nameof(sender));

			_sender = sender;
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
				_logger.Log(_logLevel, () => "An error ocurred while decoding outgoing transport message?!", ex);
			}
		}

		#endregion

		#region Public Log method

		public void Log(string destination, TransportMessageToSend message)
		{
			var encoding = MessageLoggerHelper.GetEncoding(message.Headers);

			if (encoding == null)
			{
				_logger.WarnFormat("Unable to guess encoding for outgoing message with id '{0}' and destination {1}?!", MessageLoggerHelper.GetMessageId(message.Headers), destination);
				return;
			}

			_logger.Log(_logLevel, () =>
			{
				var messageLog = new StringBuilder();

				string id = MessageLoggerHelper.GetMessageId(message.Headers);

				messageLog.AppendFormat("Outgoing message ('{0}') and destination {1} headers:", id, destination);
				Append(messageLog, id, message.Headers);

				return messageLog.ToString();
			});

			_logger.Log(_logLevel, () =>
			{
				var messageLog = new StringBuilder();

				string id = MessageLoggerHelper.GetMessageId(message.Headers);

				messageLog.AppendFormat("Outgoing message ('{0}') and destination {1} body:", id, destination);
				Append(messageLog, id, message.Body, encoding);

				return messageLog.ToString();
			});
		}

		public void Send(string destination, TransportMessageToSend message, ITransactionContext context)
		{
			Log(destination, message);

			_sender.Send(destination, message, context);
		}

		#endregion
	}
}
