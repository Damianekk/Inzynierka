﻿using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Silownia.Models;
using Silownia.DAL;
using GoogleMaps.LocationServices;

namespace Silownia.Controllers
{
    public class AdresController : Controller
    {

        private SilowniaContext db = new SilowniaContext();
        KomuAdres komuPrzypisac;

        // GET: /Adres/
        public ActionResult Index()
        {
            if (Session["USer"] != null)
            {
                return View(db.Adresy.ToList());
            }
            return HttpNotFound();
        }

        // GET: /Adres/Details/5
        public ActionResult Details(long? id)
        {
            if (Session["User"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Adres adres = db.Adresy.Find(id);
                if (adres == null)
                {
                    return HttpNotFound();
                }
                return View(adres);
            }
            return HttpNotFound();
        }

        // GET: /Adres/Create
        public ActionResult Create(long? id, KomuAdres komu)
        {
            if (Session["User"] != null)
            {
                if (id != null)
                {
                    switch (komu)
                    {
                        case KomuAdres.Osoba:
                            Osoba osoba = db.Osoby.Find(id);
                            ViewBag.Osoba = osoba;
                            komuPrzypisac = komu;
                            break;
                        case KomuAdres.Silownia:
                            Silownia.Models.Silownia silownia = db.Silownie.Find(id);
                            ViewBag.Silownia = silownia;
                            komuPrzypisac = komu;
                            break;
                    }
                }
                return View();
            }
            return HttpNotFound();
        }

        // POST: /Adres/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AdresID,KodPocztowy,Kraj,Miasto,Ulica,NrBudynku,NrLokalu")] Adres adres, long? id, KomuAdres komu)
        {
            if (Session["User"] != null)
            {
                object redirectTo = null;
                if (ModelState.IsValid)
                {
                    switch (komu)
                    {
                        case KomuAdres.Osoba:
                            Osoba osoba = db.Osoby.Find(id);
                            redirectTo = osoba.GetType().BaseType.Name;
                            osoba.Adres = adres;

                            //adres.Osoba = osoba;
                            break;
                        case KomuAdres.Silownia:
                            Silownia.Models.Silownia silownia = db.Silownie.Find(id);
                            silownia.Adres = adres;
                            var locationService = new GoogleLocationService();
                            AddressData adr = new AddressData();
                            adr.Country = adres.Kraj;
                            adr.City = adres.Miasto;
                            adr.Zip = adres.KodPocztowy;
                            adr.Address = adres.Ulica + " " + adres.NrBudynku + " " + adres.NrLokalu;
                            var point = locationService.GetLatLongFromAddress(adr);
                            silownia.Szerokosc = point.Longitude;
                            silownia.Dlugosc = point.Latitude;
                            redirectTo = silownia.GetType().BaseType.Name;
                            break;
                    }
                    db.Adresy.Add(adres);
                    db.SaveChanges();
                    return RedirectToAction("Index", redirectTo.ToString());
                }

                return View(adres);
            }
            return HttpNotFound();
        }

        // GET: /Adres/Edit/5
        public ActionResult Edit(long? id)
        {
            if (Session["User"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Adres adres = db.Adresy.Find(id);
                if (adres == null)
                {
                    return HttpNotFound();
                }
                return View(adres);
            }
            return HttpNotFound();
        }

        // POST: /Adres/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AdresID,KodPocztowy,Kraj,Miasto,Ulica,NrBudynku,NrLokalu")] Adres adres)
        {
            if (Session["User"] != null)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(adres).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(adres);
            }
            return HttpNotFound();
        }

        // GET: /Adres/Delete/5
        public ActionResult Delete(long? id)
        {
            if (Session["User"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Adres adres = db.Adresy.Find(id);
                if (adres == null)
                {
                    return HttpNotFound();
                }
                return View(adres);
            }
            return HttpNotFound();
        }

        // POST: /Adres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            if (Session["User"] != null)
            {
                Adres adres = db.Adresy.Find(id);
                Silownia.Models.Silownia silownia = db.Silownie.Where(w => w.Adres.AdresID == id).FirstOrDefault();
                Osoba osoba;

                if (silownia != null)
                {
                    silownia.Adres = null;
                }
                else
                {
                    osoba = db.Osoby.Where(o => o.Adres.AdresID == id).FirstOrDefault();
                    if (osoba != null)
                        osoba.Adres = null;
                }

                db.Adresy.Remove(adres);
                db.SaveChanges();
                return RedirectToAction("Index");
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
