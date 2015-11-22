using System;
using System.Text;
using System.Security.Cryptography;

namespace HermaFx.Cryptography
{
	// INFO: For a brief description of ASN.1 encoding of PKCS#1 / PKCS#8 objects, see:
	//		 https://polarssl.org/kb/cryptography/asn1-key-structures-in-der-and-pem

	/// <summary>
	/// Helper functions to handle misc key management tasks.
	/// </summary>
	public static class CryptoHelper
	{
		private class Oid
		{
			// pkcs 1
			public const string rsaEncryption = "1.2.840.113549.1.1.1";
			// pkcs 7
			public const string data = "1.2.840.113549.1.7.1";
			public const string signedData = "1.2.840.113549.1.7.2";
			public const string envelopedData = "1.2.840.113549.1.7.3";
			public const string signedAndEnvelopedData = "1.2.840.113549.1.7.4";
			public const string digestedData = "1.2.840.113549.1.7.5";
			public const string encryptedData = "1.2.840.113549.1.7.6";
			// pkcs 9
			public const string contentType = "1.2.840.113549.1.9.3";
			public const string messageDigest = "1.2.840.113549.1.9.4";
			public const string signingTime = "1.2.840.113549.1.9.5";
			public const string countersignature = "1.2.840.113549.1.9.6";

			public Oid()
			{
			}
		}

		#region Private methods
		private static byte[] RemoveLeadingZero(byte[] bigInt)
		{
			int start = 0;
			int length = bigInt.Length;
			if (bigInt[0] == 0x00)
			{
				start = 1;
				length--;
			}
			byte[] bi = new byte[length];
			Buffer.BlockCopy(bigInt, start, bi, 0, length);
			return bi;
		}

		private static byte[] Normalize(byte[] bigInt, int length)
		{
			if (bigInt.Length == length)
				return bigInt;
			else if (bigInt.Length > length)
				return RemoveLeadingZero(bigInt);
			else
			{
				// pad with 0
				byte[] bi = new byte[length];
				Buffer.BlockCopy(bigInt, 0, bi, (length - bigInt.Length), bigInt.Length);
				return bi;
			}
		}

		private static byte[] UniqueIdentifier(byte[] id)
		{
			// UniqueIdentifier  ::=  BIT STRING
			ASN1 uid = new ASN1(0x03);
			// first byte in a BITSTRING is the number of unused bits in the first byte
			byte[] v = new byte[id.Length + 1];
			Buffer.BlockCopy(id, 0, v, 1, id.Length);
			uid.Value = v;
			return uid.GetBytes();
		}
		#endregion

		#region Common RSA methods
		private static TResult ExecUsingEphemeralRsaCsp<TResult>(RSAParameters keyParameters, Func<RSACryptoServiceProvider,TResult> action)
		{
			// Select target CSP
			var cspParams = new CspParameters();
#if NET_4_0
			cspParams.Flags |= CspProviderFlags.CreateEphemeralKey;
#endif

			using (var rsa = new RSACryptoServiceProvider(cspParams))
			{
				rsa.ImportParameters(keyParameters);
				rsa.PersistKeyInCsp = false;
				return action(rsa);
			}
		}


		/// <summary>
		/// Generates the RSA key pair.
		/// </summary>
		/// <returns>
		/// The newly generated key pair as an ASN1-encoded byte array.
		/// </returns>
		public static RSAParameters GenerateRsaKeyPair(int keySize)
		{
			var cspParams = new CspParameters() { };

#if NET_4_0
			cspParams.Flags |= CspProviderFlags.CreateEphemeralKey;
#endif

			using (var rsa = new RSACryptoServiceProvider(keySize, cspParams))
			{
				try
				{
					// Do something with the key...
					// Encrypt, export, etc.
				}
				finally
				{
					rsa.PersistKeyInCsp = false;
				}

				return rsa.ExportParameters(true);
			}
		}

		/// <summary>
		/// Gets the RSA public key parameters.
		/// </summary>
		/// <param name="keyParameters">The key parameters.</param>
		/// <returns></returns>
		public static RSAParameters GetRsaPublicKeyParameters(RSAParameters keyParameters)
		{
			return new RSAParameters()
			{
				Exponent = keyParameters.Exponent.Clone() as byte[],
				Modulus = keyParameters.Modulus.Clone() as byte[]
			};
		}

		/// <summary>
		/// Gets the finger print for.
		/// </summary>
		/// <param name="keyParameters">The rsa key parameters.</param>
		/// <returns></returns>
		public static byte[] GetPublicKeyFingerPrintFor(RSAParameters keyParameters)
		{
			using (var hasher = SHA1.Create())
			{
				var pubKey = GetRsaPublicKeyParameters(keyParameters);
				return hasher.ComputeHash(EncodeAsPkcs8PublicKey(pubKey));
			}
		}

		/// <summary>
		/// Gets the finger print for.
		/// </summary>
		/// <param name="keyParameters">The rsa key parameters.</param>
		/// <returns></returns>
		public static byte[] GetAsn1PublicKeyFingerPrintFor(RSAParameters keyParameters)
		{
			using (var hasher = SHA1.Create())
			{
				var pubKey = GetRsaPublicKeyParameters(keyParameters);
				return hasher.ComputeHash(EncodeAsAsn1(pubKey));
			}
		}

		/// <summary>
		/// Determines whether [is valid private key] [the specified key parameters].
		/// </summary>
		/// <param name="keyParameters">The key parameters.</param>
		/// <returns>
		///   <c>true</c> if [is valid private key] [the specified key parameters]; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsValidPrivateKey(RSAParameters keyParameters)
		{
			return keyParameters.Modulus != null && keyParameters.Exponent != null
				&& keyParameters.D != null && keyParameters.DP != null
				&& keyParameters.DQ != null && keyParameters.InverseQ != null
				&& keyParameters.P != null && keyParameters.Q != null;
		}

		/// <summary>
		/// Determines whether [is valid public key] [the specified key parameters].
		/// </summary>
		/// <param name="keyParameters">The key parameters.</param>
		/// <returns>
		///   <c>true</c> if [is valid public key] [the specified key parameters]; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsValidPublicKey(RSAParameters keyParameters)
		{
			return keyParameters.Modulus != null && keyParameters.Exponent != null;
		}
		#endregion

		#region RSA Encryption & Decryption
		/// <summary>
		/// Encrypts the input data using the specified key parameters.
		/// </summary>
		/// <param name="keyParameters">The key parameters.</param>
		/// <param name="inputData">The input data.</param>
		/// <returns>The encrypted data.</returns>
		/// <exception cref="System.InvalidOperationException">A public RSA key is required.</exception>
		public static byte[] RawEncrypt(RSAParameters keyParameters, byte[] inputData)
		{
			if (!IsValidPublicKey(keyParameters))
				throw new InvalidOperationException("A public RSA key is required.");

			return ExecUsingEphemeralRsaCsp(keyParameters, x => x.Encrypt(inputData, true));
		}

		/// <summary>
		/// Encrypts the input string using the specified key parameters.
		/// </summary>
		/// <param name="keyParameters">The key parameters.</param>
		/// <param name="inputString">The input string.</param>
		/// <returns>The encrypted data encoded as base64.</returns>
		public static string RawEncrypt(RSAParameters keyParameters, string inputString)
		{
			var inputData = Encoding.UTF8.GetBytes(inputString);
			var encryptedData = RawEncrypt(keyParameters, inputData);
			return Convert.ToBase64String(encryptedData);
		}

		/// <summary>
		/// Decrypts the input data using the specified key parameters.
		/// </summary>
		/// <param name="keyParameters">The key parameters.</param>
		/// <param name="inputData">The input data.</param>
		/// <returns></returns>
		public static byte[] RawDecrypt(RSAParameters keyParameters, byte[] inputData)
		{
			if (!IsValidPrivateKey(keyParameters))
				throw new InvalidOperationException("A private RSA key is required.");

			return ExecUsingEphemeralRsaCsp(keyParameters, x => x.Decrypt(inputData, true));
		}

		/// <summary>
		/// Decrypts the (base64-encoded) input string using the specified key parameters.
		/// </summary>
		/// <param name="keyParameters">The key parameters.</param>
		/// <param name="inputString">The input string.</param>
		/// <returns>The decrypted data as an UTF-8 string.</returns>
		public static string RawDecrypt(RSAParameters keyParameters, string inputString)
		{
			var inputData = Convert.FromBase64String(inputString);
			var decryptedData = RawDecrypt(keyParameters, inputData);
			return Encoding.UTF8.GetString(decryptedData);
		}

		/// <summary>
		/// Performs a simple encryption of inputData using a 
		/// combination of PublicKey + SymmetricKey encryption.
		/// </summary>
		/// <param name="rsa">The RSA.</param>
		/// <param name="input">The input.</param>
		/// <returns></returns>
		/// <remarks>See: http://pages.infinit.net/ctech/20031101-0151.html </remarks>
		public static byte[] SimpleEncrypt(RSA rsa, byte[] input)
		{
			var sa = SymmetricAlgorithm.Create("AES");
			var ct = sa.CreateEncryptor();
			byte[] encrypt = ct.TransformFinalBlock(input, 0, input.Length);

			var fmt = new RSAPKCS1KeyExchangeFormatter(rsa);
			byte[] keyex = fmt.CreateKeyExchange(sa.Key);

			// return the key exchange, the IV (public) and encrypted data
			byte[] result = new byte[keyex.Length + sa.IV.Length + encrypt.Length];
			Buffer.BlockCopy(keyex, 0, result, 0, keyex.Length);
			Buffer.BlockCopy(sa.IV, 0, result, keyex.Length, sa.IV.Length);
			Buffer.BlockCopy(encrypt, 0, result, keyex.Length + sa.IV.Length, encrypt.Length);
			return result;
		}

		/// <summary>
		/// Performs a simple encryption of inputData using a 
		/// combination of PublicKey + SymmetricKey encryption.
		/// </summary>
		/// <param name="keyParameters">The key parameters.</param>
		/// <param name="input">The input.</param>
		/// <returns></returns>
		/// <exception cref="System.InvalidOperationException">A public RSA key is required.</exception>
		public static byte[] SimpleEncrypt(RSAParameters keyParameters, byte[] input)
		{
			if (!IsValidPublicKey(keyParameters))
				throw new InvalidOperationException("A public RSA key is required.");

			return ExecUsingEphemeralRsaCsp(keyParameters, x => SimpleEncrypt(x, input));
		}

		/// <summary>
		/// Performs a simple encryption of inputData using a 
		/// combination of PublicKey + SymmetricKey encryption.
		/// </summary>
		/// <param name="keyParameters">The key parameters.</param>
		/// <param name="input">The input.</param>
		/// <returns></returns>
		/// <exception cref="System.InvalidOperationException">A public RSA key is required.</exception>
		public static byte[] SimpleEncrypt(RSAParameters keyParameters, string input)
		{
			if (!IsValidPublicKey(keyParameters))
				throw new InvalidOperationException("A public RSA key is required.");

			var bytes = Encoding.UTF8.GetBytes(input);
			return ExecUsingEphemeralRsaCsp(keyParameters, x => SimpleEncrypt(x, bytes));
		}

		/// <summary>
		/// Performs a simple decryption of input using a 
		/// combination of PublicKey + SymmetricKey decryption.
		/// </summary>
		/// <param name="rsa">The RSA.</param>
		/// <param name="input">The input.</param>
		/// <returns></returns>
		public static byte[] SimpleDecrypt(RSA rsa, byte[] input)
		{
			byte[] keyex = new byte[rsa.KeySize >> 3];
			Buffer.BlockCopy(input, 0, keyex, 0, keyex.Length);

			var def = new RSAPKCS1KeyExchangeDeformatter(rsa);
			byte[] key = def.DecryptKeyExchange(keyex);

			var sa = SymmetricAlgorithm.Create("AES");
			byte[] iv = new byte[sa.IV.Length];
			Buffer.BlockCopy(input, keyex.Length, iv, 0, iv.Length);

			var ct = sa.CreateDecryptor(key, iv);
			byte[] decrypt = ct.TransformFinalBlock(input, keyex.Length + iv.Length, input.Length - (keyex.Length + iv.Length));
			return decrypt;
		}

		/// <summary>
		/// Performs a simple decryption of input using a 
		/// combination of PublicKey + SymmetricKey decryption.
		/// </summary>
		/// <param name="keyParameters">The key parameters.</param>
		/// <param name="input">The input.</param>
		/// <returns></returns>
		/// <exception cref="System.InvalidOperationException">A private RSA key is required.</exception>
		public static byte[] SimpleDecrypt(RSAParameters keyParameters, byte[] input)
		{
			if (!IsValidPublicKey(keyParameters))
				throw new InvalidOperationException("A private RSA key is required.");

			return ExecUsingEphemeralRsaCsp(keyParameters, x => SimpleDecrypt(x, input));
		}

		/// <summary>
		/// Performs a simple decryption of input using a 
		/// combination of PublicKey + SymmetricKey decryption.
		/// </summary>
		/// <param name="keyParameters">The key parameters.</param>
		/// <param name="input">The input.</param>
		/// <returns></returns>
		/// <exception cref="System.InvalidOperationException">A private RSA key is required.</exception>
		public static byte[] SimpleDecrypt(RSAParameters keyParameters, string input)
		{
			if (!IsValidPublicKey(keyParameters))
				throw new InvalidOperationException("A private RSA key is required.");

			var bytes = Encoding.UTF8.GetBytes(input);
			return ExecUsingEphemeralRsaCsp(keyParameters, x => SimpleDecrypt(x, bytes));
		}
		#endregion

		#region RSA Signing & Verification
		/// <summary>
		/// Performs a simple RSA signing of the specified hash based on PKCS#1 1.5 padding schemes.
		/// </summary>
		/// <param name="rsa">The RSA.</param>
		/// <param name="hashAlgorithm">The hash algorithm.</param>
		/// <param name="hashValue">The hash value.</param>
		/// <returns></returns>
		public static byte[] SimpleSignHash(RSA rsa, string hashAlgorithm, byte[] hashValue)
		{
			// Create an RSAPKCS1SignatureFormatter object and pass it the 
			// RSA instance to transfer the private key.
			var formatter = new RSAPKCS1SignatureFormatter(rsa);
			formatter.SetHashAlgorithm(hashAlgorithm);
			return formatter.CreateSignature(hashValue);
		}

		/// <summary>
		/// Performs a simple RSA signing of the specified hash based on PKCS#1 1.5 padding schemes.
		/// </summary>
		/// <param name="parameters">The parameters.</param>
		/// <param name="hashAlgorithm">The hash algorithm.</param>
		/// <param name="hashValue">The hash value.</param>
		/// <returns></returns>
		/// <exception cref="System.InvalidOperationException">A private RSA key is required.</exception>
		public static byte[] SimpleSignHash(RSAParameters parameters, string hashAlgorithm, byte[] hashValue)
		{
			if (!IsValidPrivateKey(parameters))
				throw new InvalidOperationException("A private RSA key is required.");

			return ExecUsingEphemeralRsaCsp(parameters, x => SimpleSignHash(x, hashAlgorithm, hashValue));
		}

		/// <summary>
		/// Performs a simple RSA signing of the specified bytes based on PKCS#1 1.5 padding schemes.
		/// </summary>
		/// <param name="rsa">The RSA.</param>
		/// <param name="hashAlgorithm">The hash algorithm.</param>
		/// <param name="bytes">The bytes.</param>
		/// <returns></returns>
		public static byte[] SimpleSign(RSA rsa, string hashAlgorithm, byte[] bytes)
		{
			var hasher = HashAlgorithm.Create(hashAlgorithm);
			return SimpleSignHash(rsa, hashAlgorithm, hasher.ComputeHash(bytes));
		}

		/// <summary>
		/// Performs a simple RSA signing of the specified bytes based on PKCS#1 1.5 padding schemes.
		/// </summary>
		/// <param name="rsa">The RSA.</param>
		/// <param name="hashAlgorithm">The hash algorithm.</param>
		/// <param name="bytes">The bytes.</param>
		/// <returns></returns>
		public static byte[] SimpleSign(RSAParameters parameters, string hashAlgorithm, byte[] bytes)
		{
			var hasher = HashAlgorithm.Create(hashAlgorithm);
			return SimpleSignHash(parameters, hashAlgorithm, hasher.ComputeHash(bytes));
		}

		/// <summary>
		/// Verifies a simple RSA signature of the specified hash based on PKCS#1 1.5 padding schemes.
		/// </summary>
		/// <param name="rsa">The RSA.</param>
		/// <param name="hashAlgorithm">The hash algorithm.</param>
		/// <param name="hashValue">The hash value.</param>
		/// <param name="signature">The signature.</param>
		/// <returns></returns>
		public static bool VerifySignHash(RSA rsa, string hashAlgorithm, byte[] hashValue, byte[] signature)
		{
			var deformatter = new RSAPKCS1SignatureDeformatter(rsa);
			deformatter.SetHashAlgorithm(hashAlgorithm);
			return deformatter.VerifySignature(hashValue, signature);
		}

		/// <summary>
		/// Verifies a simple RSA signature of the specified hash based on PKCS#1 1.5 padding schemes.
		/// </summary>
		/// <param name="rsa">The RSA.</param>
		/// <param name="hashAlgorithm">The hash algorithm.</param>
		/// <param name="hashValue">The hash value.</param>
		/// <param name="signature">The signature.</param>
		/// <returns></returns>
		public static bool VerifySignHash(RSAParameters parameters, string hashAlgorithm, byte[] hashValue, byte[] signature)
		{
			if (!IsValidPublicKey(parameters))
				throw new InvalidOperationException("A public RSA key is required.");

			return ExecUsingEphemeralRsaCsp(parameters, x => VerifySignHash(x, hashAlgorithm, hashValue, signature));
		}

		/// <summary>
		/// Verifies a simple RSA signature of the specified bytes based on PKCS#1 1.5 padding schemes.
		/// </summary>
		/// <param name="rsa">The RSA.</param>
		/// <param name="hashAlgorithm">The hash algorithm.</param>
		/// <param name="hashValue">The hash value.</param>
		/// <param name="signature">The signature.</param>
		/// <returns></returns>
		public static bool VerifySign(RSA rsa, string hashAlgorithm, byte[] bytes, byte[] signature)
		{
			var hasher = HashAlgorithm.Create(hashAlgorithm);
			return VerifySignHash(rsa, hashAlgorithm, hasher.ComputeHash(bytes), signature);
		}

		/// <summary>
		/// Verifies a simple RSA signature of the specified hash based on PKCS#1 1.5 padding schemes.
		/// </summary>
		/// <param name="rsa">The RSA.</param>
		/// <param name="hashAlgorithm">The hash algorithm.</param>
		/// <param name="hashValue">The hash value.</param>
		/// <param name="signature">The signature.</param>
		/// <returns></returns>
		public static bool VerifySign(RSAParameters parameters, string hashAlgorithm, byte[] bytes, byte[] signature)
		{
			var hasher = HashAlgorithm.Create(hashAlgorithm);
			return VerifySignHash(parameters, hashAlgorithm, hasher.ComputeHash(bytes), signature);
		}
		#endregion

		#region PKCS#1 helpers
		/// <summary>
		/// Encodes the RSAParameters as an asn1-encoded RSAPublicKey or RSAPrivateKey struct, as defined by PKCS#1 / rfc3447.
		/// </summary>
		/// <param name="rsa">The RSA.</param>
		/// <returns></returns>
		/// <remarks>Adapted from Mono.Security</remarks>
		public static byte[] EncodeAsAsn1(RSAParameters param)
		{
			ASN1 asn1;

			// See: http://tools.ietf.org/html/rfc3447
			//		- A.1.1 RSA public key syntax
			//		- A.1.2 RSA private key syntax

			if (param.D == null)
			{
				// Emit a RSAPublicKey ASN.1 type.
				asn1 = new ASN1(0x30);
				asn1.Add(ASN1Convert.FromUnsignedBigInteger(param.Modulus));
				asn1.Add(ASN1Convert.FromUnsignedBigInteger(param.Exponent));
			}
			else
			{
				// Emit a RSAPrivateKey ASN.1 type.
				asn1 = new ASN1(0x30);
				asn1.Add(new ASN1(0x02, new byte[1] { 0x00 }));
				asn1.Add(ASN1Convert.FromUnsignedBigInteger(param.Modulus));
				asn1.Add(ASN1Convert.FromUnsignedBigInteger(param.Exponent));
				asn1.Add(ASN1Convert.FromUnsignedBigInteger(param.D));
				asn1.Add(ASN1Convert.FromUnsignedBigInteger(param.P));
				asn1.Add(ASN1Convert.FromUnsignedBigInteger(param.Q));
				asn1.Add(ASN1Convert.FromUnsignedBigInteger(param.DP));
				asn1.Add(ASN1Convert.FromUnsignedBigInteger(param.DQ));
				asn1.Add(ASN1Convert.FromUnsignedBigInteger(param.InverseQ));
			}

			return asn1.GetBytes();
		}

		/// <summary>
		/// Decodes the asn1 encoded RSAPrivateKey struct, as defined by PKCS#1 / rfc3447.
		/// </summary>
		/// <param name="asn1blob">The asn1-encoded blob.</param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentException">
		/// invalid private key format
		/// or
		/// missing version
		/// or
		/// not enough key parameters
		/// </exception>
		/// <exception cref="System.ArgumentException">invalid private key format
		/// or
		/// missing version
		/// or
		/// not enough key parameters</exception>
		/// <remarks>
		/// Adapted from Mono.Security
		/// </remarks>
		public static RSAParameters DecodeAsn1RsaPrivateKey(byte[] asn1blob)
		{
			ASN1 privateKey = new ASN1(asn1blob);
			if (privateKey.Tag != 0x30)
				throw new ArgumentException("invalid private key format");

			ASN1 version = privateKey[0];
			if (version.Tag != 0x02)
				throw new ArgumentException("missing version");

			if (privateKey.Count < 9)
				throw new ArgumentException("not enough key parameters");

			RSAParameters param = new RSAParameters();
			// note: MUST remove leading 0 - else MS wont import the key
			param.Modulus = RemoveLeadingZero(privateKey[1].Value);
			int keysize = param.Modulus.Length;
			int keysize2 = (keysize >> 1); // half-size
			// size must be normalized - else MS wont import the key
			param.D = Normalize(privateKey[3].Value, keysize);
			param.DP = Normalize(privateKey[6].Value, keysize2);
			param.DQ = Normalize(privateKey[7].Value, keysize2);
			param.Exponent = RemoveLeadingZero(privateKey[2].Value);
			param.InverseQ = Normalize(privateKey[8].Value, keysize2);
			param.P = Normalize(privateKey[4].Value, keysize2);
			param.Q = Normalize(privateKey[5].Value, keysize2);

			return param;
		}

		/// <summary>
		/// Decodes the PKCS#11 / rfc3447 asn1-encoded RSA public key.
		/// </summary>
		/// <param name="asn1blob">The asn1blob.</param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentException">invalid asn1 data.</exception>
		private static RSAParameters DecodeAsn1RsaPublicKey(ASN1 asn1)
		{
			if (asn1.Count != 2)
				throw new ArgumentException("invalid asn1 data. Tags count: " + asn1.Count);

			return new RSAParameters()
			{
				Modulus = RemoveLeadingZero(asn1[0].Value),
				Exponent = RemoveLeadingZero(asn1[1].Value)
			};
		}

		/// <summary>
		/// Decodes the PKCS#11 / rfc3447 asn1-encoded RSA public key.
		/// </summary>
		/// <param name="asn1blob">The asn1blob.</param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentException">invalid asn1 data.</exception>
		public static RSAParameters DecodeAsn1RsaPublicKey(byte[] asn1blob)
		{
			return DecodeAsn1RsaPublicKey(new ASN1(asn1blob));
		}

		/// <summary>
		/// Gets the asn1 encoded public key for.
		/// </summary>
		/// <param name="keyParameters">The key parameters.</param>
		/// <returns></returns>
		public static byte[] GetAsn1EncodedPublicKeyFor(RSAParameters keyParameters)
		{
			var pubKey = GetRsaPublicKeyParameters(keyParameters);
			return EncodeAsAsn1(pubKey);
		}

		/// <summary>
		/// Gets the asn1 encoded public key.
		/// </summary>
		/// <param name="asn1PrivateKey">The asn1-encoded private key.</param>
		/// <returns></returns>
		public static byte[] GetAsn1EncodedPublicKeyFor(byte[] asn1PrivateKey)
		{
			var privKey = DecodeAsn1RsaPrivateKey(asn1PrivateKey);
			return GetAsn1EncodedPublicKeyFor(privKey);
		}
		#endregion

		#region PKCS#7 methods
		private static ASN1 AlgorithmIdentifier(string oid)
		{
			ASN1 ai = new ASN1(0x30);
			ai.Add(ASN1Convert.FromOid(oid));
			ai.Add(new ASN1(0x05));	// NULL
			return ai;
		}

		private static ASN1 AlgorithmIdentifier(string oid, ASN1 parameters)
		{
			ASN1 ai = new ASN1(0x30);
			ai.Add(ASN1Convert.FromOid(oid));
			ai.Add(parameters);
			return ai;
		}
		#endregion

		#region PKCS#8 helpers
		/// <summary>
		/// Encodes as PKCS8 private key.
		/// </summary>
		/// <param name="keyParameters">The key parameters.</param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentException">Argument should be an RSA private key.</exception>
		public static byte[] EncodeAsPkcs8PrivateKey(RSAParameters keyParameters)
		{
			if (!IsValidPrivateKey(keyParameters))
				throw new ArgumentException("Argument should be an RSA private key.");

			var privKeyInfo = new PKCS8.PrivateKeyInfo()
			{
				Algorithm = Oid.rsaEncryption,
				PrivateKey = EncodeAsAsn1(keyParameters)
			};

			return privKeyInfo.GetBytes();
		}

		/// <summary>
		/// Encodes as PKCS8 public key.
		/// </summary>
		/// <param name="keyParameters">The key parameters.</param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentException">Argument should be an RSA public key.</exception>
		public static byte[] EncodeAsPkcs8PublicKey(RSAParameters keyParameters)
		{
			if (!IsValidPublicKey(keyParameters))
				throw new ArgumentException("Argument should be an RSA public key.");

			var asn1 = new ASN1(0x30);
			asn1.Add(AlgorithmIdentifier(Oid.rsaEncryption));
			asn1.Add(new ASN1(UniqueIdentifier(GetAsn1EncodedPublicKeyFor(keyParameters))));

			return asn1.GetBytes();
		}

		/// <summary>
		/// Decodes the PKCS8 private key.
		/// </summary>
		/// <param name="pkcs8blob">The pkcs8blob.</param>
		/// <returns></returns>
		public static RSAParameters DecodePkcs8PrivateKey(byte[] pkcs8blob)
		{
			var privKeyInfo = new PKCS8.PrivateKeyInfo(pkcs8blob);

			if (privKeyInfo.Algorithm != Oid.rsaEncryption)
				throw new NotSupportedException("Only RSA keys are currently supported.");

			return DecodeAsn1RsaPrivateKey(privKeyInfo.PrivateKey);
		}

		public static RSAParameters DecodePkcs8PublicKey(byte[] pkcs8blob)
		{
			// See: http://tools.ietf.org/html/rfc5280
			//		http://www.cryptopp.com/wiki/Keys_and_Formats#Dumping_PKCS_.238_and_X.509_Keys
			//
			//	SubjectPublicKeyInfo  ::=  SEQUENCE  {
			//		algorithm            AlgorithmIdentifier,
			//		subjectPublicKey     BIT STRING  }

			ASN1 publicKeyInfo = new ASN1(pkcs8blob);
			if (publicKeyInfo.Tag != 0x30)
				throw new CryptographicException("invalid PublicKeyInfo");

			ASN1 keyAlgorithm = publicKeyInfo[0];
			if (keyAlgorithm.Tag != 0x30)
				throw new CryptographicException("invalid algorithm");

			ASN1 algorithm = keyAlgorithm[0];
			if (algorithm.Tag != 0x06)
				throw new CryptographicException("missing algorithm OID");

			if (ASN1Convert.ToOid(algorithm) != Oid.rsaEncryption)
				throw new NotSupportedException("Only RSA keys are currently supported.");

			var keyBits = publicKeyInfo[1];

			if (keyBits.Tag != 0x03)
				throw new CryptographicException("Invalid or corrupted key container, expecint a BIT STRING, found tag: " + keyBits.Tag);

			var sanitizedKeyBits = RemoveLeadingZero(keyBits.Value);
			return DecodeAsn1RsaPublicKey(new ASN1(sanitizedKeyBits));
		}
		#endregion
	}
}
