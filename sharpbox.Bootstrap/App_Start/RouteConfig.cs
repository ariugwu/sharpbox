using System.Web.Mvc;
using System.Web.Routing;

namespace sharpbox.Bootstrap
{
    using System.Web.Http;

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapHttpRoute(
name: "API Default",
routeTemplate: "api/{controller}/{id}",
defaults: new { id = RouteParameter.Optional }
);
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
