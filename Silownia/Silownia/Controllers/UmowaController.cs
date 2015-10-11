using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Silownia.Models;
using Silownia.DAL;
using System;
using Microsoft.AspNet.Identity;
 

namespace Silownia.Controllers
{
    public class UmowaController : Controller
    {
        private SilowniaContext db = new SilowniaContext();

        // GET: /Umowa/
        public ActionResult Index()
        {
            var umowy = db.Umowy.Include(u => u.Silownia);
            return View(umowy.ToList());
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
        public ActionResult Create()
        {
            
            ViewBag.SilowniaID = new SelectList(db.Silownie, "SilowniaID", "Nazwa");
            ViewBag.RecepcjonistaID = new SelectList(db.Recepcjonisci, "OsobaID", "imieNazwisko");
            var a = from Osoby in db.Osoby.OfType<Recepcjonista>() select Osoby;
 
             Recepcjonista  recepcjonista = null;
            var user = User.Identity.GetUserName();
            foreach (Recepcjonista rec in a)
            { 
                if (rec.imieNazwisko.Replace(" ", "") == user)
                {
                    recepcjonista = rec;
                    break;
                }
            }
       
            return View(new Umowa // W ten sposób tworze obiekt nadaje aktualny czas / przypisuje do Daty podpisania umowy i zwracam widok z datą (teraz)
            {
                DataPodpisania = DateTime.Now
              // ,Recepcjonista = recepcjonista 
            });
        }

        // POST: /Umowa/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include= "UmowaID,SilowniaID,DataPodpisania,DataZakonczenia,Cena,RecepcjonistaID")] long? id, Umowa umowa)
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
            ViewBag.RecepcjonistaID = new SelectList(db.Recepcjonisci, "OsobaID", "imieNazwisko");
             
            if (ModelState.IsValid)
            {
                Klient osoba = db.Klienci.Find(id);
                db.Umowy.Add(umowa);
                umowa.Klient = osoba;
                osoba.Umowa.Add(umowa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            ViewBag.SilowniaID = new SelectList(db.Silownie, "SilowniaID", "Nazwa", umowa.SilowniaID);
            return View(umowa);
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
        public ActionResult Edit([Bind(Include="UmowaID,SilowniaID,DataPodpisania,DataZakonczenia,Cena")] Umowa umowa)
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
