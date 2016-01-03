using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Silownia.DAL;
using Silownia.Models;
using PagedList;
using Silownia.Helpers;
using System.Globalization;
using Microsoft.AspNet.Identity;

namespace Silownia.Controllers
{
    public class MasazController : Controller
    {
        private SilowniaContext db = new SilowniaContext();

        // GET: Masaz
        public ActionResult Index(string imieNazwisko, string SilowniaID, string MasazystaID, int page = 1, int pageSize = 10, AkcjaEnumMasaz akcja = AkcjaEnumMasaz.Brak, String info = null)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" | Session["Auth"].ToString() == "Administrator")
                {
                    ViewBag.SilowniaID = new SelectList(db.Silownie.DistinctBy(a => new { a.Nazwa }), "Nazwa", "Nazwa");
                    ViewBag.MasazystaID = new SelectList(db.Masazysci.DistinctBy(a => new { a.Pesel }), "imieNazwisko", "imieNazwisko");

                    var masaze = from Masaze in db.Masaze select Masaze;

                    if (!String.IsNullOrEmpty(imieNazwisko))
                        foreach (string wyraz in imieNazwisko.Split(' '))
                            masaze = masaze.Search(wyraz, i => i.Klient.Imie, i => i.Klient.Nazwisko);

                    masaze = masaze.Search(SilowniaID, i => i.Masazysta.Silownia.Nazwa);

                    if (!String.IsNullOrEmpty(MasazystaID))
                        foreach (string wyraz in MasazystaID.Split(' '))
                            masaze = masaze.Search(wyraz, i => i.Masazysta.Imie, i => i.Masazysta.Nazwisko);

                    //previous solution
                    // masaze = masaze.Search(imieNazwisko, i => i.Klient.Imie, i => i.Klient.Nazwisko);
                    // masaze = masaze.Search(MasazystaID, i => i.Masazysta.Imie, i => i.Masazysta.Nazwisko);

                    var final = masaze.OrderBy(p => p.Klient.Nazwisko);
                    var ileWynikow = masaze.Count();
                    if ((ileWynikow / page) <= 1)
                    {
                        page = 1;
                    }
                    var kk = ileWynikow / page;

                    PagedList<Masaz> model = new PagedList<Masaz>(final, page, pageSize);

                    if (akcja != AkcjaEnumMasaz.Brak)
                    {
                        ViewBag.info = info;
                        ViewBag.Akcja = akcja;
                    }

                    return View(model);
                }
            }
              return HttpNotFound();
        }

        // GET: Masaz/Details/5
        public ActionResult Details(long? id)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" | Session["Auth"].ToString() == "Administrator")
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Masaz masaz = db.Masaze.Find(id);
                    if (masaz == null)
                    {
                        return HttpNotFound();
                    }
                    return View(masaz);
                }
            }
             return HttpNotFound();
        }

        // GET: Masaz/Create
        public ActionResult Create(long? id)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" | Session["Auth"].ToString() == "Administrator")
                {
                    ViewBag.MasazystaID = new SelectList(db.Masazysci, "OsobaID", "imieNazwisko");
                    var a = from Osoby in db.Masazysci select Osoby;

                    Masazysta masazysta = null;
                    var user = User.Identity.GetUserName();
                    foreach (Masazysta mas in a)
                    {
                        if (mas.imieNazwisko.Replace(" ", "") == user)
                        {
                            masazysta = mas;
                            break;
                        }

                        Osoba osoba = db.Osoby.Find(id);
                        ViewBag.Osoba = osoba;
                    }

                    return View();
                }
            }
             return HttpNotFound();
        }

        // POST: Masaz/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "MasazID,MasazystaID,DataMasazu,MasazStart,CzasTrwania")] long? id, Masaz masaz)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" | Session["Auth"].ToString() == "Administrator")
                {
                    ViewBag.MasazystaID = new SelectList(db.Masazysci, "OsobaID", "imieNazwisko", masaz.MasazystaID);

                    if (ModelState.IsValid && !aktywnyMasaz(id, masaz.DataMasazu) && !zajetyMasazysta(masaz.MasazystaID, masaz.DataMasazu))
                    {
                        #region Klient
                        Klient klient = db.Klienci.Find(id);
                        masaz.Klient = klient;
                        klient.Masaze.Add(masaz);
                        #endregion

                        #region Masazysta
                        Masazysta masazysta = db.Masazysci.Find(masaz.MasazystaID);
                        masaz.Masazysta = masazysta;
                        masazysta.Masaze.Add(masaz);
                        #endregion

                        masaz.DataMasazu = masaz.DataMasazu.AddHours(System.Convert.ToDouble(masaz.MasazStart.Hour));
                        masaz.DataMasazu = masaz.DataMasazu.AddMinutes(System.Convert.ToDouble(masaz.MasazStart.Minute));
                        masaz.DataMasazuKoniec = masaz.DataMasazu.AddMinutes(System.Convert.ToDouble(masaz.CzasTrwania));
                        masaz.kosztMasazu = (masaz.CzasTrwania * masaz.Masazysta.StawkaGodzinowa) / 60;



                        db.Masaze.Add(masaz);
                        db.SaveChanges();

                        return RedirectToAction("Index", new { akcja = AkcjaEnumMasaz.DodanoMasaz, info = klient.imieNazwisko });
                    }
                    return View(masaz);
                }
            }
             return HttpNotFound();

        }


        bool aktywnyMasaz(long? klientID, DateTime dataOd)
        {
            var checkMasaz = db.Masaze.Where(o => o.Klient.OsobaID == klientID && dataOd >= o.DataMasazu && dataOd <= o.DataMasazuKoniec).ToList();
            var checkTrening = db.TreningiPersonalne.Where(o => o.Klient.OsobaID == klientID && dataOd >= o.TreningStart && dataOd <= o.TreningKoniec).ToList();
            if (checkMasaz.Count == 1)
            {
                TempData["msg"] = "<script>alert('Klient ma już umówiony masaż w tym terminie');</script>";
                return true; // klient ma masaż w tym terminie
            }
            else if (checkTrening.Count == 1)
            {
                TempData["msg"] = "<script>alert('Klient ma już umówiony trening w tym terminie');</script>";
                return true; // klient ma trening w tym terminie
            }
            else return false; // klient nie ma masażu w terminie
        }

        bool zajetyMasazysta(long? MasazystaID, DateTime dataOd)
        {
            var check = db.Masaze.Where(o => o.Masazysta.OsobaID == MasazystaID && dataOd >= o.DataMasazu && dataOd <= o.DataMasazuKoniec).ToList();

            if (check.Count == 1)
            {
                TempData["msg"] = "<script>alert('Masażysta ma już zaplanowany masaż w tym terminie.');</script>";
                return true; // masażysta ma masaż w tym terminie
            }
            else return false; // masażysta nie ma masażu w terminie
        }

        // GET: Masaz/Edit/5
        public ActionResult Edit(long? id)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" | Session["Auth"].ToString() == "Administrator")
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Masaz masaz = db.Masaze.Find(id);
                    if (masaz == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.MasazystaID = new SelectList(db.Masazysci, "OsobaID", "imieNazwisko", masaz.MasazystaID);
                    return View(masaz);
                }
            }
              return HttpNotFound();
        }

        // POST: Masaz/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "MasazID,MasazystaID,DataMasazu,DataMasazu,CzasTrwania")] Masaz masaz)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" | Session["Auth"].ToString() == "Administrator")
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(masaz).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    ViewBag.MasazystaID = new SelectList(db.Masazysci, "OsobaID", "imieNazwisko", masaz.MasazystaID);
                    return View(masaz);
                }
            }
              return HttpNotFound();
        }

        // GET: Masaz/Delete/5
        public ActionResult Delete(long? id)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" | Session["Auth"].ToString() == "Administrator")
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Masaz masaz = db.Masaze.Find(id);
                    if (masaz == null)
                    {
                        return HttpNotFound();
                    }
                    return View(masaz);
                }
            }
              return HttpNotFound();
        }

        // POST: Masaz/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" | Session["Auth"].ToString() == "Administrator")
                {
                    Masaz masaz = db.Masaze.Find(id);
                    db.Masaze.Remove(masaz);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { akcja = AkcjaEnumMasaz.UsunietoMasaz });
                }
            }
              return HttpNotFound();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
