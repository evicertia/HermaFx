using System;
using System.Text;
using System.ComponentModel;
using System.Collections.Specialized;

using NUnit.Framework;

using HermaFx.Settings;

namespace HermaFx.Text
{
	[TestFixture]
	public class EncodingConverterTest
	{
		#region Fake Encoding
		private class FakeEncoding : Encoding
		{
			#region Not Implemented Methods
			public override int GetByteCount(char[] chars, int index, int count)
			{
				throw new NotImplementedException();
			}

			public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
			{
				throw new NotImplementedException();
			}

			public override int GetCharCount(byte[] bytes, int index, int count)
			{
				throw new NotImplementedException();
			}

			public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
			{
				throw new NotImplementedException();
			}

			public override int GetMaxByteCount(int charCount)
			{
				throw new NotImplementedException();
			}

			public override int GetMaxCharCount(int byteCount)
			{
				throw new NotImplementedException();
			}
			#endregion

			#region Properties
			public override string BodyName
			{
				get
				{
					return "FAKE";
				}
			}

			public override string EncodingName
			{
				get
				{
					return "FAKE Encoding";
				}
			}
			#endregion
		}
		#endregion

		[Settings]
		public interface DataCoding
		{
			[DefaultValue("FAKE")]
			[TypeConverter(typeof(EncodingConverter))]
			[EncodingResolver(new [] { typeof(FakeEncoding) })]
			Encoding Encoder { get; set; }
		}

		private static readonly NameValueCollection _dict = new NameValueCollection()
		{
			{ typeof(DataCoding).Namespace + ":Encoder", "FAKE" }
		};

		[Test]
		public void EncodingConverterWithValueFakeEncoding()
		{
			var model = new SettingsAdapter().Create<DataCoding>(_dict);

			Assert.AreEqual(model.Encoder.GetType(), typeof(FakeEncoding), "#0");
		}
	}
}
