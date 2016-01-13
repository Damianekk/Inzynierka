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

        // GET: Konserwacja
        public ActionResult Index(string nazwaSprzetu, string SilowniaID, string KonserwatorID, string Status, int page = 1, int pageSize = 10, AkcjaEnumKonserwacja akcja = AkcjaEnumKonserwacja.Brak, String info = null)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" || Session["Auth"].ToString() == "Administrator")
                {
                    ViewBag.SilowniaID = new SelectList(db.Silownie.DistinctBy(a => new { a.Nazwa }), "Nazwa", "Nazwa");
                    ViewBag.Status = new SelectList(db.Konserwacje.DistinctBy(a => new { a.Status }), "Status", "Status");
                    ViewBag.KonserwatorID = new SelectList(db.Konserwatorzy.DistinctBy(a => new { a.Pesel }), "imieNazwisko", "imieNazwisko");

                    var konserwacje = from Konserwacje in db.Konserwacje select Konserwacje;

                    if (!String.IsNullOrEmpty(nazwaSprzetu))
                        konserwacje = konserwacje.Search(nazwaSprzetu, i => i.Sprzet.Nazwa);

                    konserwacje = konserwacje.Search(SilowniaID, i => i.Sprzet.Sala.Silownia.Nazwa);
                    konserwacje = konserwacje.Search(Status, i => i.Status);

                    if (!String.IsNullOrEmpty(KonserwatorID))
                        foreach (string wyraz in KonserwatorID.Split(' '))
                            konserwacje = konserwacje.Search(wyraz, i => i.Konserwator.Imie, i => i.Konserwator.Nazwisko);

                    var final = konserwacje.OrderBy(p => p.Sprzet.Nazwa);
                    var ileWynikow = konserwacje.Count();
                    if ((ileWynikow / page) <= 1)
                    {
                        page = 1;
                    }
                    var kk = ileWynikow / page;

                    PagedList<Konserwacja> model = new PagedList<Konserwacja>(final, page, pageSize);

                    if (akcja != AkcjaEnumKonserwacja.Brak)
                    {
                        ViewBag.info = info;
                        ViewBag.Akcja = akcja;
                    }

                    return View(model);
                }
            }
            return HttpNotFound();
        }

        // GET: Konserwacja/Details/5
        public ActionResult Details(long? id)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" || Session["Auth"].ToString() == "Administrator")
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

        // GET: Konserwacja/Create
        public ActionResult Create(long? id)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" || Session["Auth"].ToString() == "Administrator")
                {
                    ViewBag.KonserwatorID = new SelectList(db.Konserwatorzy, "OsobaID", "imieNazwisko");
                    var a = from Osoby in db.Konserwatorzy select Osoby;

                    Konserwator konserwator = null;
                    var user = User.Identity.GetUserName();
                    foreach (Konserwator kons in a)
                    {
                        if (kons.imieNazwisko.Replace(" ", "") == user)
                        {
                            konserwator = kons;
                            break;
                        }

                        Osoba osoba = db.Osoby.Find(id);
                        ViewBag.Osoba = osoba;
                    }

                    return View(new Konserwacja
                    {
                        Data_zgłoszenia = DateTime.Now,
                    });
                }
            }
            return HttpNotFound();
        }

        // POST: Konserwacja/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "KonserwacjaID,Opis_usterki,Data_zgłoszenia,Data_naprawy,Status,KonserwatorID")] long? id, Konserwacja konserwacja)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" || Session["Auth"].ToString() == "Administrator")
                {
                    ViewBag.KonserwatorID = new SelectList(db.Konserwatorzy, "OsobaID", "imieNazwisko", konserwacja.KonserwatorID);

                    if (ModelState.IsValid)
                    {
                        #region Sprzet
                        Sprzet sprzet = db.Sprzety.Find(id);
                        konserwacja.Sprzet = sprzet;
                        sprzet.Konserwacje.Add(konserwacja);
                        #endregion

                        #region Konserwator
                        Konserwator konserwator = db.Konserwatorzy.Find(konserwacja.KonserwatorID);
                        konserwacja.Konserwator = konserwator;
                        konserwator.Konserwacje.Add(konserwacja);
                        #endregion

                        db.Konserwacje.Add(konserwacja);
                        db.SaveChanges();
                        return RedirectToAction("Index", new { akcja = AkcjaEnumKonserwacja.DodanoKonserwacje, info = konserwacja.Sprzet.Nazwa });
                    }

                    return View(konserwacja);
                }
            }
            return HttpNotFound();
        }

        // GET: Konserwacja/Edit/5
        public ActionResult Edit(long? id)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" || Session["Auth"].ToString() == "Administrator")
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
                    ViewBag.KonserwatorID = new SelectList(db.Konserwatorzy, "OsobaID", "imieNazwisko", konserwacja.KonserwatorID);

                    return View(konserwacja);
                }
            }
            return HttpNotFound();
        }

        // POST: Konserwacja/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "KonserwacjaID,Opis_usterki,Data_zgłoszenia,Data_naprawy,Status,KonserwatorID")] Konserwacja konserwacja)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" || Session["Auth"].ToString() == "Administrator")
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(konserwacja).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    ViewBag.KonserwatorID = new SelectList(db.Konserwatorzy, "OsobaID", "imieNazwisko", konserwacja.KonserwatorID);
                    return View(konserwacja);
                }
            }
            return HttpNotFound();
        }

        // GET: Konserwacja/Delete/5
        public ActionResult Delete(long? id)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" || Session["Auth"].ToString() == "Administrator")
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

        // POST: Konserwacja/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" || Session["Auth"].ToString() == "Administrator")
                {
                    Konserwacja konserwacja = db.Konserwacje.Find(id);
                    db.Konserwacje.Remove(konserwacja);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { akcja = AkcjaEnumKonserwacja.UsunietoKonserwacje });
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
