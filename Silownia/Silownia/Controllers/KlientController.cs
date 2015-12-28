using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Silownia.Models;
using Silownia.DAL;
using PagedList;
using System.Collections.Generic;
 

namespace Silownia.Controllers
{
   

    public class KlientController : Controller
    {
        private SilowniaContext db = new SilowniaContext();


        // GET: /Klient/
 
        public ActionResult Index(string Miasto,string imieNazwisko, bool czyUmowa =false ,int page=1 ,int pageSize = 10 , AkcjaEnum akcja = AkcjaEnum.Brak , String info = null)
        {

            var Miasta = db.Klienci.Where(u => (u.OsobaID != null) && (u.Adres != null)).DistinctBy(a => new { a.Adres.Miasto }).Select(x => x.Adres);


            ViewBag.Miasto = new SelectList(Miasta, "Miasto", "Miasto");
            //ViewBag.srchImieNazwisko = imieNazwisko;   Póki co niech będzie zakomentowane 
            //ViewBag.czyUmowa = czyUmowa;
   
            var osoby = from Osoby in db.Klienci select Osoby;

            osoby = osoby.Search(imieNazwisko, i => i.Imie, i => i.Nazwisko);
            osoby = osoby.Search(Miasto, m => m.Adres.Miasto);
            if(czyUmowa)
            osoby = osoby.Where(u => u.Umowy.Count > 0);
          
            var final = osoby.OrderBy(p => p.Imie);
            var ileWynikow = osoby.Count();
            if ((ileWynikow / page) <= 1)
            {
                page = 1;
            }
            var kk = ileWynikow / page;

                PagedList<Klient> model = new PagedList<Klient>(final, page, pageSize);


            if(akcja != AkcjaEnum.Brak)
            {
                ViewBag.info = info;
                ViewBag.Akcja = akcja;
            }
 
            return View(model);

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
            var z = klient;
            return View(z);
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
        public ActionResult Create([Bind(Include = "OsobaID,Imie,Nazwisko,DataUrodzenia,Mail,NrTelefonu,Adres")] Klient klient)
        {
           

            if (ModelState.IsValid)
            {
                db.Klienci.Add(klient);
                db.SaveChanges();
                return RedirectToAction("Index", new { akcja = AkcjaEnum.DodanoKlienta , info = klient.imieNazwisko });
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
        public ActionResult Edit([Bind(Include = "OsobaID,Imie,Nazwisko,DataUrodzenia,Mail,NrTelefonu")] Klient klient)
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
            db.Klienci.Remove(klient);
            db.SaveChanges();
            return RedirectToAction("Index", new { akcja = AkcjaEnum.UsunietoKlienta, info = klient.imieNazwisko });
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
