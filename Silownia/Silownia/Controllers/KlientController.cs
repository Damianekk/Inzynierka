﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Silownia.Models;
using Silownia.DAL;
using PagedList;

namespace Silownia.Controllers
{
    public class KlientController : Controller
    {
    
        private SilowniaContext db = new SilowniaContext();

        // GET: /Klient/
        public ActionResult Index(string Miasto,string imieNazwisko,int page=1 ,int pageSize = 10)
        {
            ViewBag.srchMiasto = Miasto;
            ViewBag.srchImieNazwisko = imieNazwisko;
       //     Test t = new Test();
            


            var a = from Osoby in db.Osoby.OfType<Klient>() select Osoby;
            if (!String.IsNullOrEmpty(Miasto))
            {
                a =  a.Where(s => s.Adres.Miasto.Contains(Miasto));
            }
            else
            {

            }

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

                PagedList<Klient> model = new PagedList<Klient>(final, page, pageSize);
            
            return View(model);
          //  return View(db.Osoby.ToList());
        }

        // GET: /Klient/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Klient klient = db.Klienci.Find(id);
            if (klient == null)
            {
                return HttpNotFound();
            }
            return View(klient);
        }

        // GET: /Klient/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Klient/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="OsobaID,Imie,Nazwisko,DataUrodzenia,Mail,NrTelefonu,Adres")] Klient klient)
        {
           

            if (ModelState.IsValid)
            {
                db.Osoby.Add(klient);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(klient);
        }

        // GET: /Klient/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Klient klient = db.Klienci.Find(id);
            if (klient == null)
            {
                return HttpNotFound();
            }
            return View(klient);
        }

        // POST: /Klient/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="OsobaID,Imie,Nazwisko,DataUrodzenia,Mail,NrTelefonu")] Klient klient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(klient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(klient);
        }

        // GET: /Klient/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Klient klient = db.Klienci.Find(id);
            if (klient == null)
            {
                return HttpNotFound();
            }
            return View(klient);
        }

        // POST: /Klient/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Klient klient = db.Klienci.Find(id);
            db.Osoby.Remove(klient);
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
