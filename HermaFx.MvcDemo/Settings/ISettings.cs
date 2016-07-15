using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

using HermaFx;
using HermaFx.ComponentModel;
using HermaFx.DataAnnotations;
using HermaFx.Settings;
using System.Configuration;

namespace HermaFx.MvcDemo
{
	[Settings]
	public interface ISettings
	{
		[Required]
		string SampleSetting { get; set; }

		[Setting]
		UserInterfaceSettings UserInterface { get; set; }
	}

	public interface UserInterfaceSettings
	{
		[DefaultValue("es-ES")]
		string DefaultLanguage { get; set; }

		#region List/Grid Settings

		[DefaultValue("10,25,50,100")]
		[TypeConverter(typeof(StringArrayConverter))]
		[MinElements(1), MaxElements(5)]
		[ValidateElementsUsing(typeof(UserInterfaceSettings), "ListPageSizeMax")]
		uint[] ListPageSizes { get; set; }

		[DefaultValue(100), Range(1, 100)]
		uint ListPageSizeMax { get; set; }

		[DefaultValue(20)]
		uint ListPageSizeDefault { get; set; }

		[DefaultValue(4), Range(1, 8)]
		int ListNumberOfPages { get; set; }

		#endregion

		#region AutoComplete
		/// <summary>
		/// Gets or sets the auto-complete widget's maximum items returned.
		/// </summary>
		/// <value>
		/// The automatic complete maximum items.
		/// </value>
		[DefaultValue(20), Range(1, 100)]
		int AutoCompleteMaxItems { get; set; }

		/// <summary>
		/// Gets or sets the auto-complete widget's delay (in ms).
		/// </summary>
		/// <value>
		/// The automatic complete delay.
		/// </value>
		[DefaultValue(250), Range(100, 10000)]
		int AutoCompleteDelay { get; set; }

		/// <summary>
		/// Gets or sets the minimum input chars the user should write down before auti-complete widget's fires.
		/// </summary>
		/// <value>
		/// The automatic complete minimum input chars.
		/// </value>
		[DefaultValue(1), Range(0, 10)]
		int AutoCompleteMinimumInputChars { get; set; }
		#endregion

		#region MultiLineText
		[DefaultValue(20)]
		int MultilineDefaultRows { get; set; }
		#endregion
	}
}
