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
using System.Web.Script.Serialization;


namespace Silownia.Controllers
{


    public class KlientController : Controller
    {
        private SilowniaContext db = new SilowniaContext();


        // GET: /Klient/

        public ActionResult Index(string Miasto, string imieNazwisko, bool czyUmowa = false, int page = 1, int pageSize = 10, AkcjaEnum akcja = AkcjaEnum.Brak, String info = null)
        {
            // if (Session["User"] != null)
            {
                var Miasta = db.Klienci.Where(u => (u.OsobaID != null) && (u.Adres != null)).DistinctBy(a => new { a.Adres.Miasto }).Select(x => x.Adres);


                ViewBag.Miasto = new SelectList(Miasta, "Miasto", "Miasto");

                var osoby = from Osoby in db.Klienci select Osoby;


                if (!String.IsNullOrEmpty(imieNazwisko))
                    foreach (string wyraz in imieNazwisko.Split(' '))
                        osoby = osoby.Search(wyraz, i => i.Imie, i => i.Nazwisko);
                osoby = osoby.Search(Miasto, m => m.Adres.Miasto);
                if (czyUmowa)
                    osoby = osoby.Where(u => u.Umowy.Count > 0);

                var final = osoby.OrderBy(p => p.Nazwisko);
                var ileWynikow = osoby.Count();
                if ((ileWynikow / page) <= 1)
                {
                    page = 1;
                }
                var kk = ileWynikow / page;

                PagedList<Klient> model = new PagedList<Klient>(final, page, pageSize);


                if (akcja != AkcjaEnum.Brak)
                {
                    ViewBag.info = info;
                    ViewBag.Akcja = akcja;
                }

                return View(model);
            }
            //  return HttpNotFound();

        }

        // GET: /Klient/Details/5
        public ActionResult Details(long? id)
        {
            //  if (Session["User"] != null)
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
            //    return HttpNotFound();
        }

        // GET: /Klient/Create
        public ActionResult Create()
        {
            //  if (Session["User"] != null)
            {
                return View();
            }
            //    return HttpNotFound();
        }

        // POST: /Klient/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "OsobaID,Imie,Nazwisko,DataUrodzenia,Mail,NrTelefonu,Adres")] Klient klient)
        {
            //  if (Session["User"] != null)
            {
                if (ModelState.IsValid)
                {
                    db.Klienci.Add(klient);
                    db.SaveChanges();

                    Uzytkownik uzytkownik = new Uzytkownik();
                    uzytkownik.IDOsoby = klient.OsobaID;
                    uzytkownik.Login = klient.Mail;
                    uzytkownik.Haslo = klient.Imie + klient.Nazwisko;
                    uzytkownik.Rola = RoleEnum.Klient.GetDescription();
                    db.Uzytkownicy.Add(uzytkownik);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { akcja = AkcjaEnum.DodanoKlienta, info = klient.imieNazwisko });
                }

                return View(klient);
            }
            //  return HttpNotFound();
        }

        // GET: /Klient/Edit/5
        public ActionResult Edit(long? id)
        {
            //  if (Session["User"] != null)
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
            // return HttpNotFound();
        }

        // POST: /Klient/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "OsobaID,Imie,Nazwisko,DataUrodzenia,Mail,NrTelefonu")] Klient klient)
        {
            //   if (Session["User"] != null)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(klient).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(klient);
            }
            //   return HttpNotFound();
        }

        // GET: /Klient/Delete/5
        public ActionResult Delete(long? id)
        {
            //  if (Session["User"] != null)
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
            //  return HttpNotFound();
        }

        // POST: /Klient/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            // if (Session["User"] != null)
            {
                Klient klient = db.Klienci.Find(id);
                db.Klienci.Remove(klient);
                db.SaveChanges();

                Uzytkownik uzytkownik = db.Uzytkownicy.Where(w => w.IDOsoby == klient.OsobaID).First();
                db.Uzytkownicy.Remove(uzytkownik);
                db.SaveChanges();

                return RedirectToAction("Index", new { akcja = AkcjaEnum.UsunietoKlienta, info = klient.imieNazwisko });
            }
            //  return HttpNotFound();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [HttpPost]
        public JsonResult KlientInfoJSON()
        {

            var jsonSerialiser = new JavaScriptSerializer();
            var klienci = db.Klienci.Where(s => s.Adres != null).ToList<Silownia.Models.Klient>();

            //  klienci.RemoveAll(item => item.Adres != null);
            var z = klienci.Select(x => new
            {
                dlugosc = x.Dlugosc,
                szerokosc = x.Szerokosc,
                imieNazwisko = x.imieNazwisko,
                numerTelefonu = x.NrTelefonu,
                email = x.Mail
            });
            return Json(z);

            //  return Json(new { ok = true, myData = klienci }, JsonRequestBehavior.AllowGet);
        }
    }
}
