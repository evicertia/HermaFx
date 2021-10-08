using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

using SysX509 = System.Security.Cryptography.X509Certificates;

namespace HermaFx.X509Certificates
{
	public enum X509FindBy
	{
		FilePath = 0
	}

	public class CertificateString 
	{
		private IDictionary<string, string> _entries = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
		private readonly string _certificateString = null;
		private X509Certificate2 _certificate = null;

		public StoreName? StoreName { get; private set; }
		public StoreLocation? StoreLocation { get; private set; }
		// Std find by type enum...
		public X509FindType? X509FindType { get; private set; }
		// Non-Std find by enum..
		public X509FindBy? X509FindBy { get; private set; }
		public string X509FindValue { get; private set; }
		public string KeyContainerName { get; private set; }
		public bool ValidOnly { get; private set; }

		public CertificateString(string certificateString)
		{
			if (string.IsNullOrEmpty(certificateString))
				throw new ArgumentNullException("certificateString");

			_certificateString = certificateString;

			// FIXME: Allow escaped semicolon caracters (ie. \; or semicolons between quotes "...;...")
			var list = certificateString.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

			foreach (var item in list)
			{
				// FIXME: Allow escaped equals caracters (ie. \= or between quotes "...=...")
				var tmp = item.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

				if (tmp.Count() != 2)
					throw new ArgumentException("Malformed certificate string property '{0}' has no property name (missing '=')");

				_entries[tmp[0]] = tmp[1];
			}

			if (_entries.ContainsKey("StoreName"))
				StoreName = (StoreName)Enum.Parse(typeof(StoreName), _entries["StoreName"]);
			
			if (_entries.ContainsKey("StoreLocation"))
				StoreLocation = (StoreLocation)Enum.Parse(typeof(StoreLocation), _entries["StoreLocation"]);

			// TODO: Check invalid find by combinations.
			if (_entries.ContainsKey("X509FindType"))
				X509FindType = (X509FindType)Enum.Parse(typeof(X509FindType), _entries["X509FindType"]);

			if (_entries.ContainsKey("X509FindBy"))
				X509FindBy = (X509FindBy)Enum.Parse(typeof(X509FindBy), _entries["X509FindBy"]);

			if (_entries.ContainsKey("X509FindValue"))
				X509FindValue = _entries["X509FindValue"];

			if (_entries.ContainsKey("Thumbprint"))
			{
				X509FindType = SysX509.X509FindType.FindByThumbprint;
				X509FindValue = _entries["Thumbprint"];
			}

			if (_entries.ContainsKey("KeyContainerName"))
				KeyContainerName = _entries["KeyContainerName"];

			if (_entries.ContainsKey("ValidOnly"))
				ValidOnly = bool.Parse(_entries["ValidOnly"]);
		}

		public X509Certificate2 _GetCertificate()
		{
			X509Store store = null;
			X509Certificate2Collection certs = null;
			var sn = this.StoreName.GetValueOrDefault(SysX509.StoreName.My);
			var sl = this.StoreLocation.GetValueOrDefault(SysX509.StoreLocation.LocalMachine);
			var xft = this.X509FindType.GetValueOrDefault(SysX509.X509FindType.FindByThumbprint);

			try
			{
				if (this.X509FindBy != null)
				{
					switch (this.X509FindBy.Value)
					{
						default:
							throw new NotImplementedException("Unsupported X509FindBy method.");
					}
				}

				// Try by looking up in store..
				store = new X509Store(sn, sl);
				store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
				certs = store.Certificates.Find(xft, this.X509FindValue, ValidOnly);

				if (certs == null || certs.Count == 0)
				{
					var str = string.Format("No valid certificate found for: {0}", _certificateString);
					throw new ApplicationException(str);
				}

				if (certs.Count > 1)
				{
					var str = string.Format("More than one matching certificate found for: {0}", _certificateString);
					throw new ApplicationException(str);
				}

#if false // FIXME: Setup certificate validation on mono
				if (!certs[0].Verify())
				{
					var str = string.Format("Certificate verification failed for: {0}", certificateString);
					throw new ApplicationException(str);
				}
#endif

				// Fix to avoid mono's bug #1201
				// See: https://github.com/mono/mono/commit/b52404b35394c9941b521622564e3dc061c95118
				if (!string.IsNullOrEmpty(this.KeyContainerName))
				{
					var csp = new CspParameters();
					csp.KeyContainerName = this.KeyContainerName;
					csp.Flags = CspProviderFlags.UseExistingKey;
					csp.Flags = csp.Flags | (sl == SysX509.StoreLocation.LocalMachine ? CspProviderFlags.UseMachineKeyStore : 0);
					certs[0].PrivateKey = new RSACryptoServiceProvider(csp);
				}

				return certs[0];
			}
			finally
			{
				if (store != null)
					store.Close();
			}
		}

		public X509Certificate2 GetCertificate()
		{
			if (_certificate == null)
				return _certificate = _GetCertificate();

			return _certificate;
		}

		public static X509Certificate2 GetCertificate(string certificateString)
		{
			var cs = new CertificateString(certificateString);
			return cs.GetCertificate();
		}
	}
}
