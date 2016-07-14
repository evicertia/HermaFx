using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;

using HermaFx;
using HermaFx.Reflection;

namespace FluentBootstrap
{
	public static class BootstrapHelperExtensions
	{
		private static readonly MethodInfo _GetConfigAccessor = typeof(BootstrapHelper).GetMethod("GetConfig", BindingFlags.Instance | BindingFlags.NonPublic);

		public static BootstrapConfig GetConfig(this BootstrapHelper builder)
		{
			return (BootstrapConfig)_GetConfigAccessor.Invoke(builder, null);
		}
	}
}