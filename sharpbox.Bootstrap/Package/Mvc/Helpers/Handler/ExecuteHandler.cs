using System.Web.Mvc;
using sharpbox.Dispatch.Model;
using sharpbox.WebLibrary.Core;

namespace sharpbox.WebLibrary.Mvc.Helpers.Handler
{
    public class ExecuteHandler<T> : LifecycleHandler<T>
    {
        public override void HandleRequest(WebContext<T> webContext, Controller controller)
        {
            webContext.Response = webContext.AppContext.Dispatch.Process<T>(webContext.WebRequest.CommandName, webContext.WebRequest.CommandName.Name, new object[] { webContext.WebRequest.Instance });
            if (webContext.Response.ResponseType == ResponseTypes.Error)
            {
                controller.ModelState.AddModelError("Processing Error", webContext.Response.Message);
            }
        }
    }
}