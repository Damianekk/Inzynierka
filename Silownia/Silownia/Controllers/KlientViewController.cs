﻿using Silownia.DAL;
using Silownia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Silownia.Helpers;

namespace Silownia.Controllers
{
    public class KlientViewController : Controller
    {
        private SilowniaContext db = new SilowniaContext();
        public ActionResult Index(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Trenerzy = new SelectList(db.Trenerzy, "OsobaID", "imieNazwisko");
            Klient klient = db.Klienci.Find(id);

            var wiad = db.Wiadomosci.Where(o => o.OsobaOdbierajaca.OsobaID == id);

            
            foreach (Wiadomosc w in wiad)
            {
                w.Status = StatusWiadomosciEnum.Odebrany;
                w.Odebrano = DateTime.Now;
            }

            if (wiad.Count() > 0)
                ViewBag.Wiad = wiad.ToList<Wiadomosc>();
                
            else
                ViewBag.Wiad = null;
        
            if (klient == null)
            {
                return HttpNotFound();
            }
            var z = klient;
            
            return View(z);
        }

        public ActionResult Act(string username,long trenerID)
        {
            var loginInfo = "Lasecka";// User.Identity.Name;
            var osWys =from Osoby 
                    in db.Klienci
                    where Osoby.Nazwisko == loginInfo
                    select Osoby;
            var osOdb = db.Osoby.Find(trenerID);

            Wiadomosc wiadomosc = new Wiadomosc
            {
                OsobaWysylajaca = osWys.First(),
                OsobaOdbierajaca = osOdb,
                Tresc = username,
                Wyslano = DateTime.Now,
                Odebrano = null,
                Status = StatusWiadomosciEnum.Wyslany
            };

            db.Wiadomosci.Add(wiadomosc);
            db.SaveChanges();
            
          
            return null;
        }

    }
}