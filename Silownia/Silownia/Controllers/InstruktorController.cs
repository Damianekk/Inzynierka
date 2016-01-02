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
    public class InstruktorController : Controller
    {
        private SilowniaContext db = new SilowniaContext();

        // GET: Instruktor
        public ActionResult Index(string imieNazwisko, string SilowniaID, int page = 1, int pageSize = 10, AkcjaEnumInstruktor akcja = AkcjaEnumInstruktor.Brak, String info = null)
        {
            if (Session["Auth"].ToString() == "Recepcjonista" | Session["Auth"].ToString() == "Administrator")
            {
                ViewBag.SilowniaID = new SelectList(db.Silownie.DistinctBy(a => new { a.Nazwa }), "Nazwa", "Nazwa");
                var instruktorzy = from Osoby in db.Instruktorzy.OfType<Instruktor>() select Osoby;

                if (!String.IsNullOrEmpty(imieNazwisko))
                    foreach (string wyraz in imieNazwisko.Split(' '))
                        instruktorzy = instruktorzy.Search(wyraz, i => i.Imie, i => i.Nazwisko);

                instruktorzy = instruktorzy.Search(SilowniaID, i => i.Silownia.Nazwa);

                var final = instruktorzy.OrderBy(p => p.Imie);
                var ileWynikow = instruktorzy.Count();
                if ((ileWynikow / page) <= 1)
                {
                    page = 1;
                }
                var kk = ileWynikow / page;

                PagedList<Instruktor> model = new PagedList<Instruktor>(final, page, pageSize);

                if (akcja != AkcjaEnumInstruktor.Brak)
                {
                    ViewBag.info = info;
                    ViewBag.Akcja = akcja;
                }

                return View(model);
            }
            return HttpNotFound();
        }

        // GET: Instruktor/Details/5
        public ActionResult Details(long? id)
        {
            if (Session["Auth"].ToString() == "Recepcjonista" | Session["Auth"].ToString() == "Administrator")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Instruktor instruktor = db.Instruktorzy.Find(id);

                if (instruktor == null)
                {
                    return HttpNotFound();
                }
                return View(instruktor);
            }
            return HttpNotFound();
        }

        // GET: Instruktor/Create
        public ActionResult Create()
        {
            if (Session["Auth"].ToString() == "Recepcjonista" | Session["Auth"].ToString() == "Administrator")
            {
                ViewBag.SilowniaID = new SelectList(db.Silownie, "SilowniaID", "Nazwa");
                return View();
            }
            return HttpNotFound();
        }

        // POST: Instruktor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "OsobaID,Imie,Nazwisko,DataUrodzenia,NrTelefonu,Pesel,DataZatrudnienia,Pensja,SilowniaID")] Instruktor instruktor)
        {
            if (Session["Auth"].ToString() == "Recepcjonista" | Session["Auth"].ToString() == "Administrator")
            {
                if (ModelState.IsValid)
                {
                    instruktor.DataZatrudnienia = DateTime.Now;
                    db.Instruktorzy.Add(instruktor);
                    db.SaveChanges();

                    Uzytkownik pracownik = new Uzytkownik();
                    pracownik.IDOsoby = instruktor.OsobaID;
                    pracownik.Login = instruktor.Nazwisko;
                    pracownik.Haslo = instruktor.Imie + instruktor.Nazwisko;
                    pracownik.Rola = "Instruktor";
                    
                    db.Uzytkownicy.Add(pracownik);
                    db.SaveChanges();

                    return RedirectToAction("Index", new { akcja = AkcjaEnumInstruktor.DodanoInstruktora, info = instruktor.imieNazwisko });
                }

                ViewBag.SilowniaID = new SelectList(db.Silownie, "SilowniaID", "Nazwa");
                return View(instruktor);
            }
            return HttpNotFound();
        }

        // GET: Instruktor/Edit/5
        public ActionResult Edit(long? id)
        {
            if (Session["Auth"].ToString() == "Recepcjonista" | Session["Auth"].ToString() == "Administrator")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Instruktor instruktor = db.Instruktorzy.Find(id);
                if (instruktor == null)
                {
                    return HttpNotFound();
                }
                ViewBag.SilowniaID = new SelectList(db.Silownie, "SilowniaID", "Nazwa");
                return View(instruktor);
            }
            return HttpNotFound();
        }
        // POST: Instruktor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "OsobaID,Imie,Nazwisko,DataUrodzenia,NrTelefonu,Pesel,DataZatrudnienia,Pensja,SilowniaID")] Instruktor instruktor)
        {
            if (Session["Auth"].ToString() == "Recepcjonista" | Session["Auth"].ToString() == "Administrator")
            {
                if (ModelState.IsValid)
                {
                    db.Entry(instruktor).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.SilowniaID = new SelectList(db.Silownie, "SilowniaID", "Nazwa");
                return View(instruktor);
            }
            return HttpNotFound();
        }

        // GET: Instruktor/Delete/5
        public ActionResult Delete(long? id)
        {
            if (Session["Auth"].ToString() == "Recepcjonista" | Session["Auth"].ToString() == "Administrator")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Instruktor instruktor = db.Instruktorzy.Find(id);
                if (instruktor == null)
                {
                    return HttpNotFound();
                }
                return View(instruktor);
            }
            return HttpNotFound();
        }

        // POST: Instruktor/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            if (Session["Auth"].ToString() == "Recepcjonista" | Session["Auth"].ToString() == "Administrator")
            {
                Instruktor instruktor = db.Instruktorzy.Find(id);
                db.Osoby.Remove(instruktor);
                db.SaveChanges();
                return RedirectToAction("Index", new { akcja = AkcjaEnumInstruktor.UsunietoInstruktora, info = instruktor.imieNazwisko });
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
