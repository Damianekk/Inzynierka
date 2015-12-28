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

namespace Silownia.Controllers
{
    public class MasazystaController : Controller
    {
        private SilowniaContext db = new SilowniaContext();

        // GET: Masazysta

        public ActionResult Index(string imieNazwisko, string SilowniaID, int page = 1, int pageSize = 10, AkcjaEnumMasazysta akcja = AkcjaEnumMasazysta.Brak, String info = null)
        {
            ViewBag.srchImieNazwisko = imieNazwisko;

            ViewBag.SilowniaID = new SelectList(db.Silownie.DistinctBy(a => new { a.Nazwa }), "Nazwa", "Nazwa");

            var osoby = from Osoby in db.Masazysci select Osoby;

            osoby = osoby.Search(imieNazwisko, i => i.Imie, i => i.Nazwisko);
            osoby = osoby.Search(SilowniaID, i => i.Silownia.Nazwa);

            var final = osoby.OrderBy(p => p.Imie);
            var ileWynikow = osoby.Count();
            if ((ileWynikow / page) <= 1)
            {
                page = 1;
            }
            var kk = ileWynikow / page;

            PagedList<Masazysta> model = new PagedList<Masazysta>(final, page, pageSize);

            if (akcja != AkcjaEnumMasazysta.Brak)
            {
                ViewBag.info = info;
                ViewBag.Akcja = akcja;
            }

            return View(model);
        }


        // GET: Masazysta/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Masazysta masazysta = db.Masazysci.Find(id);
            if (masazysta == null)
            {
                return HttpNotFound();
            }
            return View(masazysta);
        }

        // GET: Masazysta/Create
        public ActionResult Create()
        {
            ViewBag.SilowniaID = new SelectList(db.Silownie, "SilowniaID", "Nazwa");
            return View();
        }

        // POST: Masazysta/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OsobaID,Imie,Nazwisko,DataUrodzenia,NrTelefonu,Pesel,DataZatrudnienia,Pensja,StawkaGodzinowa,SilowniaID")] Masazysta masazysta)
        {
            if (ModelState.IsValid)
            {
                db.Osoby.Add(masazysta);
                db.SaveChanges();
                return RedirectToAction("Index", new { akcja = AkcjaEnumMasazysta.DodanoMasazyste, info = masazysta.imieNazwisko });
            }

            ViewBag.SilowniaID = new SelectList(db.Silownie, "SilowniaID", "Nazwa", masazysta.SilowniaID);
            return View(new Masazysta
            {
                DataZatrudnienia = DateTime.Now
            }
            );
        }

        // GET: Masazysta/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Masazysta masazysta = db.Masazysci.Find(id);
            if (masazysta == null)
            {
                return HttpNotFound();
            }
            ViewBag.SilowniaID = new SelectList(db.Silownie, "SilowniaID", "Nazwa", masazysta.Silownia);
            return View(masazysta);
        }

        // POST: Masazysta/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OsobaID,Imie,Nazwisko,DataUrodzenia,NrTelefonu,Pesel,DataZatrudnienia,Pensja,StawkaGodzinowa,SilowniaID")] Masazysta masazysta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(masazysta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SilowniaID = new SelectList(db.Silownie, "SilowniaID", "Nazwa", masazysta.SilowniaID);
            return View(masazysta);
        }

        // GET: Masazysta/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Masazysta masazysta = db.Masazysci.Find(id);
            if (masazysta == null)
            {
                return HttpNotFound();
            }
            return View(masazysta);
        }

        // POST: Masazysta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Masazysta masazysta = db.Masazysci.Find(id);
            db.Osoby.Remove(masazysta);
            db.SaveChanges();
            return RedirectToAction("Index", new { akcja = AkcjaEnumMasazysta.UsunietoMasazyste, info = masazysta.imieNazwisko });
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
