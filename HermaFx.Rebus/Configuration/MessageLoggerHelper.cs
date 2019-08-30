using System;
using System.Collections.Generic;
using System.Text;

using Rebus.Shared;

using HermaFx;

namespace Rebus.Configuration
{
	public class MessageLoggerHelper
	{
		public static string GetMessageId(IDictionary<string, object> headers)
		{
			if (!headers.ContainsKey(Headers.MessageId))
				return null;

			return Convert.ToString(headers[Headers.MessageId]);
		}

		public static Encoding GetEncoding(IDictionary<string, object> headers)
		{
			var encodingWebName = headers.GetValueOrDefault(Headers.Encoding, "").ToString();

			if (string.IsNullOrWhiteSpace(encodingWebName))
				return null;

			try
			{
				return Encoding.GetEncoding(encodingWebName);
			}
			catch (Exception)
			{
				return null;
			}
		}
	}
}
