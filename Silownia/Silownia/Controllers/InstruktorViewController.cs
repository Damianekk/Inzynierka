using Silownia.DAL;
using Silownia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Silownia.Helpers;
using Newtonsoft.Json;

namespace Silownia.Controllers
{
    public class InstruktorViewController : Controller
    {
        private SilowniaContext db = new SilowniaContext();
        public ActionResult Index(long? id)
        {
            if (Session["Auth"] != null)
            {
                if (Session["Auth"].ToString() == "Instruktor")
                {
                    if (id == null) // jeżeli nie podano idOsoby jako parametr 
                    {
                        if (String.IsNullOrEmpty(Session["loggedUserID"].ToString())) // sprawdzamy czy zalogowana jest osoba
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                        else
                        {
                            id = (long)(Session["loggedUserID"]); // jeśli jest to bierzemy jej id z sesji 
                        }
                    }
                    Osoba os = db.Osoby.Find(id);
                   
                    if (os == null)
                    {
                        return HttpNotFound();
                    }
                    var z = os;

                    return View(z);
                }
            }
            return HttpNotFound();

        }

    }
}