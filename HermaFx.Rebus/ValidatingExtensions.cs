using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Rebus;
using Rebus.Configuration;

using HermaFx.DataAnnotations;

namespace HermaFx.Rebus
{
	public static class ValidatingExtensions
	{
		public static RebusConfigurer ValidateOutgoingMessages(this RebusConfigurer configurer, IEnumerable<Type> inclusions = null, IEnumerable<Type> exclusions = null)
		{
			configurer.Events(evs =>
				evs.MessageSent += (ibus, dest, message) =>
				{
					if (message == null) return;
					if (exclusions != null && exclusions.Contains(message.GetType())) return;
					if (inclusions != null && !inclusions.Contains(message.GetType())) return;

					ExtendedValidator.EnsureIsValid(message);
				});

			return configurer;
		}

		public static RebusConfigurer ValidateIncomingMessages(this RebusConfigurer configurer, IEnumerable<Type> inclusions = null, IEnumerable<Type> exclusions = null)
		{
			configurer.Events(evs =>
				evs.BeforeHandling += (ibus, message, handler) =>
				{
					if (message == null) return;
					if (exclusions != null && exclusions.Contains(message.GetType())) return;
					if (inclusions != null && !inclusions.Contains(message.GetType())) return;

					ExtendedValidator.EnsureIsValid(message);
				});

			return configurer;
		}
	}
}
