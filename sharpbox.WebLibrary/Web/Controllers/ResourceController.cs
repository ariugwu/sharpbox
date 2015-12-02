namespace sharpbox.WebLibrary.Web.Controllers
{
    using Core.Models;

    public class ResourceController : SharpboxController<Localization.Model.Resource>
    {
        public ResourceController()
            : base(new ExampleAppContext())
        {
            
        }
    }
}