using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace HermaFx.Utils
{

	public class AdvancedGuidParserStubs
	{
		public string GuidString => "725a29a5-58ed-4d07-ab45-f676dda2e342";
		public Guid Guid => Guid.Parse(GuidString);

		public string ZBase32String => "wwwiwhzpmydw5k4f635p5ezdee";
		public string Base64StringPadding => "pSlacu1YB02rRfZ23aLjQg==";
		public string Base64StringNoPadding => Base64StringPadding.TrimEnd('=');
		public string InvalidString => "Qg==";

	}

	[TestFixture]
	public class AdvancedGuidParserFixture
	{
		private AdvancedGuidParser _guidParser = new AdvancedGuidParser();
		private AdvancedGuidParserStubs _stubs = new AdvancedGuidParserStubs();

		#region ZBase32

		[Test]
		public void ParseFromZBase32String_Return_valid_Guid()
		{
			Assert.That(_guidParser.ParseFromZBase32String(_stubs.ZBase32String), Is.EqualTo(_stubs.Guid));
		}

		[Test]
		public void ParseFromZBase32String_Expect_Exception_When_Length_Is_NotValid()
		{
			Assert.Throws<ArgumentException>(() => _guidParser.ParseFromZBase32String(_stubs.InvalidString));
		}

		[Test]
		public void TryParseFromZBase32String_Output_Valid_Guid_Returning_True_From_Valid_String()
		{
			Guid result;

			Assert.That(_guidParser.TryParseFromBase32String(_stubs.ZBase32String, out result), Is.True);
			Assert.That(result, Is.EqualTo(_stubs.Guid));
		}

		[Test]
		public void TryParseFromZBase32String_Output_Guid_Empty_Returning_False_From_NotValid_String()
		{
			Guid result;

			Assert.That(_guidParser.TryParseFromBase32String(_stubs.InvalidString, out result), Is.False);
			Assert.That(result, Is.EqualTo(Guid.Empty));
		}

		#endregion

		#region Base64

		[Test]
		public void ParseFromBase64String_From_Valid_String_With_Padding_Return_valid_Guid()
		{
			Assert.That(_guidParser.ParseFromBase64String(_stubs.Base64StringPadding), Is.EqualTo(_stubs.Guid));
		}

		[Test]
		public void ParseFromBase64String_From_Valid_String_Wo_Padding_Return_valid_Guid()
		{
			Assert.That(_guidParser.ParseFromBase64String(_stubs.Base64StringNoPadding), Is.EqualTo(_stubs.Guid));
		}

		[Test]
		public void ParseFromBase64String_From_Invalid_String_Lenght_Expect_Exception()
		{
			Assert.Throws<ArgumentException>(() => _guidParser.ParseFromBase64String(_stubs.InvalidString));
		}

		[Test]
		public void TryParseFromBase64String_Output_Valid_Guid_Returning_True_From_Valid_String()
		{
			Guid result;

			Assert.That(_guidParser.TryParseFromBase64String(_stubs.Base64StringPadding, out result), Is.True);
			Assert.That(result, Is.EqualTo(_stubs.Guid));
		}

		[Test]
		public void TryParseFromBase64String_Output_Guid_Empty_Returning_False_From_NotValid_String()
		{
			Guid result;

			Assert.That(_guidParser.TryParseFromBase64String(_stubs.InvalidString, out result), Is.False);
			Assert.That(result, Is.EqualTo(Guid.Empty));
		}

		#endregion

		#region Parse

		// From Base64
		[Test]
		public void Parse_Return_Valid_Guid_For_Valid_Input_Base64String_With_Padding()
		{
			Assert.That(_guidParser.Parse(_stubs.Base64StringPadding), Is.EqualTo(_stubs.Guid));
		}

		[Test]
		public void Parse_Return_Valid_Guid_For_Valid_Input_Base64String_Wo_Padding()
		{
			Assert.That(_guidParser.Parse(_stubs.Base64StringNoPadding), Is.EqualTo(_stubs.Guid));
		}

		// From Zbase32
		[Test]
		public void Parse_Return_Valid_Guid_For_Valid_Input_ZBase32String()
		{
			Assert.That(_guidParser.Parse(_stubs.ZBase32String), Is.EqualTo(_stubs.Guid));
		}

		// Guid String 
		[Test]
		public void Parse_Return_Valid_Guid_For_Valid_Input_GuidString()
		{
			Assert.That(_guidParser.Parse(_stubs.GuidString.ToString()), Is.EqualTo(_stubs.Guid));
		}

		[Test]
		public void Parse_Expect_Exception_For_Invalid_InputStrings()
		{
			Assert.Throws<FormatException>(() => _guidParser.Parse(_stubs.InvalidString));
		}

		#endregion

	}
}
