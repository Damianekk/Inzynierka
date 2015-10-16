using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Silownia.DAL;
using GoogleMaps.LocationServices;

namespace Silownia.Controllers
{
    public class SilowniaController : Controller
    {
        private SilowniaContext db = new SilowniaContext();

        // GET: /Silownia/
        public ActionResult Index()
        {
            return View(db.Silownie.ToList());
        }

        // GET: /Silownia/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Silownia silownia = db.Silownie.Find(id);
            ViewBag.Dlugosc = silownia.Dlugosc;
            ViewBag.Szerokosc = silownia.Szerokosc;

            if (silownia == null)
            {
                return HttpNotFound();
            }
            return View(silownia);
        }

        // GET: /Silownia/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Silownia/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SilowniaID,Nazwa,GodzinaOtwarcia,GodzinaZamkniecia,Powierzchnia,NrTelefonu")] Models.Silownia silownia)
        {
            var address = "Złota 44, Warszawa";

            var locationService = new GoogleLocationService();
            var point = locationService.GetLatLongFromAddress(address);

            var latitude = point.Latitude;
            var longitude = point.Longitude;
            if (ModelState.IsValid)
            {
                silownia.Dlugosc = longitude;
                silownia.Szerokosc = latitude;
                db.Silownie.Add(silownia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
           


            return View(silownia);
        }

        // GET: /Silownia/Edit/5
        public ActionResult Edit(long? id)
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

        // POST: /Silownia/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SilowniaID,Nazwa,GodzinaOtwarcia,GodzinaZamkniecia,Powierzchnia,NrTelefonu")] Models.Silownia silownia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(silownia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(silownia);
        }

        // GET: /Silownia/Delete/5
        public ActionResult Delete(long? id)
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

        // POST: /Silownia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Models.Silownia silownia = db.Silownie.Find(id);
            db.Silownie.Remove(silownia);
            db.SaveChanges();
            return RedirectToAction("Index");
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
