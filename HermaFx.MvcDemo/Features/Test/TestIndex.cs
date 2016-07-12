using System;
using System.Collections.Generic;
using System.Globalization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using HermaFx.DataAnnotations;
using HermaFx;

namespace HermaFx.MvcDemo.Features
{
	[DisplayName("Test de controles") /*, Html.VisualState(VisualState.Primary)*/]
	public class TestIndex
	{
		private const string _actionName = "ActionSubmit";

		#region MESSAGES
		public class Messages
		{
			public const string ClearSuccess = "Se ha vaciado la colección completa con id {0}";
			public const string SubmitSuccess = "Los datos se enviaron correctamente";

			public const string UniqueId = "Identificador único";

			public const string Name = "Nombre";
			public const string NameRequiredError = "El campo Nombre es requerido";
			public const string LastName = "Apellido";
			public const string LastNameRequiredError = "El campo Apellido es requerido";
			public const string LastName2 = "Apellido2 (Smal size)";

			public const string BornDate = "Fecha Nacimiento (DateTime)";
			public const string Age = "Edad (Large size)";
			public const string Email = "Correo ej: Nombre <nombre@servidor.com>";

			public const string Contact = "Contacto (Enum con item decorados)";
			public const string ContactMobile = "Móvil";
			public const string ContactTelFix = "Tel. fijo";
			public const string ContactEmail = "Email";
			public const string ContactFacebook = "Facebook";
			public const string ContactDay = "Día de contacto (Dynamic SelectList)";

			public const string Address = "Dirección (ReadOnly)";
			public const string Password = "Contraseña (Password)";

			public const string Products = "Productos";
			public const string Bandwidth = "Ancho de banda (3-200)MB";
			public const string BandwidthMax = "Ancho de banda máximo por defecto (150)MB";
			public const string BandwidthLimitError = "Límite de Ancho de banda superado";
			public const string NetAmountEur = "Importe Neto (EUR)";
			public const string NetAmountUsd = "Importe Neto (USD)";

			public const string Country = "Pais";
			public const string States = "Provincias";

			public const string Comments = "Comentarios";
			public const string Agreep = "Aceptar";
			public const string AgreepDesc = "Acepto los términos y condiciones";
		}
		#endregion

		public enum _ContactMethod
		{
			[Display(Name = Messages.ContactMobile)]
			Mobile = 0,
			[Display(Name = Messages.ContactTelFix)]
			TelFix,
			[Display(Name = Messages.ContactEmail)]
			Email,
			[Display(Name = Messages.ContactFacebook)]
			Facebbok
		}

		public enum _Action
		{
			Unknown = 0,
			Submit,
			Clear,
			Clear2
		}

		[ScaffoldColumn(false)]
		public _Action ActionSubmit { get; set; }

		// Guid
		[Display(Name = Messages.UniqueId)]
		public Guid UniqueId { get; set; }

		// Strings
		[Display(Name = Messages.Name)]
		[RequiredIf(_actionName, _Action.Submit, ErrorMessage = Messages.NameRequiredError)]
		public string Name { get; set; }

		[Display(Name = Messages.LastName)]
		[RequiredIf(_actionName, _Action.Submit, ErrorMessage = Messages.LastNameRequiredError)]
		public string LastName { get; set; }

		// Size control
		[Display(Name = Messages.LastName2)]
		//[ControlSize(ControlSize.Small)]
		public string LastName2 { get; set; }
		[Display(Name = Messages.Age)]
		//[ControlSize(ControlSize.Large)]
		public string Age { get; set; }

		// Html Encoding test
		[Display(Name = Messages.Email)]
		public string Email { get; set; }

		// DateTime
		[Display(Name = Messages.BornDate)]
		[DataType(DataType.DateTime)]
		public DateTime BornDate { get; set; }

		// DropDownList select opition (Instead of RadioButonList)
		// Enum
		[Display(Name = Messages.Contact)]
		public _ContactMethod Contact { get; set; }

#if false
		// SelectList
		[DisplayName(Messages.ContactDay)]
		[DropDownList(nameof(ContactDayList))]
		public string ContactDayId { get; set; }
		public IEnumerable<SelectListItem> ContactDayList
		{
			get
			{
				var dayNames = CultureInfo.CurrentCulture.DateTimeFormat.DayNames;
				return dayNames.ToSelectList(x => x, x => string.Format("{0} ({1})", x, dayNames.ToList().FindIndex(_ => _ == x)), ContactDayId);
			}
		}
#endif

		// String ReadOnly
		[ReadOnly(true)]
		[Display(Name = Messages.Address)]
		public string Address { get; set; }

		// Password
		[DataType(DataType.Password)]
		[Display(Name = Messages.Password)]
		public string Password { get; set; }

#if false
		// DropDownList (Products)
		[Display(Name = Messages.Products)]
		[DropDownList(nameof(ProductsList))]
		public string ProductId { get; set; }
		[HiddenInputList(DisplayValue = false)]
		public IEnumerable<string> Products { get; set; }
		public Data.Product Product { get { return ProductId.IfNotNull(x => new Data.Products()[x]); } }
		public IEnumerable<SelectListItem> ProductsList
		{
			get
			{
				return new Data.Products().ToSelectList(x => x.Identifier, x => x.SelectText, ProductId);
			}
		}
#endif
		// Numeric
		[Range(3, 200, ErrorMessage = Messages.BandwidthLimitError)]
		[Display(Name = Messages.Bandwidth)]
		public Int16 Bandwidth { get; set; }

		// Numeric
		[DefaultValue(150)]
		[Display(Name = Messages.BandwidthMax)]
		public Int16 BandwidthMax { get; set; }

		// Currency EUR
		[Display(Name = Messages.NetAmountEur) /*, Currency("EUR")*/]
		public decimal NetAmountEur { get; set; }

		// Currency USD
		[Display(Name = Messages.NetAmountUsd) /*, Currency("USD") */]
		public decimal NetAmountUsd { get; set; }

		// TextArea
		[Display(Name = Messages.Comments)]
		//[MultilineText(10)]
		public string Comments { get; set; }

		// CheckBox with Description!!!
		[Display(Name = Messages.Agreep, Description = Messages.AgreepDesc)]
		public bool Agreep { get; set; }

		//SelectList simple
		[Display(Name = Messages.Country)]
		//[SelectList]
		public string Country { get; set; }

		//SelectList Multiple
		[Display(Name = Messages.States)]
		//[SelectList(MultiSelect = true)]
		public IEnumerable<string> States { get; set; }
	}
}