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

namespace Silownia.Controllers
{
    public class MasazystaController : Controller
    {
        private SilowniaContext db = new SilowniaContext();

        // GET: Masazysta

        public ActionResult Index(string imieNazwisko, string SilowniaID, int page = 1, int pageSize = 10, AkcjaEnumMasazysta akcja = AkcjaEnumMasazysta.Brak, String info = null)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" || Session["Auth"].ToString() == "Administrator")
                {
                    ViewBag.SilowniaID = new SelectList(db.Silownie.DistinctBy(a => new { a.Nazwa }), "Nazwa", "Nazwa");

                    var osoby = from Osoby in db.Masazysci select Osoby;
                    ViewBag.SilowniaID = new SelectList(db.Silownie.DistinctBy(a => new { a.Nazwa }), "Nazwa", "Nazwa");

                    if (!String.IsNullOrEmpty(imieNazwisko))
                        foreach (string wyraz in imieNazwisko.Split(' '))
                            osoby = osoby.Search(wyraz, i => i.Imie, i => i.Nazwisko);
                    osoby = osoby.Search(SilowniaID, i => i.Silownia.Nazwa);

                    var final = osoby.OrderBy(p => p.Nazwisko);
                    var ileWynikow = osoby.Count();
                    if ((ileWynikow / page) <= 1)
                    {
                        page = 1;
                    }
                    var kk = ileWynikow / page;

                    PagedList<Masazysta> model = new PagedList<Masazysta>(final, page, pageSize);

                    if (akcja != AkcjaEnumMasazysta.Brak)
                    {
                        ViewBag.info = info;
                        ViewBag.Akcja = akcja;
                    }

                    return View(model);
                }
            }
            return HttpNotFound();
        }


        // GET: Masazysta/Details/5
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
                    Masazysta masazysta = db.Masazysci.Find(id);
                    if (masazysta == null)
                    {
                        return HttpNotFound();
                    }
                    return View(masazysta);
                }
            }
            return HttpNotFound();
        }

        // GET: Masazysta/Create
        public ActionResult Create()
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" || Session["Auth"].ToString() == "Administrator")
                {
                    ViewBag.SilowniaID = new SelectList(db.Silownie, "SilowniaID", "Nazwa");
                    return View();
                }
            }
            return HttpNotFound();
        }

        // POST: Masazysta/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "OsobaID,Imie,Nazwisko,DataUrodzenia,NrTelefonu,Pesel,DataZatrudnienia,Pensja,StawkaGodzinowa,SilowniaID")] Masazysta masazysta)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" || Session["Auth"].ToString() == "Administrator")
                {
                    if (ModelState.IsValid)
                    {
                        masazysta.DataZatrudnienia = DateTime.Now;
                        db.Osoby.Add(masazysta);
                        db.SaveChanges();

                        Uzytkownik pracownik = new Uzytkownik();
                        pracownik.IDOsoby = masazysta.OsobaID;
                        pracownik.Login = masazysta.Pesel.ToString();
                        pracownik.Haslo = masazysta.Imie + masazysta.Nazwisko;
                        pracownik.Rola = RoleEnum.Masazysta.GetDescription();
                        db.Uzytkownicy.Add(pracownik);
                        db.SaveChanges();

                        return RedirectToAction("Index", new { akcja = AkcjaEnumMasazysta.DodanoMasazyste, info = masazysta.imieNazwisko });
                    }

                    ViewBag.SilowniaID = new SelectList(db.Silownie, "SilowniaID", "Nazwa", masazysta.SilowniaID);
                    return View(new Masazysta
                    {
                        DataZatrudnienia = DateTime.Now
                    }
                    );
                }
            }
            return HttpNotFound();
        }

        // GET: Masazysta/Edit/5
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
                    Masazysta masazysta = db.Masazysci.Find(id);
                    if (masazysta == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.SilowniaID = new SelectList(db.Silownie, "SilowniaID", "Nazwa", masazysta.Silownia);
                    return View(masazysta);
                }
            }
            return HttpNotFound();
        }

        // POST: Masazysta/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "OsobaID,Imie,Nazwisko,DataUrodzenia,NrTelefonu,Pesel,DataZatrudnienia,Pensja,StawkaGodzinowa,SilowniaID")] Masazysta masazysta)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" || Session["Auth"].ToString() == "Administrator")
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(masazysta).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    ViewBag.SilowniaID = new SelectList(db.Silownie, "SilowniaID", "Nazwa", masazysta.SilowniaID);
                    return View(masazysta);
                }
            }
            return HttpNotFound();
        }

        // GET: Masazysta/Delete/5
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
                    Masazysta masazysta = db.Masazysci.Find(id);
                    if (masazysta == null)
                    {
                        return HttpNotFound();
                    }
                    return View(masazysta);
                }
            }
            return HttpNotFound();
        }

        // POST: Masazysta/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" || Session["Auth"].ToString() == "Administrator")
                {
                    Masazysta masazysta = db.Masazysci.Find(id);
                    db.Osoby.Remove(masazysta);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { akcja = AkcjaEnumMasazysta.UsunietoMasazyste, info = masazysta.imieNazwisko });
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
