using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Silownia.Models;
using Silownia.DAL;

namespace Silownia.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public AccountController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (returnUrl == "/Account/LogOff")
            {
                Session["User"] = null;
                Session["Rola"] = null;
            }
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(Uzytkownik model, string returnUrl)
        {
            SilowniaContext db = new SilowniaContext();
            Uzytkownik uzytkownik = db.Uzytkownicy.Where(w => w.Login == model.Login).First();

                if (uzytkownik != null)
                {
                    Session["User"] = uzytkownik.Login;
                    Session["Rola"] = uzytkownik.Rola;
                   /* if (uzytkownik.Login == "admin")
                    {
                        FormsAuthentication.SetAuthCookie(uzytkownik.Login, true);
                      
                    }*/
                    if (uzytkownik.Rola == "Klient")
                    {
                        FormsAuthentication.SetAuthCookie(uzytkownik.Login, true);
                        Klient klient = db.Klienci.Find(uzytkownik.IDOsoby);
                        return View("~/Views/KlientView/Index.cshtml", klient);
                    }
                    if (uzytkownik.Rola == "Recepcjonista")
                    {
                        FormsAuthentication.SetAuthCookie(uzytkownik.Login, true);
                        Recepcjonista recepcjonista = db.Recepcjonisci.Find(uzytkownik.IDOsoby);
                        return View("~/Views/Recepcjonista/Index.cshtml", recepcjonista);
                    }
                    if(uzytkownik.Rola == "Instruktor")
                    {
                        FormsAuthentication.SetAuthCookie(uzytkownik.Login, true);
                        Trener trener = db.Trenerzy.Find(uzytkownik.IDOsoby);
                        return View("~/Views/Trener/Index.cshtml", trener);
                     }
                }
                else
                {
                    ModelState.AddModelError("", "Użytkownik nie istnieje.");
                }

            // If we got this far, something failed, redisplay form
            return View("~/Views/Account/Login.cshtml");
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            this.Session["User"] = null;
            this.Session["Auth"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}