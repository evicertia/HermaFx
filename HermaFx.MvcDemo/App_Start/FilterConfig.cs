using HermaFx.MvcDemo.Infrastructure;

namespace HermaFx.MvcDemo.App_Start
{
	public class FilterConfig
	{
		public static void Configure(System.Web.Mvc.GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}
	}
}
