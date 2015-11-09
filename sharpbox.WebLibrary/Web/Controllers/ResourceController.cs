namespace sharpbox.WebLibrary.Web.Controllers
{
    using Core.Models;

    public class ResourceController : SharpboxScaffoldController<Localization.Model.Resource>
    {
        public ResourceController()
            : base(new ExampleAppContext())
        {
            
        }
    }
}