using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using System.Data.Entity;
using System.Web.Mvc;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Silownia.Models;
using Silownia.Helpers;
using Silownia.DAL;

namespace Silownia.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        private SzyfrowanieHasel szyfr = new SzyfrowanieHasel("mojklucz", "mojwektor", 32);

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
                Session["Auth"] = null;
                Session["loggedUserID"] = null;
               
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
            Uzytkownik uzytkownik = db.Uzytkownicy.Where(w => w.Login == model.Login).FirstOrDefault();

            if (uzytkownik != null)
            {
                if (model.Login == "admin")
                {
                    if (model.Haslo == uzytkownik.Haslo)
                    {
                        Session["User"] = uzytkownik.Login;
                        Session["Auth"] = uzytkownik.Rola;
                        Session["loggedUserID"] = uzytkownik.IDOsoby;
                        return RedirectToAction("Index", "Silownia", new { id = 1 });
                    }
                }
                else
                {
                    string haslo = szyfr.Decrypt(uzytkownik.Haslo);
                    if (model.Haslo == haslo)
                    {
                        Session["User"] = uzytkownik.Login;
                        Session["Auth"] = uzytkownik.Rola;
                        Session["loggedUserID"] = uzytkownik.IDOsoby;
                        if (uzytkownik.Rola == RoleEnum.Klient.GetDescription())
                        {
                            Klient klient = db.Klienci.Find(uzytkownik.IDOsoby);
                            return RedirectToAction("Index", "KlientView", new { id = uzytkownik.IDOsoby });
                        }
                        if (uzytkownik.Rola == RoleEnum.Recepcjonista.GetDescription())
                        {
                            Recepcjonista recepcjonista = db.Recepcjonisci.Find(uzytkownik.IDOsoby);
                            return RedirectToAction("Index", "Silownia", new { id = recepcjonista.SilowniaID });
                        }
                        if (uzytkownik.Rola == RoleEnum.Trener.GetDescription())
                        {
                            Trener trener = db.Trenerzy.Find(uzytkownik.IDOsoby);
                            return RedirectToAction("Index", "TrenerView", new { id = uzytkownik.IDOsoby });
                        }
                        if (uzytkownik.Rola == RoleEnum.Konserwator.GetDescription())
                        {
                            Konserwator konserwator = db.Konserwatorzy.Find(uzytkownik.IDOsoby);
                            return RedirectToAction("Index", "KonserwatorView", new { id = uzytkownik.IDOsoby });
                        }
                        if (uzytkownik.Rola == RoleEnum.Masazysta.GetDescription())
                        {
                            Masazysta masazysta = db.Masazysci.Find(uzytkownik.IDOsoby);
                            return RedirectToAction("Index", "MasazystaView", new { id = uzytkownik.IDOsoby });
                        }
                        if (uzytkownik.Rola == RoleEnum.Instruktor.GetDescription())
                        {
                            Instruktor instruktor = db.Instruktorzy.Find(uzytkownik.IDOsoby);
                            return RedirectToAction("Index", "InstruktorView", new { id = uzytkownik.IDOsoby });
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Niepoprawny login lub hasło");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Użytkownik nie istnieje.");
            }

            
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
            this.Session["loggedUserID"] = null;
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