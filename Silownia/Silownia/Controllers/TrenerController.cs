using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Silownia.Models;
using Silownia.DAL;
using PagedList;
using Silownia.Helpers;
using System.Collections.Generic;

namespace Silownia.Controllers
{
    public class TrenerController : Controller
    {
        private SilowniaContext db = new SilowniaContext();

        // GET: /Trener/
        public ActionResult Index(string imieNazwisko, string SilowniaID, string SpecjalizacjaID, int page = 1, int pageSize = 10, AkcjaEnumTrener akcja = AkcjaEnumTrener.Brak, String info = null)
        {
            //ViewBag.srchImieNazwisko = imieNazwisko;

            ViewBag.SilowniaID = new SelectList(db.Silownie.DistinctBy(a => new { a.Nazwa }), "Nazwa", "Nazwa");
            ViewBag.SpecjalizacjaID = new SelectList(db.Specjalizacje.DistinctBy(a => new { a.Nazwa }), "Nazwa", "Nazwa");
      
            var osoby = from Osoby in db.Trenerzy select Osoby;

            if (!String.IsNullOrEmpty(imieNazwisko))
                foreach (string wyraz in imieNazwisko.Split(' '))
                    osoby = osoby.Search(wyraz, i => i.Imie, i => i.Nazwisko);
            osoby = osoby.Search(SpecjalizacjaID, i => i.Specjalizacja.Nazwa);
            osoby = osoby.Search(SilowniaID, i => i.Silownia.Nazwa);

            var final = osoby.OrderBy(p => p.Imie);
            var ileWynikow = osoby.Count();
            if ((ileWynikow / page) <= 1)
            {
                page = 1;
            }
            var kk = ileWynikow / page;

            PagedList<Trener> model = new PagedList<Trener>(final, page, pageSize);

            if (akcja != AkcjaEnumTrener.Brak)
            {
                ViewBag.info = info;
                ViewBag.Akcja = akcja;
            }

            return View(model);
        }

        // GET: /Trener/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trener trener = db.Trenerzy.Find(id);
            if (trener == null)
            {
                return HttpNotFound();
            }
            return View(trener);
        }

        // GET: /Trener/Create
        public ActionResult Create()
        {
            ViewBag.SpecjalizacjaID = new SelectList(db.Specjalizacje, "SpecjalizacjaID", "Nazwa");
            ViewBag.SilowniaID = new SelectList(db.Silownie, "SilowniaID", "Nazwa");
            return View();
        }

        // POST: /Trener/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OsobaID,Imie,Nazwisko,DataUrodzenia,DataZatrudnienia,Pensja,SilowniaID,SpecjalizacjaID,StawkaGodzinowa")] Trener trener)
        {
            if (ModelState.IsValid)
            {
                db.Trenerzy.Add(trener);
                db.SaveChanges();
                return RedirectToAction("Index", new { akcja = AkcjaEnumTrener.DodanoTrenera, info = trener.imieNazwisko });
            }

            ViewBag.SpecjalizacjaID = new SelectList(db.Specjalizacje, "SpecjalizacjaID", "Nazwa", trener.Specjalizacja);
            ViewBag.SilowniaID = new SelectList(db.Silownie, "SilowniaID", "Nazwa");
            return View(new Trener
            {
                DataZatrudnienia = DateTime.Now
            });
        }

        // GET: /Trener/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trener trener = db.Trenerzy.Find(id);
            if (trener == null)
            {
                return HttpNotFound();
            }
            ViewBag.SpecjalizacjaID = new SelectList(db.Specjalizacje, "SpecjalizacjaID", "Nazwa", trener.Specjalizacja);
            ViewBag.SilowniaID = new SelectList(db.Silownie, "SilowniaID", "Nazwa", trener.Silownia);
            return View(trener);
        }

        // POST: /Trener/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OsobaID,Imie,Nazwisko,DataUrodzenia,DataZatrudnienia,Pensja,SilowniaID,SpecjalizacjaID,StawkaGodzinowa")] Trener trener)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trener).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SpecjalizacjaID = new SelectList(db.Specjalizacje, "SpecjalizacjaID", "Nazwa", trener.Specjalizacja);
            ViewBag.SilowniaID = new SelectList(db.Silownie, "SilowniaID", "Nazwa", trener.Silownia);
            return View(trener);
        }

        // GET: /Trener/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trener trener = db.Trenerzy.Find(id);
            if (trener == null)
            {
                return HttpNotFound();
            }
            return View(trener);
        }

        // POST: /Trener/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Trener trener = db.Trenerzy.Find(id);
            db.Trenerzy.Remove(trener);
            db.SaveChanges();
            return RedirectToAction("Index", new { akcja = AkcjaEnumTrener.UsunietoTrenera, info = trener.imieNazwisko });
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
