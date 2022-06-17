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
		[Settings]
		public interface DataCoding
		{
			[DefaultValue("UTF-16")]
			[TypeConverter(typeof(EncodingConverter))]
			Encoding Encoding1 { get; set; }

			[DefaultValue("ASCII")]
			[TypeConverter(typeof(EncodingConverter))]
			Encoding Encoding2 { get; set; }
		}

		private static readonly NameValueCollection _dict = new NameValueCollection()
		{
			{ typeof(DataCoding).Namespace + ":Encoding1", "UTF-8" },
		};

		[Test]
		public void CanConvertStringSettingsToEncodingUsingConverters()
		{
			var model = new SettingsAdapter().Create<DataCoding>(_dict);

			Assert.That(model.Encoding1, Is.AssignableTo<UTF8Encoding>());
			Assert.That(model.Encoding2, Is.AssignableTo<ASCIIEncoding>());
		}
	}
}
