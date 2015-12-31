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
         //   if (Session["User"] != null)
            {
                ViewBag.SilowniaID = new SelectList(db.Silownie.DistinctBy(a => new { a.Nazwa }), "Nazwa", "Nazwa");
                ViewBag.SpecjalizacjaID = new SelectList(db.Specjalizacje.DistinctBy(a => new { a.Nazwa }), "Nazwa", "Nazwa");

                var osoby = from Osoby in db.Trenerzy select Osoby;

                if (!String.IsNullOrEmpty(imieNazwisko))
                    foreach (string wyraz in imieNazwisko.Split(' '))
                        osoby = osoby.Search(wyraz, i => i.Imie, i => i.Nazwisko);
                osoby = osoby.Search(SpecjalizacjaID, i => i.Specjalizacja.Nazwa);
                osoby = osoby.Search(SilowniaID, i => i.Silownia.Nazwa);

                var final = osoby.OrderBy(p => p.Nazwisko);
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
         //   return HttpNotFound();
        }

        // GET: /Trener/Details/5
        public ActionResult Details(long? id)
        {
         //   if (Session["User"] != null)
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
         //   return HttpNotFound();
        }

        // GET: /Trener/Create
        public ActionResult Create()
        {
          //  if (Session["User"] != null)
            {
                ViewBag.SpecjalizacjaID = new SelectList(db.Specjalizacje, "SpecjalizacjaID", "Nazwa");
                ViewBag.SilowniaID = new SelectList(db.Silownie, "SilowniaID", "Nazwa");
                return View();
            }
         //   return HttpNotFound();
        }

        // POST: /Trener/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OsobaID,Imie,Nazwisko,DataUrodzenia,DataZatrudnienia,Pensja,SilowniaID,SpecjalizacjaID,StawkaGodzinowa")] Trener trener)
        {
         //   if (Session["User"] != null)
            {
                if (ModelState.IsValid)
                {
                    trener.DataZatrudnienia = DateTime.Now;
                    db.Trenerzy.Add(trener);
                    db.SaveChanges();

                    Uzytkownik pracownik = new Uzytkownik();
                    pracownik.IDOsoby = trener.OsobaID;
                    pracownik.Login = trener.Nazwisko;
                    pracownik.Haslo = trener.Imie + trener.Nazwisko;
                    pracownik.Rola = "Trener";
                    db.Uzytkownicy.Add(pracownik);
                    db.SaveChanges();

                    return RedirectToAction("Index", new { akcja = AkcjaEnumTrener.DodanoTrenera, info = trener.imieNazwisko });
                }

                ViewBag.SpecjalizacjaID = new SelectList(db.Specjalizacje, "SpecjalizacjaID", "Nazwa", trener.Specjalizacja);
                ViewBag.SilowniaID = new SelectList(db.Silownie, "SilowniaID", "Nazwa");

                
                return View(trener);
            }
         //   return HttpNotFound();
        }

        // GET: /Trener/Edit/5
        public ActionResult Edit(long? id)
        {
         //   if (Session["User"] != null)
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
        //    return HttpNotFound();
        }

        // POST: /Trener/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OsobaID,Imie,Nazwisko,DataUrodzenia,DataZatrudnienia,Pensja,SilowniaID,SpecjalizacjaID,StawkaGodzinowa")] Trener trener)
        {
         //   if (Session["User"] != null)
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
        //    return HttpNotFound();
        }

        // GET: /Trener/Delete/5
        public ActionResult Delete(long? id)
        {
         //   if (Session["User"] != null)
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
        //    return HttpNotFound();
        }

        // POST: /Trener/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
         //   if (Session["User"] != null)
            {
                Trener trener = db.Trenerzy.Find(id);
                db.Trenerzy.Remove(trener);
                db.SaveChanges();
                return RedirectToAction("Index", new { akcja = AkcjaEnumTrener.UsunietoTrenera, info = trener.imieNazwisko });
            }
         //   return HttpNotFound();
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
