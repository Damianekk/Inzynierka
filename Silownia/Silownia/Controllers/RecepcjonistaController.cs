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
    public class RecepcjonistaController : Controller
    {
        private SilowniaContext db = new SilowniaContext();

        // GET: Recepcjonista
        public ActionResult Index(string imieNazwisko, string SilowniaID, int page = 1, int pageSize = 10, AkcjaEnumRecepcjonista akcja = AkcjaEnumRecepcjonista.Brak, String info = null)
        {
       //     if (Session["User"] != null)
            {
                ViewBag.SilowniaID = new SelectList(db.Silownie.DistinctBy(a => new { a.Nazwa }), "Nazwa", "Nazwa");
                var recepcjonisci = from Osoby in db.Recepcjonisci.OfType<Recepcjonista>() select Osoby;

                if (!String.IsNullOrEmpty(imieNazwisko))
                    foreach (string wyraz in imieNazwisko.Split(' '))
                        recepcjonisci = recepcjonisci.Search(wyraz, i => i.Imie, i => i.Nazwisko);

                recepcjonisci = recepcjonisci.Search(SilowniaID, i => i.Silownia.Nazwa);

                var final = recepcjonisci.OrderBy(p => p.Nazwisko);
                var ileWynikow = recepcjonisci.Count();
                if ((ileWynikow / page) <= 1)
                {
                    page = 1;
                }
                var kk = ileWynikow / page;

                PagedList<Recepcjonista> model = new PagedList<Recepcjonista>(final, page, pageSize);

                if (akcja != AkcjaEnumRecepcjonista.Brak)
                {
                    ViewBag.info = info;
                    ViewBag.Akcja = akcja;
                }

                return View(model);
            }
          //  return HttpNotFound();
        }

        // GET: Recepcjonista/Details/5
        public ActionResult Details(long? id)
        {
         //   if (Session["User"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Recepcjonista recepcjonista = db.Recepcjonisci.Find(id);

                if (recepcjonista == null)
                {
                    return HttpNotFound();
                }
                return View(recepcjonista);
            }
            // return HttpNotFound();
        }

        // GET: Recepcjonista/Create
        public ActionResult Create()
        {
          //  if (Session["User"] != null)
            {
                ViewBag.SilowniaID = new SelectList(db.Silownie, "SilowniaID", "Nazwa");
                return View();
            }
          //  return HttpNotFound();
        }

        // POST: Recepcjonista/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OsobaID,Imie,Nazwisko,DataUrodzenia,Pesel,NrTelefonu,DataZatrudnienia,Pensja,SilowniaID")] Recepcjonista recepcjonista)
        {
         //   if (Session["User"] != null)
            {
                if (ModelState.IsValid)
                {
                    recepcjonista.DataZatrudnienia = DateTime.Now;
                    db.Recepcjonisci.Add(recepcjonista);
                    db.SaveChanges();

                    Uzytkownik pracownik = new Uzytkownik();
                    pracownik.IDOsoby = recepcjonista.OsobaID;
                    pracownik.Login = recepcjonista.Nazwisko;
                    pracownik.Haslo = recepcjonista.Imie + recepcjonista.Nazwisko;
                    pracownik.Rola = RoleEnum.Recepcjonista.GetDescription();
                    db.Uzytkownicy.Add(pracownik);
                    db.SaveChanges();

                    return RedirectToAction("Index", new { akcja = AkcjaEnumRecepcjonista.DodanoRecepcjoniste, info = recepcjonista.imieNazwisko });
                }

                ViewBag.SilowniaID = new SelectList(db.Silownie, "SilowniaID", "Nazwa");
                return View(recepcjonista);
            }
         //   return HttpNotFound();
        }

        // GET: Recepcjonista/Edit/5
        public ActionResult Edit(long? id)
        {
         //   if (Session["User"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Recepcjonista recepcjonista = db.Recepcjonisci.Find(id);
                if (recepcjonista == null)
                {
                    return HttpNotFound();
                }
                ViewBag.SilowniaID = new SelectList(db.Silownie, "SilowniaID", "Nazwa");
                return View(recepcjonista);
            }
          //  return HttpNotFound();
        }

        // POST: Recepcjonista/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OsobaID,Imie,Nazwisko,DataUrodzenia,Pesel,NrTelefonu,DataZatrudnienia,Pensja,SilowniaID")] Recepcjonista recepcjonista)
        {
         //   if (Session["User"] != null)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(recepcjonista).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.SilowniaID = new SelectList(db.Silownie, "SilowniaID", "Nazwa");
                return View(recepcjonista);
            }
        //    return HttpNotFound();
        }

        // GET: Recepcjonista/Delete/5
        public ActionResult Delete(long? id)
        {
         //   if (Session["User"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Recepcjonista recepcjonista = db.Recepcjonisci.Find(id);
                if (recepcjonista == null)
                {
                    return HttpNotFound();
                }
                return View(recepcjonista);
            }
         //   return HttpNotFound();
        }

        // POST: Recepcjonista/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
        //    if (Session["User"] != null)
            {
                Recepcjonista recepcjonista = db.Recepcjonisci.Find(id);
                db.Recepcjonisci.Remove(recepcjonista);
                db.SaveChanges();

                Uzytkownik uzytkownik = db.Uzytkownicy.Where(w => w.IDOsoby == recepcjonista.OsobaID).First();
                db.Uzytkownicy.Remove(uzytkownik);
                db.SaveChanges();

                return RedirectToAction("Index", new { akcja = AkcjaEnumRecepcjonista.UsunietoRecepcjoniste, info = recepcjonista.imieNazwisko });
            }
         //   return HttpNotFound();
        }

        public ActionResult FillRecepcjonista(int silownia)
        {
            var recepcjonisci = db.Recepcjonisci.Where(c => c.SilowniaID == silownia);
            return Json(recepcjonisci, JsonRequestBehavior.AllowGet);
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
