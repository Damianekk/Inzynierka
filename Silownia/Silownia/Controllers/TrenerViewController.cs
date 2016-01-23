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
using System.Web.Script.Serialization;

namespace Silownia.Controllers
{
    public class TrenerViewController : Controller
    {
        private SilowniaContext db = new SilowniaContext();
        public ActionResult Index(long? id, AkcjaEnumTrening? akcja)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Trener")
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
                    if (id == (long)Session["loggedUserID"])
                    {
                        ViewBag.Klienci = new SelectList(db.Klienci, "OsobaID", "imieNazwisko");
                        Osoba os = db.Osoby.Find(id);

                        var wiad = db.Wiadomosci.Where(o => o.OsobaOdbierajaca.OsobaID == id);

                        if (akcja != AkcjaEnumTrening.Brak)
                        {
                            ViewBag.Akcja = akcja;
                        }

                        foreach (Wiadomosc w in wiad)
                        {
                            w.Status = StatusWiadomosciEnum.Odebrany;
                            w.Odebrano = DateTime.Now;
                        }

                        if (wiad.Count() > 0)
                            ViewBag.Wiad = wiad.ToList<Wiadomosc>();

                        else
                            ViewBag.Wiad = null;

                        if (os == null)
                        {
                            return HttpNotFound();
                        }
                        var z = os;

                        return View(z);
                    }
                }
            }
            return HttpNotFound();

        }

        public ActionResult Act(string username, long trenerID)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Trener")
                {
                    var osWys = db.Osoby.Find(Session["loggedUserID"]);
                    var osOdb = db.Osoby.Find(trenerID);

                    Wiadomosc wiadomosc = new Wiadomosc
                    {
                        OsobaWysylajaca = osWys,
                        OsobaOdbierajaca = osOdb,
                        Tresc = username,
                        Wyslano = DateTime.Now,
                        Odebrano = null,
                        Status = StatusWiadomosciEnum.Wyslany
                    };

                    db.Wiadomosci.Add(wiadomosc);
                    db.SaveChanges();


                    return null;
                }
            }
            return HttpNotFound();
        }

        [HttpPost]
        public JsonResult OdbierzWiad()
        {

            var jsonSerialiser = new JavaScriptSerializer();
            var silownie = db.Silownie.Where(s => s.Adres != null).ToList<Silownia.Models.Silownia>();

            long loggUsID = (long)Session["loggedUserID"];
            //  silownie.RemoveAll(item => item.Adres != null);
            var wiad = db.Wiadomosci.Where(o => o.OsobaOdbierajaca.OsobaID == loggUsID);


            foreach (Wiadomosc w in wiad)
            {
                w.Status = StatusWiadomosciEnum.Odebrany;
                w.Odebrano = DateTime.Now;
            }

            if (wiad.Count() > 0)
                ViewBag.Wiad = wiad.ToList<Wiadomosc>();


            var z = wiad.Select(x => new
            {
                osWys = (x.OsobaWysylajaca.Imie + " " + x.OsobaWysylajaca.Nazwisko),
                tresc = x.Tresc,
                dataWys = x.Wyslano.Value

            });

            return Json(z);

            //  return Json(new { ok = true, myData = silownie }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UsunTrenerowiTreningPersonalny(long id)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Trener")
                {
                    var trP = db.TreningiPersonalne.Find(id);
                    db.TreningiPersonalne.Remove(trP);
                    db.SaveChanges();
                    return RedirectToAction("Index", "TrenerView", new { akcja = AkcjaEnumTrening.UsunietoTrening });
                }
            }
            return HttpNotFound();
        }
    }
}