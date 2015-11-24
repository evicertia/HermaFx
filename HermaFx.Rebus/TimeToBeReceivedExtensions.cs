using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HermaFx.DataAnnotations;

using Rebus;
using Rebus.Shared;
using Rebus.Configuration;

namespace HermaFx.Rebus
{
	public static class RebusConfigurerExtensions
	{
		public static RebusConfigurer UseTimeToExpireFrom<TAttribute>(this RebusConfigurer configurer, Func<TAttribute, TimeSpan> getter)
			where TAttribute : System.Attribute
		{
			Guard.IsNotNull(() => configurer, configurer);
			Guard.IsNotNull(() => getter, getter);

			return configurer.Events(e =>
			{
				e.MessageSent += (advbus, destination, message) =>
				{
					var attribute = message.GetType()
						.GetCustomAttributes(typeof(TimeoutAttribute), false)
						.Cast<TimeoutAttribute>()
						.SingleOrDefault();

					if (attribute != null)
					{
						advbus.AttachHeader(message, Headers.TimeToBeReceived, getter.ToString());
					}
				};
			});
		}

		public static RebusConfigurer UseTimeoutAttribute(this RebusConfigurer configurer)
		{
			return configurer.UseTimeToExpireFrom<TimeoutAttribute>(x => x.Timeout);
		}
	}

}
