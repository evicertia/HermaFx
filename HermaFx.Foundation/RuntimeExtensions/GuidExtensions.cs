using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HermaFx
{
	public static class GuidExtensions
	{
		public static bool TryParse(string input, out Guid result)
		{
			try
			{
				result = new Guid(input);
				return true;
			}
			catch { }

			result = default(Guid);
			return false;
		}
	}
}
