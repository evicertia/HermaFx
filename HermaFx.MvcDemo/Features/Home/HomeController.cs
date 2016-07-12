using System.Web.Mvc;

namespace HermaFx.MvcDemo.Controllers
{
	public partial class HomeController : Controller
	{
		// GET: Home
		public virtual ActionResult Index()
		{
			return View();
		}
	}
}