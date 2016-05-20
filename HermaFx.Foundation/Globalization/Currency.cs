using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using HermaFx;
using HermaFx.DesignPatterns;

namespace HermaFx.Globalization
{
	public sealed class Currency
	{
		#region Fields
		private static IDictionary<string, NumberFormatInfo> _cleanNumberFormatInfos = new Dictionary<string, NumberFormatInfo>();
		private Func<string> _name;
		#endregion

		#region Properties
		/// <summary>
		/// Gets the ISO code of this currency.
		/// </summary>
		/// <value>
		/// The code.
		/// </value>
		public string Code { get; private set; }
		public string Symbol { get; private set; }
		public bool SymbolBeforeValue { get; private set; }
		public string Name => _name();
		public string LocalName { get; private set; }
		#endregion

		#region Methods
		private static NumberFormatInfo GetNumberFormatInfo()
		{
			var culture = CultureInfo.CurrentUICulture;

			lock (_cleanNumberFormatInfos)
			{
				if (!_cleanNumberFormatInfos.ContainsKey(culture.Name))
				{
					var obj = culture.NumberFormat.Clone() as NumberFormatInfo;
					obj.CurrencySymbol = "";
					_cleanNumberFormatInfos[culture.Name] = obj;
				}
			}

			return _cleanNumberFormatInfos[culture.Name];
		}

		private string FormatInternal(IFormatProvider provider, decimal amount, bool hideSymbol)
		{
			provider = provider ?? GetNumberFormatInfo();

			return hideSymbol ? string.Format(provider, "{0:c}", amount)
				: SymbolBeforeValue ? string.Format(provider, "{0} {1:c}", Symbol, amount)
					: string.Format(provider, "{0:c} {1}", amount, Symbol);
		}

		public string Format(IFormatProvider provider, short amount, bool hideSymbol = false) => FormatInternal(provider, Convert.ToDecimal(amount), hideSymbol);
		public string Format(IFormatProvider provider, ushort amount, bool hideSymbol = false) => FormatInternal(provider, Convert.ToDecimal(amount), hideSymbol);
		public string Format(IFormatProvider provider, int amount, bool hideSymbol = false) => FormatInternal(provider, Convert.ToDecimal(amount), hideSymbol);
		public string Format(IFormatProvider provider, uint amount, bool hideSymbol = false) => FormatInternal(provider, Convert.ToDecimal(amount), hideSymbol);
		public string Format(IFormatProvider provider, long amount, bool hideSymbol = false) => FormatInternal(provider, Convert.ToDecimal(amount), hideSymbol);
		public string Format(IFormatProvider provider, ulong amount, bool hideSymbol = false) => FormatInternal(provider, Convert.ToDecimal(amount), hideSymbol);
		public string Format(IFormatProvider provider, double amount, bool hideSymbol = false) => FormatInternal(provider, Convert.ToDecimal(amount), hideSymbol);
		public string Format(IFormatProvider provider, decimal amount, bool hideSymbol = false) => FormatInternal(provider, amount, hideSymbol);

		public string Format(short amount, bool hideSymbol = false) => FormatInternal(null, Convert.ToDecimal(amount), hideSymbol);
		public string Format(ushort amount, bool hideSymbol = false) => FormatInternal(null, Convert.ToDecimal(amount), hideSymbol);
		public string Format(int amount, bool hideSymbol = false) => FormatInternal(null, Convert.ToDecimal(amount), hideSymbol);
		public string Format(uint amount, bool hideSymbol = false) => FormatInternal(null, Convert.ToDecimal(amount), hideSymbol);
		public string Format(long amount, bool hideSymbol = false) => FormatInternal(null, Convert.ToDecimal(amount), hideSymbol);
		public string Format(ulong amount, bool hideSymbol = false) => FormatInternal(null, Convert.ToDecimal(amount), hideSymbol);
		public string Format(double amount, bool hideSymbol = false) => FormatInternal(null, Convert.ToDecimal(amount), hideSymbol);
		public string Format(decimal amount, bool hideSymbol = false) => FormatInternal(null, amount, hideSymbol);

		#endregion

		#region .ctor
		public Currency(string id, string symbol, bool symbolBeforeValue, Func<string> name, string localName)
		{
			Guard.IsNotNullNorWhitespace(id, nameof(id));
			Guard.IsNotNullNorWhitespace(symbol, nameof(symbol));
			Guard.IsNotNull(name, nameof(name));
			Guard.IsNotNullNorWhitespace(localName, nameof(localName));

			Code = id;
			Symbol = symbol;
			SymbolBeforeValue = symbolBeforeValue;
			_name = name;
			LocalName = localName;
		}
		#endregion
	}
}
