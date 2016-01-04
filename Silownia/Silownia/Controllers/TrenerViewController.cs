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
    public class TrenerViewController : Controller
    {
        private SilowniaContext db = new SilowniaContext();
        public ActionResult Index(long? id)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Trener")
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    ViewBag.Trenerzy = new SelectList(db.Klienci, "OsobaID", "imieNazwisko");
                    Osoba os = db.Osoby.Find(id);

                    var wiad = db.Wiadomosci.Where(o => o.OsobaOdbierajaca.OsobaID == id);



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
            return HttpNotFound();

        }

        public ActionResult Act(string username, long trenerID)
        {



            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Trener")
                {
                    var osWys = db.Osoby.Find(Session["UserID"]);
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

    }
}