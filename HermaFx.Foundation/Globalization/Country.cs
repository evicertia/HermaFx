using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using HermaFx;
using HermaFx.DesignPatterns;

namespace HermaFx.Globalization
{
	public sealed class Country
	{
		#region Fields
		private Func<string> _name;
		private Func<string> _official_Name_EN;
		#endregion

		#region Properties
		/// <summary>
		/// Alpha-2 codes from ISO 3166-1
		/// </summary>
		public string ISO3166_1_Alpha2 { get; private set; }
		/// <summary>
		/// Alpha-3 codes from ISO 3166-1 (synonymous with World Bank Codes)
		/// </summary>
		public string ISO3166_1_Alpha3 { get; private set; }
		/// <summary>
		/// Numeric codes from ISO 3166-1 (synonymous with UN Statistics M49 Codes)
		/// </summary>
		public string ISO3166_1_Num { get; private set; }
		/// <summary>
		/// Codes assigned by the International Telecommunications Union
		/// </summary>
		public string ITU { get; private set; }
		/// <summary>
		/// MAchine-Readable Cataloging codes from the Library of Congress
		/// </summary>
		public string MARC { get; private set; }
		/// <summary>
		/// Country abbreviations by the World Meteorological Organization
		/// </summary>
		public string WMO { get; private set; }
		/// <summary>
		/// Distinguishing signs of vehicles in international traffic
		/// </summary>
		public string DS { get; private set; }
		/// <summary>
		/// Country code from ITU-T recommendation E.164, sometimes followed by area code
		/// </summary>
		public string Dial { get; private set; }
		/// <summary>
		/// Codes from the U.S. standard FIPS PUB 10-4
		/// </summary>
		public string FIPS { get; private set; }
		/// <summary>
		/// ISO 4217 currency alphabetic code
		/// </summary>
		public string ISO4217_Currency_Alphabetic { get; private set; }
		/// <summary>
		/// ISO 4217 country name
		/// </summary>
		public string ISO4217_Currency_Country { get; private set; }
		/// <summary>
		/// ISO 4217 currency number of minor units
		/// </summary>
		public string ISO4217_Currency_Minor_Unit { get; private set; }
		/// <summary>
		/// ISO 4217 currency name
		/// </summary>
		public string ISO4217_Currency_Name { get; private set; }
		/// <summary>
		/// ISO 4217 currency numeric code
		/// </summary>
		public string ISO4217_Currency_Numeric { get; private set; }
		/// <summary>
		/// Country status, based on the CIA World Factbook
		/// </summary>
		public string IsIndependent { get; private set; }
		/// <summary>
		/// Capital city from Geonames
		/// </summary>
		public string Capital { get; private set; }
		/// <summary>
		/// Continent from Geonames
		/// </summary>
		public string Continent { get; private set; }
		/// <summary>
		/// Top level domain from Geonames
		/// </summary>
		public string TLD { get; private set; }
		/// <summary>
		/// Languages from Geonames
		/// </summary>
		public string Languages { get; private set; }
		/// <summary>
		/// Indicates if the country is a member of the European Union
		/// </summary>
		public bool IsMemberOfEU { get; private set; }

		public string Name => _name();
		public string Official_Name_EN => _official_Name_EN();
		#endregion

		#region .ctor
		public Country(string iso3166_1_alpha3, Func<string> name, Func<string> official_name_en, string iso3166_1_alpha2, string iso3166_1_num,
			string itu, string marc, string wmo, string ds, string dial, string fips,
			string iso4217_currency_alphabetic, string iso4217_currency_country, string iso4217_currency_minor_unit,
			string iso4217_currency_name, string iso4217_currency_numeric,
			string isIndependent, string capital, string continent, string tld, string languages, bool isMemberOfEu)
		{
			Guard.IsNotNullNorWhitespace(iso3166_1_alpha3, nameof(iso3166_1_alpha3));
			Guard.IsNotNull(name, nameof(name));
			Guard.IsNotNullNorWhitespace(iso3166_1_alpha2, nameof(iso3166_1_alpha2));
			Guard.IsNotNullNorWhitespace(fips, nameof(fips));

			ISO3166_1_Alpha3 = iso3166_1_alpha3;
			_name = name;
			_official_Name_EN = official_name_en;
			ISO3166_1_Alpha2= iso3166_1_alpha2;
			ISO3166_1_Num = iso3166_1_num;
			ITU = itu;
			MARC = marc;
			WMO = wmo;
			DS = ds;
			Dial = dial;
			FIPS = fips;
			ISO4217_Currency_Alphabetic = iso4217_currency_alphabetic;
			ISO4217_Currency_Country = iso4217_currency_country;
			ISO4217_Currency_Minor_Unit = iso4217_currency_minor_unit;
			ISO4217_Currency_Name = iso4217_currency_name;
			ISO4217_Currency_Numeric = iso4217_currency_numeric;
			IsIndependent = isIndependent;
			Capital = capital;
			Continent = continent;
			TLD = tld;
			Languages = languages;
			IsMemberOfEU = isMemberOfEu;
		}
		#endregion
	}
}
