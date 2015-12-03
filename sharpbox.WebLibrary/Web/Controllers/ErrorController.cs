using System;
using sharpbox.App;

namespace sharpbox.WebLibrary.Web.Controllers
{
    public class ErrorController : SharpboxController<Exception>
    {
        public ErrorController(AppContext appContext) : base(appContext)
        {
        }
    }
}
