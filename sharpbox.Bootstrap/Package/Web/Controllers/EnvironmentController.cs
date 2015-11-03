namespace sharpbox.Bootstrap.Package.Web.Controllers
{
    using Models;
    using WebLibrary.Web.Controllers;

    public class EnvironmentController : SharpboxController<App.Model.Environment>
    {
        public EnvironmentController()
            : base(new ExampleAppContext())
        {
            
        }
    }
}