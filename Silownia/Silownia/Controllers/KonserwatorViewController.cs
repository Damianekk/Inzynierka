using Silownia.DAL;
using Silownia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Silownia.Helpers;
using Newtonsoft.Json;

namespace Silownia.Controllers
{
    public class KonserwatorViewController : Controller
    {
        private SilowniaContext db = new SilowniaContext();
        public ActionResult Index(long? id, AkcjaEnumKonserwacja? akcja)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Konserwator")
                {
                    if (id == null) // jeżeli nie podano idOsoby jako parametr 
                    {
                        if (String.IsNullOrEmpty(Session["loggedUserID"].ToString())) // sprawdzamy czy zalogowana jest osoba
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                        else
                        {
                            id = (long)(Session["loggedUserID"]); // jeśli jest to bierzemy jej id z sesji 
                        }
                    }
                    Osoba os = db.Osoby.Find(id);

                    if (akcja != AkcjaEnumKonserwacja.Brak)
                    {
                        ViewBag.Akcja = akcja;
                    }

                    if (os == null)
                    {
                        return HttpNotFound();
                    }
                    var z = os;

                    return View(z);
                }
            }
            return HttpNotFound();

        }


        public ActionResult PrzyjmijKonserwacje(long id)
        {
            var kons = db.Konserwacje.Find(id);
            kons.Status = "w naprawie";
            db.SaveChanges();
            return RedirectToAction("Index", "KonserwatorView", new { akcja = AkcjaEnumKonserwacja.PrzyjetoKonserwacje });
        }

        public ActionResult ZamknijKonserwacje(long id)
        {
            var kons = db.Konserwacje.Find(id);
            kons.Status = "zakończona";
            kons.Data_naprawy = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("Index", "KonserwatorView", new { akcja = AkcjaEnumKonserwacja.ZamknietoKonserwacje });
        }
    }
}