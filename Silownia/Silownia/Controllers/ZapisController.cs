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
    public class ZapisController : Controller
    {
        private SilowniaContext db = new SilowniaContext();

        // GET: Zapis
        public ActionResult Index(int page = 1, int pageSize = 10, AkcjaZapisEnum akcja = AkcjaZapisEnum.Brak)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Klient")
                {
                    var zajeciaGrup = from ZajeciaGrupowe in db.ZajeciaGrup select ZajeciaGrupowe;
                    var final = zajeciaGrup.OrderBy(p => p.Instruktor.Nazwisko);
                    var ileWynikow = zajeciaGrup.Count();
                    if ((ileWynikow / page) <= 1)
                    {
                        page = 1;
                    }
                    var kk = ileWynikow / page;

                    PagedList<ZajeciaGrupowe> model = new PagedList<ZajeciaGrupowe>(final, page, pageSize);

                    if (akcja != AkcjaZapisEnum.Brak)
                    {
                        ViewBag.Akcja = akcja;
                    }

                    return View(model);
                }
            }
            return HttpNotFound();
        }

        // POST: ZajeciaGrup/Zapis/5
        public ActionResult Save(int? id)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Klient")
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
                    string login = Session["User"].ToString();
                    Uzytkownik uzytkownik = db.Uzytkownicy.Where(s => s.Login == login).First();

                    Klient klient = db.Klienci.Find(uzytkownik.IDOsoby);

                    if (klient.KlienciTreningiGrupowe.Where(s => s.TreningID == zajecia.TreningID).Count() != 0)
                    {
                        var trening = klient.KlienciTreningiGrupowe.Where(s => s.TreningID == zajecia.TreningID).First();
                        if (klient.KlienciTreningiGrupowe.Contains(trening) == true)
                        {
                            return RedirectToAction("Index", new { akcja = AkcjaZapisEnum.JuzZapis });
                        }
                    }
                    KlientZajeciaGrupowe zajeciagrup = new KlientZajeciaGrupowe();
                    zajeciagrup.Klient = klient;
                    zajeciagrup.OsobaID = zajecia.Instruktor.OsobaID;
                    zajeciagrup.TreningID = zajecia.TreningID;
                    zajeciagrup.ZajeciaGrupowe = zajecia;

                    klient.KlienciTreningiGrupowe.Add(zajeciagrup);
                    db.SaveChanges();

                    zajecia.KlientZajeciaGrupowe.Add(zajeciagrup);
                    db.SaveChanges();

                    return RedirectToAction("Index", new { akcja = AkcjaZapisEnum.DodanoZapis });
                }
            }
            return HttpNotFound();
        }

        public ActionResult Remove(int? id)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Klient")
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
                    string login = Session["User"].ToString();
                    Uzytkownik uzytkownik = db.Uzytkownicy.Where(s => s.Login == login).First();

                    Klient klient = db.Klienci.Find(uzytkownik.IDOsoby);

                    if (klient.KlienciTreningiGrupowe.Where(s => s.TreningID == zajecia.TreningID).Count() != 0)
                    {
                        var trening = klient.KlienciTreningiGrupowe.Where(s => s.TreningID == zajecia.TreningID).First();
                        if (klient.KlienciTreningiGrupowe.Contains(trening))
                        {
                            klient.KlienciTreningiGrupowe.Remove(trening);
                            db.SaveChanges();

                            zajecia.KlientZajeciaGrupowe.Remove(trening);
                            db.SaveChanges();

                            return RedirectToAction("Index", new { akcja = AkcjaZapisEnum.UsunietoZapis });
                        }
                    }
                    return RedirectToAction("Index", new { akcja = AkcjaZapisEnum.NieDaSieZapis });
                }
            }
            return HttpNotFound();
        }

        // GET: ZajeciaGrup/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Klient")
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
