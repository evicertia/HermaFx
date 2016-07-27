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
		private Func<string> _officialNameEn;
		#endregion

		#region Properties
		/// <summary>
		/// Alpha-2 codes from ISO 3166-1
		/// </summary>
		public string Iso2 { get; private set; }
		/// <summary>
		/// Alpha-3 codes from ISO 3166-1 (synonymous with World Bank Codes)
		/// </summary>
		public string Iso3 { get; private set; }
		/// <summary>
		/// Numeric codes from ISO 3166-1 (synonymous with UN Statistics M49 Codes)
		/// </summary>
		public string IsoNum { get; private set; }
		/// <summary>
		/// Codes assigned by the International Telecommunications Union
		/// </summary>
		public string Itu { get; private set; }
		/// <summary>
		/// MAchine-Readable Cataloging codes from the Library of Congress
		/// </summary>
		public string Marc { get; private set; }
		/// <summary>
		/// Country abbreviations by the World Meteorological Organization
		/// </summary>
		public string Wmo { get; private set; }
		/// <summary>
		/// Distinguishing signs of vehicles in international traffic
		/// </summary>
		public string Ds { get; private set; }
		/// <summary>
		/// Country code from ITU-T recommendation E.164, sometimes followed by area code
		/// </summary>
		public string Dial { get; private set; }
		/// <summary>
		/// Codes from the U.S. standard FIPS PUB 10-4
		/// </summary>
		public string Fips { get; private set; }
		/// <summary>
		/// ISO 4217 currency alphabetic code
		/// </summary>
		public string CurrencyAlphabetic { get; private set; }
		/// <summary>
		/// ISO 4217 country name
		/// </summary>
		public string CurrencyCountry { get; private set; }
		/// <summary>
		/// ISO 4217 currency number of minor units
		/// </summary>
		public string CurrencyMinorUnit { get; private set; }
		/// <summary>
		/// ISO 4217 currency name
		/// </summary>
		public string CurrencyName { get; private set; }
		/// <summary>
		/// ISO 4217 currency numeric code
		/// </summary>
		public string CurrencyNumeric { get; private set; }
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
		public string Tld { get; private set; }
		/// <summary>
		/// Languages from Geonames
		/// </summary>
		public string[] Languages { get; private set; }
		/// <summary>
		/// Indicates if the country is a member of the European Union
		/// </summary>
		/// <seealso cref="http://europa.eu/about-eu/countries/index_en.htm"/>
		public bool IsMemberOfEu { get; private set; }

		public string Name => _name();
		public string OfficialNameEn => _officialNameEn();
		#endregion

		#region .ctor
		public Country(string iso3, Func<string> name, Func<string> officialNameEn, string iso2, string isoNum,
			string itu, string marc, string wmo, string ds, string dial, string fips,
			string currencyAlphabetic, string currencyCountry, string currencyMinorUnit,
			string currencyName, string currencyNumeric,
			string isIndependent, string capital, string continent, string tld, string[] languages, bool isMemberOfEu)
		{
			Guard.IsNotNullNorWhitespace(iso3, nameof(iso3));
			Guard.IsNotNull(name, nameof(name));
			Guard.IsNotNullNorWhitespace(iso2, nameof(iso2));
			Guard.IsNotNullNorWhitespace(fips, nameof(fips));

			Iso3 = iso3;
			_name = name;
			_officialNameEn = officialNameEn;
			Iso2= iso2;
			IsoNum = isoNum;
			Itu = itu;
			Marc = marc;
			Wmo = wmo;
			Ds = ds;
			Dial = dial;
			Fips = fips;
			CurrencyAlphabetic = currencyAlphabetic;
			CurrencyCountry = currencyCountry;
			CurrencyMinorUnit = currencyMinorUnit;
			CurrencyName = currencyName;
			CurrencyNumeric = currencyNumeric;
			IsIndependent = isIndependent;
			Capital = capital;
			Continent = continent;
			Tld = tld;
			Languages = languages;
			IsMemberOfEu = isMemberOfEu;
		}
		#endregion
	}
}
