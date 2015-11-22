using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HermaFx
{
	public static class Converter
	{
		public static T ChangeType<T>(object instance)
		{
			return (T)Convert.ChangeType(instance, typeof(T));
		}
	}
}
