using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Silownia.Models;
using Silownia.DAL;

namespace Silownia.Controllers
{
    public class ZajeciaController : Controller
    {
        private SilowniaContext db = new SilowniaContext();

        // GET: /Zajecia/
        public ActionResult Index()
        {
            return View(db.Treningi.OfType<TreningPersonalny>().ToList());
        }

        // GET: /Zajecia/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trening trening = db.Treningi.Find(id);
            if (trening == null)
            {
                return HttpNotFound();
            }
            return View(trening);
        }

        // GET: /Zajecia/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Zajecia/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="TreningID,Tytul,Poczatek,Koniec,OpisTreningu")] Trening trening)
        {
            if (ModelState.IsValid)
            {
                db.Treningi.Add(trening);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(trening);
        }

        // GET: /Zajecia/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trening trening = db.Treningi.Find(id);
            if (trening == null)
            {
                return HttpNotFound();
            }
            return View(trening);
        }

        // POST: /Zajecia/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="TreningID,Tytul,Poczatek,Koniec,OpisTreningu")] Trening trening)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trening).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(trening);
        }

        // GET: /Zajecia/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trening trening = db.Treningi.Find(id);
            if (trening == null)
            {
                return HttpNotFound();
            }
            return View(trening);
        }

        // POST: /Zajecia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Trening trening = db.Treningi.Find(id);
            db.Treningi.Remove(trening);
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
