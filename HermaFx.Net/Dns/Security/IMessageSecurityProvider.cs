using HermaFx.Net.Dns.Query;

namespace HermaFx.Net.Dns.Security
{
	public interface IMessageSecurityProvider
	{
		DnsQueryRequest SecureMessage(DnsQueryRequest dnsQueryRequest);
	}
}
