using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Silownia.Models;
using Silownia.DAL;
using System;
using Microsoft.AspNet.Identity;
using System.Globalization;
using PagedList;
using Silownia.Helpers;
using System.Collections.Generic;

namespace Silownia.Controllers
{
    public class UmowaController : Controller
    {
        private SilowniaContext db = new SilowniaContext();

        // GET: /Umowa/
        public ActionResult Index(string imieNazwisko, string SilowniaID, int page = 1, int pageSize = 10, AkcjaEnumUmowa akcja = AkcjaEnumUmowa.Brak, String info = null)
        {
          //  ViewBag.srchImieNazwisko = imieNazwisko;

            ViewBag.SilowniaID = new SelectList(db.Silownie.DistinctBy(a => new { a.Nazwa }), "Nazwa", "Nazwa");
     
            var umowy = from Umowy in db.Umowy select Umowy;

            umowy = umowy.Search(imieNazwisko, i => i.Klient.Imie, i => i.Klient.Nazwisko);
            umowy = umowy.Search(SilowniaID, i => i.Silownia.Nazwa);

            var final = umowy.OrderBy(p => p.Klient.Imie);
            var ileWynikow = umowy.Count();
            if ((ileWynikow / page) <= 1)
            {
                page = 1;
            }
            var kk = ileWynikow / page;


            PagedList<Umowa> model = new PagedList<Umowa>(final, page, pageSize);
            return View(model);
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
        public ActionResult Create(long? id)
        {

            ViewBag.SilowniaID = new SelectList(db.Silownie, "SilowniaID", "Nazwa");
            ViewBag.RecepcjonistaID = new SelectList(db.Recepcjonisci, "OsobaID", "imieNazwisko");
            var a = from Osoby in db.Recepcjonisci select Osoby;

            Recepcjonista recepcjonista = null;
            var user = User.Identity.GetUserName();
            foreach (Recepcjonista rec in a)
            {
                if (rec.imieNazwisko.Replace(" ", "") == user)
                {
                    recepcjonista = rec;
                    break;
                }

                Osoba osoba = db.Osoby.Find(id);
                ViewBag.Osoba = osoba;
            }

            return View(new Umowa // W ten sposób tworze obiekt nadaje aktualny czas / przypisuje do Daty podpisania umowy i zwracam widok z datą (teraz)
            {
                DataPodpisania = DateTime.Now,
                // tu przydałoby się dodać datę now + miesiąc
                DataZakonczenia = (DateTime.Now).AddMonths(1),
                
                // ,Recepcjonista = recepcjonista 
            });
        }

        // POST: /Umowa/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // public ActionResult Create([Bind(Include= "UmowaID,SilowniaID,DataPodpisania,DataZakonczenia,Cena,RecepcjonistaID")] long? id, Umowa umowa)
        public ActionResult Create([Bind(Include = "UmowaID,DataPodpisania,DataZakonczenia,RecepcjonistaID,Cena")] long? id, Umowa umowa)
        {
            //if (ModelState.IsValid)
            //{
            //    Osoba osoba = db.Osoby.Find(id);
            //    db.Adresy.Add(adres);
            //    osoba.Adres = adres;
            //    adres.Osoba = osoba;
            //    db.SaveChanges();
            //    return RedirectToAction("Index", "Klient");
            //}
            ViewBag.RecepcjonistaID = new SelectList(db.Recepcjonisci, "OsobaID", "imieNazwisko", umowa.RecepcjonistaID);
            ViewBag.SilowniaID = new SelectList(db.Silownie, "SilowniaID", "Nazwa", umowa.SilowniaID);

            if (ModelState.IsValid && !aktywnaUmowa(id, umowa.DataPodpisania, umowa.DataZakonczenia))
            {

                #region Klient
                Klient klient = db.Klienci.Find(id);
                umowa.Klient = klient;
                klient.Umowy.Add(umowa);
                #endregion

                #region Recepcjonista
                Recepcjonista recepcjonista = db.Recepcjonisci.Find(umowa.RecepcjonistaID);
                umowa.Recepcjonista = recepcjonista;
                recepcjonista.Umowy.Add(umowa);
                #endregion

                #region Silownia
                Models.Silownia silownia = db.Silownie.Find(umowa.SilowniaID);
                umowa.Silownia = silownia;
                silownia.Umowy.Add(umowa);
                #endregion
                db.Umowy.Add(umowa);
                db.SaveChanges();
                return RedirectToAction("Index", new { akcja = AkcjaEnumUmowa.DodanoUmowe, info = klient.imieNazwisko });
            }

            
            return View(umowa);
        }

        bool aktywnaUmowa(long? klientID, DateTime umowaOd, DateTime umowaDo)
        {
            //var check = from u in db.Umowy
            //            from k in db.Klienci
            //            where u.Klient.OsobaID == klientID && (u.DataPodpisania >= umowaOd.Date && u.DataZakonczenia.Date <= umowaDo)
            //            select u;

            var check = db.Umowy.Where(o => o.Klient.OsobaID == klientID && DbFunctions.TruncateTime(o.DataPodpisania) <= umowaOd.Date && DbFunctions.TruncateTime(o.DataZakonczenia) >= umowaDo.Date).ToList();

            if (check.Count == 1)
            {
                TempData["msg"] = "<script>alert('Klient posiada umowe w wybranym terminie');</script>";
                return true; // klient ma umowe w tym terminie
            }
            else return false; // klient nie ma umowy
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
        public ActionResult Edit([Bind(Include = "UmowaID,SilowniaID,DataPodpisania,DataZakonczenia,Cena")] Umowa umowa)
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
            return RedirectToAction("Index", new { akcja = AkcjaEnumUmowa.UsunietoUmowe });
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
