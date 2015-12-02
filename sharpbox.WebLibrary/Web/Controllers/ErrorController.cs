using System;
using sharpbox.App;
using sharpbox.WebLibrary.Core.Wiring;

namespace sharpbox.WebLibrary.Web.Controllers
{
    public class ErrorController : SharpboxController<Exception>
    {
        public ErrorController(AppContext appContext, IAppWiring appWiring, IAppPersistence appPersistence) : base(appContext, appWiring, appPersistence)
        {
        }

        public ErrorController(AppContext appContext) : base(appContext)
        {
        }
    }
}
