using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Silownia.Models;
using Silownia.DAL;

namespace Silownia.Controllers
{
    public class AdresController : Controller
    {
        
        private SilowniaContext db = new SilowniaContext();

        // GET: /Adres/
        public ActionResult Index()
        {
            return View(db.Adresy.ToList());
        }

        // GET: /Adres/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Adres adres = db.Adresy.Find(id);
            if (adres == null)
            {
                return HttpNotFound();
            }
            return View(adres);
        }

        // GET: /Adres/Create
        public ActionResult Create(long? id,KomuAdres komu)
        {
            if (id != null)
            {
                switch (komu)
                {
                    case KomuAdres.Osoba:
                        Osoba osoba = db.Osoby.Find(id);
                        ViewBag.Osoba = osoba;
                        break;
                    case KomuAdres.Silownia:
                        Silownia.Models.Silownia silownia = db.Silownie.Find(id);
                        ViewBag.Silownia = silownia;
                        break;

                    

                }
            }
            return View();
        }

        // POST: /Adres/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="AdresID,KodPocztowy,Kraj,Miasto,Ulica,NrBudynku,NrLokalu")] Adres adres,long? id)
        {
            if (ModelState.IsValid)
            {
                Osoba osoba = db.Klienci.Find(id);
                db.Adresy.Add(adres);
                osoba.Adres = adres;
                adres.Osoba = osoba;
                db.SaveChanges();
                return RedirectToAction("Index","Klient");
            }

            return View(adres);
        }

        // GET: /Adres/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Adres adres = db.Adresy.Find(id);
            if (adres == null)
            {
                return HttpNotFound();
            }
            return View(adres);
        }

        // POST: /Adres/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="AdresID,KodPocztowy,Kraj,Miasto,Ulica,NrBudynku,NrLokalu")] Adres adres)
        {
            if (ModelState.IsValid)
            {
                db.Entry(adres).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(adres);
        }

        // GET: /Adres/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Adres adres = db.Adresy.Find(id);
            if (adres == null)
            {
                return HttpNotFound();
            }
            return View(adres);
        }

        // POST: /Adres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Adres adres = db.Adresy.Find(id);
            db.Adresy.Remove(adres);
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
