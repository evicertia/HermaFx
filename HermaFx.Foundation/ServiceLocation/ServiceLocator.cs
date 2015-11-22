using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

//using Microsoft.Practices.ServiceLocation;

namespace HermaFx.ServiceLocation
{
#if false
	/// <summary>
	/// This is a helper for accessing dependencies via the Common Service Locator (CSL).  But while
	/// the CSL will throw object reference errors if used before initialization, this will inform
	/// you of what the problem is.  Perhaps it would be more aptly named "InformativeServiceLocator."
	/// </summary>
	public static class ServiceLocator<TDependency>
			where TDependency : class
	{
		private static void InvokeLocator(Action<IServiceProvider> locator)
		{
			try
			{
				locator(ServiceLocator.Current);
			}
			catch (NullReferenceException ex)
			{
				throw new NullReferenceException("ServiceLocator has not been initialized; " +
						"I was trying to retrieve " + typeof(TDependency).ToString(), ex);
			}
			catch (ActivationException ex)
			{
				throw new ActivationException("The needed dependency of type " + typeof(TDependency).Name +
						" could not be located with the ServiceLocator. You'll need to register it with " +
						"the Common Service Locator (CSL) via your IoC's CSL adapter.", ex);
			}
		}


		/// <summary>
		/// Gets the service.
		/// </summary>
		/// <returns></returns>
		public static TDependency GetService()
		{
			TDependency service = null;

			InvokeLocator(x => { service = x.GetService(typeof(TDependency)) as TDependency; });
			return service;
		}

		/// <summary>
		/// Gets the service.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		public static TDependency GetService(string name)
		{
			TDependency service = null;

			InvokeLocator(x => { service = x.GetInstance<TDependency>(name); });
			return service;
		}

		/// <summary>
		/// Gets all services.
		/// </summary>
		/// <returns></returns>
		public static IEnumerable<TDependency> GetAllServices()
		{
			IEnumerable<TDependency> services = null;

			InvokeLocator(x => { services = x.GetAllInstances<TDependency>(); });
			return services;
		}
	}
#endif
}
