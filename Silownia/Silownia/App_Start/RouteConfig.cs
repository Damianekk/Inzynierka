using System.Web.Mvc;
using System.Web.Routing;

namespace Silownia
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );


            routes.MapRoute("MasazystaWSilowni",
                           "Masaz/MasazystaWSilowni/",
                           new { controller = "SilowniaMasazystaViewModel", action = "MasazystaWSilowni" },
                           new[] { "Silownia.Controllers" });

            routes.MapRoute("TrenerWSilowni",
                          "TreningPersonalny/TrenerWSilowni/",
                          new { controller = "SilowniaTrenerViewModel", action = "TrenerWSilowni" },
                          new[] { "Silownia.Controllers" });
        }
    }
}
