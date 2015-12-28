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
            ViewBag.SilowniaID = new SelectList(db.Silownie.DistinctBy(a => new { a.Nazwa }), "Nazwa", "Nazwa");
            ViewBag.MasazystaID = new SelectList(db.Masazysci.DistinctBy(a => new { a.Pesel }), "imieNazwisko", "imieNazwisko");

            var masaze = from Masaze in db.Masaze select Masaze;

            masaze = masaze.Search(imieNazwisko, i => i.Klient.Imie, i => i.Klient.Nazwisko);
            masaze = masaze.Search(SilowniaID, i => i.Masazysta.Silownia.Nazwa);
            masaze = masaze.Search(MasazystaID, i => i.Masazysta.Imie, i => i.Masazysta.Nazwisko);

            var final = masaze.OrderBy(p => p.Klient.Imie);
            var ileWynikow = masaze.Count();
            if ((ileWynikow / page) <= 1)
            {
                page = 1;
            }
            var kk = ileWynikow / page;

            PagedList<Masaz> model = new PagedList<Masaz>(final, page, pageSize);
            return View(model);
        }

        // GET: Masaz/Details/5
        public ActionResult Details(long? id)
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

        // GET: Masaz/Create
        public ActionResult Create(long? id)
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

        // POST: Masaz/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MasazID,MasazystaID,DataMasazu,CzasTrwania")] long? id, Masaz masaz)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Masaze.Add(masaz);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            ViewBag.MasazystaID = new SelectList(db.Masazysci, "OsobaID", "imieNazwisko", masaz.MasazystaID);

            if (ModelState.IsValid)
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

                db.Masaze.Add(masaz);
                db.SaveChanges();

                return RedirectToAction("Index", new { akcja = AkcjaEnumMasaz.DodanoMasaz, info = klient.imieNazwisko });
            }


            return View(masaz);
        }

        // GET: Masaz/Edit/5
        public ActionResult Edit(long? id)
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

        // POST: Masaz/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MasazID,MasazystaID,DataMasazu,CzasTrwania")] Masaz masaz)
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

        // GET: Masaz/Delete/5
        public ActionResult Delete(long? id)
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

        // POST: Masaz/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Masaz masaz = db.Masaze.Find(id);
            db.Masaze.Remove(masaz);
            db.SaveChanges();
            return RedirectToAction("Index", new { akcja = AkcjaEnumMasaz.UsunietoMasaz });
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
