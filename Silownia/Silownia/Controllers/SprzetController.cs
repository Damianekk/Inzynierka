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
    public class SprzetController : Controller
    {
        private SilowniaContext db = new SilowniaContext();

        // GET: Sprzet
        public ActionResult Index(string nazwa, string SilowniaID,  int page = 1, int pageSize = 10, AkcjaEnumSprzet akcja = AkcjaEnumSprzet.Brak, String info = null)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" || Session["Auth"].ToString() == "Administrator")
                {
                    ViewBag.SilowniaID = new SelectList(db.Silownie.DistinctBy(a => new { a.Nazwa }), "Nazwa", "Nazwa");

                    var sprzety = from Sprzety in db.Sprzety select Sprzety;

                    if (!String.IsNullOrEmpty(nazwa))
                        sprzety = sprzety.Search(nazwa, i => i.Nazwa);

                    sprzety = sprzety.Search(SilowniaID, i => i.Sala.Silownia.Nazwa);

                    var final = sprzety.OrderBy(p => p.Nazwa);
                    var ileWynikow = sprzety.Count();
                    if ((ileWynikow / page) <= 1)
                    {
                        page = 1;
                    }
                    var kk = ileWynikow / page;

                    PagedList<Sprzet> model = new PagedList<Sprzet>(final, page, pageSize);

                    if (akcja != AkcjaEnumSprzet.Brak)
                    {
                        ViewBag.info = info;
                        ViewBag.Akcja = akcja;
                    }

                    return View(model);
                }
            }
            return HttpNotFound();
        }

        // GET: Sprzet/Details/5
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
                    Sprzet sprzet = db.Sprzety.Find(id);
                    if (sprzet == null)
                    {
                        return HttpNotFound();
                    }
                    return View(sprzet);
                }
            }
            return HttpNotFound();
        }

        // GET: Sprzet/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sprzet/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SprzetID,Nazwa,Data_zakupu,Cena_zakupu")] long? id, Sprzet sprzet)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" || Session["Auth"].ToString() == "Administrator")
                {
                    if (ModelState.IsValid)
                    {
                        #region Sala
                        Sala sala = db.Sale.Find(id);
                        sprzet.Sala = sala;
                        sala.Sprzety.Add(sprzet);
                        #endregion

                        db.Sprzety.Add(sprzet);
                        db.SaveChanges();

                        return RedirectToAction("Index", new { akcja = AkcjaEnumSprzet.DodanoSprzet, info = sala.Numer_sali });
                    }
                    return View(sprzet);
                }
            }
            return HttpNotFound();
        }

        // GET: Sprzet/Edit/5
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
                    Sprzet sprzet = db.Sprzety.Find(id);
                    if (sprzet == null)
                    {
                        return HttpNotFound();
                    }
                    return View(sprzet);
                }
            }
            return HttpNotFound();
        }

        // POST: Sprzet/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SprzetID,Nazwa,Data_zakupu,Cena_zakupu")] Sprzet sprzet)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" || Session["Auth"].ToString() == "Administrator")
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(sprzet).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    return View(sprzet);
                }
            }
            return HttpNotFound();
        }

        // GET: Sprzet/Delete/5
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
                    Sprzet sprzet = db.Sprzety.Find(id);
                    if (sprzet == null)
                    {
                        return HttpNotFound();
                    }
                    return View(sprzet);
                }
            }
            return HttpNotFound();
        }

        // POST: Sprzet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" || Session["Auth"].ToString() == "Administrator")
                {
                    Sprzet sprzet = db.Sprzety.Find(id);
                    db.Sprzety.Remove(sprzet);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { akcja = AkcjaEnumSprzet.UsunietoSprzet });
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
