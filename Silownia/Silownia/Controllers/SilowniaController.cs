using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Silownia.DAL;
using System.Web.Script.Serialization;
using System;
using PagedList;
using Silownia.Models;
using System.Data;
using System.Collections.Generic;

namespace Silownia.Controllers
{
    public class SilowniaController : Controller
    {
        private SilowniaContext db = new SilowniaContext();

        // GET: /Silownia/
        public ActionResult Index(string Miasto, int page = 1, int pageSize = 10, AkcjaEnumSilownia akcja = AkcjaEnumSilownia.Brak, String info = null)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" || Session["Auth"].ToString() == "Administrator")
                {
                    var Miasta = db.Silownie.Where(u => (u.SilowniaID != null) && (u.Adres != null)).DistinctBy(a => new { a.Adres.Miasto }).Select(x => x.Adres);


                    ViewBag.Miasto = new SelectList(Miasta, "Miasto", "Miasto");

                    var silownie = from Silownie in db.Silownie select Silownie;
                    silownie = silownie.Search(Miasto, m => m.Adres.Miasto);

                    var final = silownie.OrderBy(p => p.Nazwa);
                    var ileWynikow = silownie.Count();
                    if ((ileWynikow / page) <= 1)
                    {
                        page = 1;
                    }
                    var kk = ileWynikow / page;

                    PagedList<Models.Silownia> model = new PagedList<Models.Silownia>(final, page, pageSize);


                    if (akcja != AkcjaEnumSilownia.Brak)
                        ViewBag.Akcja = akcja;

                    if (info != null)
                        ViewBag.info = info;

                    return View(model);
                }
            }
            return HttpNotFound();
        }

        // GET: /Silownia/Details/5
        public ActionResult Details(long? id, bool? mniejSzczegolow = false, bool? mniejszaMapa = false)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" || Session["Auth"].ToString() == "Administrator")
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }

                    Models.Silownia silownia = db.Silownie.Find(id);
                    ViewBag.mniejSzczegolow = mniejSzczegolow;
                    ViewBag.mniejszaMapa = mniejszaMapa;

                    if (silownia == null)
                    {
                        return HttpNotFound();
                    }
                    return View(silownia);
                }
            }
            return HttpNotFound();
        }

        // GET: /Silownia/Create
        [MyAuthorize(RoleEnum.Administrator)]
        public ActionResult Create()
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" || Session["Auth"].ToString() == "Administrator")
                {
                    return View();
                }
            }
            return HttpNotFound();
        }

        // POST: /Silownia/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [MyAuthorize(RoleEnum.Administrator)]
        public ActionResult Create([Bind(Include = "SilowniaID,Nazwa,GodzinaOtwarcia,GodzinaZamkniecia,Powierzchnia,NrTelefonu,DodatkoweInfo")] Models.Silownia silownia)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" || Session["Auth"].ToString() == "Administrator")
                {
                    if (ModelState.IsValid)
                    {
                        db.Silownie.Add(silownia);
                        db.SaveChanges();
                        return RedirectToAction("Create", "Adres", new { id = silownia.SilowniaID, komu = KomuAdres.Silownia, akcja = AkcjaEnumSilownia.DodanoSilownie, info = silownia.Nazwa });
                    }
                    return View(silownia);
                }
            }
            return HttpNotFound();
        }

        // GET: /Silownia/Edit/5
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
                    Models.Silownia silownia = db.Silownie.Find(id);

                    if (silownia == null)
                    {
                        return HttpNotFound();
                    }
                    return View(silownia);
                }
            }
            return HttpNotFound();
        }

        // POST: /Silownia/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "SilowniaID,Nazwa,GodzinaOtwarcia,GodzinaZamkniecia,Powierzchnia,NrTelefonu,DodatkoweInfo")] Models.Silownia silownia)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" || Session["Auth"].ToString() == "Administrator")
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(silownia).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    return View(silownia);
                }
            }
            return HttpNotFound();
        }

        // GET: /Silownia/Delete/5
        [MyAuthorize(RoleEnum.Administrator)]
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
                    Models.Silownia silownia = db.Silownie.Find(id);
                    if (silownia == null)
                    {
                        return HttpNotFound();
                    }
                    return View(silownia);
                }
            }
            return HttpNotFound();
        }

        // POST: /Silownia/Delete/5
        [HttpPost, ActionName("Delete")]
        [MyAuthorize(RoleEnum.Administrator)]
        public ActionResult DeleteConfirmed(long id)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Recepcjonista" || Session["Auth"].ToString() == "Administrator")
                {
                    Models.Silownia silownia = db.Silownie.Find(id);
                    db.Silownie.Remove(silownia);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { akcja = AkcjaEnumSilownia.UsunietoSilownie, info = silownia.Nazwa });
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
        
        [HttpPost]
        public JsonResult SilowniaInfoJSON()
        {

            var jsonSerialiser = new JavaScriptSerializer();
            var silownie = db.Silownie.Where(s => s.Adres != null).ToList<Silownia.Models.Silownia>();

            //  silownie.RemoveAll(item => item.Adres != null);
            var z = silownie.Select(x => new
                        {
                            dlugosc = x.Dlugosc,
                            szerokosc = x.Szerokosc,
                            godzOtw = x.GodzinaOtwarcia,
                            godzZam = x.GodzinaZamkniecia,
                            nazwa = x.Nazwa,
                            info = x.infoDodatkowe

                        });
            return Json(z);

          //  return Json(new { ok = true, myData = silownie }, JsonRequestBehavior.AllowGet);
        }
    }
}
