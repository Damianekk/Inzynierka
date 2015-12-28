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
using Silownia.Helpers;
using PagedList;

namespace Silownia.Controllers
{
    public class RecepcjonistaController : Controller
    {
        private SilowniaContext db = new SilowniaContext();

        // GET: Recepcjonista
        public ActionResult Index(string imieNazwisko, string SilowniaID, int page = 1, int pageSize = 10, AkcjaEnumRecepcjonista akcja = AkcjaEnumRecepcjonista.Brak, String info = null)
        {
            //ViewBag.srchImieNazwisko = imieNazwisko;

            ViewBag.SilowniaID = new SelectList(db.Silownie.DistinctBy(a => new { a.Nazwa }), "Nazwa", "Nazwa");
            var recepcjonisci = from Osoby in db.Recepcjonisci.OfType<Recepcjonista>() select Osoby;

            if (!String.IsNullOrEmpty(imieNazwisko))
                foreach (string wyraz in imieNazwisko.Split(' '))
                    recepcjonisci = recepcjonisci.Search(wyraz, i => i.Imie, i => i.Nazwisko);
            
            recepcjonisci = recepcjonisci.Search(SilowniaID, i => i.Silownia.Nazwa);

            var final = recepcjonisci.OrderBy(p => p.Imie);
            var ileWynikow = recepcjonisci.Count();
            if ((ileWynikow / page) <= 1)
            {
                page = 1;
            }
            var kk = ileWynikow / page;

            PagedList<Recepcjonista> model = new PagedList<Recepcjonista>(final, page, pageSize);

            if (akcja != AkcjaEnumRecepcjonista.Brak)
            {
                ViewBag.info = info;
                ViewBag.Akcja = akcja;
            }

            return View(model);
        }

        // GET: Recepcjonista/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recepcjonista recepcjonista = db.Recepcjonisci.Find(id);

            if (recepcjonista == null)
            {
                return HttpNotFound();
            }
            return View(recepcjonista);
        }

        // GET: Recepcjonista/Create
        public ActionResult Create()
        {
            ViewBag.SilowniaID = new SelectList(db.Silownie, "SilowniaID", "Nazwa");
            return View();
        }

        // POST: Recepcjonista/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OsobaID,Imie,Nazwisko,DataUrodzenia,Pesel,NrTelefonu,DataZatrudnienia,Pensja,SilowniaID")] Recepcjonista recepcjonista)
        {
            if (ModelState.IsValid)
            {
                db.Recepcjonisci.Add(recepcjonista);
                db.SaveChanges();
                return RedirectToAction("Index", new { akcja = AkcjaEnumRecepcjonista.DodanoRecepcjoniste, info = recepcjonista.imieNazwisko });
            }

            ViewBag.SilowniaID = new SelectList(db.Silownie, "SilowniaID", "Nazwa");
            return View(recepcjonista);
        }

        // GET: Recepcjonista/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recepcjonista recepcjonista = db.Recepcjonisci.Find(id);
            if (recepcjonista == null)
            {
                return HttpNotFound();
            }
            ViewBag.SilowniaID = new SelectList(db.Silownie, "SilowniaID", "Nazwa");
            return View(recepcjonista);
        }

        // POST: Recepcjonista/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OsobaID,Imie,Nazwisko,DataUrodzenia,Pesel,NrTelefonu,DataZatrudnienia,Pensja,SilowniaID")] Recepcjonista recepcjonista)
        {
            if (ModelState.IsValid)
            {
                db.Entry(recepcjonista).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SilowniaID = new SelectList(db.Silownie, "SilowniaID", "Nazwa");
            return View(recepcjonista);
        }

        // GET: Recepcjonista/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recepcjonista recepcjonista = db.Recepcjonisci.Find(id);
            if (recepcjonista == null)
            {
                return HttpNotFound();
            }
            return View(recepcjonista);
        }

        // POST: Recepcjonista/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Recepcjonista recepcjonista = db.Recepcjonisci.Find(id);
            db.Recepcjonisci.Remove(recepcjonista);
            db.SaveChanges();
            return RedirectToAction("Index", new { akcja = AkcjaEnumRecepcjonista.UsunietoRecepcjoniste, info = recepcjonista.imieNazwisko });
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
