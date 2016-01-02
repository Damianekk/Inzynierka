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
    public class SalaController : Controller
    {
        private SilowniaContext db = new SilowniaContext();

        // GET: /Sala/
        public ActionResult Index()
        {
            if(Session["User"] != null)
            {
                return View(db.Sale.ToList());
            }
            return HttpNotFound();
        }

        // GET: /Sala/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["User"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Sala sala = db.Sale.Find(id);
                if (sala == null)
                {
                    return HttpNotFound();
                }
                return View(sala);
            }
            return HttpNotFound();
        }

        // GET: /Sala/Create
        public ActionResult Create()
        {
            if (Session["User"] != null)
            {
                ViewBag.SilowniaID = new SelectList(db.Silownie, "SilowniaID", "Nazwa");
                return View();
            }
            return HttpNotFound();
        }

        // POST: /Sala/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include="Numer_sali,Rodzaj,Status,Opis, SilowniaID")] Sala sala)
        {
            if (Session["User"] != null)
            {
                if (ModelState.IsValid)
                {
                    db.Sale.Add(sala);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.SilowniaID = new SelectList(db.Silownie, "SilowniaID", "Nazwa");
                return View(sala);
            }
            return HttpNotFound();
        }

        // GET: /Sala/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["User"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Sala sala = db.Sale.Find(id);
                if (sala == null)
                {
                    return HttpNotFound();
                }
                ViewBag.SilowniaID = new SelectList(db.Silownie, "SilowniaID", "Nazwa", sala.Silownia);
                return View(sala);
            }
            return HttpNotFound();
        }

        // POST: /Sala/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include="Numer_sali,Rodzaj,Status,Opis")] Sala sala)
        {
            if (Session["User"] != null)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(sala).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.SilowniaID = new SelectList(db.Silownie, "SilowniaID", "Nazwa", sala.Silownia);
                return View(sala);
            }
            return HttpNotFound();
        }

        // GET: /Sala/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["User"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Sala sala = db.Sale.Find(id);
                if (sala == null)
                {
                    return HttpNotFound();
                }
                return View(sala);
            }
            return HttpNotFound();
        }

        // POST: /Sala/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["User"] != null)
            {
                Sala sala = db.Sale.Find(id);
                db.Sale.Remove(sala);
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
