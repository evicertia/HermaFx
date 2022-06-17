using System;
using System.Configuration;
using System.IO;
using System.Reflection;

using Machine.Specifications;

namespace HermaFx.SimpleConfig.Tests
{
	public class AssemblyContext : IAssemblyContext
	{
		public void OnAssemblyComplete() { }
		public void OnAssemblyStart()
		{
			if (Utils.EnvironmentHelper.RunningOnDotNet)
			{
				// XXX: Sys.Configuration under net5+ loads testhost' config file.
				var dir = AppDomain.CurrentDomain.BaseDirectory;
				var file = Path.Combine(dir, $"{Assembly.GetExecutingAssembly().GetName().Name}.dll.config");
				AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", file);
			}
		}
	}
}
