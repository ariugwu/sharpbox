using System.Web.Mvc;
using sharpbox.Dispatch.Model;
using sharpbox.WebLibrary.Core;
using sharpbox.WebLibrary.Web.Controllers;

namespace sharpbox.WebLibrary.Mvc.Helpers.Handler
{
    public class ExecuteHandler<T> : LifecycleHandler<T>
    {
      public override void HandleRequest(WebContext<T> webContext, SharpboxController<T> controller)
        {
            webContext.WebResponse.Response = webContext.AppContext.Dispatch.Process<T>(webContext.WebRequest.CommandName, webContext.WebRequest.CommandName.Name, new object[] { webContext.WebRequest.Instance });
            
        if (webContext.WebResponse.Response.ResponseType == ResponseTypes.Error)
            {
              controller.ModelState.AddModelError("Processing Error", webContext.WebResponse.Response.Message);
              webContext.WebResponse.ModelErrors.Push(new ModelError(webContext.WebResponse.Response.Message));
            }
        }
    }
}