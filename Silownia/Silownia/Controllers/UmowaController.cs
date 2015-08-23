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
    public class UmowaController : Controller
    {
        private SilowniaContext db = new SilowniaContext();

        // GET: /Umowa/
        public ActionResult Index()
        {
            var umowy = db.Umowy.Include(u => u.Silownia);
            return View(umowy.ToList());
        }

        // GET: /Umowa/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Umowa umowa = db.Umowy.Find(id);
            if (umowa == null)
            {
                return HttpNotFound();
            }
            return View(umowa);
        }

        // GET: /Umowa/Create
        public ActionResult Create()
        {
            ViewBag.SilowniaID = new SelectList(db.Silownie, "SilowniaID", "Nazwa");
            return View();
        }

        // POST: /Umowa/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="UmowaID,SilowniaID,DataPodpisania,DataZakonczenia,Cena")] Umowa umowa)
        {
            if (ModelState.IsValid)
            {
                db.Umowy.Add(umowa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SilowniaID = new SelectList(db.Silownie, "SilowniaID", "Nazwa", umowa.SilowniaID);
            return View(umowa);
        }

        // GET: /Umowa/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Umowa umowa = db.Umowy.Find(id);
            if (umowa == null)
            {
                return HttpNotFound();
            }
            ViewBag.SilowniaID = new SelectList(db.Silownie, "SilowniaID", "Nazwa", umowa.SilowniaID);
            return View(umowa);
        }

        // POST: /Umowa/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="UmowaID,SilowniaID,DataPodpisania,DataZakonczenia,Cena")] Umowa umowa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(umowa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SilowniaID = new SelectList(db.Silownie, "SilowniaID", "Nazwa", umowa.SilowniaID);
            return View(umowa);
        }

        // GET: /Umowa/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Umowa umowa = db.Umowy.Find(id);
            if (umowa == null)
            {
                return HttpNotFound();
            }
            return View(umowa);
        }

        // POST: /Umowa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Umowa umowa = db.Umowy.Find(id);
            db.Umowy.Remove(umowa);
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
