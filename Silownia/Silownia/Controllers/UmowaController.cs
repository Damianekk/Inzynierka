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
using Silownia.ViewModel;

namespace Silownia.Controllers
{
    public class UmowaController : Controller
    {
        private SilowniaContext db = new SilowniaContext();


        // GET: /Umowa/
        public ActionResult Index(string imieNazwisko, string SilowniaID, int page = 1, int pageSize = 10, AkcjaEnumUmowa akcja = AkcjaEnumUmowa.Brak, String info = null)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" || Session["Auth"].ToString() == "Administrator")
                {
                    ViewBag.SilowniaID = new SelectList(db.Silownie.DistinctBy(a => new { a.Nazwa }), "Nazwa", "Nazwa");

                    var umowy = from Umowy in db.Umowy select Umowy;

                    if (!String.IsNullOrEmpty(imieNazwisko))
                        foreach (string wyraz in imieNazwisko.Split(' '))
                            umowy = umowy.Search(wyraz, i => i.Klient.Imie, i => i.Klient.Nazwisko);
                    umowy = umowy.Search(SilowniaID, i => i.Silownia.Nazwa);

                    var final = umowy.OrderBy(p => p.Klient.Nazwisko);
                    var ileWynikow = umowy.Count();
                    if ((ileWynikow / page) <= 1)
                    {
                        page = 1;
                    }
                    var kk = ileWynikow / page;


                    PagedList<Umowa> model = new PagedList<Umowa>(final, page, pageSize);

                    if (akcja != AkcjaEnumUmowa.Brak)
                    {
                        ViewBag.info = info;
                        ViewBag.Akcja = akcja;
                    }

                    return View(model);
                }
            }
            return HttpNotFound();
        }

        // GET: /Umowa/Details/5
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
                    Umowa umowa = db.Umowy.Find(id);
                    if (umowa == null)
                    {
                        return HttpNotFound();
                    }
                    return View(umowa);
                }
            }
            return HttpNotFound();
        }

        // GET: /Umowa/Create
        public ActionResult Create(long? id)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" || Session["Auth"].ToString() == "Administrator")
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
                        // minimalna umowa na miesiąc
                        DataZakonczenia = (DateTime.Now).AddMonths(1),

                        // ,Recepcjonista = recepcjonista 
                    });
                }
            }
            return HttpNotFound();
        }

        // POST: /Umowa/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        // public ActionResult Create([Bind(Include= "UmowaID,SilowniaID,DataPodpisania,DataZakonczenia,Cena,RecepcjonistaID")] long? id, Umowa umowa)
        public ActionResult Create([Bind(Include = "UmowaID,DataPodpisania,DataZakonczenia,RecepcjonistaID,Cena")] long? id, Umowa umowa)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" || Session["Auth"].ToString() == "Administrator")
                {

                    umowa.SilowniaID = Int32.Parse(Request["SilownieSelectLista"]);
                    umowa.RecepcjonistaID = Int32.Parse(Request["RecepcjonistaSelectLista"]);

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
            }
            return HttpNotFound();
        }

        bool aktywnaUmowa(long? klientID, DateTime umowaOd, DateTime umowaDo)
        {
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
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" || Session["Auth"].ToString() == "Administrator")
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
            }
            return HttpNotFound();
        }

        // POST: /Umowa/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "UmowaID,SilowniaID,DataPodpisania,DataZakonczenia,Cena")] Umowa umowa)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" || Session["Auth"].ToString() == "Administrator")
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
            }
            return HttpNotFound();
        }

        // GET: /Umowa/Delete/5
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
                    Umowa umowa = db.Umowy.Find(id);
                    if (umowa == null)
                    {
                        return HttpNotFound();
                    }
                    return View(umowa);
                }
            }
            return HttpNotFound();
        }

        // POST: /Umowa/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" || Session["Auth"].ToString() == "Administrator")
                {
                    Umowa umowa = db.Umowy.Find(id);
                    db.Umowy.Remove(umowa);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { akcja = AkcjaEnumUmowa.UsunietoUmowe });
                }
            }
            return HttpNotFound();
        }


        public ActionResult ListaSilowni()
        {
            List<SelectListItem> NazwySilowni = new List<SelectListItem>();
            DropDownListyViewModel UmowyWSilce = new DropDownListyViewModel();

            List<Models.Silownia> silownie = db.Silownie.ToList();
            silownie.ForEach(x =>
            {
                NazwySilowni.Add(new SelectListItem { Text = x.Nazwa, Value = x.SilowniaID.ToString() });
            });
            UmowyWSilce.SilownieSelectLista = NazwySilowni;
            return View(UmowyWSilce);

        }

        public ActionResult RecepcjonistaWSilowni(string SilkaId)
        {
            int silowniaId;
            List<SelectListItem> recepcjonista = new List<SelectListItem>();

            if (!string.IsNullOrEmpty(SilkaId))
            {
                silowniaId = Convert.ToInt32(SilkaId);
                List<Recepcjonista> recepcjonisciLista = db.Pracownicy.OfType<Recepcjonista>().Where(x => x.SilowniaID == silowniaId).ToList();
                recepcjonisciLista.ForEach(x =>
                {
                    recepcjonista.Add(new SelectListItem { Text = x.imieNazwisko, Value = x.OsobaID.ToString() });
                });
            }
            return Json(recepcjonista, JsonRequestBehavior.AllowGet);
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
