using sharpbox.Bootstrap.Models;

namespace sharpbox.WebLibrary.Web.Controllers
{
    public class EnvironmentController : SharpboxScaffoldController<App.Model.Environment>
    {
        public EnvironmentController()
            : base(new ExampleAppContext())
        {
            
        }
    }
}