using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HermaFx.DataAnnotations
{
	[Serializable]
	public class AggregateValidationException : ValidationException
	{
		public IEnumerable<ValidationException> ValidationExceptions { get; private set; }

		public AggregateValidationException(string message, IEnumerable<ValidationException> exceptions)
			: base(message)
		{
			if (message == null) throw new ArgumentNullException("message");
			if (exceptions == null) throw new ArgumentNullException("exceptions");

			this.ValidationExceptions = exceptions;
		}
		protected AggregateValidationException(
			System.Runtime.Serialization.SerializationInfo info,
			System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}
}
