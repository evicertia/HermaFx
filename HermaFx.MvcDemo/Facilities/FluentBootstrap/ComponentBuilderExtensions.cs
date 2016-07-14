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
	public static class ComponentBuilderExtensions
	{
		private static readonly MethodInfo _GetConfigAccessor = typeof(ComponentBuilder).GetMethod("GetConfig", BindingFlags.Instance | BindingFlags.NonPublic);

		public static BootstrapConfig GetConfig(this ComponentBuilder builder)
		{
			return (BootstrapConfig)_GetConfigAccessor.Invoke(builder, null);
		}
	}
}