using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Silownia.Models;
using Silownia.DAL;
using System.IO;
using PagedList;
using Silownia.Helpers;

namespace Silownia.Controllers
{
    public class SalaController : Controller
    {
        private SilowniaContext db = new SilowniaContext();

        // GET: /Sala/
        public ActionResult Index(string nazwa, string SilowniaID, int page = 1, int pageSize = 10, AkcjaEnumSprzet akcja = AkcjaEnumSprzet.Brak, String info = null)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" | Session["Auth"].ToString() == "Administrator")
                {
                    ViewBag.SilowniaID = new SelectList(db.Silownie.DistinctBy(a => new { a.Nazwa }), "Nazwa", "Nazwa");

                    var sala = from Sale in db.Sale select Sale;

                    sala = sala.Search(SilowniaID, i => i.Silownia.Nazwa);

                    var final = sala.OrderBy(p => p.Rodzaj);
                    var ileWynikow = sala.Count();
                    if ((ileWynikow / page) <= 1)
                    {
                        page = 1;
                    }
                    var kk = ileWynikow / page;

                    PagedList<Sala> model = new PagedList<Sala>(final, page, pageSize);

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

         // GET: /Sala/Details/5
        public ActionResult Details(long? id)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" | Session["Auth"].ToString() == "Administrator")
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Sala sala = db.Sale.Find(id);
                    if (sala == null)
                    {
                        return HttpNotFound();
                    }
                    return View(sala);
                }
            }
            return HttpNotFound();
        }

        // GET: /Sala/Create
        public ActionResult Create()
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" | Session["Auth"].ToString() == "Administrator")
                {
                    ViewBag.SilowniaID = new SelectList(db.Silownie, "SilowniaID", "Nazwa");
                    return View();
                }
            }
            return HttpNotFound();
        }

        // POST: /Sala/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(long? id, [Bind(Include = "Numer_sali,Rodzaj,Status,Opis,Zdjecie,SilowniaID")] Sala sala, HttpPostedFileBase file)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" | Session["Auth"].ToString() == "Administrator")
                {
                    if (ModelState.IsValid)
                    {
                        ViewBag.SilowniaID = new SelectList(db.Silownie.DistinctBy(a => new { a.Nazwa }), "Nazwa", "Nazwa");
                        #region Silownia
                        Models.Silownia silownia = db.Silownie.Find(id);
                        sala.Silownia = silownia;
                        silownia.Sale.Add(sala);
                        #endregion
                        string FileName = "";
                        byte[] bytes;

                        int BytesToRead;

                        int numBytesRead;

                        if (file != null)
                        {
                            FileName = Path.GetFileName(file.FileName);
                            bytes = new byte[file.ContentLength];
                            BytesToRead = (int)file.ContentLength;
                            numBytesRead = 0;

                            while (BytesToRead > 0)
                            {
                                int n = file.InputStream.Read(bytes, numBytesRead, BytesToRead);

                                if (n == 0)
                                {
                                    break;
                                }
                                numBytesRead += n;
                                BytesToRead -= n;
                            }
                            sala.Zdjecie = bytes;

                        }
                        db.Sale.Add(sala);
                        db.SaveChanges();

                        return RedirectToAction("Index", new { akcja = AkcjaEnumSala.DodanoSale, info = sala.Numer_sali });
                    }
                    
                    
                    return View(sala);
                }
            }
            return HttpNotFound();
        }

        public ActionResult Zdjecie(int id, int img)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" | Session["Auth"].ToString() == "Administrator")
                {
                    ViewBag.Pic = img;
                    Sala foto = db.Sale.Find(id);
                    return View(foto);
                }
            }
            return HttpNotFound();
        }



        // GET: /Sala/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" | Session["Auth"].ToString() == "Administrator")
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Sala sala = db.Sale.Find(id);
                    if (sala == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.SilowniaID = new SelectList(db.Silownie, "SilowniaID", "Nazwa", sala.Silownia);
                    return View(sala);
                }
            }
            return HttpNotFound();
        }

        // POST: /Sala/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Numer_sali,Rodzaj,Status,Opis,Zdjecie,SilowniaID")] Sala sala)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" | Session["Auth"].ToString() == "Administrator")
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(sala).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    ViewBag.SilowniaID = new SelectList(db.Silownie, "SilowniaID", "Nazwa", sala.Silownia);
                    return View(sala);
                }
            }
            return HttpNotFound();
        }

        // GET: /Sala/Delete/5
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
                    Sala sala = db.Sale.Find(id);
                    if (sala == null)
                    {
                        return HttpNotFound();
                    }
                    return View(sala);
                }
            }
            return HttpNotFound();
        }

        // POST: /Sala/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" | Session["Auth"].ToString() == "Administrator")
                {
                    Sala sala = db.Sale.Find(id);
                    db.Sale.Remove(sala);
                    db.SaveChanges();
                    return RedirectToAction("Index");
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
