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
    public class ZajeciaGrupController : Controller
    {
        private SilowniaContext db = new SilowniaContext();

        // GET: ZajęciaGrupowe
        public ActionResult Index(string SilowniaID, bool czyPrzyszlosc = false, int page = 1, int pageSize = 10, AkcjaEnumTrening akcja = AkcjaEnumTrening.Brak, String info = null)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" | Session["Auth"].ToString() == "Administrator")
                {
                    ViewBag.SilowniaID = new SelectList(db.Silownie.DistinctBy(a => new { a.Nazwa }), "Nazwa", "Nazwa");

                    var zajeciaGrup = from ZajeciaGrupowe in db.ZajeciaGrup select ZajeciaGrupowe;

                    zajeciaGrup = zajeciaGrup.Search(SilowniaID, i => i.Instruktor.Silownia.Nazwa);

                    if (czyPrzyszlosc)
                        zajeciaGrup = zajeciaGrup.Where(u => u.TreningStart.Day >= DateTime.Now.Day);

                    var final = zajeciaGrup.OrderBy(p => p.Instruktor.Nazwisko);
                    var ileWynikow = zajeciaGrup.Count();
                    if ((ileWynikow / page) <= 1)
                    {
                        page = 1;
                    }
                    var kk = ileWynikow / page;

                    PagedList<ZajeciaGrupowe> model = new PagedList<ZajeciaGrupowe>(final, page, pageSize);


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

        // GET: ZajeciaGrup/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" | Session["Auth"].ToString() == "Administrator")
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    ZajeciaGrupowe zajecia = db.ZajeciaGrup.Find(id);
                    if (zajecia == null)
                    {
                        return HttpNotFound();
                    }
                    return View(zajecia);
                }
            }
            return HttpNotFound();
        }

        // GET: ZajeciaGrup/Create
        public ActionResult Create(long? id)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" | Session["Auth"].ToString() == "Administrator")
                {
                    ViewBag.InstruktorID = new SelectList(db.Instruktorzy, "OsobaID", "imieNazwisko");
                    ViewBag.SalaID = new SelectList(db.Sale, "Numer_sali", "Rodzaj");
                    var a = from Osoby in db.Instruktorzy select Osoby;

                    Instruktor instruktor = null;
                    var user = User.Identity.GetUserName();
                    foreach (Instruktor instr in a)
                    {
                        if (instr.imieNazwisko.Replace(" ", "") == user)
                        {
                            instruktor = instr;
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

        // POST: ZajeciaGrup/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "TreningID,TreningStart,TreningStartGodzina,CzasTrwania,InstruktorID,SalaID,NazwaTreningu,OpisTreningu")] long? id, ZajeciaGrupowe zajecia)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" | Session["Auth"].ToString() == "Administrator")
                {
                    ViewBag.InstruktorID = new SelectList(db.Instruktorzy, "OsobaID", "imieNazwisko", zajecia.InstruktorID);
                    ViewBag.SalaID = new SelectList(db.Sale, "Numer_sali", "Numer_sali", zajecia.SalaID);

                    zajecia.TreningStart = zajecia.TreningStart.AddHours(System.Convert.ToDouble(zajecia.TreningStartGodzina.Hour));
                    zajecia.TreningStart = zajecia.TreningStart.AddMinutes(System.Convert.ToDouble(zajecia.TreningStartGodzina.Minute));
                    zajecia.TreningKoniec = zajecia.TreningStart.AddMinutes(System.Convert.ToDouble(zajecia.CzasTrwania));

                    if (ModelState.IsValid && !zajetyTrener(zajecia.InstruktorID, zajecia.TreningStart) && !zajetaSala(zajecia.SalaID, zajecia.TreningStart))
                    {
                        #region Instruktor
                        Instruktor instruktor = db.Instruktorzy.Find(zajecia.InstruktorID);
                        zajecia.Instruktor = instruktor;
                        instruktor.ZajeciaGrup.Add(zajecia);
                        #endregion

                        #region Sala
                        Sala sala = db.Sale.Find(zajecia.SalaID);
                        zajecia.Sala = sala;
                        sala.ZajeciaGrup.Add(zajecia);
                        #endregion

                        //zajecia.TreningStart = zajecia.TreningStart.AddHours(System.Convert.ToDouble(zajecia.TreningStartGodzina.Hour));
                        //zajecia.TreningStart = zajecia.TreningStart.AddMinutes(System.Convert.ToDouble(zajecia.TreningStartGodzina.Minute));
                        //zajecia.TreningKoniec = zajecia.TreningStart.AddMinutes(System.Convert.ToDouble(zajecia.CzasTrwania));

                        db.ZajeciaGrup.Add(zajecia);
                        db.SaveChanges();

                        return RedirectToAction("Index", new { akcja = AkcjaEnumTrening.DodanoTrening, info = " " });
                    }
                    return View(zajecia);
                }
            }
            return HttpNotFound();
        }

        bool zajetyTrener(long instruktor, DateTime dataOd)
        {
            var check = db.ZajeciaGrup.Where(o => o.Instruktor.OsobaID == instruktor && dataOd >= o.TreningStart && dataOd <= o.TreningKoniec).ToList();

            if (check.Count == 1)
            {
                TempData["msg"] = "<script>alert('Instruktor ma już zaplanowane zajęcia w tym terminie.');</script>";
                return true; //zajęty
            }
            else return false; // wolny
        }

        bool zajetaSala(long sala, DateTime dataOd)
        {
            var check = db.ZajeciaGrup.Where(o => o.Sala.Numer_sali == sala && dataOd >= o.TreningStart && dataOd <= o.TreningKoniec).ToList();

            if (check.Count == 1)
            {
                TempData["msg"] = "<script>alert('Sala jest zajęta w tym terminie.');</script>";
                return true; //zajęty
            }
            else return false; // wolny
        }

        // GET: ZajeciaGrup/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" | Session["Auth"].ToString() == "Administrator")
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    ZajeciaGrupowe zajecia = db.ZajeciaGrup.Find(id);
                    if (zajecia == null)
                    {
                        return HttpNotFound();
                    }
                    return View(zajecia);
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
                if (Session["Auth"].ToString() == "Recepcjonista" | Session["Auth"].ToString() == "Administrator")
                {
                    ZajeciaGrupowe zajecia = db.ZajeciaGrup.Find(id);
                    db.ZajeciaGrup.Remove(zajecia);
                    db.SaveChanges();

                    return RedirectToAction("Index", new { akcja = AkcjaEnumTrening.UsunietoTrening });
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
