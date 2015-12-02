namespace sharpbox.WebLibrary.Web.Controllers
{
    using Core.Models;

    public class EnvironmentController : SharpboxController<App.Model.Environment>
    {
        public EnvironmentController()
            : base(new ExampleAppContext())
        {
            
        }
    }
}