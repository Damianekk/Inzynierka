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

namespace Silownia.Controllers
{
    public class RecepcjonistaController : Controller
    {
        private SilowniaContext db = new SilowniaContext();

        // GET: Recepcjonista
        public ActionResult Index()
        {
            var a = from Osoby in db.Recepcjonisci.OfType<Recepcjonista>() select Osoby;
            return View(a);
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
            return View();
        }

        // POST: Recepcjonista/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OsobaID,Imie,Nazwisko,DataUrodzenia,DataZatrudnienia,Pensja")] Recepcjonista recepcjonista)
        {
            if (ModelState.IsValid)
            {
                db.Recepcjonisci.Add(recepcjonista);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

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
            return View(recepcjonista);
        }

        // POST: Recepcjonista/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OsobaID,Imie,Nazwisko,DataUrodzenia,DataZatrudnienia,Pensja")] Recepcjonista recepcjonista)
        {
            if (ModelState.IsValid)
            {
                db.Entry(recepcjonista).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
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
            return RedirectToAction("Index");
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
