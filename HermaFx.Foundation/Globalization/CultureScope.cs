using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Globalization;

namespace HermaFx
{
	/// <summary>
	/// An IDisposable which sets a temporal CultureInfo, so it can be used within using() scopes.
	/// </summary>
	public sealed class CultureScope : IDisposable
	{
		private readonly CultureInfo _culture;
		private readonly CultureInfo _uiCulture;

		public CultureScope(string lang)
		{
			_culture = Thread.CurrentThread.CurrentCulture;
			_uiCulture = Thread.CurrentThread.CurrentUICulture;
			
			SetCulture(lang);
		}

		public CultureScope(CultureInfo culture)
			: this(culture.Name)
		{
		}

		public static void SetCulture(string lang)
		{
			if (string.IsNullOrEmpty(lang))
			{
				Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
				Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
				return;
			}

			var culture = CultureInfo.GetCultureInfo(lang);
			Thread.CurrentThread.CurrentCulture = culture;
			Thread.CurrentThread.CurrentUICulture = culture;
		}

		public void Dispose()
		{
			Thread.CurrentThread.CurrentCulture = _culture;
			Thread.CurrentThread.CurrentUICulture = _uiCulture;
		}
	}
}
