using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Sockets;

namespace HermaFx.DataAnnotations
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class IPAddressAttribute : ValidationAttribute
	{
		private const string _defaultErrorMessage = "Values is not a valid {0} network address";
		private readonly AddressFamily _family;

		public IPAddressAttribute()
			: this(AddressFamily.Unknown)
		{
		}

		public IPAddressAttribute(AddressFamily family)
				: base(string.Format(_defaultErrorMessage, family))
		{
			_family = family;
		}

		public override bool IsValid(object value)
		{
			IPAddress address = value as IPAddress;

			if (address == null)
			{
				var str = Convert.ToString(value);
				if (!IPAddress.TryParse(str, out address)) return false;
			}

			if (_family != AddressFamily.Unknown)
			{
				return address.AddressFamily == _family;
			}

			return true;
		}
	}
}