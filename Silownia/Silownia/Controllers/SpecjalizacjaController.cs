using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Silownia.Models;
using Silownia.DAL;

namespace Silownia.Controllers
{
    public class SpecjalizacjaController : Controller
    {
        private SilowniaContext db = new SilowniaContext();

        // GET: /Specjalizacja/
        public ActionResult Index()
        {
            if (Session["User"] != null)
            {
                return View(db.Specjalizacje.ToList());
            }
            return HttpNotFound();
        }

        // GET: /Specjalizacja/Details/5
        public ActionResult Details(long? id)
        {
            if (Session["User"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Specjalizacja specjalizacja = db.Specjalizacje.Find(id);
                if (specjalizacja == null)
                {
                    return HttpNotFound();
                }
                return View(specjalizacja);
            }
            return HttpNotFound();
        }

        // GET: /Specjalizacja/Create
        public ActionResult Create()
        {
            if (Session["User"] != null)
            {
                return View();
            }
            return HttpNotFound();
        }

        // POST: /Specjalizacja/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include="SpecjalizacjaID,Nazwa")] Specjalizacja specjalizacja)
        {
            if (Session["User"] != null)
            {
                if (ModelState.IsValid)
                {
                    db.Specjalizacje.Add(specjalizacja);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(specjalizacja);
            }
            return HttpNotFound();
        }

        // GET: /Specjalizacja/Edit/5
        public ActionResult Edit(long? id)
        {
            if (Session["User"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Specjalizacja specjalizacja = db.Specjalizacje.Find(id);
                if (specjalizacja == null)
                {
                    return HttpNotFound();
                }
                return View(specjalizacja);
            }
            return HttpNotFound();
        }

        // POST: /Specjalizacja/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include="SpecjalizacjaID,Nazwa")] Specjalizacja specjalizacja)
        {
            if (Session["User"] != null)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(specjalizacja).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(specjalizacja);
            }
            return HttpNotFound();
        }

        // GET: /Specjalizacja/Delete/5
        public ActionResult Delete(long? id)
        {
           if (Session["User"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Specjalizacja specjalizacja = db.Specjalizacje.Find(id);
                if (specjalizacja == null)
                {
                    return HttpNotFound();
                }
                return View(specjalizacja);
            }
            return HttpNotFound();
        }

        // POST: /Specjalizacja/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            if (Session["User"] != null)
            {
                Specjalizacja specjalizacja = db.Specjalizacje.Find(id);
                db.Specjalizacje.Remove(specjalizacja);
                db.SaveChanges();
                return RedirectToAction("Index");
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
