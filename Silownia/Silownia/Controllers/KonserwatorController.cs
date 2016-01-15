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
using Silownia.Helpers;
using PagedList;


namespace Silownia.Controllers
{
    public class KonserwatorController : Controller
    {
        private SilowniaContext db = new SilowniaContext();

        // GET: Recepcjonista
        public ActionResult Index(string imieNazwisko, string SilowniaID, int page = 1, int pageSize = 10, AkcjaEnumKonserwator akcja = AkcjaEnumKonserwator.Brak, String info = null)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" || Session["Auth"].ToString() == "Administrator")
                {
                    ViewBag.SilowniaID = new SelectList(db.Silownie.DistinctBy(a => new { a.Nazwa }), "Nazwa", "Nazwa");
                    var konserwatorzy = from Osoby in db.Konserwatorzy.OfType<Konserwator>() select Osoby;

                    if (!String.IsNullOrEmpty(imieNazwisko))
                        foreach (string wyraz in imieNazwisko.Split(' '))
                            konserwatorzy = konserwatorzy.Search(wyraz, i => i.Imie, i => i.Nazwisko);

                    konserwatorzy = konserwatorzy.Search(SilowniaID, i => i.Silownia.Nazwa);

                    var final = konserwatorzy.OrderBy(p => p.Nazwisko);
                    var ileWynikow = konserwatorzy.Count();
                    if ((ileWynikow / page) <= 1)
                    {
                        page = 1;
                    }
                    var kk = ileWynikow / page;

                    PagedList<Konserwator> model = new PagedList<Konserwator>(final, page, pageSize);

                    if (akcja != AkcjaEnumKonserwator.Brak)
                        ViewBag.Akcja = akcja;

                    if (info != null)
                        ViewBag.info = info;

                    return View(model);
                }
            }
            return HttpNotFound();
        }

        // GET: Recepcjonista/Details/5
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
                    Konserwator konserwator = db.Konserwatorzy.Find(id);

                    if (konserwator == null)
                    {
                        return HttpNotFound();
                    }
                    return View(konserwator);
                }
            }
            return HttpNotFound();
        }

        // GET: Recepcjonista/Create
        [MyAuthorize(RoleEnum.Administrator)]
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

        // POST: Recepcjonista/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OsobaID,Imie,Nazwisko,DataUrodzenia,Pesel,NrTelefonu,DataZatrudnienia,Pensja,SilowniaID")] Konserwator konserwator)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" || Session["Auth"].ToString() == "Administrator")
                {
                    if (ModelState.IsValid)
                    {
                        konserwator.DataZatrudnienia = DateTime.Now;
                        db.Konserwatorzy.Add(konserwator);
                        db.SaveChanges();

                        Uzytkownik pracownik = new Uzytkownik();
                        pracownik.IDOsoby = konserwator.OsobaID;
                        pracownik.Login = konserwator.Pesel.ToString();

                        //string haslo = instruktor.Imie + instruktor.Nazwisko;
                        //string szyfrowanie = szyfr.Encrypt(haslo);
                        //pracownik.Haslo = szyfrowanie;

                        //to bedzie do zakomentowania - zakomentowane do odkomentowania
                        pracownik.Haslo = konserwator.Imie + konserwator.Nazwisko;

                        pracownik.Rola = RoleEnum.Konserwator.GetDescription();
                        db.Uzytkownicy.Add(pracownik);
                        db.SaveChanges();

                        return RedirectToAction("Index", new { akcja = AkcjaEnumKonserwator.DodanoKonserwatora, info = konserwator.imieNazwisko });
                    }

                    ViewBag.SilowniaID = new SelectList(db.Silownie, "SilowniaID", "Nazwa");
                    return View(konserwator);
                }
            }
            return HttpNotFound();
        }


        // GET: Recepcjonista/Edit/5
        [MyAuthorize(RoleEnum.Administrator)]
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
                    Konserwator konserwator = db.Konserwatorzy.Find(id);
                    if (konserwator == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.SilowniaID = new SelectList(db.Silownie, "SilowniaID", "Nazwa");
                    return View(konserwator);
                }
            }
            return HttpNotFound();
        }

        // POST: Konserwator/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OsobaID,Imie,Nazwisko,DataUrodzenia,Dlugosc,Szerokosc,ZdjecieProfilowe,NrTelefonu,Pesel,DataZatrudnienia,Pensja,SilowniaID")] Konserwator konserwator)
        {
            if (ModelState.IsValid)
            {
                db.Entry(konserwator).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SilowniaID = new SelectList(db.Silownie, "SilowniaID", "Nazwa", konserwator.SilowniaID);
            return View(konserwator);
        }


        // GET: Konserwator/Delete/5
        [MyAuthorize(RoleEnum.Administrator)]
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
                    Konserwator konserwator = db.Konserwatorzy.Find(id);
                    if (konserwator == null)
                    {
                        return HttpNotFound();
                    }
                    return View(konserwator);
                }
            }
            return HttpNotFound();
        }

        // POST: Konserwator/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [MyAuthorize(RoleEnum.Administrator)]
        public ActionResult DeleteConfirmed(long id)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" || Session["Auth"].ToString() == "Administrator")
                {
                    Konserwator konserwator = db.Konserwatorzy.Find(id);
                    db.Konserwatorzy.Remove(konserwator);
                    db.SaveChanges();

                    Uzytkownik uzytkownik = db.Uzytkownicy.Where(w => w.IDOsoby == konserwator.OsobaID).First();
                    db.Uzytkownicy.Remove(uzytkownik);
                    db.SaveChanges();

                    return RedirectToAction("Index", new { akcja = AkcjaEnumKonserwator.UsunietoKonserwatora, info = konserwator.imieNazwisko });
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
