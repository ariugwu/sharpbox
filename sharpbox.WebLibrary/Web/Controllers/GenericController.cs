
namespace sharpbox.WebLibrary.Web.Controllers
{
    using App;

    using sharpbox.Common.App;

    public class GenericController<T> : SharpboxController<T> where T : class, new()
    {
        public GenericController(AppContext appContext) : base(appContext)
        {
        }
    }
}
