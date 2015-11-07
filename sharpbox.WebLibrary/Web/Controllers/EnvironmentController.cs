namespace sharpbox.WebLibrary.Web.Controllers
{
    using sharpbox.WebLibrary.Core.Models;

    public class EnvironmentController : SharpboxScaffoldController<App.Model.Environment>
    {
        public EnvironmentController()
            : base(new ExampleAppContext())
        {
            
        }
    }
}