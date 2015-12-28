using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Silownia.DAL;
using System.Web.Script.Serialization;
using System;
using PagedList;
using Silownia.Models;
using System.Data;
using System.Collections.Generic;


namespace Silownia.Controllers
{
    public class HomeController : Controller
    {

        private SilowniaContext db = new SilowniaContext();

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

        [HttpPost]
        public JsonResult SilowniaInfoJSON()
        {

            var jsonSerialiser = new JavaScriptSerializer();
            var silownie = db.Silownie.Where(s => s.Adres != null).ToList<Silownia.Models.Silownia>();

            //  silownie.RemoveAll(item => item.Adres != null);
            var z = silownie.Select(x => new
            {
                dlugosc = x.Dlugosc,
                szerokosc = x.Szerokosc,
                godzOtw = x.GodzinaOtwarcia,
                godzZam = x.GodzinaZamkniecia,
                nazwa = x.Nazwa,
                info = x.infoDodatkowe

            });
            return Json(z);

            //  return Json(new { ok = true, myData = silownie }, JsonRequestBehavior.AllowGet);
        }
    }
}