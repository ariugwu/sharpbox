using System.Web.Mvc;

namespace sharpbox.Bootstrap.Package.Web.Controllers
{
    using Models;
    using Common.Data.Web.Controllers;

    public class EnvironmentController : SharpboxController<App.Model.Environment>
    {
        public EnvironmentController()
            : base(new ExampleAppContext())
        {
            
        }
    }
}