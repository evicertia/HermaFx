using System;
using System.Linq;

using HermaFx.DesignPatterns;

namespace HermaFx.Globalization
{
	[Serializable]
	public class Currencies : EnhancedEnumType<Currencies, Currency>
	{
		#region .ctor
		public Currencies() : base(x => x.Code)
		{
		}
		#endregion

		#region Enhanced Enum Members

#if false
		// CURRENCIES FROM FRAMEWORK. This list is obtained using LinqPad
		// Generated 10/05/2016

		System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.SpecificCultures)
		.Select(x => new System.Globalization.RegionInfo(x.Name))
		.GroupBy(c => c.ISOCurrencySymbol)
		.Select(x => x.First())
		.OrderBy(c => c.ISOCurrencySymbol)
		.Select(x =>
			string.Format("public static Currency {0} = new Currency(\"{0}\", \"{1}\", {2}, () => \"{3}\", \"{4}\");",
				x.ISOCurrencySymbol,
				x.CurrencySymbol,
				new[] { 0, 2 }.Contains(new System.Globalization.CultureInfo(x.Name).NumberFormat.CurrencyPositivePattern).ToString().ToLower(),
				x.CurrencyEnglishName, x.CurrencyNativeName
			)
		)
		.Aggregate((cur, next) => cur + "\n" + next)
#endif

		public static Currency AED = new Currency("AED", "د.إ.‏", true, () => "UAE Dirham", "درهم اماراتي");
		public static Currency AFN = new Currency("AFN", "؋", true, () => "Afghani", "افغانى");
		public static Currency ALL = new Currency("ALL", "Lek", false, () => "Albanian Lek", "Lek");
		public static Currency AMD = new Currency("AMD", "դր.", false, () => "Armenian Dram", "դրամ");
		public static Currency ARS = new Currency("ARS", "$", true, () => "Argentine Peso", "Peso");
		public static Currency AUD = new Currency("AUD", "$", true, () => "Australian Dollar", "Australian Dollar");
		public static Currency AZN = new Currency("AZN", "man.", false, () => "Azerbaijanian Manat", "manat");
		public static Currency BAM = new Currency("BAM", "KM", false, () => "Convertible Marks", "konvertibilna marka");
		public static Currency BDT = new Currency("BDT", "৳", true, () => "Bangladeshi Taka", "টাকা");
		public static Currency BGN = new Currency("BGN", "лв.", false, () => "Bulgarian Lev", "България лев");
		public static Currency BHD = new Currency("BHD", "د.ب.‏", true, () => "Bahraini Dinar", "دينار بحريني");
		public static Currency BND = new Currency("BND", "$", true, () => "Brunei Dollar", "Ringgit");
		public static Currency BOB = new Currency("BOB", "$b", true, () => "Boliviano", "Boliviano");
		public static Currency BRL = new Currency("BRL", "R$", true, () => "Real", "Real");
		public static Currency BYR = new Currency("BYR", "р.", false, () => "Belarusian Ruble", "рубль");
		public static Currency BZD = new Currency("BZD", "BZ$", true, () => "Belize Dollar", "Belize Dollar");
		public static Currency CAD = new Currency("CAD", "$", true, () => "Canadian Dollar", "ᑲᓇᑕᐅᑉ ᑮᓇᐅᔭᖓ");
		public static Currency CHF = new Currency("CHF", "fr.", true, () => "Swiss Franc", "Franc svizzer");
		public static Currency CLP = new Currency("CLP", "$", true, () => "Chilean Peso", "Peso");
		public static Currency CNY = new Currency("CNY", "¥", true, () => "PRC Renminbi", "མི་དམངས་ཤོག་སྒོཪ།");
		public static Currency COP = new Currency("COP", "$", true, () => "Colombian Peso", "Peso");
		public static Currency CRC = new Currency("CRC", "₡", true, () => "Costa Rican Colon", "Colón");
		public static Currency CSD = new Currency("CSD", "Din.", false, () => "Serbian Dinar", "dinar");
		public static Currency CZK = new Currency("CZK", "Kč", false, () => "Czech Koruna", "Koruna Česká");
		public static Currency DKK = new Currency("DKK", "kr.", true, () => "Danish Krone", "Dansk krone");
		public static Currency DOP = new Currency("DOP", "RD$", true, () => "Dominican Peso", "Peso");
		public static Currency DZD = new Currency("DZD", "DZD", false, () => "Algerian Dinar", "Dinar");
		public static Currency EEK = new Currency("EEK", "kr", false, () => "Estonian Kroon", "Kroon");
		public static Currency EGP = new Currency("EGP", "ج.م.‏", true, () => "Egyptian Pound", "جنيه مصري");
		public static Currency ETB = new Currency("ETB", "ETB", true, () => "Ethiopian Birr", "ብር");
		public static Currency EUR = new Currency("EUR", "€", false, () => "Euro", "euro");
		public static Currency GBP = new Currency("GBP", "£", true, () => "UK Pound Sterling", "UK Pound Sterling");
		public static Currency GEL = new Currency("GEL", "Lari", false, () => "Lari", "ლარი");
		public static Currency GTQ = new Currency("GTQ", "Q", true, () => "Guatemalan Quetzal", "Quetzal");
		public static Currency HKD = new Currency("HKD", "HK$", true, () => "Hong Kong Dollar", "港幣");
		public static Currency HNL = new Currency("HNL", "L.", true, () => "Honduran Lempira", "Lempira");
		public static Currency HRK = new Currency("HRK", "kn", false, () => "Croatian Kuna", "hrvatska kuna");
		public static Currency HUF = new Currency("HUF", "Ft", false, () => "Hungarian Forint", "forint");
		public static Currency IDR = new Currency("IDR", "Rp", true, () => "Indonesian Rupiah", "Rupiah");
		public static Currency ILS = new Currency("ILS", "₪", true, () => "Israeli New Shekel", "שקל חדש");
		public static Currency INR = new Currency("INR", "रु", true, () => "Indian Rupee", "रुपया");
		public static Currency IQD = new Currency("IQD", "د.ع.‏", true, () => "Iraqi Dinar", "دينار عراقي");
		public static Currency IRR = new Currency("IRR", "ريال", true, () => "Iranian Rial", "رىال");
		public static Currency ISK = new Currency("ISK", "kr.", false, () => "Icelandic Krona", "Króna");
		public static Currency JMD = new Currency("JMD", "J$", true, () => "Jamaican Dollar", "Jamaican Dollar");
		public static Currency JOD = new Currency("JOD", "د.ا.‏", true, () => "Jordanian Dinar", "دينار اردني");
		public static Currency JPY = new Currency("JPY", "¥", true, () => "Japanese Yen", "円");
		public static Currency KES = new Currency("KES", "S", true, () => "Kenyan Shilling", "Shilingi");
		public static Currency KGS = new Currency("KGS", "сом", false, () => "som", "сом");
		public static Currency KHR = new Currency("KHR", "៛", false, () => "Riel", "x179Aៀល");
		public static Currency KRW = new Currency("KRW", "₩", true, () => "Korean Won", "원");
		public static Currency KWD = new Currency("KWD", "د.ك.‏", true, () => "Kuwaiti Dinar", "دينار كويتي");
		public static Currency KZT = new Currency("KZT", "Т", true, () => "Tenge", "Т");
		public static Currency LAK = new Currency("LAK", "₭", false, () => "Kip", "ກີບ");
		public static Currency LBP = new Currency("LBP", "ل.ل.‏", true, () => "Lebanese Pound", "ليرة لبناني");
		public static Currency LKR = new Currency("LKR", "රු.", true, () => "Sri Lanka Rupee", "රුපියල්");
		public static Currency LTL = new Currency("LTL", "Lt", false, () => "Lithuanian Litas", "Litas");
		public static Currency LVL = new Currency("LVL", "Ls", true, () => "Latvian Lats", "Lats");
		public static Currency LYD = new Currency("LYD", "د.ل.‏", true, () => "Libyan Dinar", "دينار ليبي");
		public static Currency MAD = new Currency("MAD", "د.م.‏", true, () => "Moroccan Dirham", "درهم مغربي");
		public static Currency MKD = new Currency("MKD", "ден.", false, () => "Macedonian Denar", "денар");
		public static Currency MNT = new Currency("MNT", "₮", false, () => "Tugrik", "Төгрөг");
		public static Currency MOP = new Currency("MOP", "MOP", true, () => "Macao Pataca", "澳門幣");
		public static Currency MVR = new Currency("MVR", "ރ.", false, () => "Rufiyaa", "ރުފިޔާ");
		public static Currency MXN = new Currency("MXN", "$", true, () => "Mexican Peso", "Peso");
		public static Currency MYR = new Currency("MYR", "RM", true, () => "Malaysian Ringgit", "Ringgit Malaysia");
		public static Currency NIO = new Currency("NIO", "N", true, () => "Nigerian Naira", "Naira");
		public static Currency NOK = new Currency("NOK", "kr", true, () => "Norwegian Krone", "Norsk krone");
		public static Currency NPR = new Currency("NPR", "रु", true, () => "Nepalese Rupees", "रुपैयाँ");
		public static Currency NZD = new Currency("NZD", "$", true, () => "New Zealand Dollar", "tāra");
		public static Currency OMR = new Currency("OMR", "ر.ع.‏", true, () => "Omani Rial", "ريال عماني");
		public static Currency PAB = new Currency("PAB", "B/.", true, () => "Panamanian Balboa", "Balboa");
		public static Currency PEN = new Currency("PEN", "S/.", true, () => "Peruvian Nuevo Sol", "Nuevo Sol");
		public static Currency PHP = new Currency("PHP", "PhP", true, () => "Philippine Peso", "Philippine Peso");
		public static Currency PKR = new Currency("PKR", "Rs", true, () => "Pakistan Rupee", "روپيه");
		public static Currency PLN = new Currency("PLN", "zł", false, () => "Polish Zloty", "Złoty");
		public static Currency PYG = new Currency("PYG", "Gs", true, () => "Paraguay Guarani", "Guaraní");
		public static Currency QAR = new Currency("QAR", "ر.ق.‏", true, () => "Qatari Rial", "ريال قطري");
		public static Currency RON = new Currency("RON", "lei", false, () => "Romanian Leu", "Leu");
		public static Currency RSD = new Currency("RSD", "Din.", false, () => "Serbian Dinar", "dinar");
		public static Currency RUB = new Currency("RUB", "р.", false, () => "Russian Ruble", "рубль");
		public static Currency RWF = new Currency("RWF", "RWF", true, () => "Rwandan Franc", "Ifaranga");
		public static Currency SAR = new Currency("SAR", "ر.س.‏", true, () => "Saudi Riyal", "ريال سعودي");
		public static Currency SEK = new Currency("SEK", "kr", false, () => "Swedish Krona", "Svensk krona");
		public static Currency SGD = new Currency("SGD", "$", true, () => "Singapore Dollar", "新币");
		public static Currency SYP = new Currency("SYP", "ل.س.‏", true, () => "Syrian Pound", "جنيه سوري");
		public static Currency THB = new Currency("THB", "฿", true, () => "Thai Baht", "บาท");
		public static Currency TJS = new Currency("TJS", "т.р.", false, () => "Ruble", "рубл");
		public static Currency TMT = new Currency("TMT", "m.", false, () => "Turkmen manat", "manat");
		public static Currency TND = new Currency("TND", "د.ت.‏", true, () => "Tunisian Dinar", "دينار تونسي");
		public static Currency TRY = new Currency("TRY", "TL", false, () => "Turkish Lira", "Türk Lirası");
		public static Currency TTD = new Currency("TTD", "TT$", true, () => "Trinidad Dollar", "Trinidad Dollar");
		public static Currency TWD = new Currency("TWD", "NT$", true, () => "New Taiwan Dollar", "新台幣");
		public static Currency UAH = new Currency("UAH", "₴", false, () => "Ukrainian Grivna", "гривня");
		public static Currency USD = new Currency("USD", "$", true, () => "US Dollar", "US Dollar");
		public static Currency UYU = new Currency("UYU", "$U", true, () => "Peso Uruguayo", "Peso");
		public static Currency UZS = new Currency("UZS", "so'm", false, () => "Uzbekistan Som", "so‘m");
		public static Currency VEF = new Currency("VEF", "Bs. F.", true, () => "Venezuelan Bolivar", "Bolívar");
		public static Currency VND = new Currency("VND", "₫", false, () => "Vietnamese Dong", "Đồng");
		public static Currency XOF = new Currency("XOF", "XOF", false, () => "XOF Senegal", "XOF Senegal");
		public static Currency YER = new Currency("YER", "ر.ي.‏", true, () => "Yemeni Rial", "ريال يمني");
		public static Currency ZAR = new Currency("ZAR", "R", true, () => "South African Rand", "Rand");
		public static Currency ZWL = new Currency("ZWL", "Z$", true, () => "Zimbabwe Dollar", "Zimbabwe Dollar");

		#endregion

		public Currency this[string value]
		{
			get
			{
				return this.Single(x => x.Code.Equals(value, StringComparison.InvariantCultureIgnoreCase));
			}
		}

		public static Currency Get(string identifier)
		{
			return new Currencies()[identifier];
		}
	}
}
