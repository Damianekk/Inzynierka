using System.Web.Mvc;

namespace Silownia.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "O nas";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Skontaktuj się!";

            return View();
        }
    }
}