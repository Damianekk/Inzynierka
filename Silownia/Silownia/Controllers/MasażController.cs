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
using Kendo.Mvc.UI;

namespace Silownia.Controllers
{
    public class MasażController : Controller
    {
        private SilowniaContext db = new SilowniaContext();

        // GET: /Masaż/
       
        public ActionResult Index()
        {
            //var masaze = from sal in db.Masazysci
            //             join mas in db.Masaze on sal.OsobaID equals mas.MasazID
            //             select new { Masaz = mas.MasazID, Sala = sal.Masaze
                return View();
            
        }
        public JsonResult GetMasaz()
        {
            return Json(db.Masaze
                            .Select(m => new { ID = m.MasazID, typ = m.Typ })
                            .OrderBy(ord => ord.typ)
                            .ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMasazyste([DataSourceRequest] DataSourceRequest request, int? masazID)
        {
            var masazysta = db.Masazysci.OrderBy(o => o.imieNazwisko).AsQueryable();
            
            if (masazID.HasValue)
            {
                masazysta = masazysta.Where(m => m.OsobaID == masazID);
            }

            var listaMasazy = masazysta.Select(p => new Masazysta
            {
               Nazwisko = p.Nazwisko,
               Imie = p.Imie,
               NrTelefonu = p.NrTelefonu,
               Masaze = p.Masaze
            });

            return Json(listaMasazy.ToList(), JsonRequestBehavior.AllowGet);
        }
       



















        // GET: /Masaż/Details/5
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

        // GET: /Masaż/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: /Masaż/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="MasazID,DataMasazu,CzasTrwania")] Masaz masaz)
        {
            if (ModelState.IsValid)
            {
                db.Masaze.Add(masaz);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(masaz);
        }

        // GET: /Masaż/Edit/5
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
            return View(masaz);
        }

        // POST: /Masaż/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="MasazID,DataMasazu,CzasTrwania")] Masaz masaz)
        {
            if (ModelState.IsValid)
            {
                db.Entry(masaz).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(masaz);
        }

        // GET: /Masaż/Delete/5
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

        // POST: /Masaż/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Masaz masaz = db.Masaze.Find(id);
            db.Masaze.Remove(masaz);
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
