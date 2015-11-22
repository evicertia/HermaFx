using System;
using System.Collections.Generic;

namespace HermaFx
{
	public static class RandomExtensions
	{
		/// <summary>
		/// Returns an array of random bytes of 'length' size.
		/// </summary>
		/// <param name="this">The this.</param>
		/// <param name="length">The length.</param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentNullException">@this</exception>
		public static byte[] NextBytes(this Random @this, int length)
		{
			if (@this == null) throw new ArgumentNullException("@this");

			var result = new byte[length];
			@this.NextBytes(result);
			return result;
		}
	}
}
