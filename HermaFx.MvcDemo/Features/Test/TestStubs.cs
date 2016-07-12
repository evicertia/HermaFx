using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace HermaFx.MvcDemo.Features
{
	public static class TestStubs
	{
		public static TestIndex PopulateControlData()
		{
			return new TestIndex()
			{
				UniqueId = Guid.NewGuid(),

				Name = "Antonio",
				LastName = "Recio",
				LastName2 = " Mayorista",
				BornDate = DateTime.UtcNow,
				Age = "47",
				Email = "Antonio <antonio.recio@gmail.com>",
				Contact = TestIndex._ContactMethod.Mobile,
				//ContactDayId = CultureInfo.CurrentCulture.DateTimeFormat.DayNames[2],

				Address = "Read Only Street, Nº 2, Madrid, Spain",
				Password = "SecretoIberico",

				//ProductId = Data.Products.RmailPremium.Identifier.ToString(),
				Bandwidth = 50,
				NetAmountEur = 15.23M,
				NetAmountUsd = 18.01M,

				Comments = string.Format("Esto es un comentario multi{0}línea insertado el: {1} (UTC)", Environment.NewLine, DateTime.UtcNow),
				Agreep = true
			};
		}
	}
}