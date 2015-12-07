using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace sharpbox.Bootstrap
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //Controller Factory
            //ControllerBuilder.Current.SetControllerFactory(typeof(sharpbox.WebLibrary.Web.Controllers.SharpControllerFactory));

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
