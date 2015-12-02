
namespace sharpbox.WebLibrary.Web.Controllers
{
    using App;
    using Core.Wiring;

    public class GenericController<T> : SharpboxController<T> where T : class, new()
    {
        public GenericController(AppContext appContext, IAppWiring appWiring, IAppPersistence appPersistence) : base(appContext, appWiring, appPersistence)
        {
        }

        public GenericController(AppContext appContext) : base(appContext)
        {
        }
    }
}
