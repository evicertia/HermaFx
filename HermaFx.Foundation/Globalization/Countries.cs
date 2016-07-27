using System;
using System.Linq;

using HermaFx.DesignPatterns;

namespace HermaFx.Globalization
{
	[Serializable]
	public class Countries : EnhancedEnumType<Countries, Country>
	{
		#region .ctor
		public Countries() : base(x => x.Iso3)
		{
		}
		#endregion

		#region Enhanced Enum Members

#if false
		// COUNTRIES FROM FRAMEWORK. This list is obtained using LinqPad
		// Generated 26/07/2016
		// More Info: http://data.okfn.org/data/core/country-codes
		//
		// PS: You need to set the IsMemberOfEU property value manually after adding the elements.

		public string GetCSV(string url)
		{
			HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
			HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

			StreamReader sr = new StreamReader(resp.GetResponseStream());
			string results = sr.ReadToEnd();
			sr.Close();

			return results;
		}

		void Main()
		{
			var csvData = GetCSV(@"http://data.okfn.org/data/core/country-codes/r/country-codes.csv").Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Skip(1);

			var splittedByQuotes = csvData.Select(row => row.Split(new char[] { '\"' }));

			var rowsFormatted = splittedByQuotes.Select(x => string.Concat(x.Select(
				(row, index) =>
					index % 2 == 0
					? row.Replace(',', ';')
					: row)));

			var countries = rowsFormatted.Select(cty => cty.Split(';'))
			.Where(row => !row[0].IsNullOrWhiteSpace())
			.Select(row => new
			{
				Name = row[0],
				Official_Name_EN = row[1],
				Iso2 = row[3],
				Iso3 = row[4],
				IsoNum = row[5],
				Itu = row[6],
				Marc = row[7],
				Wmo = row[8],
				Ds = row[9],
				Dial = row[10],
				Fips = row[12],
				CurrencyAlphabetic = row[15],
				CurrencyCountry = row[16],
				CurrencyMinorUnit = row[17],
				CurrencyName = row[18],
				CurrencyNumeric = row[19],
				IsIndependent = row[20],
				Capital = row[21],
				Continent = row[22],
				Tld = row[23],
				Languages = row[24]
			})
			.OrderBy(row => row.Iso3)
			.Select(x =>
						string.Format("public static Country {0} = new Country(\"{0}\", () => \"{1}\",() => \"{2}\", \"{3}\", \"{4}\", \"{5}\", \"{6}\"" +
							", \"{7}\", \"{8}\", \"{9}\", \"{10}\", \"{11}\", \"{12}\", \"{13}\", \"{14}\", \"{15}\", \"{16}\"" +
							", \"{17}\", \"{18}\", \"{19}\", new string[] {{\"{20}\"}} , {21});",
							x.Iso3,
							x.Name,
							x.Official_Name_EN,
							x.Iso2,
							x.IsoNum,
							x.Itu,
							x.Marc,
							x.Wmo,
							x.Ds,
							x.Dial,
							x.Fips,
							x.CurrencyAlphabetic,
							x.CurrencyCountry,
							x.CurrencyMinorUnit,
							x.CurrencyName,
							x.CurrencyNumeric,
							x.IsIndependent,
							x.Capital,
							x.Continent,
							x.Tld,
							string.Join("\",\"", x.Languages.Split(',')),
							"false")).Dump();
		}

#endif

		public static Country ABW = new Country("ABW", () => "Aruba", () => "Aruba", "AW", "533", "ABW", "aw", "NU", "", "297", "AA", "AWG", "ARUBA", "2", "Aruban Florin", "533", "Part of NL", "Oranjestad", "", ".aw", new string[] { "nl-AW", "es", "en" }, false);
		public static Country AFG = new Country("AFG", () => "Afghanistan", () => "Afghanistan", "AF", "004", "AFG", "af", "AF", "AFG", "93", "AF", "AFN", "AFGHANISTAN", "2", "Afghani", "971", "Yes", "Kabul", "AS", ".af", new string[] { "fa-AF", "ps", "uz-AF", "tk" }, false);
		public static Country AGO = new Country("AGO", () => "Angola", () => "Angola", "AO", "024", "AGL", "ao", "AN", "", "244", "AO", "AOA", "ANGOLA", "2", "Kwanza", "973", "Yes", "Luanda", "AF", ".ao", new string[] { "pt-AO" }, false);
		public static Country AIA = new Country("AIA", () => "Anguilla", () => "Anguilla", "AI", "660", "AIA", "am", "", "", "1-264", "AV", "XCD", "ANGUILLA", "2", "East Caribbean Dollar", "951", "Territory of GB", "The Valley", "", ".ai", new string[] { "en-AI" }, false);
		public static Country ALA = new Country("ALA", () => "Åland Islands", () => "Åland Islands", "AX", "248", "", "", "", "FIN", "358", "", "EUR", "ÅLAND ISLANDS", "2", "Euro", "978", "Part of FI", "Mariehamn", "EU", ".ax", new string[] { "sv-AX" }, false);
		public static Country ALB = new Country("ALB", () => "Albania", () => "Albania", "AL", "008", "ALB", "aa", "AB", "AL", "355", "AL", "ALL", "ALBANIA", "2", "Lek", "008", "Yes", "Tirana", "EU", ".al", new string[] { "sq", "el" }, false);
		public static Country AND = new Country("AND", () => "Andorra", () => "Andorra", "AD", "020", "AND", "an", "", "AND", "376", "AN", "EUR", "ANDORRA", "2", "Euro", "978", "Yes", "Andorra la Vella", "EU", ".ad", new string[] { "ca" }, false);
		public static Country ARE = new Country("ARE", () => "United Arab Emirates", () => "United Arab Emirates", "AE", "784", "UAE", "ts", "ER", "", "971", "AE", "AED", "UNITED ARAB EMIRATES", "2", "UAE Dirham", "784", "Yes", "Abu Dhabi", "AS", ".ae", new string[] { "ar-AE", "fa", "en", "hi", "ur" }, false);
		public static Country ARG = new Country("ARG", () => "Argentina", () => "Argentina", "AR", "032", "ARG", "ag", "AG", "RA", "54", "AR", "ARS", "ARGENTINA", "2", "Argentine Peso", "032", "Yes", "Buenos Aires", "SA", ".ar", new string[] { "es-AR", "en", "it", "de", "fr", "gn" }, false);
		public static Country ARM = new Country("ARM", () => "Armenia", () => "Armenia", "AM", "051", "ARM", "ai", "AY", "AM", "374", "AM", "AMD", "ARMENIA", "2", "Armenian Dram", "051", "Yes", "Yerevan", "AS", ".am", new string[] { "hy" }, false);
		public static Country ASM = new Country("ASM", () => "American Samoa", () => "American Samoa", "AS", "016", "SMA", "as", "", "USA", "1-684", "AQ", "USD", "AMERICAN SAMOA", "2", "US Dollar", "840", "Territory of US", "Pago Pago", "OC", ".as", new string[] { "en-AS", "sm", "to" }, false);
		public static Country ATA = new Country("ATA", () => "Antarctica", () => "", "AQ", "010", "", "ay", "AA", "", "672", "AY", "", "", "", "", "", "International", "", "AN", ".aq", new string[] { "" }, false);
		public static Country ATF = new Country("ATF", () => "French Southern Territories", () => "", "TF", "260", "", "fs", "", "F", "262", "FS", "", "", "", "", "", "Territory of FR", "Port-aux-Francais", "AN", ".tf", new string[] { "fr" }, false);
		public static Country ATG = new Country("ATG", () => "Antigua & Barbuda", () => "Antigua and Barbuda", "AG", "028", "ATG", "aq", "AT", "", "1-268", "AC", "XCD", "ANTIGUA AND BARBUDA", "2", "East Caribbean Dollar", "951", "Yes", "St. John's", "", ".ag", new string[] { "en-AG" }, false);
		public static Country AUS = new Country("AUS", () => "Australia", () => "Australia", "AU", "036", "AUS", "at", "AU", "AUS", "61", "AS", "AUD", "AUSTRALIA", "2", "Australian Dollar", "036", "Yes", "Canberra", "OC", ".au", new string[] { "en-AU" }, false);
		public static Country AUT = new Country("AUT", () => "Austria", () => "Austria", "AT", "040", "AUT", "au", "OS", "A", "43", "AU", "EUR", "AUSTRIA", "2", "Euro", "978", "Yes", "Vienna", "EU", ".at", new string[] { "de-AT", "hr", "hu", "sl" }, true);
		public static Country AZE = new Country("AZE", () => "Azerbaijan", () => "Azerbaijan", "AZ", "031", "AZE", "aj", "AJ", "AZ", "994", "AJ", "AZN", "AZERBAIJAN", "2", "Azerbaijanian Manat", "944", "Yes", "Baku", "AS", ".az", new string[] { "az", "ru", "hy" }, false);
		public static Country BDI = new Country("BDI", () => "Burundi", () => "Burundi", "BI", "108", "BDI", "bd", "BI", "RU", "257", "BY", "BIF", "BURUNDI", "0", "Burundi Franc", "108", "Yes", "Bujumbura", "AF", ".bi", new string[] { "fr-BI", "rn" }, false);
		public static Country BEL = new Country("BEL", () => "Belgium", () => "Belgium", "BE", "056", "BEL", "be", "BX", "B", "32", "BE", "EUR", "BELGIUM", "2", "Euro", "978", "Yes", "Brussels", "EU", ".be", new string[] { "nl-BE", "fr-BE", "de-BE" }, true);
		public static Country BEN = new Country("BEN", () => "Benin", () => "Benin", "BJ", "204", "BEN", "dm", "BJ", "DY", "229", "BN", "XOF", "BENIN", "0", "CFA Franc BCEAO", "952", "Yes", "Porto-Novo", "AF", ".bj", new string[] { "fr-BJ" }, false);
		public static Country BES = new Country("BES", () => "Caribbean Netherlands", () => "Bonaire, Sint Eustatius and Saba", "BQ", "535", "ATN", "ca", "NU", "", "599", "NL", "USD", "BONAIRE, SINT EUSTATIUS AND SABA", "2", "US Dollar", "840", "Part of NL", "", "", ".bq", new string[] { "nl", "pap", "en" }, false);
		public static Country BFA = new Country("BFA", () => "Burkina Faso", () => "Burkina Faso", "BF", "854", "BFA", "uv", "HV", "BF", "226", "UV", "XOF", "BURKINA FASO", "0", "CFA Franc BCEAO", "952", "Yes", "Ouagadougou", "AF", ".bf", new string[] { "fr-BF" }, false);
		public static Country BGD = new Country("BGD", () => "Bangladesh", () => "Bangladesh", "BD", "050", "BGD", "bg", "BW", "BD", "880", "BG", "BDT", "BANGLADESH", "2", "Taka", "050", "Yes", "Dhaka", "AS", ".bd", new string[] { "bn-BD", "en" }, false);
		public static Country BGR = new Country("BGR", () => "Bulgaria", () => "Bulgaria", "BG", "100", "BUL", "bu", "BU", "BG", "359", "BU", "BGN", "BULGARIA", "2", "Bulgarian Lev", "975", "Yes", "Sofia", "EU", ".bg", new string[] { "bg", "tr-BG", "rom" }, true);
		public static Country BHR = new Country("BHR", () => "Bahrain", () => "Bahrain", "BH", "048", "BHR", "ba", "BN", "BRN", "973", "BA", "BHD", "BAHRAIN", "3", "Bahraini Dinar", "048", "Yes", "Manama", "AS", ".bh", new string[] { "ar-BH", "en", "fa", "ur" }, false);
		public static Country BHS = new Country("BHS", () => "Bahamas", () => "Bahamas", "BS", "044", "BAH", "bf", "BA", "BS", "1-242", "BF", "BSD", "BAHAMAS", "2", "Bahamian Dollar", "044", "Yes", "Nassau", "", ".bs", new string[] { "en-BS" }, false);
		public static Country BIH = new Country("BIH", () => "Bosnia", () => "Bosnia and Herzegovina", "BA", "070", "BIH", "bn", "BG", "BIH", "387", "BK", "BAM", "BOSNIA AND HERZEGOVINA", "2", "Convertible Mark", "977", "Yes", "Sarajevo", "EU", ".ba", new string[] { "bs", "hr-BA", "sr-BA" }, false);
		public static Country BLM = new Country("BLM", () => "St. Barthélemy", () => "Saint Barthélemy", "BL", "652", "", "sc", "", "", "590", "TB", "EUR", "SAINT BARTHÉLEMY", "2", "Euro", "978", "Part of FR", "Gustavia", "", ".gp", new string[] { "fr" }, false);
		public static Country BLR = new Country("BLR", () => "Belarus", () => "Belarus", "BY", "112", "BLR", "bw", "BY", "BY", "375", "BO", "BYR", "BELARUS", "0", "Belarussian Ruble", "974", "Yes", "Minsk", "EU", ".by", new string[] { "be", "ru" }, false);
		public static Country BLZ = new Country("BLZ", () => "Belize", () => "Belize", "BZ", "084", "BLZ", "bh", "BH", "BZ", "501", "BH", "BZD", "BELIZE", "2", "Belize Dollar", "084", "Yes", "Belmopan", "", ".bz", new string[] { "en-BZ", "es" }, false);
		public static Country BMU = new Country("BMU", () => "Bermuda", () => "Bermuda", "BM", "060", "BER", "bm", "BE", "", "1-441", "BD", "BMD", "BERMUDA", "2", "Bermudian Dollar", "060", "Territory of GB", "Hamilton", "", ".bm", new string[] { "en-BM", "pt" }, false);
		public static Country BOL = new Country("BOL", () => "Bolivia", () => "Bolivia (Plurinational State of)", "BO", "068", "BOL", "bo", "BO", "BOL", "591", "BL", "", "", "", "", "", "Yes", "Sucre", "SA", ".bo", new string[] { "es-BO", "qu", "ay" }, false);
		public static Country BRA = new Country("BRA", () => "Brazil", () => "Brazil", "BR", "076", "B", "bl", "BZ", "BR", "55", "BR", "BRL", "BRAZIL", "2", "Brazilian Real", "986", "Yes", "Brasilia", "SA", ".br", new string[] { "pt-BR", "es", "en", "fr" }, false);
		public static Country BRB = new Country("BRB", () => "Barbados", () => "Barbados", "BB", "052", "BRB", "bb", "BR", "BDS", "1-246", "BB", "BBD", "BARBADOS", "2", "Barbados Dollar", "052", "Yes", "Bridgetown", "", ".bb", new string[] { "en-BB" }, false);
		public static Country BRN = new Country("BRN", () => "Brunei", () => "Brunei Darussalam", "BN", "096", "BRU", "bx", "BD", "BRU", "673", "BX", "BND", "BRUNEI DARUSSALAM", "2", "Brunei Dollar", "096", "Yes", "Bandar Seri Begawan", "AS", ".bn", new string[] { "ms-BN", "en-BN" }, false);
		public static Country BTN = new Country("BTN", () => "Bhutan", () => "Bhutan", "BT", "064", "BTN", "bt", "", "", "975", "BT", "INR", "BHUTAN", "2", "Indian Rupee", "356", "Yes", "Thimphu", "AS", ".bt", new string[] { "dz" }, false);
		public static Country BVT = new Country("BVT", () => "Bouvet Island", () => "", "BV", "074", "", "bv", "BV", "", "47", "BV", "", "", "", "", "", "Territory of NO", "", "AN", ".bv", new string[] { "" }, false);
		public static Country BWA = new Country("BWA", () => "Botswana", () => "Botswana", "BW", "072", "BOT", "bs", "BC", "BW", "267", "BC", "BWP", "BOTSWANA", "2", "Pula", "072", "Yes", "Gaborone", "AF", ".bw", new string[] { "en-BW", "tn-BW" }, false);
		public static Country CAF = new Country("CAF", () => "Central African Republic", () => "Central African Republic", "CF", "140", "CAF", "cx", "CE", "RCA", "236", "CT", "XAF", "CENTRAL AFRICAN REPUBLIC", "0", "CFA Franc BEAC", "950", "Yes", "Bangui", "AF", ".cf", new string[] { "fr-CF", "sg", "ln", "kg" }, false);
		public static Country CAN = new Country("CAN", () => "Canada", () => "Canada", "CA", "124", "CAN", "xxc", "CN", "CDN", "1", "CA", "CAD", "CANADA", "2", "Canadian Dollar", "124", "Yes", "Ottawa", "", ".ca", new string[] { "en-CA", "fr-CA", "iu" }, false);
		public static Country CCK = new Country("CCK", () => "Cocos (Keeling) Islands", () => "", "CC", "166", "ICO", "xb", "KK", "AUS", "61", "CK", "", "", "", "", "", "Territory of AU", "West Island", "AS", ".cc", new string[] { "ms-CC", "en" }, false);
		public static Country CHE = new Country("CHE", () => "Switzerland", () => "Switzerland", "CH", "756", "SUI", "sz", "SW", "CH", "41", "SZ", "CHF", "SWITZERLAND", "2", "Swiss Franc", "756", "Yes", "Bern", "EU", ".ch", new string[] { "de-CH", "fr-CH", "it-CH", "rm" }, false);
		public static Country CHL = new Country("CHL", () => "Chile", () => "Chile", "CL", "152", "CHL", "cl", "CH", "RCH", "56", "CI", "CLP", "CHILE", "0", "Chilean Peso", "152", "Yes", "Santiago", "SA", ".cl", new string[] { "es-CL" }, false);
		public static Country CHN = new Country("CHN", () => "China", () => "China", "CN", "156", "CHN", "cc", "CI", "", "86", "CH", "CNY", "CHINA", "2", "Yuan Renminbi", "156", "Yes", "Beijing", "AS", ".cn", new string[] { "zh-CN", "yue", "wuu", "dta", "ug", "za" }, false);
		public static Country CIV = new Country("CIV", () => "Côte d’Ivoire", () => "Côte d'Ivoire", "CI", "384", "CTI", "iv", "IV", "CI", "225", "IV", "XOF", "CÔTE D'IVOIRE", "0", "CFA Franc BCEAO", "952", "Yes", "Yamoussoukro", "AF", ".ci", new string[] { "fr-CI" }, false);
		public static Country CMR = new Country("CMR", () => "Cameroon", () => "Cameroon", "CM", "120", "CME", "cm", "CM", "CAM", "237", "CM", "XAF", "CAMEROON", "0", "CFA Franc BEAC", "950", "Yes", "Yaounde", "AF", ".cm", new string[] { "en-CM", "fr-CM" }, false);
		public static Country COD = new Country("COD", () => "Congo - Kinshasa", () => "Democratic Republic of the Congo", "CD", "180", "COD", "cg", "ZR", "ZRE", "243", "CG", "", "", "", "", "", "Yes", "Kinshasa", "AF", ".cd", new string[] { "fr-CD", "ln", "kg" }, false);
		public static Country COG = new Country("COG", () => "Congo - Brazzaville", () => "Congo", "CG", "178", "COG", "cf", "CG", "RCB", "242", "CF", "XAF", "CONGO", "0", "CFA Franc BEAC", "950", "Yes", "Brazzaville", "AF", ".cg", new string[] { "fr-CG", "kg", "ln-CG" }, false);
		public static Country COK = new Country("COK", () => "Cook Islands", () => "Cook Islands", "CK", "184", "CKH", "cw", "KU", "NZ", "682", "CW", "NZD", "COOK ISLANDS", "2", "New Zealand Dollar", "554", "Associated with NZ", "Avarua", "OC", ".ck", new string[] { "en-CK", "mi" }, false);
		public static Country COL = new Country("COL", () => "Colombia", () => "Colombia", "CO", "170", "CLM", "ck", "CO", "CO", "57", "CO", "COP", "COLOMBIA", "2", "Colombian Peso", "170", "Yes", "Bogota", "SA", ".co", new string[] { "es-CO" }, false);
		public static Country COM = new Country("COM", () => "Comoros", () => "Comoros", "KM", "174", "COM", "cq", "IC", "", "269", "CN", "KMF", "COMOROS", "0", "Comoro Franc", "174", "Yes", "Moroni", "AF", ".km", new string[] { "ar", "fr-KM" }, false);
		public static Country CPV = new Country("CPV", () => "Cape Verde", () => "Cabo Verde", "CV", "132", "CPV", "cv", "CV", "", "238", "CV", "CVE", "CABO VERDE", "2", "Cabo Verde Escudo", "132", "Yes", "Praia", "AF", ".cv", new string[] { "pt-CV" }, false);
		public static Country CRI = new Country("CRI", () => "Costa Rica", () => "Costa Rica", "CR", "188", "CTR", "cr", "CS", "CR", "506", "CS", "CRC", "COSTA RICA", "2", "Costa Rican Colon", "188", "Yes", "San Jose", "", ".cr", new string[] { "es-CR", "en" }, false);
		public static Country CUB = new Country("CUB", () => "Cuba", () => "Cuba", "CU", "192", "CUB", "cu", "CU", "C", "53", "CU", "CUP", "CUBA", "2", "Cuban Peso", "192", "Yes", "Havana", "", ".cu", new string[] { "es-CU" }, false);
		public static Country CUW = new Country("CUW", () => "Curaçao", () => "Curaçao", "CW", "531", "", "co", "", "", "599", "UC", "ANG", "CURAÇAO", "2", "Netherlands Antillean Guilder", "532", "Part of NL", "Willemstad", "", ".cw", new string[] { "nl", "pap" }, false);
		public static Country CXR = new Country("CXR", () => "Christmas Island", () => "", "CX", "162", "CHR", "xa", "KI", "AUS", "61", "KT", "", "", "", "", "", "Territory of AU", "Flying Fish Cove", "AS", ".cx", new string[] { "en", "zh", "ms-CC" }, false);
		public static Country CYM = new Country("CYM", () => "Cayman Islands", () => "Cayman Islands", "KY", "136", "CYM", "cj", "GC", "", "1-345", "CJ", "KYD", "CAYMAN ISLANDS", "2", "Cayman Islands Dollar", "136", "Territory of GB", "George Town", "", ".ky", new string[] { "en-KY" }, false);
		public static Country CYP = new Country("CYP", () => "Cyprus", () => "Cyprus", "CY", "196", "CYP", "cy", "CY", "CY", "357", "CY", "EUR", "CYPRUS", "2", "Euro", "978", "Yes", "Nicosia", "EU", ".cy", new string[] { "el-CY", "tr-CY", "en" }, true);
		public static Country CZE = new Country("CZE", () => "Czech Republic", () => "Czech Republic", "CZ", "203", "CZE", "xr", "CZ", "CZ", "420", "EZ", "CZK", "CZECH REPUBLIC", "2", "Czech Koruna", "203", "Yes", "Prague", "EU", ".cz", new string[] { "cs", "sk" }, true);
		public static Country DEU = new Country("DEU", () => "Germany", () => "Germany", "DE", "276", "D", "gw", "DL", "D", "49", "GM", "EUR", "GERMANY", "2", "Euro", "978", "Yes", "Berlin", "EU", ".de", new string[] { "de" }, true);
		public static Country DJI = new Country("DJI", () => "Djibouti", () => "Djibouti", "DJ", "262", "DJI", "ft", "DJ", "", "253", "DJ", "DJF", "DJIBOUTI", "0", "Djibouti Franc", "262", "Yes", "Djibouti", "AF", ".dj", new string[] { "fr-DJ", "ar", "so-DJ", "aa" }, false);
		public static Country DMA = new Country("DMA", () => "Dominica", () => "Dominica", "DM", "212", "DMA", "dq", "DO", "WD", "1-767", "DO", "XCD", "DOMINICA", "2", "East Caribbean Dollar", "951", "Yes", "Roseau", "", ".dm", new string[] { "en-DM" }, false);
		public static Country DNK = new Country("DNK", () => "Denmark", () => "Denmark", "DK", "208", "DNK", "dk", "DN", "DK", "45", "DA", "DKK", "DENMARK", "2", "Danish Krone", "208", "Yes", "Copenhagen", "EU", ".dk", new string[] { "da-DK", "en", "fo", "de-DK" }, true);
		public static Country DOM = new Country("DOM", () => "Dominican Republic", () => "Dominican Republic", "DO", "214", "DOM", "dr", "DR", "DOM", "1-809,1-829,1-849", "DR", "DOP", "DOMINICAN REPUBLIC", "2", "Dominican Peso", "214", "Yes", "Santo Domingo", "", ".do", new string[] { "es-DO" }, false);
		public static Country DZA = new Country("DZA", () => "Algeria", () => "Algeria", "DZ", "012", "ALG", "ae", "AL", "DZ", "213", "AG", "DZD", "ALGERIA", "2", "Algerian Dinar", "012", "Yes", "Algiers", "AF", ".dz", new string[] { "ar-DZ" }, false);
		public static Country ECU = new Country("ECU", () => "Ecuador", () => "Ecuador", "EC", "218", "EQA", "ec", "EQ", "EC", "593", "EC", "USD", "ECUADOR", "2", "US Dollar", "840", "Yes", "Quito", "SA", ".ec", new string[] { "es-EC" }, false);
		public static Country EGY = new Country("EGY", () => "Egypt", () => "Egypt", "EG", "818", "EGY", "ua", "EG", "ET", "20", "EG", "EGP", "EGYPT", "2", "Egyptian Pound", "818", "Yes", "Cairo", "AF", ".eg", new string[] { "ar-EG", "en", "fr" }, false);
		public static Country ERI = new Country("ERI", () => "Eritrea", () => "Eritrea", "ER", "232", "ERI", "ea", "", "", "291", "ER", "ERN", "ERITREA", "2", "Nakfa", "232", "Yes", "Asmara", "AF", ".er", new string[] { "aa-ER", "ar", "tig", "kun", "ti-ER" }, false);
		public static Country ESH = new Country("ESH", () => "Western Sahara", () => "Western Sahara", "EH", "732", "AOE", "ss", "", "", "212", "WI", "MAD", "WESTERN SAHARA", "2", "Moroccan Dirham", "504", "In contention", "El-Aaiun", "AF", ".eh", new string[] { "ar", "mey" }, false);
		public static Country ESP = new Country("ESP", () => "Spain", () => "Spain", "ES", "724", "E", "sp", "SP", "E", "34", "SP", "EUR", "SPAIN", "2", "Euro", "978", "Yes", "Madrid", "EU", ".es", new string[] { "es-ES", "ca", "gl", "eu", "oc" }, true);
		public static Country EST = new Country("EST", () => "Estonia", () => "Estonia", "EE", "233", "EST", "er", "EO", "EST", "372", "EN", "EUR", "ESTONIA", "2", "Euro", "978", "Yes", "Tallinn", "EU", ".ee", new string[] { "et", "ru" }, true);
		public static Country ETH = new Country("ETH", () => "Ethiopia", () => "Ethiopia", "ET", "231", "ETH", "et", "ET", "ETH", "251", "ET", "ETB", "ETHIOPIA", "2", "Ethiopian Birr", "230", "Yes", "Addis Ababa", "AF", ".et", new string[] { "am", "en-ET", "om-ET", "ti-ET", "so-ET", "sid" }, false);
		public static Country FIN = new Country("FIN", () => "Finland", () => "Finland", "FI", "246", "FIN", "fi", "FI", "FIN", "358", "FI", "EUR", "FINLAND", "2", "Euro", "978", "Yes", "Helsinki", "EU", ".fi", new string[] { "fi-FI", "sv-FI", "smn" }, true);
		public static Country FJI = new Country("FJI", () => "Fiji", () => "Fiji", "FJ", "242", "FJI", "fj", "FJ", "FJI", "679", "FJ", "FJD", "FIJI", "2", "Fiji Dollar", "242", "Yes", "Suva", "OC", ".fj", new string[] { "en-FJ", "fj" }, false);
		public static Country FLK = new Country("FLK", () => "Falkland Islands", () => "Falkland Islands (Malvinas)", "FK", "238", "FLK", "fk", "FK", "", "500", "FK", "FKP", "FALKLAND ISLANDS (MALVINAS)", "2", "Falkland Islands Pound", "238", "Territory of GB", "Stanley", "SA", ".fk", new string[] { "en-FK" }, false);
		public static Country FRA = new Country("FRA", () => "France", () => "France", "FR", "250", "F", "fr", "FR", "F", "33", "FR", "EUR", "FRANCE", "2", "Euro", "978", "Yes", "Paris", "EU", ".fr", new string[] { "fr-FR", "frp", "br", "co", "ca", "eu", "oc" }, true);
		public static Country FRO = new Country("FRO", () => "Faroe Islands", () => "Faeroe Islands", "FO", "234", "FRO", "fa", "FA", "FO", "298", "FO", "", "", "", "", "", "Part of DK", "Torshavn", "EU", ".fo", new string[] { "fo", "da-FO" }, false);
		public static Country FSM = new Country("FSM", () => "Micronesia", () => "Micronesia (Federated States of)", "FM", "583", "FSM", "fm", "", "", "691", "FM", "", "", "", "", "", "Yes", "Palikir", "OC", ".fm", new string[] { "en-FM", "chk", "pon", "yap", "kos", "uli", "woe", "nkr", "kpg" }, false);
		public static Country GAB = new Country("GAB", () => "Gabon", () => "Gabon", "GA", "266", "GAB", "go", "GO", "G", "241", "GB", "XAF", "GABON", "0", "CFA Franc BEAC", "950", "Yes", "Libreville", "AF", ".ga", new string[] { "fr-GA" }, false);
		public static Country GBR = new Country("GBR", () => "UK", () => "United Kingdom of Great Britain and Northern Ireland", "GB", "826", "G", "xxk", "UK", "GB", "44", "UK", "", "", "", "", "", "Yes", "London", "EU", ".uk", new string[] { "en-GB", "cy-GB", "gd" }, true);
		public static Country GEO = new Country("GEO", () => "Georgia", () => "Georgia", "GE", "268", "GEO", "gs", "GG", "GE", "995", "GG", "GEL", "GEORGIA", "2", "Lari", "981", "Yes", "Tbilisi", "AS", ".ge", new string[] { "ka", "ru", "hy", "az" }, false);
		public static Country GGY = new Country("GGY", () => "Guernsey", () => "Guernsey", "GG", "831", "", "uik", "", "GBG", "44", "GK", "GBP", "GUERNSEY", "2", "Pound Sterling", "826", "Crown dependency of GB", "St Peter Port", "EU", ".gg", new string[] { "en", "fr" }, false);
		public static Country GHA = new Country("GHA", () => "Ghana", () => "Ghana", "GH", "288", "GHA", "gh", "GH", "GH", "233", "GH", "GHS", "GHANA", "2", "Ghana Cedi", "936", "Yes", "Accra", "AF", ".gh", new string[] { "en-GH", "ak", "ee", "tw" }, false);
		public static Country GIB = new Country("GIB", () => "Gibraltar", () => "Gibraltar", "GI", "292", "GIB", "gi", "GI", "GBZ", "350", "GI", "GIP", "GIBRALTAR", "2", "Gibraltar Pound", "292", "Territory of GB", "Gibraltar", "EU", ".gi", new string[] { "en-GI", "es", "it", "pt" }, false);
		public static Country GIN = new Country("GIN", () => "Guinea", () => "Guinea", "GN", "324", "GUI", "gv", "GN", "RG", "224", "GV", "GNF", "GUINEA", "0", "Guinea Franc", "324", "Yes", "Conakry", "AF", ".gn", new string[] { "fr-GN" }, false);
		public static Country GLP = new Country("GLP", () => "Guadeloupe", () => "Guadeloupe", "GP", "312", "GDL", "gp", "MF", "F", "590", "GP", "EUR", "GUADELOUPE", "2", "Euro", "978", "Part of FR", "Basse-Terre", "", ".gp", new string[] { "fr-GP" }, false);
		public static Country GMB = new Country("GMB", () => "Gambia", () => "Gambia", "GM", "270", "GMB", "gm", "GB", "WAG", "220", "GA", "GMD", "GAMBIA", "2", "Dalasi", "270", "Yes", "Banjul", "AF", ".gm", new string[] { "en-GM", "mnk", "wof", "wo", "ff" }, false);
		public static Country GNB = new Country("GNB", () => "Guinea-Bissau", () => "Guinea-Bissau", "GW", "624", "GNB", "pg", "GW", "", "245", "PU", "XOF", "GUINEA-BISSAU", "0", "CFA Franc BCEAO", "952", "Yes", "Bissau", "AF", ".gw", new string[] { "pt-GW", "pov" }, false);
		public static Country GNQ = new Country("GNQ", () => "Equatorial Guinea", () => "Equatorial Guinea", "GQ", "226", "GNE", "eg", "GQ", "", "240", "EK", "XAF", "EQUATORIAL GUINEA", "0", "CFA Franc BEAC", "950", "Yes", "Malabo", "AF", ".gq", new string[] { "es-GQ", "fr" }, false);
		public static Country GRC = new Country("GRC", () => "Greece", () => "Greece", "GR", "300", "GRC", "gr", "GR", "GR", "30", "GR", "EUR", "GREECE", "2", "Euro", "978", "Yes", "Athens", "EU", ".gr", new string[] { "el-GR", "en", "fr" }, true);
		public static Country GRD = new Country("GRD", () => "Grenada", () => "Grenada", "GD", "308", "GRD", "gd", "GD", "WG", "1-473", "GJ", "XCD", "GRENADA", "2", "East Caribbean Dollar", "951", "Yes", "St. George's", "", ".gd", new string[] { "en-GD" }, false);
		public static Country GRL = new Country("GRL", () => "Greenland", () => "Greenland", "GL", "304", "GRL", "gl", "GL", "DK", "299", "GL", "DKK", "GREENLAND", "2", "Danish Krone", "208", "Part of DK", "Nuuk", "", ".gl", new string[] { "kl", "da-GL", "en" }, false);
		public static Country GTM = new Country("GTM", () => "Guatemala", () => "Guatemala", "GT", "320", "GTM", "gt", "GU", "GCA", "502", "GT", "GTQ", "GUATEMALA", "2", "Quetzal", "320", "Yes", "Guatemala City", "", ".gt", new string[] { "es-GT" }, false);
		public static Country GUF = new Country("GUF", () => "French Guiana", () => "French Guiana", "GF", "254", "GUF", "fg", "FG", "F", "594", "FG", "EUR", "FRENCH GUIANA", "2", "Euro", "978", "Part of FR", "Cayenne", "SA", ".gf", new string[] { "fr-GF" }, false);
		public static Country GUM = new Country("GUM", () => "Guam", () => "Guam", "GU", "316", "GUM", "gu", "GM", "USA", "1-671", "GQ", "USD", "GUAM", "2", "US Dollar", "840", "Territory of US", "Hagatna", "OC", ".gu", new string[] { "en-GU", "ch-GU" }, false);
		public static Country GUY = new Country("GUY", () => "Guyana", () => "Guyana", "GY", "328", "GUY", "gy", "GY", "GUY", "592", "GY", "GYD", "GUYANA", "2", "Guyana Dollar", "328", "Yes", "Georgetown", "SA", ".gy", new string[] { "en-GY" }, false);
		public static Country HKG = new Country("HKG", () => "Hong Kong", () => "China,  Hong Kong Special Administrative Region", "HK", "344", "HKG", "", "HK", "HK", "852", "HK", "", "", "", "", "", "Part of CN", "Hong Kong", "AS", ".hk", new string[] { "zh-HK", "yue", "zh", "en" }, false);
		public static Country HMD = new Country("HMD", () => "Heard & McDonald Islands", () => "", "HM", "334", "", "hm", "", "AUS", "672", "HM", "", "", "", "", "", "Territory of AU", "", "AN", ".hm", new string[] { "" }, false);
		public static Country HND = new Country("HND", () => "Honduras", () => "Honduras", "HN", "340", "HND", "ho", "HO", "", "504", "HO", "HNL", "HONDURAS", "2", "Lempira", "340", "Yes", "Tegucigalpa", "", ".hn", new string[] { "es-HN" }, false);
		public static Country HRV = new Country("HRV", () => "Croatia", () => "Croatia", "HR", "191", "HRV", "ci", "RH", "HR", "385", "HR", "HRK", "CROATIA", "2", "Croatian Kuna", "191", "Yes", "Zagreb", "EU", ".hr", new string[] { "hr-HR", "sr" }, true);
		public static Country HTI = new Country("HTI", () => "Haiti", () => "Haiti", "HT", "332", "HTI", "ht", "HA", "RH", "509", "HA", "USD", "HAITI", "2", "US Dollar", "840", "Yes", "Port-au-Prince", "", ".ht", new string[] { "ht", "fr-HT" }, false);
		public static Country HUN = new Country("HUN", () => "Hungary", () => "Hungary", "HU", "348", "HNG", "hu", "HU", "H", "36", "HU", "HUF", "HUNGARY", "2", "Forint", "348", "Yes", "Budapest", "EU", ".hu", new string[] { "hu-HU" }, true);
		public static Country IDN = new Country("IDN", () => "Indonesia", () => "Indonesia", "ID", "360", "INS", "io", "ID", "RI", "62", "ID", "IDR", "INDONESIA", "2", "Rupiah", "360", "Yes", "Jakarta", "AS", ".id", new string[] { "id", "en", "nl", "jv" }, false);
		public static Country IMN = new Country("IMN", () => "Isle of Man", () => "Isle of Man", "IM", "833", "", "uik", "", "GBM", "44", "IM", "GBP", "ISLE OF MAN", "2", "Pound Sterling", "826", "Crown dependency of GB", "Douglas", "EU", ".im", new string[] { "en", "gv" }, false);
		public static Country IND = new Country("IND", () => "India", () => "India", "IN", "356", "IND", "ii", "IN", "IND", "91", "IN", "INR", "INDIA", "2", "Indian Rupee", "356", "Yes", "New Delhi", "AS", ".in", new string[] { "en-IN", "hi", "bn", "te", "mr", "ta", "ur", "gu", "kn", "ml", "or", "pa", "as", "bh", "sat", "ks", "ne", "sd", "kok", "doi", "mni", "sit", "sa", "fr", "lus", "inc" }, false);
		public static Country IOT = new Country("IOT", () => "British Indian Ocean Territory", () => "", "IO", "086", "BIO", "bi", "", "", "246", "IO", "", "", "", "", "", "Territory of GB", "Diego Garcia", "AS", ".io", new string[] { "en-IO" }, false);
		public static Country IRL = new Country("IRL", () => "Ireland", () => "Ireland", "IE", "372", "IRL", "ie", "IE", "IRL", "353", "EI", "EUR", "IRELAND", "2", "Euro", "978", "Yes", "Dublin", "EU", ".ie", new string[] { "en-IE", "ga-IE" }, true);
		public static Country IRN = new Country("IRN", () => "Iran", () => "Iran (Islamic Republic of)", "IR", "364", "IRN", "ir", "IR", "IR", "98", "IR", "", "", "", "", "", "Yes", "Tehran", "AS", ".ir", new string[] { "fa-IR", "ku" }, false);
		public static Country IRQ = new Country("IRQ", () => "Iraq", () => "Iraq", "IQ", "368", "IRQ", "iq", "IQ", "IRQ", "964", "IZ", "IQD", "IRAQ", "3", "Iraqi Dinar", "368", "Yes", "Baghdad", "AS", ".iq", new string[] { "ar-IQ", "ku", "hy" }, false);
		public static Country ISL = new Country("ISL", () => "Iceland", () => "Iceland", "IS", "352", "ISL", "ic", "IL", "IS", "354", "IC", "ISK", "ICELAND", "0", "Iceland Krona", "352", "Yes", "Reykjavik", "EU", ".is", new string[] { "is", "en", "de", "da", "sv", "no" }, false);
		public static Country ISR = new Country("ISR", () => "Israel", () => "Israel", "IL", "376", "ISR", "is", "IS", "IL", "972", "IS", "ILS", "ISRAEL", "2", "New Israeli Sheqel", "376", "Yes", "Jerusalem", "AS", ".il", new string[] { "he", "ar-IL", "en-IL", "" }, false);
		public static Country ITA = new Country("ITA", () => "Italy", () => "Italy", "IT", "380", "I", "it", "IY", "I", "39", "IT", "EUR", "ITALY", "2", "Euro", "978", "Yes", "Rome", "EU", ".it", new string[] { "it-IT", "de-IT", "fr-IT", "sc", "ca", "co", "sl" }, true);
		public static Country JAM = new Country("JAM", () => "Jamaica", () => "Jamaica", "JM", "388", "JMC", "jm", "JM", "JA", "1-876", "JM", "JMD", "JAMAICA", "2", "Jamaican Dollar", "388", "Yes", "Kingston", "", ".jm", new string[] { "en-JM" }, false);
		public static Country JEY = new Country("JEY", () => "Jersey", () => "Jersey", "JE", "832", "", "uik", "", "GBJ", "44", "JE", "GBP", "JERSEY", "2", "Pound Sterling", "826", "Crown dependency of GB", "Saint Helier", "EU", ".je", new string[] { "en", "pt" }, false);
		public static Country JOR = new Country("JOR", () => "Jordan", () => "Jordan", "JO", "400", "JOR", "jo", "JD", "HKJ", "962", "JO", "JOD", "JORDAN", "3", "Jordanian Dinar", "400", "Yes", "Amman", "AS", ".jo", new string[] { "ar-JO", "en" }, false);
		public static Country JPN = new Country("JPN", () => "Japan", () => "Japan", "JP", "392", "J", "ja", "JP", "J", "81", "JA", "JPY", "JAPAN", "0", "Yen", "392", "Yes", "Tokyo", "AS", ".jp", new string[] { "ja" }, false);
		public static Country KAZ = new Country("KAZ", () => "Kazakhstan", () => "Kazakhstan", "KZ", "398", "KAZ", "kz", "KZ", "KZ", "7", "KZ", "KZT", "KAZAKHSTAN", "2", "Tenge", "398", "Yes", "Astana", "AS", ".kz", new string[] { "kk", "ru" }, false);
		public static Country KEN = new Country("KEN", () => "Kenya", () => "Kenya", "KE", "404", "KEN", "ke", "KN", "EAK", "254", "KE", "KES", "KENYA", "2", "Kenyan Shilling", "404", "Yes", "Nairobi", "AF", ".ke", new string[] { "en-KE", "sw-KE" }, false);
		public static Country KGZ = new Country("KGZ", () => "Kyrgyzstan", () => "Kyrgyzstan", "KG", "417", "KGZ", "kg", "KG", "KS", "996", "KG", "KGS", "KYRGYZSTAN", "2", "Som", "417", "Yes", "Bishkek", "AS", ".kg", new string[] { "ky", "uz", "ru" }, false);
		public static Country KHM = new Country("KHM", () => "Cambodia", () => "Cambodia", "KH", "116", "CBG", "cb", "KP", "K", "855", "CB", "KHR", "CAMBODIA", "2", "Riel", "116", "Yes", "Phnom Penh", "AS", ".kh", new string[] { "km", "fr", "en" }, false);
		public static Country KIR = new Country("KIR", () => "Kiribati", () => "Kiribati", "KI", "296", "KIR", "gb", "KB", "", "686", "KR", "AUD", "KIRIBATI", "2", "Australian Dollar", "036", "Yes", "Tarawa", "OC", ".ki", new string[] { "en-KI", "gil" }, false);
		public static Country KNA = new Country("KNA", () => "St. Kitts & Nevis", () => "Saint Kitts and Nevis", "KN", "659", "KNA", "xd", "AT", "", "1-869", "SC", "XCD", "SAINT KITTS AND NEVIS", "2", "East Caribbean Dollar", "951", "Yes", "Basseterre", "", ".kn", new string[] { "en-KN" }, false);
		public static Country KOR = new Country("KOR", () => "South Korea", () => "Republic of Korea", "KR", "410", "KOR", "ko", "KO", "ROK", "82", "KS", "", "", "", "", "", "Yes", "Seoul", "AS", ".kr", new string[] { "ko-KR", "en" }, false);
		public static Country KWT = new Country("KWT", () => "Kuwait", () => "Kuwait", "KW", "414", "KWT", "ku", "KW", "KWT", "965", "KU", "KWD", "KUWAIT", "3", "Kuwaiti Dinar", "414", "Yes", "Kuwait City", "AS", ".kw", new string[] { "ar-KW", "en" }, false);
		public static Country LAO = new Country("LAO", () => "Laos", () => "Lao People's Democratic Republic", "LA", "418", "LAO", "ls", "LA", "LAO", "856", "LA", "LAK", "LAO PEOPLE’S DEMOCRATIC REPUBLIC", "2", "Kip", "418", "Yes", "Vientiane", "AS", ".la", new string[] { "lo", "fr", "en" }, false);
		public static Country LBN = new Country("LBN", () => "Lebanon", () => "Lebanon", "LB", "422", "LBN", "le", "LB", "RL", "961", "LE", "LBP", "LEBANON", "2", "Lebanese Pound", "422", "Yes", "Beirut", "AS", ".lb", new string[] { "ar-LB", "fr-LB", "en", "hy" }, false);
		public static Country LBR = new Country("LBR", () => "Liberia", () => "Liberia", "LR", "430", "LBR", "lb", "LI", "LB", "231", "LI", "LRD", "LIBERIA", "2", "Liberian Dollar", "430", "Yes", "Monrovia", "AF", ".lr", new string[] { "en-LR" }, false);
		public static Country LBY = new Country("LBY", () => "Libya", () => "Libya", "LY", "434", "LBY", "ly", "LY", "LAR", "218", "LY", "LYD", "LIBYA", "3", "Libyan Dinar", "434", "Yes", "Tripoli", "AF", ".ly", new string[] { "ar-LY", "it", "en" }, false);
		public static Country LCA = new Country("LCA", () => "St. Lucia", () => "Saint Lucia", "LC", "662", "LCA", "xk", "LC", "WL", "1-758", "ST", "XCD", "SAINT LUCIA", "2", "East Caribbean Dollar", "951", "Yes", "Castries", "", ".lc", new string[] { "en-LC" }, false);
		public static Country LIE = new Country("LIE", () => "Liechtenstein", () => "Liechtenstein", "LI", "438", "LIE", "lh", "", "FL", "423", "LS", "CHF", "LIECHTENSTEIN", "2", "Swiss Franc", "756", "Yes", "Vaduz", "EU", ".li", new string[] { "de-LI" }, false);
		public static Country LKA = new Country("LKA", () => "Sri Lanka", () => "Sri Lanka", "LK", "144", "CLN", "ce", "SB", "CL", "94", "CE", "LKR", "SRI LANKA", "2", "Sri Lanka Rupee", "144", "Yes", "Colombo", "AS", ".lk", new string[] { "si", "ta", "en" }, false);
		public static Country LSO = new Country("LSO", () => "Lesotho", () => "Lesotho", "LS", "426", "LSO", "lo", "LS", "LS", "266", "LT", "ZAR", "LESOTHO", "2", "Rand", "710", "Yes", "Maseru", "AF", ".ls", new string[] { "en-LS", "st", "zu", "xh" }, false);
		public static Country LTU = new Country("LTU", () => "Lithuania", () => "Lithuania", "LT", "440", "LTU", "li", "LT", "LT", "370", "LH", "EUR", "LITHUANIA", "2", "Euro", "978", "Yes", "Vilnius", "EU", ".lt", new string[] { "lt", "ru", "pl" }, true);
		public static Country LUX = new Country("LUX", () => "Luxembourg", () => "Luxembourg", "LU", "442", "LUX", "lu", "BX", "L", "352", "LU", "EUR", "LUXEMBOURG", "2", "Euro", "978", "Yes", "Luxembourg", "EU", ".lu", new string[] { "lb", "de-LU", "fr-LU" }, true);
		public static Country LVA = new Country("LVA", () => "Latvia", () => "Latvia", "LV", "428", "LVA", "lv", "LV", "LV", "371", "LG", "EUR", "LATVIA", "2", "Euro", "978", "Yes", "Riga", "EU", ".lv", new string[] { "lv", "ru", "lt" }, true);
		public static Country MAC = new Country("MAC", () => "Macau", () => "China, Macao Special Administrative Region", "MO", "446", "MAC", "", "MU", "", "853", "MC", "", "", "", "", "", "Part of CN", "Macao", "AS", ".mo", new string[] { "zh", "zh-MO", "pt" }, false);
		public static Country MAF = new Country("MAF", () => "St. Martin", () => "Saint Martin (French part)", "MF", "663", "", "st", "", "", "590", "RN", "EUR", "SAINT MARTIN (FRENCH PART)", "2", "Euro", "978", "Part of FR", "Marigot", "", ".gp", new string[] { "fr" }, false);
		public static Country MAR = new Country("MAR", () => "Morocco", () => "Morocco", "MA", "504", "MRC", "mr", "MC", "MA", "212", "MO", "MAD", "MOROCCO", "2", "Moroccan Dirham", "504", "Yes", "Rabat", "AF", ".ma", new string[] { "ar-MA", "fr" }, false);
		public static Country MCO = new Country("MCO", () => "Monaco", () => "Monaco", "MC", "492", "MCO", "mc", "", "MC", "377", "MN", "EUR", "MONACO", "2", "Euro", "978", "Yes", "Monaco", "EU", ".mc", new string[] { "fr-MC", "en", "it" }, false);
		public static Country MDA = new Country("MDA", () => "Moldova", () => "Republic of Moldova", "MD", "498", "MDA", "mv", "RM", "MD", "373", "MD", "", "", "", "", "", "Yes", "Chisinau", "EU", ".md", new string[] { "ro", "ru", "gag", "tr" }, false);
		public static Country MDG = new Country("MDG", () => "Madagascar", () => "Madagascar", "MG", "450", "MDG", "mg", "MG", "RM", "261", "MA", "MGA", "MADAGASCAR", "2", "Malagasy Ariary", "969", "Yes", "Antananarivo", "AF", ".mg", new string[] { "fr-MG", "mg" }, false);
		public static Country MDV = new Country("MDV", () => "Maldives", () => "Maldives", "MV", "462", "MLD", "xc", "MV", "", "960", "MV", "MVR", "MALDIVES", "2", "Rufiyaa", "462", "Yes", "Male", "AS", ".mv", new string[] { "dv", "en" }, false);
		public static Country MEX = new Country("MEX", () => "Mexico", () => "Mexico", "MX", "484", "MEX", "mx", "MX", "MEX", "52", "MX", "MXN", "MEXICO", "2", "Mexican Peso", "484", "Yes", "Mexico City", "", ".mx", new string[] { "es-MX" }, false);
		public static Country MHL = new Country("MHL", () => "Marshall Islands", () => "Marshall Islands", "MH", "584", "MHL", "xe", "MH", "", "692", "RM", "USD", "MARSHALL ISLANDS", "2", "US Dollar", "840", "Yes", "Majuro", "OC", ".mh", new string[] { "mh", "en-MH" }, false);
		public static Country MKD = new Country("MKD", () => "Macedonia", () => "The former Yugoslav Republic of Macedonia", "MK", "807", "MKD", "xn", "MJ", "MK", "389", "MK", "", "", "", "", "", "Yes", "Skopje", "EU", ".mk", new string[] { "mk", "sq", "tr", "rmm", "sr" }, false);
		public static Country MLI = new Country("MLI", () => "Mali", () => "Mali", "ML", "466", "MLI", "ml", "MI", "RMM", "223", "ML", "XOF", "MALI", "0", "CFA Franc BCEAO", "952", "Yes", "Bamako", "AF", ".ml", new string[] { "fr-ML", "bm" }, false);
		public static Country MLT = new Country("MLT", () => "Malta", () => "Malta", "MT", "470", "MLT", "mm", "ML", "M", "356", "MT", "EUR", "MALTA", "2", "Euro", "978", "Yes", "Valletta", "EU", ".mt", new string[] { "mt", "en-MT" }, true);
		public static Country MMR = new Country("MMR", () => "Myanmar", () => "Myanmar", "MM", "104", "MYA", "br", "BM", "BUR", "95", "BM", "MMK", "MYANMAR", "2", "Kyat", "104", "Yes", "Nay Pyi Taw", "AS", ".mm", new string[] { "my" }, false);
		public static Country MNE = new Country("MNE", () => "Montenegro", () => "Montenegro", "ME", "499", "MNE", "mo", "", "MNE", "382", "MJ", "EUR", "MONTENEGRO", "2", "Euro", "978", "Yes", "Podgorica", "EU", ".me", new string[] { "sr", "hu", "bs", "sq", "hr", "rom" }, false);
		public static Country MNG = new Country("MNG", () => "Mongolia", () => "Mongolia", "MN", "496", "MNG", "mp", "MO", "MGL", "976", "MG", "MNT", "MONGOLIA", "2", "Tugrik", "496", "Yes", "Ulan Bator", "AS", ".mn", new string[] { "mn", "ru" }, false);
		public static Country MNP = new Country("MNP", () => "Northern Mariana Islands", () => "Northern Mariana Islands", "MP", "580", "MRA", "nw", "MY", "USA", "1-670", "CQ", "USD", "NORTHERN MARIANA ISLANDS", "2", "US Dollar", "840", "Commonwealth of US", "Saipan", "OC", ".mp", new string[] { "fil", "tl", "zh", "ch-MP", "en-MP" }, false);
		public static Country MOZ = new Country("MOZ", () => "Mozambique", () => "Mozambique", "MZ", "508", "MOZ", "mz", "MZ", "MOC", "258", "MZ", "MZN", "MOZAMBIQUE", "2", "Mozambique Metical", "943", "Yes", "Maputo", "AF", ".mz", new string[] { "pt-MZ", "vmw" }, false);
		public static Country MRT = new Country("MRT", () => "Mauritania", () => "Mauritania", "MR", "478", "MTN", "mu", "MT", "RIM", "222", "MR", "MRO", "MAURITANIA", "2", "Ouguiya", "478", "Yes", "Nouakchott", "AF", ".mr", new string[] { "ar-MR", "fuc", "snk", "fr", "mey", "wo" }, false);
		public static Country MSR = new Country("MSR", () => "Montserrat", () => "Montserrat", "MS", "500", "MSR", "mj", "", "", "1-664", "MH", "XCD", "MONTSERRAT", "2", "East Caribbean Dollar", "951", "Territory of GB", "Plymouth", "", ".ms", new string[] { "en-MS" }, false);
		public static Country MTQ = new Country("MTQ", () => "Martinique", () => "Martinique", "MQ", "474", "MRT", "mq", "MR", "F", "596", "MB", "EUR", "MARTINIQUE", "2", "Euro", "978", "Part of FR", "Fort-de-France", "", ".mq", new string[] { "fr-MQ" }, false);
		public static Country MUS = new Country("MUS", () => "Mauritius", () => "Mauritius", "MU", "480", "MAU", "mf", "MA", "MS", "230", "MP", "MUR", "MAURITIUS", "2", "Mauritius Rupee", "480", "Yes", "Port Louis", "AF", ".mu", new string[] { "en-MU", "bho", "fr" }, false);
		public static Country MWI = new Country("MWI", () => "Malawi", () => "Malawi", "MW", "454", "MWI", "mw", "MW", "MW", "265", "MI", "MWK", "MALAWI", "2", "Kwacha", "454", "Yes", "Lilongwe", "AF", ".mw", new string[] { "ny", "yao", "tum", "swk" }, false);
		public static Country MYS = new Country("MYS", () => "Malaysia", () => "Malaysia", "MY", "458", "MLA", "my", "MS", "MAL", "60", "MY", "MYR", "MALAYSIA", "2", "Malaysian Ringgit", "458", "Yes", "Kuala Lumpur", "AS", ".my", new string[] { "ms-MY", "en", "zh", "ta", "te", "ml", "pa", "th" }, false);
		public static Country MYT = new Country("MYT", () => "Mayotte", () => "Mayotte", "YT", "175", "MYT", "ot", "", "", "262", "MF", "EUR", "MAYOTTE", "2", "Euro", "978", "Part of FR", "Mamoudzou", "AF", ".yt", new string[] { "fr-YT" }, false);
		public static Country NAM = new Country("NAM", () => "Namibia", () => "Namibia", "", "516", "NMB", "sx", "NM", "NAM", "264", "WA", "ZAR", "NAMIBIA", "2", "Rand", "710", "Yes", "Windhoek", "AF", ".na", new string[] { "en-NA", "af", "de", "hz", "naq" }, false);
		public static Country NCL = new Country("NCL", () => "New Caledonia", () => "New Caledonia", "NC", "540", "NCL", "nl", "NC", "F", "687", "NC", "XPF", "NEW CALEDONIA", "0", "CFP Franc", "953", "Territory of FR", "Noumea", "OC", ".nc", new string[] { "fr-NC" }, false);
		public static Country NER = new Country("NER", () => "Niger", () => "Niger", "NE", "562", "NGR", "ng", "NR", "RN", "227", "NG", "XOF", "NIGER", "0", "CFA Franc BCEAO", "952", "Yes", "Niamey", "AF", ".ne", new string[] { "fr-NE", "ha", "kr", "dje" }, false);
		public static Country NFK = new Country("NFK", () => "Norfolk Island", () => "Norfolk Island", "NF", "574", "NFK", "nx", "NF", "AUS", "672", "NF", "AUD", "NORFOLK ISLAND", "2", "Australian Dollar", "036", "Territory of AU", "Kingston", "OC", ".nf", new string[] { "en-NF" }, false);
		public static Country NGA = new Country("NGA", () => "Nigeria", () => "Nigeria", "NG", "566", "NIG", "nr", "NI", "NGR", "234", "NI", "NGN", "NIGERIA", "2", "Naira", "566", "Yes", "Abuja", "AF", ".ng", new string[] { "en-NG", "ha", "yo", "ig", "ff" }, false);
		public static Country NIC = new Country("NIC", () => "Nicaragua", () => "Nicaragua", "NI", "558", "NCG", "nq", "NK", "NIC", "505", "NU", "NIO", "NICARAGUA", "2", "Cordoba Oro", "558", "Yes", "Managua", "", ".ni", new string[] { "es-NI", "en" }, false);
		public static Country NIU = new Country("NIU", () => "Niue", () => "Niue", "NU", "570", "NIU", "xh", "", "NZ", "683", "NE", "NZD", "NIUE", "2", "New Zealand Dollar", "554", "Associated with NZ", "Alofi", "OC", ".nu", new string[] { "niu", "en-NU" }, false);
		public static Country NLD = new Country("NLD", () => "Netherlands", () => "Netherlands", "NL", "528", "HOL", "ne", "NL", "NL", "31", "NL", "EUR", "NETHERLANDS", "2", "Euro", "978", "Yes", "Amsterdam", "EU", ".nl", new string[] { "nl-NL", "fy-NL" }, true);
		public static Country NOR = new Country("NOR", () => "Norway", () => "Norway", "NO", "578", "NOR", "no", "NO", "N", "47", "NO", "NOK", "NORWAY", "2", "Norwegian Krone", "578", "Yes", "Oslo", "EU", ".no", new string[] { "no", "nb", "nn", "se", "fi" }, false);
		public static Country NPL = new Country("NPL", () => "Nepal", () => "Nepal", "NP", "524", "NPL", "np", "NP", "NEP", "977", "NP", "NPR", "NEPAL", "2", "Nepalese Rupee", "524", "Yes", "Kathmandu", "AS", ".np", new string[] { "ne", "en" }, false);
		public static Country NRU = new Country("NRU", () => "Nauru", () => "Nauru", "NR", "520", "NRU", "nu", "NW", "NAU", "674", "NR", "AUD", "NAURU", "2", "Australian Dollar", "036", "Yes", "Yaren", "OC", ".nr", new string[] { "na", "en-NR" }, false);
		public static Country NZL = new Country("NZL", () => "New Zealand", () => "New Zealand", "NZ", "554", "NZL", "nz", "NZ", "NZ", "64", "NZ", "NZD", "NEW ZEALAND", "2", "New Zealand Dollar", "554", "Yes", "Wellington", "OC", ".nz", new string[] { "en-NZ", "mi" }, false);
		public static Country OMN = new Country("OMN", () => "Oman", () => "Oman", "OM", "512", "OMA", "mk", "OM", "", "968", "MU", "OMR", "OMAN", "3", "Rial Omani", "512", "Yes", "Muscat", "AS", ".om", new string[] { "ar-OM", "en", "bal", "ur" }, false);
		public static Country PAK = new Country("PAK", () => "Pakistan", () => "Pakistan", "PK", "586", "PAK", "pk", "PK", "PK", "92", "PK", "PKR", "PAKISTAN", "2", "Pakistan Rupee", "586", "Yes", "Islamabad", "AS", ".pk", new string[] { "ur-PK", "en-PK", "pa", "sd", "ps", "brh" }, false);
		public static Country PAN = new Country("PAN", () => "Panama", () => "Panama", "PA", "591", "PNR", "pn", "PM", "PA", "507", "PM", "USD", "PANAMA", "2", "US Dollar", "840", "Yes", "Panama City", "", ".pa", new string[] { "es-PA", "en" }, false);
		public static Country PCN = new Country("PCN", () => "Pitcairn Islands", () => "Pitcairn", "PN", "612", "PTC", "pc", "PT", "", "870", "PC", "NZD", "PITCAIRN", "2", "New Zealand Dollar", "554", "Territory of GB", "Adamstown", "OC", ".pn", new string[] { "en-PN" }, false);
		public static Country PER = new Country("PER", () => "Peru", () => "Peru", "PE", "604", "PRU", "pe", "PR", "PE", "51", "PE", "PEN", "PERU", "2", "Nuevo Sol", "604", "Yes", "Lima", "SA", ".pe", new string[] { "es-PE", "qu", "ay" }, false);
		public static Country PHL = new Country("PHL", () => "Philippines", () => "Philippines", "PH", "608", "PHL", "ph", "PH", "RP", "63", "RP", "PHP", "PHILIPPINES", "2", "Philippine Peso", "608", "Yes", "Manila", "AS", ".ph", new string[] { "tl", "en-PH", "fil" }, false);
		public static Country PLW = new Country("PLW", () => "Palau", () => "Palau", "PW", "585", "PLW", "pw", "", "", "680", "PS", "USD", "PALAU", "2", "US Dollar", "840", "Yes", "Melekeok", "OC", ".pw", new string[] { "pau", "sov", "en-PW", "tox", "ja", "fil", "zh" }, false);
		public static Country PNG = new Country("PNG", () => "Papua New Guinea", () => "Papua New Guinea", "PG", "598", "PNG", "pp", "NG", "PNG", "675", "PP", "PGK", "PAPUA NEW GUINEA", "2", "Kina", "598", "Yes", "Port Moresby", "OC", ".pg", new string[] { "en-PG", "ho", "meu", "tpi" }, false);
		public static Country POL = new Country("POL", () => "Poland", () => "Poland", "PL", "616", "POL", "pl", "PL", "PL", "48", "PL", "PLN", "POLAND", "2", "Zloty", "985", "Yes", "Warsaw", "EU", ".pl", new string[] { "pl" }, true);
		public static Country PRI = new Country("PRI", () => "Puerto Rico", () => "Puerto Rico", "PR", "630", "PTR", "pr", "PU", "USA", "1", "RQ", "USD", "PUERTO RICO", "2", "US Dollar", "840", "Commonwealth of US", "San Juan", "", ".pr", new string[] { "en-PR", "es-PR" }, false);
		public static Country PRK = new Country("PRK", () => "North Korea", () => "Democratic People's Republic of Korea", "KP", "408", "KRE", "kn", "KR", "", "850", "KN", "", "", "", "", "", "Yes", "Pyongyang", "AS", ".kp", new string[] { "ko-KP" }, false);
		public static Country PRT = new Country("PRT", () => "Portugal", () => "Portugal", "PT", "620", "POR", "po", "PO", "P", "351", "PO", "EUR", "PORTUGAL", "2", "Euro", "978", "Yes", "Lisbon", "EU", ".pt", new string[] { "pt-PT", "mwl" }, true);
		public static Country PRY = new Country("PRY", () => "Paraguay", () => "Paraguay", "PY", "600", "PRG", "py", "PY", "PY", "595", "PA", "PYG", "PARAGUAY", "0", "Guarani", "600", "Yes", "Asuncion", "SA", ".py", new string[] { "es-PY", "gn" }, false);
		public static Country PSE = new Country("PSE", () => "Palestine", () => "State of Palestine", "PS", "275", "", "gz,wj", "", "", "970", "GZ,WE", "", "", "", "", "", "In contention", "East Jerusalem", "AS", ".ps", new string[] { "ar-PS" }, false);
		public static Country PYF = new Country("PYF", () => "French Polynesia", () => "French Polynesia", "PF", "258", "OCE", "fp", "PF", "F", "689", "FP", "XPF", "FRENCH POLYNESIA", "0", "CFP Franc", "953", "Territory of FR", "Papeete", "OC", ".pf", new string[] { "fr-PF", "ty" }, false);
		public static Country QAT = new Country("QAT", () => "Qatar", () => "Qatar", "QA", "634", "QAT", "qa", "QT", "Q", "974", "QA", "QAR", "QATAR", "2", "Qatari Rial", "634", "Yes", "Doha", "AS", ".qa", new string[] { "ar-QA", "es" }, false);
		public static Country REU = new Country("REU", () => "Réunion", () => "Réunion", "RE", "638", "REU", "re", "RE", "F", "262", "RE", "EUR", "RÉUNION", "2", "Euro", "978", "Part of FR", "Saint-Denis", "AF", ".re", new string[] { "fr-RE" }, false);
		public static Country ROU = new Country("ROU", () => "Romania", () => "Romania", "RO", "642", "ROU", "rm", "RO", "RO", "40", "RO", "RON", "ROMANIA", "2", "New Romanian Leu", "946", "Yes", "Bucharest", "EU", ".ro", new string[] { "ro", "hu", "rom" }, true);
		public static Country RUS = new Country("RUS", () => "Russia", () => "Russian Federation", "RU", "643", "RUS", "ru", "RS", "RUS", "7", "RS", "RUB", "RUSSIAN FEDERATION", "2", "Russian Ruble", "643", "Yes", "Moscow", "EU", ".ru", new string[] { "ru", "tt", "xal", "cau", "ady", "kv", "ce", "tyv", "cv", "udm", "tut", "mns", "bua", "myv", "mdf", "chm", "ba", "inh", "tut", "kbd", "krc", "ava", "sah", "nog" }, false);
		public static Country RWA = new Country("RWA", () => "Rwanda", () => "Rwanda", "RW", "646", "RRW", "rw", "RW", "RWA", "250", "RW", "RWF", "RWANDA", "0", "Rwanda Franc", "646", "Yes", "Kigali", "AF", ".rw", new string[] { "rw", "en-RW", "fr-RW", "sw" }, false);
		public static Country SAU = new Country("SAU", () => "Saudi Arabia", () => "Saudi Arabia", "SA", "682", "ARS", "su", "SD", "SA", "966", "SA", "SAR", "SAUDI ARABIA", "2", "Saudi Riyal", "682", "Yes", "Riyadh", "AS", ".sa", new string[] { "ar-SA" }, false);
		public static Country SDN = new Country("SDN", () => "Sudan", () => "Sudan", "SD", "729", "SDN", "sj", "SU", "SUD", "249", "SU", "SDG", "SUDAN", "2", "Sudanese Pound", "938", "Yes", "Khartoum", "AF", ".sd", new string[] { "ar-SD", "en", "fia" }, false);
		public static Country SEN = new Country("SEN", () => "Senegal", () => "Senegal", "SN", "686", "SEN", "sg", "SG", "SN", "221", "SG", "XOF", "SENEGAL", "0", "CFA Franc BCEAO", "952", "Yes", "Dakar", "AF", ".sn", new string[] { "fr-SN", "wo", "fuc", "mnk" }, false);
		public static Country SGP = new Country("SGP", () => "Singapore", () => "Singapore", "SG", "702", "SNG", "si", "SR", "SGP", "65", "SN", "SGD", "SINGAPORE", "2", "Singapore Dollar", "702", "Yes", "Singapore", "AS", ".sg", new string[] { "cmn", "en-SG", "ms-SG", "ta-SG", "zh-SG" }, false);
		public static Country SGS = new Country("SGS", () => "South Georgia & South Sandwich Islands", () => "", "GS", "239", "", "xs", "", "", "500", "SX", "", "", "", "", "", "Territory of GB", "Grytviken", "AN", ".gs", new string[] { "en" }, false);
		public static Country SHN = new Country("SHN", () => "St. Helena", () => "Saint Helena", "SH", "654", "SHN", "xj", "HE", "", "290 n", "SH", "", "", "", "", "", "Territory of GB", "Jamestown", "AF", ".sh", new string[] { "en-SH" }, false);
		public static Country SJM = new Country("SJM", () => "Svalbard & Jan Mayen", () => "Svalbard and Jan Mayen Islands", "SJ", "744", "NOR", "", "SZ", "", "47", "SV,JN", "", "", "", "", "", "Territory of NO", "Longyearbyen", "EU", ".sj", new string[] { "no", "ru" }, false);
		public static Country SLB = new Country("SLB", () => "Solomon Islands", () => "Solomon Islands", "SB", "090", "SLM", "bp", "SO", "", "677", "BP", "SBD", "SOLOMON ISLANDS", "2", "Solomon Islands Dollar", "090", "Yes", "Honiara", "OC", ".sb", new string[] { "en-SB", "tpi" }, false);
		public static Country SLE = new Country("SLE", () => "Sierra Leone", () => "Sierra Leone", "SL", "694", "SRL", "sl", "SL", "WAL", "232", "SL", "SLL", "SIERRA LEONE", "2", "Leone", "694", "Yes", "Freetown", "AF", ".sl", new string[] { "en-SL", "men", "tem" }, false);
		public static Country SLV = new Country("SLV", () => "El Salvador", () => "El Salvador", "SV", "222", "SLV", "es", "ES", "ES", "503", "ES", "USD", "EL SALVADOR", "2", "US Dollar", "840", "Yes", "San Salvador", "", ".sv", new string[] { "es-SV" }, false);
		public static Country SMR = new Country("SMR", () => "San Marino", () => "San Marino", "SM", "674", "SMR", "sm", "", "RSM", "378", "SM", "EUR", "SAN MARINO", "2", "Euro", "978", "Yes", "San Marino", "EU", ".sm", new string[] { "it-SM" }, false);
		public static Country SOM = new Country("SOM", () => "Somalia", () => "Somalia", "SO", "706", "SOM", "so", "SI", "SO", "252", "SO", "SOS", "SOMALIA", "2", "Somali Shilling", "706", "Yes", "Mogadishu", "AF", ".so", new string[] { "so-SO", "ar-SO", "it", "en-SO" }, false);
		public static Country SPM = new Country("SPM", () => "St. Pierre & Miquelon", () => "Saint Pierre and Miquelon", "PM", "666", "SPM", "xl", "FP", "F", "508", "SB", "EUR", "SAINT PIERRE AND MIQUELON", "2", "Euro", "978", "Part of FR", "Saint-Pierre", "", ".pm", new string[] { "fr-PM" }, false);
		public static Country SRB = new Country("SRB", () => "Serbia", () => "Serbia", "RS", "688", "SRB", "rb", "YG", "SRB", "381 p", "RI,KV", "RSD", "SERBIA", "2", "Serbian Dinar", "941", "Yes", "Belgrade", "EU", ".rs", new string[] { "sr", "hu", "bs", "rom" }, false);
		public static Country SSD = new Country("SSD", () => "South Sudan", () => "South Sudan", "SS", "728", "SSD", "sd", "", "", "211", "OD", "SSP", "SOUTH SUDAN", "2", "South Sudanese Pound", "728", "Yes", "Juba", "AF", "", new string[] { "en" }, false);
		public static Country STP = new Country("STP", () => "São Tomé & Príncipe", () => "Sao Tome and Principe", "ST", "678", "STP", "sf", "TP", "", "239", "TP", "STD", "SAO TOME AND PRINCIPE", "2", "Dobra", "678", "Yes", "Sao Tome", "AF", ".st", new string[] { "pt-ST" }, false);
		public static Country SUR = new Country("SUR", () => "Suriname", () => "Suriname", "SR", "740", "SUR", "sr", "SM", "SME", "597", "NS", "SRD", "SURINAME", "2", "Surinam Dollar", "968", "Yes", "Paramaribo", "SA", ".sr", new string[] { "nl-SR", "en", "srn", "hns", "jv" }, false);
		public static Country SVK = new Country("SVK", () => "Slovakia", () => "Slovakia", "SK", "703", "SVK", "xo", "SQ", "SK", "421", "LO", "EUR", "SLOVAKIA", "2", "Euro", "978", "Yes", "Bratislava", "EU", ".sk", new string[] { "sk", "hu" }, true);
		public static Country SVN = new Country("SVN", () => "Slovenia", () => "Slovenia", "SI", "705", "SVN", "xv", "LJ", "SLO", "386", "SI", "EUR", "SLOVENIA", "2", "Euro", "978", "Yes", "Ljubljana", "EU", ".si", new string[] { "sl", "sh" }, true);
		public static Country SWE = new Country("SWE", () => "Sweden", () => "Sweden", "SE", "752", "S", "sw", "SN", "S", "46", "SW", "SEK", "SWEDEN", "2", "Swedish Krona", "752", "Yes", "Stockholm", "EU", ".se", new string[] { "sv-SE", "se", "sma", "fi-SE" }, true);
		public static Country SWZ = new Country("SWZ", () => "Swaziland", () => "Swaziland", "SZ", "748", "SWZ", "sq", "SV", "SD", "268", "WZ", "SZL", "SWAZILAND", "2", "Lilangeni", "748", "Yes", "Mbabane", "AF", ".sz", new string[] { "en-SZ", "ss-SZ" }, false);
		public static Country SXM = new Country("SXM", () => "Sint Maarten", () => "Sint Maarten (Dutch part)", "SX", "534", "", "sn", "", "", "1-721", "NN", "ANG", "SINT MAARTEN (DUTCH PART)", "2", "Netherlands Antillean Guilder", "532", "Part of NL", "Philipsburg", "", ".sx", new string[] { "nl", "en" }, false);
		public static Country SYC = new Country("SYC", () => "Seychelles", () => "Seychelles", "SC", "690", "SEY", "se", "SC", "SY", "248", "SE", "SCR", "SEYCHELLES", "2", "Seychelles Rupee", "690", "Yes", "Victoria", "AF", ".sc", new string[] { "en-SC", "fr-SC" }, false);
		public static Country SYR = new Country("SYR", () => "Syria", () => "Syrian Arab Republic", "SY", "760", "SYR", "sy", "SY", "SYR", "963", "SY", "SYP", "SYRIAN ARAB REPUBLIC", "2", "Syrian Pound", "760", "Yes", "Damascus", "AS", ".sy", new string[] { "ar-SY", "ku", "hy", "arc", "fr", "en" }, false);
		public static Country TCA = new Country("TCA", () => "Turks & Caicos Islands", () => "Turks and Caicos Islands", "TC", "796", "TCA", "tc", "TI", "", "1-649", "TK", "USD", "TURKS AND CAICOS ISLANDS", "2", "US Dollar", "840", "Territory of GB", "Cockburn Town", "", ".tc", new string[] { "en-TC" }, false);
		public static Country TCD = new Country("TCD", () => "Chad", () => "Chad", "TD", "148", "TCD", "cd", "CD", "TCH", "235", "CD", "XAF", "CHAD", "0", "CFA Franc BEAC", "950", "Yes", "N'Djamena", "AF", ".td", new string[] { "fr-TD", "ar-TD", "sre" }, false);
		public static Country TGO = new Country("TGO", () => "Togo", () => "Togo", "TG", "768", "TGO", "tg", "TG", "TG", "228", "TO", "XOF", "TOGO", "0", "CFA Franc BCEAO", "952", "Yes", "Lome", "AF", ".tg", new string[] { "fr-TG", "ee", "hna", "kbp", "dag", "ha" }, false);
		public static Country THA = new Country("THA", () => "Thailand", () => "Thailand", "TH", "764", "THA", "th", "TH", "T", "66", "TH", "THB", "THAILAND", "2", "Baht", "764", "Yes", "Bangkok", "AS", ".th", new string[] { "th", "en" }, false);
		public static Country TJK = new Country("TJK", () => "Tajikistan", () => "Tajikistan", "TJ", "762", "TJK", "ta", "TA", "TJ", "992", "TI", "TJS", "TAJIKISTAN", "2", "Somoni", "972", "Yes", "Dushanbe", "AS", ".tj", new string[] { "tg", "ru" }, false);
		public static Country TKL = new Country("TKL", () => "Tokelau", () => "Tokelau", "TK", "772", "TKL", "tl", "TK", "NZ", "690", "TL", "NZD", "TOKELAU", "2", "New Zealand Dollar", "554", "Territory of NZ", "", "OC", ".tk", new string[] { "tkl", "en-TK" }, false);
		public static Country TKM = new Country("TKM", () => "Turkmenistan", () => "Turkmenistan", "TM", "795", "TKM", "tk", "TR", "TM", "993", "TX", "TMT", "TURKMENISTAN", "2", "Turkmenistan New Manat", "934", "Yes", "Ashgabat", "AS", ".tm", new string[] { "tk", "ru", "uz" }, false);
		public static Country TLS = new Country("TLS", () => "Timor-Leste", () => "Timor-Leste", "TL", "626", "TLS", "em", "TM", "", "670", "TT", "USD", "TIMOR-LESTE", "2", "US Dollar", "840", "Yes", "Dili", "OC", ".tl", new string[] { "tet", "pt-TL", "id", "en" }, false);
		public static Country TON = new Country("TON", () => "Tonga", () => "Tonga", "TO", "776", "TON", "to", "TO", "", "676", "TN", "TOP", "TONGA", "2", "Pa’anga", "776", "Yes", "Nuku'alofa", "OC", ".to", new string[] { "to", "en-TO" }, false);
		public static Country TTO = new Country("TTO", () => "Trinidad & Tobago", () => "Trinidad and Tobago", "TT", "780", "TRD", "tr", "TD", "TT", "1-868", "TD", "TTD", "TRINIDAD AND TOBAGO", "2", "Trinidad and Tobago Dollar", "780", "Yes", "Port of Spain", "", ".tt", new string[] { "en-TT", "hns", "fr", "es", "zh" }, false);
		public static Country TUN = new Country("TUN", () => "Tunisia", () => "Tunisia", "TN", "788", "TUN", "ti", "TS", "TN", "216", "TS", "TND", "TUNISIA", "3", "Tunisian Dinar", "788", "Yes", "Tunis", "AF", ".tn", new string[] { "ar-TN", "fr" }, false);
		public static Country TUR = new Country("TUR", () => "Turkey", () => "Turkey", "TR", "792", "TUR", "tu", "TU", "TR", "90", "TU", "TRY", "TURKEY", "2", "Turkish Lira", "949", "Yes", "Ankara", "AS", ".tr", new string[] { "tr-TR", "ku", "diq", "az", "av" }, false);
		public static Country TUV = new Country("TUV", () => "Tuvalu", () => "Tuvalu", "TV", "798", "TUV", "tv", "TV", "", "688", "TV", "AUD", "TUVALU", "2", "Australian Dollar", "036", "Yes", "Funafuti", "OC", ".tv", new string[] { "tvl", "en", "sm", "gil" }, false);
		public static Country TWN = new Country("TWN", () => "Taiwan", () => "", "TW", "158", "", "ch", "", "RC", "886", "TW", "", "", "", "", "", "Yes", "Taipei", "AS", ".tw", new string[] { "zh-TW", "zh", "nan", "hak" }, false);
		public static Country TZA = new Country("TZA", () => "Tanzania", () => "United Republic of Tanzania", "TZ", "834", "TZA", "tz", "TN", "EAT", "255", "TZ", "", "", "", "", "", "Yes", "Dodoma", "AF", ".tz", new string[] { "sw-TZ", "en", "ar" }, false);
		public static Country UGA = new Country("UGA", () => "Uganda", () => "Uganda", "UG", "800", "UGA", "ug", "UG", "EAU", "256", "UG", "UGX", "UGANDA", "0", "Uganda Shilling", "800", "Yes", "Kampala", "AF", ".ug", new string[] { "en-UG", "lg", "sw", "ar" }, false);
		public static Country UKR = new Country("UKR", () => "Ukraine", () => "Ukraine", "UA", "804", "UKR", "un", "UR", "UA", "380", "UP", "UAH", "UKRAINE", "2", "Hryvnia", "980", "Yes", "Kiev", "EU", ".ua", new string[] { "uk", "ru-UA", "rom", "pl", "hu" }, false);
		public static Country UMI = new Country("UMI", () => "U.S. Outlying Islands", () => "", "UM", "581", "", "ji,xf,wk,uc,up", "", "USA", "", "FQ,HQ,DQ,JQ,KQ,MQ,BQ,LQ,WQ", "", "", "", "", "", "Territories of US", "", "OC", ".um", new string[] { "en-UM" }, false);
		public static Country URY = new Country("URY", () => "Uruguay", () => "Uruguay", "UY", "858", "URG", "uy", "UY", "ROU", "598", "UY", "UYU", "URUGUAY", "2", "Peso Uruguayo", "858", "Yes", "Montevideo", "SA", ".uy", new string[] { "es-UY" }, false);
		public static Country USA = new Country("USA", () => "US", () => "United States of America", "US", "840", "USA", "xxu", "US", "USA", "1", "US", "", "", "", "", "", "Yes", "Washington", "", ".us", new string[] { "en-US", "es-US", "haw", "fr" }, false);
		public static Country UZB = new Country("UZB", () => "Uzbekistan", () => "Uzbekistan", "UZ", "860", "UZB", "uz", "UZ", "UZ", "998", "UZ", "UZS", "UZBEKISTAN", "2", "Uzbekistan Sum", "860", "Yes", "Tashkent", "AS", ".uz", new string[] { "uz", "ru", "tg" }, false);
		public static Country VAT = new Country("VAT", () => "Vatican City", () => "Holy See", "VA", "336", "CVA", "vc", "", "V", "39-06", "VT", "", "", "", "", "", "Yes", "Vatican City", "EU", ".va", new string[] { "la", "it", "fr" }, false);
		public static Country VCT = new Country("VCT", () => "St. Vincent & Grenadines", () => "Saint Vincent and the Grenadines", "VC", "670", "VCT", "xm", "VG", "WV", "1-784", "VC", "XCD", "SAINT VINCENT AND THE GRENADINES", "2", "East Caribbean Dollar", "951", "Yes", "Kingstown", "", ".vc", new string[] { "en-VC", "fr" }, false);
		public static Country VEN = new Country("VEN", () => "Venezuela", () => "Venezuela (Bolivarian Republic of)", "VE", "862", "VEN", "ve", "VN", "YV", "58", "VE", "", "", "", "", "", "Yes", "Caracas", "SA", ".ve", new string[] { "es-VE" }, false);
		public static Country VGB = new Country("VGB", () => "British Virgin Islands", () => "British Virgin Islands", "VG", "092", "VRG", "vb", "VI", "BVI", "1-284", "VI", "", "", "", "", "", "Territory of GB", "Road Town", "", ".vg", new string[] { "en-VG" }, false);
		public static Country VIR = new Country("VIR", () => "U.S. Virgin Islands", () => "United States Virgin Islands", "VI", "850", "VIR", "vi", "VI", "USA", "1-340", "VQ", "", "", "", "", "", "Territory of US", "Charlotte Amalie", "", ".vi", new string[] { "en-VI" }, false);
		public static Country VNM = new Country("VNM", () => "Vietnam", () => "Viet Nam", "VN", "704", "VTN", "vm", "VS", "VN", "84", "VM", "VND", "VIET NAM", "0", "Dong", "704", "Yes", "Hanoi", "AS", ".vn", new string[] { "vi", "en", "fr", "zh", "km" }, false);
		public static Country VUT = new Country("VUT", () => "Vanuatu", () => "Vanuatu", "VU", "548", "VUT", "nn", "NV", "", "678", "NH", "VUV", "VANUATU", "0", "Vatu", "548", "Yes", "Port Vila", "OC", ".vu", new string[] { "bi", "en-VU", "fr-VU" }, false);
		public static Country WLF = new Country("WLF", () => "Wallis & Futuna", () => "Wallis and Futuna Islands", "WF", "876", "WAL", "wf", "FW", "F", "681", "WF", "", "", "", "", "", "Territory of FR", "Mata Utu", "OC", ".wf", new string[] { "wls", "fud", "fr-WF" }, false);
		public static Country WSM = new Country("WSM", () => "Samoa", () => "Samoa", "WS", "882", "SMO", "ws", "ZM", "WS", "685", "WS", "WST", "SAMOA", "2", "Tala", "882", "Yes", "Apia", "OC", ".ws", new string[] { "sm", "en-WS" }, false);
		public static Country YEM = new Country("YEM", () => "Yemen", () => "Yemen", "YE", "887", "YEM", "ye", "YE", "YAR", "967", "YM", "YER", "YEMEN", "2", "Yemeni Rial", "886", "Yes", "Sanaa", "AS", ".ye", new string[] { "ar-YE" }, false);
		public static Country ZAF = new Country("ZAF", () => "South Africa", () => "South Africa", "ZA", "710", "AFS", "sa", "ZA", "ZA", "27", "SF", "ZAR", "SOUTH AFRICA", "2", "Rand", "710", "Yes", "Pretoria", "AF", ".za", new string[] { "zu", "xh", "af", "nso", "en-ZA", "tn", "st", "ts", "ss", "ve", "nr" }, false);
		public static Country ZMB = new Country("ZMB", () => "Zambia", () => "Zambia", "ZM", "894", "ZMB", "za", "ZB", "Z", "260", "ZA", "ZMW", "ZAMBIA", "2", "Zambian Kwacha", "967", "Yes", "Lusaka", "AF", ".zm", new string[] { "en-ZM", "bem", "loz", "lun", "lue", "ny", "toi" }, false);
		public static Country ZWE = new Country("ZWE", () => "Zimbabwe", () => "Zimbabwe", "ZW", "716", "ZWE", "rh", "ZW", "ZW", "263", "ZI", "ZWL", "ZIMBABWE", "2", "Zimbabwe Dollar", "932", "Yes", "Harare", "AF", ".zw", new string[] { "en-ZW", "sn", "nr", "nd" }, false);



		#endregion

		public Country this[string value]
		{
			get
			{
				return this.Single(x => x.Iso3.Equals(value, StringComparison.InvariantCultureIgnoreCase));
			}
		}

		public static Country Get(string identifier)
		{
			return new Countries()[identifier];
		}
	}
}
