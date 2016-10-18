using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Rebus;
using Rebus.Shared;

namespace HermaFx.Rebus
{
	public static class MessageContextExtensions
	{
		public static Guid? TryGetOriginatorSagaId(this IMessageContext context)
		{
			if (context.Headers.ContainsKey(Headers.AutoCorrelationSagaId))
			{
				Guid sagaId;
				if (Guid.TryParse(context.Headers[Headers.AutoCorrelationSagaId].ToString(), out sagaId))
				{
					return sagaId;
				}
			}

			return null;
		}
	}
}
