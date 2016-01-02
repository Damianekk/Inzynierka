using System;
using System.Net;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Silownia.Helpers;

namespace Silownia
{

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class MyAuthorize : AuthorizeAttribute
    {
        private string[] UserProfilesRequired { get; set; }

        public MyAuthorize(params object[] userProfilesRequired)
        {
            if (userProfilesRequired.Any(p => p.GetType().BaseType != typeof(Enum)))
                throw new ArgumentException("userProfilesRequired");

            this.UserProfilesRequired = userProfilesRequired.Select(p => Enum.GetName(p.GetType(), p)).ToArray();
        }

        public override void OnAuthorization(AuthorizationContext context)
        {
            bool authorized = false;

            string auth = System.Web.HttpContext.Current.Session["Auth"].ToString();

            foreach (var role in this.UserProfilesRequired)
                if (role == auth)
                {
                    authorized = true;
                    break;
                }

            if (!authorized)
            {
                var url = new UrlHelper(context.RequestContext);
                var logonUrl = url.Action("Index", new { info = "Nie posiadasz uprawnień. Tylko administrator ma do tego prawa." });
                context.Result = new RedirectResult(logonUrl);
                return;
            }
        }
    }
}