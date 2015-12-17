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

namespace Silownia.Controllers
{
    public class TrenerController : Controller
    {
        private SilowniaContext db = new SilowniaContext();

        // GET: /Trener/
        public ActionResult Index( string imieNazwisko, int page = 1, int pageSize = 10, AkcjaEnumTrener akcja = AkcjaEnumTrener.Brak, String info = null)
        {
           // ViewBag.srchSilownia = Miasto;
            ViewBag.srchImieNazwisko = imieNazwisko;

            var a = from Osoby in db.Trenerzy select Osoby;
           // var b = from Silownie in db.Silownie select Silownie;
            /*
            if (!String.IsNullOrEmpty(Miasto))
            {
                a = a.Where(s => s.Adres.Miasto.Contains(Miasto));
               // b = b.Where(s => s.Nazwa.Contains(Silownia));
            }
            */
            if (!String.IsNullOrEmpty(imieNazwisko))
            {
                a = a.Where(s => s.Imie.Contains(imieNazwisko) || s.Nazwisko.Contains(imieNazwisko));
            }

            var final = a.OrderBy(p => p.Imie);
            var ileWynikow = a.Count();
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
            return View();
        }

        // POST: /Trener/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OsobaID,Imie,Nazwisko,DataUrodzenia,DataZatrudnienia,Pensja,SpecjalizacjaID")] Trener trener)
        {
            if (ModelState.IsValid)
            {
                db.Trenerzy.Add(trener);
                db.SaveChanges();
                return RedirectToAction("Index", new { akcja = AkcjaEnumTrener.DodanoTrenera, info = trener.imieNazwisko });
            }

            ViewBag.SpecjalizacjaID = new SelectList(db.Specjalizacje, "SpecjalizacjaID", "Nazwa", trener.Specjalizacja);
            return View(trener);
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
            return View(trener);
        }

        // POST: /Trener/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OsobaID,Imie,Nazwisko,DataUrodzenia,DataZatrudnienia,Pensja,SpecjalizacjaID")] Trener trener)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trener).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SpecjalizacjaID = new SelectList(db.Specjalizacje, "SpecjalizacjaID", "Nazwa", trener.Specjalizacja);
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
