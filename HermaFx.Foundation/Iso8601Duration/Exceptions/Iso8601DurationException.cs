using System;
using System.Runtime.Serialization;

namespace HermaFx.Iso8601Duration
{
	public class Iso8601DurationException : Exception
	{
		public Iso8601DurationException()
		{
		}

		public Iso8601DurationException(string message) : base(message)
		{
		}

		public Iso8601DurationException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected Iso8601DurationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}