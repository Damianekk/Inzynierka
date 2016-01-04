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
using PagedList;
using Silownia.Helpers;
using System.Globalization;
using Microsoft.AspNet.Identity;
namespace Silownia.Controllers
{
    public class KonserwacjaController : Controller
    {
        private SilowniaContext db = new SilowniaContext();

        // GET: /Konserwacja/
        public ActionResult Index(string imieNazwisko, string SilowniaID, string MasazystaID, int page = 1, int pageSize = 10, AkcjaEnumMasaz akcja = AkcjaEnumMasaz.Brak, String info = null)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" | Session["Auth"].ToString() == "Administrator")
                {
                    ViewBag.SilowniaID = new SelectList(db.Silownie.DistinctBy(a => new { a.Nazwa }), "Nazwa", "Nazwa");
                    ViewBag.SalaID = new SelectList(db.Sale.DistinctBy(a => new { a.Numer_sali }), "Numer_sali", "Rodzaj");
                    ViewBag.SprzetID = new SelectList(db.Sprzety.DistinctBy(a => new { a.SprzetID }), "SprzetID", "Nazwa");
                    ViewBag.KonserwatorID = new SelectList(db.Konserwatorzy.DistinctBy(a => new { a.OsobaID }), "OsobaID", "imieNazwisko");

                    var kons = from Konserwacje in db.Konserwacje select Konserwacje;

                    if (!String.IsNullOrEmpty(imieNazwisko))
                        foreach (string wyraz in imieNazwisko.Split(' '))
                            kons = kons.Search(wyraz, i => i.Konserwator.Imie, i => i.Konserwator.Nazwisko);

                    var final = kons.OrderBy(p => p.Konserwator.Nazwisko);
                    var ileWynikow = kons.Count();
                    if ((ileWynikow / page) <= 1)
                    {
                        page = 1;
                    }
                    var kk = ileWynikow / page;

                    PagedList<Konserwacja> model = new PagedList<Konserwacja>(final, page, pageSize);

                   

                    return View(model);
                }
            }
            return HttpNotFound();
        }

        // GET: Masaz/Details/5
        public ActionResult Details(long? id)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" | Session["Auth"].ToString() == "Administrator")
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Konserwacja konse = db.Konserwacje.Find(id);
                    if (konse == null)
                    {
                        return HttpNotFound();
                    }
                    return View(konse);
                }
            }
            return HttpNotFound();
        }

        // GET: Masaz/Create
        public ActionResult Create(long? id)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" | Session["Auth"].ToString() == "Administrator")
                {
                    ViewBag.SalaID = new SelectList(db.Sale, "Numer_sali", "Rodzaj");
                    ViewBag.KoncerwatorID = new SelectList(db.Konserwatorzy, "OsobaID", "imieNazwisko");
                    ViewBag.SprzetID = new SelectList(db.Sprzety, "SprzetID", "Nazwa");

                    var ust = from  Konser in db.Konserwacje select Konser;

                  return View();
                }
            }
            return HttpNotFound();
        }

        // POST: Masaz/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "KonserwacjaID,Opis_usterki,Data_zgłoszenia,Data_naprawy,Status,KonserwatorID")] long? id, Konserwacja kons)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" | Session["Auth"].ToString() == "Administrator")
                {
                    ViewBag.SilowniaID = new SelectList(db.Silownie, "Nazwa", "Nazwa", kons.KonserwacjaID );
                    ViewBag.SalaID = new SelectList(db.Sale, "Numer_sali", "Rodzaj", kons.KonserwacjaID);
                    ViewBag.SprzetID = new SelectList(db.Sprzety, "SprzetID", "Nazwa", kons.KonserwacjaID);
                    ViewBag.KonserwatorID = new SelectList(db.Konserwatorzy, "OsobaID", "imieNazwisko", kons.KonserwacjaID);

                  
                        #region Konserwator
                        Konserwator konserwator = db.Konserwatorzy.Find(id);
                        kons.Konserwator = konserwator;
                        konserwator.Konserwacje.Add(kons);
                        #endregion

                        #region Sprzet
                        Sprzet sprz = db.Sprzety.Find(kons.Sprzet);
                        kons.Sprzet = sprz;
                        sprz.Konserwacje.Add(kons);
                        #endregion
                    
                        db.Konserwacje.Add(kons);
                        db.SaveChanges();

                        return RedirectToAction("Index");
                    }
                    return View(kons);
                
            }
            return HttpNotFound();
        }


       

        // GET: Masaz/Edit/5
        public ActionResult Edit(long? id)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" | Session["Auth"].ToString() == "Administrator")
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Konserwacja konserwacja = db.Konserwacje.Find(id);
                    if (konserwacja == null)
                    {
                        return HttpNotFound();
                    }
                  
                    ViewBag.SilowniaID = new SelectList(db.Silownie, "Nazwa", "Nazwa", konserwacja.KonserwacjaID);
                    ViewBag.SalaID = new SelectList(db.Sale, "Numer_sali", "Rodzaj", konserwacja.KonserwacjaID);
                    ViewBag.SprzetID = new SelectList(db.Sprzety, "SprzetID", "Nazwa", konserwacja.KonserwacjaID);
                    ViewBag.KonserwatorID = new SelectList(db.Konserwatorzy, "OsobaID", "imieNazwisko", konserwacja.KonserwacjaID);
                    return View(konserwacja);
                }
            }
            return HttpNotFound();
        }

        // POST: Masaz/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "KonserwacjaID,Opis_usterki,Data_zgłoszenia,Data_naprawy,Status,KonserwatorID")] Konserwacja konserwacja)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" | Session["Auth"].ToString() == "Administrator")
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(konserwacja).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    ViewBag.SilowniaID = new SelectList(db.Silownie, "Nazwa", "Nazwa", konserwacja.KonserwacjaID);
                    ViewBag.SalaID = new SelectList(db.Sale, "Numer_sali", "Rodzaj", konserwacja.KonserwacjaID);
                    ViewBag.SprzetID = new SelectList(db.Sprzety, "SprzetID", "Nazwa", konserwacja.KonserwacjaID);
                    ViewBag.KonserwatorID = new SelectList(db.Konserwatorzy, "Pesel", "imieNazwisko", konserwacja.KonserwacjaID);

                    return View(konserwacja);
                }
            }
            return HttpNotFound();
        }

        // GET: Masaz/Delete/5
        public ActionResult Delete(long? id)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" | Session["Auth"].ToString() == "Administrator")
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Konserwacja konserwacja = db.Konserwacje.Find(id);
                    if (konserwacja == null)
                    {
                        return HttpNotFound();
                    }
                    return View(konserwacja);
                }
            }
            return HttpNotFound();
        }

        // POST: Masaz/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" | Session["Auth"].ToString() == "Administrator")
                {
                    Konserwacja konserwacja = db.Konserwacje.Find(id);
                    db.Konserwacje.Remove(konserwacja);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
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
