﻿using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Silownia.DAL;
using System.Web.Script.Serialization;
using System;
using PagedList;
using Silownia.Models;
using System.Data;
using System.Collections.Generic;

namespace Silownia.Controllers
{
    public class SilowniaController : Controller
    {
        private SilowniaContext db = new SilowniaContext();

        // GET: /Silownia/
        public ActionResult Index(string Miasto, int page = 1, int pageSize = 10, AkcjaEnumSilownia akcja = AkcjaEnumSilownia.Brak, String info = null)
        {
            ViewBag.Miasto = new SelectList(db.Adresy.DistinctBy(a => new { a.Miasto }), "Miasto", "Miasto");

            var silownie = from Silownie in db.Silownie select Silownie;
            silownie = silownie.Search(Miasto, m => m.Adres.Miasto);
         
            var final = silownie.OrderBy(p => p.Nazwa);
            var ileWynikow = silownie.Count();
            if ((ileWynikow / page) <= 1)
            {
                page = 1;
            }
            var kk = ileWynikow / page;

            PagedList<Models.Silownia> model = new PagedList<Models.Silownia>(final, page, pageSize);


            if (akcja != AkcjaEnumSilownia.Brak)
            {
                ViewBag.info = info;
                ViewBag.Akcja = akcja;
            }

            return View(model);
        }

        // GET: /Silownia/Details/5
        public ActionResult Details(long? id, bool? mniejSzczegolow = false, bool? mniejszaMapa = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Models.Silownia silownia = db.Silownie.Find(id);
            ViewBag.mniejSzczegolow = mniejSzczegolow;
            ViewBag.mniejszaMapa = mniejszaMapa;
          
            if (silownia == null)
            {
                return HttpNotFound();
            }
            return View(silownia);
        }

        // GET: /Silownia/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Silownia/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SilowniaID,Nazwa,GodzinaOtwarcia,GodzinaZamkniecia,Powierzchnia,NrTelefonu,DodatkoweInfo")] Models.Silownia silownia)
        {

            if (ModelState.IsValid)
            {               
                db.Silownie.Add(silownia);
                db.SaveChanges();
                return RedirectToAction("Create","Adres",new { id = silownia.SilowniaID, komu = KomuAdres.Silownia });
            }
           


            return View(silownia);
        }

        // GET: /Silownia/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Silownia silownia = db.Silownie.Find(id);

            if (silownia == null)
            {
                return HttpNotFound();
            }
            return View(silownia);
        }

        // POST: /Silownia/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SilowniaID,Nazwa,GodzinaOtwarcia,GodzinaZamkniecia,Powierzchnia,NrTelefonu,DodatkoweInfo")] Models.Silownia silownia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(silownia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(silownia);
        }

        // GET: /Silownia/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Silownia silownia = db.Silownie.Find(id);
            if (silownia == null)
            {
                return HttpNotFound();
            }
            return View(silownia);
        }

        // POST: /Silownia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Models.Silownia silownia = db.Silownie.Find(id);
            db.Silownie.Remove(silownia);
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
        [HttpPost]
        public JsonResult SilowniaInfoJSON()
        {

            var jsonSerialiser = new JavaScriptSerializer();
            var silownie = db.Silownie.Where(s => s.Adres != null).ToList<Silownia.Models.Silownia>();

            //  silownie.RemoveAll(item => item.Adres != null);
            var z = silownie.Select(x => new
                        {
                            dlugosc = x.Dlugosc,
                            szerokosc = x.Szerokosc,
                            godzOtw = x.GodzinaOtwarcia,
                            godzZam = x.GodzinaZamkniecia,
                            nazwa = x.Nazwa,
                            info = x.infoDodatkowe

                        });
            return Json(z);

          //  return Json(new { ok = true, myData = silownie }, JsonRequestBehavior.AllowGet);
        }
    }
}
