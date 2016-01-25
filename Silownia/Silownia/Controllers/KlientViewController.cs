using Silownia.DAL;
using Silownia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Silownia.Helpers;
using System.Web.Script.Serialization;

namespace Silownia.Controllers
{
    public class KlientViewController : Controller
    {
        private SilowniaContext db = new SilowniaContext();
        public ActionResult Index(long? id,AkcjaEnumMasaz? akcja)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Klient")
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

                    if(id == (long)Session["loggedUserID"])
                    {
                        if (akcja != AkcjaEnumMasaz.Brak)
                        {
                            ViewBag.Akcja = akcja;
                        }

                        ViewBag.Trenerzy = new SelectList(db.Trenerzy, "OsobaID", "imieNazwisko");
                        Klient klient = db.Klienci.Find(id);

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

                        if (klient == null)
                        {
                            return HttpNotFound();
                        }
                        var z = klient;

                        return View(z);
                    }
                }
            }
            return HttpNotFound();
        }

        public ActionResult Act(string username,long trenerID)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Klient")
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

            //  silownie.RemoveAll(item => item.Adres != null);
            long loggUsID = (long)Session["loggedUserID"];
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
                osWys = (x.OsobaWysylajaca.Imie+" "+x.OsobaWysylajaca.Nazwisko) ,
                tresc = x.Tresc,
                dataWys = x.Wyslano.Value

            });
 
            return Json(z);

            //  return Json(new { ok = true, myData = silownie }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UsunKlientowiTreningGrupowy(long id)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Klient")
                {
                  
                    ZajeciaGrupowe zajecia = db.ZajeciaGrup.Find(id);
                    if (zajecia == null)
                    {
                        return HttpNotFound();
                    }

                    Klient klient = db.Klienci.Find(Session["loggedUserID"]);

                    if (klient.KlienciTreningiGrupowe.Where(s => s.TreningID == zajecia.TreningID).Count() != 0)
                    {
                        var trening = klient.KlienciTreningiGrupowe.Where(s => s.TreningID == zajecia.TreningID).First();
                        if (klient.KlienciTreningiGrupowe.Contains(trening))
                        {
                            zajecia.ZapisaneOsoby--;
                            db.SaveChanges();

                            klient.KlienciTreningiGrupowe.Remove(trening);
                            db.SaveChanges();

                            zajecia.KlientZajeciaGrupowe.Remove(trening);
                            db.SaveChanges();

                            return RedirectToAction("Index", new { akcja = AkcjaZapisEnum.UsunietoZapis });
                        }
                    }
                    
                   
                }
            }
            return HttpNotFound();
        }

        public ActionResult UsunKlientowiMasaz(long id)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Klient")
                {
                    var masaz = db.Masaze.Find(id);
                    db.Masaze.Remove(masaz);
                    db.SaveChanges();
                    return RedirectToAction("Index", "KlientView", new { akcja = AkcjaEnumMasaz.UsunietoMasaz });
                }
            }
            return HttpNotFound();
        }

        public ActionResult UsunKlientowiTreningPersonalny(long id)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Klient")
                {
                    var trP = db.TreningiPersonalne.Find(id);
                    db.TreningiPersonalne.Remove(trP);
                    db.SaveChanges();
                    return RedirectToAction("Index", "KlientView", new { akcja = AkcjaEnumTrening.UsunietoTrening });
                }
            }
            return HttpNotFound();
        }
    }
}