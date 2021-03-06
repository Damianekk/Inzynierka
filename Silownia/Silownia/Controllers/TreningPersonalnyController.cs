﻿using System;
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
using Silownia.ViewModel;

namespace Silownia.Controllers
{
    public class TreningPersonalnyController : Controller
    {
        private SilowniaContext db = new SilowniaContext();

        // GET: TreningPersonalny
        public ActionResult Index(string imieNazwisko, string SilowniaID, string TrenerID, string SpecjalizacjaID, bool czyPrzyszlosc = false, int page = 1, int pageSize = 10, AkcjaEnumTrening akcja = AkcjaEnumTrening.Brak, String info = null)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" || Session["Auth"].ToString() == "Administrator")
                {
                    ViewBag.SilowniaID = new SelectList(db.Silownie.DistinctBy(a => new { a.Nazwa }), "Nazwa", "Nazwa");
                    ViewBag.TrenerID = new SelectList(db.Trenerzy.DistinctBy(a => new { a.Pesel }), "imieNazwisko", "imieNazwisko");
                    ViewBag.SpecjalizacjaID = new SelectList(db.Specjalizacje.DistinctBy(a => new { a.Nazwa }), "Nazwa", "Nazwa");

                    var treningiPersonalne = from TreningiPersonalne in db.TreningiPersonalne select TreningiPersonalne;

                    if (!String.IsNullOrEmpty(imieNazwisko))
                        foreach (string wyraz in imieNazwisko.Split(' '))
                            treningiPersonalne = treningiPersonalne.Search(wyraz, i => i.Klient.Imie, i => i.Klient.Nazwisko);

                    treningiPersonalne = treningiPersonalne.Search(SilowniaID, i => i.Trener.Silownia.Nazwa);

                    if (!String.IsNullOrEmpty(TrenerID))
                        foreach (string wyraz in TrenerID.Split(' '))
                            treningiPersonalne = treningiPersonalne.Search(wyraz, i => i.Trener.Imie, i => i.Trener.Nazwisko);

                    treningiPersonalne = treningiPersonalne.Search(SpecjalizacjaID, i => i.Trener.Specjalizacja.Nazwa);

                    if (czyPrzyszlosc)
                        treningiPersonalne = treningiPersonalne.Where(u => u.TreningStart.Day >= DateTime.Now.Day);

                    var final = treningiPersonalne.OrderBy(p => p.Klient.Nazwisko);
                    var ileWynikow = treningiPersonalne.Count();
                    if ((ileWynikow / page) <= 1)
                    {
                        page = 1;
                    }
                    var kk = ileWynikow / page;

                    PagedList<TreningPersonalny> model = new PagedList<TreningPersonalny>(final, page, pageSize);

                    if (akcja != AkcjaEnumTrening.Brak)
                    {
                        ViewBag.info = info;
                        ViewBag.Akcja = akcja;
                    }

                    return View(model);
                }
            }
            return HttpNotFound();
        }

        // GET: TreningPersonalny/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" || Session["Auth"].ToString() == "Administrator")
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    TreningPersonalny treningPersonalny = db.TreningiPersonalne.Find(id);
                    if (treningPersonalny == null)
                    {
                        return HttpNotFound();
                    }
                    return View(treningPersonalny);
                }
            }
            return HttpNotFound();
        }

        // GET: TreningPersonalny/Create
        public ActionResult Create(long? id)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" || Session["Auth"].ToString() == "Administrator")
                {
                    ViewBag.TrenerID = new SelectList(db.Trenerzy, "OsobaID", "imieNazwisko");
                    var a = from Osoby in db.Trenerzy select Osoby;

                    Trener trener = null;
                    var user = User.Identity.GetUserName();
                    foreach (Trener trenr in a)
                    {
                        if (trenr.imieNazwisko.Replace(" ", "") == user)
                        {
                            trener = trenr;
                            break;
                        }

                        Osoba osoba = db.Osoby.Find(id);
                        ViewBag.Osoba = osoba;
                    }

                    return View();
                }
            }
            return HttpNotFound();
        }

        // POST: TreningPersonalny/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "SilowniaID,TreningID,TreningStart,TreningStartGodzina,CzasTrwania,TrenerID")] long? id, TreningPersonalny treningPersonalny)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" || Session["Auth"].ToString() == "Administrator")
                {
                    ViewBag.TrenerID = new SelectList(db.Trenerzy, "OsobaID", "imieNazwisko", treningPersonalny.TrenerID);
                    treningPersonalny.TrenerID = Int32.Parse(Request["PracownikSelectLista"]);


                    treningPersonalny.TreningStart = treningPersonalny.TreningStart.AddHours(System.Convert.ToDouble(treningPersonalny.TreningStartGodzina.Hour));
                    treningPersonalny.TreningStart = treningPersonalny.TreningStart.AddMinutes(System.Convert.ToDouble(treningPersonalny.TreningStartGodzina.Minute));
                    treningPersonalny.TreningKoniec = treningPersonalny.TreningStart.AddMinutes(System.Convert.ToDouble(treningPersonalny.CzasTrwania));

                    if (ModelState.IsValid && !aktywneZajecia(id, treningPersonalny.TreningStart) && !zajetyTrener(treningPersonalny.TrenerID, treningPersonalny.TreningStart))
                    {
                        #region Klient
                        Klient klient = db.Klienci.Find(id);
                        treningPersonalny.Klient = klient;
                        klient.TreningiPersonalne.Add(treningPersonalny);
                        #endregion

                        #region Trener
                        Trener trener = db.Trenerzy.Find(treningPersonalny.TrenerID);
                        treningPersonalny.Trener = trener;
                        trener.TreningiPersonalne.Add(treningPersonalny);
                        #endregion

                        treningPersonalny.kosztTreningu = (treningPersonalny.CzasTrwania * treningPersonalny.Trener.StawkaGodzinowa) / 60;


                        db.TreningiPersonalne.Add(treningPersonalny);
                        db.SaveChanges();

                        return RedirectToAction("Index", new { akcja = AkcjaEnumTrening.DodanoTrening, info = klient.imieNazwisko });
                    }
                    return View(treningPersonalny);
                }
            }
            return HttpNotFound();
        }

        bool aktywneZajecia(long? klientID, DateTime dataOd)
        {
            var checkTrening = db.TreningiPersonalne.Where(o => o.Klient.OsobaID == klientID && dataOd >= o.TreningStart && dataOd < o.TreningKoniec).ToList();
            var checkMasaz = db.Masaze.Where(o => o.Klient.OsobaID == klientID && dataOd >= o.DataMasazu && dataOd < o.DataMasazuKoniec).ToList();

            if (checkTrening.Count == 1)
            {
                TempData["msg"] = "<script>alert('Klient ma już umówiony trening w tym terminie');</script>";
                return true; // klient ma trening w tym terminie
            }
            else if (checkMasaz.Count == 1)
            {
                TempData["msg"] = "<script>alert('Klient ma już umówiony masaż w tym terminie');</script>";
                return true; // klient ma masaż w tym terminie
            }

            else return false; // klient jest wolny w terminie
        }

        bool zajetyTrener(long? TrenerID, DateTime dataOd)
        {
            var check = db.TreningiPersonalne.Where(o => o.Trener.OsobaID == TrenerID && dataOd >= o.TreningStart && dataOd < o.TreningKoniec).ToList();

            if (check.Count == 1)
            {
                TempData["msg"] = "<script>alert('Trener ma już zaplanowany trening personalny z kimś innym w tym terminie.');</script>";
                return true; //zajęty
            }
            else return false; // wolny
        }

        // GET: TreningPersonalny/Edit/5
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
                    TreningPersonalny treningPersonalny = db.TreningiPersonalne.Find(id);
                    if (treningPersonalny == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.TrenerID = new SelectList(db.Trenerzy, "OsobaID", "imieNazwisko", treningPersonalny.TrenerID);
                    return View(treningPersonalny);
                }
            }
            return HttpNotFound();
        }

        // POST: TreningPersonalny/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "TreningID,TreningStart,TreningStartGodzina,CzasTrwania,TrenerID")] TreningPersonalny treningPersonalny)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" || Session["Auth"].ToString() == "Administrator")
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(treningPersonalny).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    ViewBag.TrenerID = new SelectList(db.Trenerzy, "OsobaID", "imieNazwisko", treningPersonalny.TrenerID);
                    return View(treningPersonalny);
                }
            }
            return HttpNotFound();
        }

        // GET: TreningPersonalny/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" || Session["Auth"].ToString() == "Administrator")
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    TreningPersonalny treningPersonalny = db.TreningiPersonalne.Find(id);
                    if (treningPersonalny == null)
                    {
                        return HttpNotFound();
                    }
                    return View(treningPersonalny);
                }
            }
            return HttpNotFound();
        }

        // POST: TreningPersonalny/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" || Session["Auth"].ToString() == "Administrator")
                {
                    TreningPersonalny treningPersonalny = db.TreningiPersonalne.Find(id);
                    db.TreningiPersonalne.Remove(treningPersonalny);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { akcja = AkcjaEnumTrening.UsunietoTrening });
                }
            }
            return HttpNotFound();
        }

        public ActionResult ZapisKlient(long? id)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Klient" || Session["Auth"].ToString() == "Klient")
                {
                    ViewBag.TrenerID = new SelectList(db.Trenerzy, "OsobaID", "imieNazwisko");
                    var a = from Osoby in db.Trenerzy select Osoby;

                    Trener trener = null;
                    var user = User.Identity.GetUserName();
                    foreach (Trener trenr in a)
                    {
                        if (trenr.imieNazwisko.Replace(" ", "") == user)
                        {
                            trener = trenr;
                            break;
                        }

                        Osoba osoba = db.Osoby.Find(id);
                        ViewBag.Osoba = osoba;
                    }

                    return View();
                }
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult ZapisKlient([Bind(Include = "TreningID,TreningStart,TreningStartGodzina,CzasTrwania,TrenerID, SilowniaID")] TreningPersonalny treningPersonalny)
        {
            long loggedUsID = (long)Session["loggedUserID"];
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Klient" || Session["Auth"].ToString() == "Klient")
                {
                    
                    treningPersonalny.TrenerID = Int32.Parse(Request["PracownikSelectLista"]);

                    treningPersonalny.TreningStart = treningPersonalny.TreningStart.AddHours(System.Convert.ToDouble(treningPersonalny.TreningStartGodzina.Hour));
                    treningPersonalny.TreningStart = treningPersonalny.TreningStart.AddMinutes(System.Convert.ToDouble(treningPersonalny.TreningStartGodzina.Minute));
                    treningPersonalny.TreningKoniec = treningPersonalny.TreningStart.AddMinutes(System.Convert.ToDouble(treningPersonalny.CzasTrwania));
                  

                    if (ModelState.IsValid && !aktywneZajecia(loggedUsID, treningPersonalny.TreningStart) && !zajetyTrener(treningPersonalny.TrenerID, treningPersonalny.TreningStart))
                    {
                        #region Klient
                        Klient klient = db.Klienci.Find(loggedUsID);
                        treningPersonalny.Klient = klient;
                        klient.TreningiPersonalne.Add(treningPersonalny);
                        #endregion

                        #region Trener
                        Trener trener = db.Trenerzy.Find(treningPersonalny.TrenerID);
                        treningPersonalny.Trener = trener;
                        trener.TreningiPersonalne.Add(treningPersonalny);
                        #endregion

                        treningPersonalny.kosztTreningu = (treningPersonalny.CzasTrwania * treningPersonalny.Trener.StawkaGodzinowa) / 60;

                        db.TreningiPersonalne.Add(treningPersonalny);
                        db.SaveChanges();

                        return RedirectToAction("Index", "KlientView", new { akcja = AkcjaEnumTrening.DodanoTrening }); // Póki co masaż bo czasu brak :) plan taki aby klient przyjmowal parametr po którym wszystkie enumy ktore klient moze przyjac dziedzicza (ale nie wiem jeszcze czy to bd dzialac)
                                                                                                                    // Pozwoli to na dawanie "dowolnego" enuma do kontrolera - warunek musi dziedziczyc po jakiejs nowej klasie (trzeba utworzyc) 
                    }
                    return View(treningPersonalny);
                }
            }
            return HttpNotFound();
        }

        public ActionResult ListaSilowni()
        {
            List<SelectListItem> NazwySilowni = new List<SelectListItem>();
            DropDownListyViewModel TrenerzyWSilce = new DropDownListyViewModel();

            List<Models.Silownia> silownie = db.Silownie.ToList();
            silownie.ForEach(x =>
            {
                NazwySilowni.Add(new SelectListItem { Text = x.Nazwa, Value = x.SilowniaID.ToString() });
            });
            TrenerzyWSilce.SilownieSelectLista = NazwySilowni;
            return View(TrenerzyWSilce);

        }

        public ActionResult TrenerWSilowni(string SilkaId)
        {
            int silowniaId;
            List<SelectListItem> trenerzy = new List<SelectListItem>();

            if (!string.IsNullOrEmpty(SilkaId))
            {
                silowniaId = Convert.ToInt32(SilkaId);
                List<Trener> trenerzyLista = db.Pracownicy.OfType<Trener>().Where(x => x.SilowniaID == silowniaId).ToList();
                trenerzyLista.ForEach(x =>
                {
                    trenerzy.Add(new SelectListItem { Text = x.imieNazwisko, Value = x.OsobaID.ToString() });
                });
            }
            return Json(trenerzy, JsonRequestBehavior.AllowGet);
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
