using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

using NUnit;
using NUnit.Framework;

namespace HermaFx.Cryptography
{
	[TestFixture]
	public class CryptoHelperTests
	{
		private RSAParameters GetRSAKeys()
		{
			var serializedKey = Properties.Resources.RSAKey;

			var serializer = new XmlSerializer(typeof(RSAParameters));
			RSAParameters result;

			using (TextReader reader = new StringReader(serializedKey))
			{
				result = (RSAParameters)serializer.Deserialize(reader);
			}

			return result;
		}

		[Test]
		public void RsaEncodeAndDecodeAsPkcs8PublicKey()
		{
			var kp = GetRSAKeys();

			var publicKeyPkcs8 = CryptoHelper.EncodeAsPkcs8PublicKey(kp);
			var decodedPrivateKey = CryptoHelper.DecodePkcs8PublicKey(publicKeyPkcs8);

			CollectionAssert.AreEqual(kp.Exponent, decodedPrivateKey.Exponent);
			CollectionAssert.AreEqual(kp.Modulus, decodedPrivateKey.Modulus);
		}

		[Test]
		public void RsaEncodeAndDecodeAsAsn1PublicKey()
		{
			var kp = GetRSAKeys();
			var publicKeyAsn1 = CryptoHelper.GetAsn1EncodedPublicKeyFor(kp);
			var decodedPrivateKey = CryptoHelper.DecodeAsn1RsaPublicKey(publicKeyAsn1);

			CollectionAssert.AreEqual(kp.Exponent, decodedPrivateKey.Exponent);
			CollectionAssert.AreEqual(kp.Modulus, decodedPrivateKey.Modulus);
		}

		[Test]
		public void RawEncryptionAndDecryption()
		{
			var kp = GetRSAKeys();
			var shortString = "This is a short string";
			var encodedString = CryptoHelper.RawEncrypt(kp, Encoding.UTF8.GetBytes(shortString));
			var decodedString = Encoding.UTF8.GetString(CryptoHelper.RawDecrypt(kp, encodedString));

			Assert.AreEqual(shortString, decodedString);
		}

		[Test]
		public void SimpleEncryptShortData()
		{
			var kp = GetRSAKeys();
			var longString = "This is a long string";
			var encodedString = CryptoHelper.SimpleEncrypt(kp, Encoding.UTF8.GetBytes(longString));
			var decodedString = Encoding.UTF8.GetString(CryptoHelper.SimpleDecrypt(kp, encodedString));

			Assert.AreEqual(longString, decodedString);
		}

		[Test]
		public void SimpleEncryptLongData()
		{
			var kp = GetRSAKeys();
			var longString = "This is a long string, kasjdfhaskdfjhaòihnawlknfvsdlkafnaslnvjwnvlksdnflsakfnsalkfjsakljfhaslkfjaslkfdjaslkfhaslkjdfhaksjfdhaskljdfaskldjfhasdkljfhasklfjsdahf";
			var encodedString = CryptoHelper.SimpleEncrypt(kp, Encoding.UTF8.GetBytes(longString));
			var decodedString = Encoding.UTF8.GetString(CryptoHelper.SimpleDecrypt(kp, encodedString));

			Assert.AreEqual(longString, decodedString);
		}

		[Test]
		public void GenerateCrc32ForString()
		{
			var @string = "Demo String Value";
			var bytes = Encoding.ASCII.GetBytes(@string);
			var calculatedHash = String.Empty;
			var expectedHash = "e2942afc"; //< Hex

			using (var crc32 = new Crc32())
				foreach (byte b in crc32.ComputeHash(bytes)) calculatedHash += b.ToString("x2").ToLower(); //< Hex

			Assert.AreEqual(expectedHash, calculatedHash);
		}
	}
}
