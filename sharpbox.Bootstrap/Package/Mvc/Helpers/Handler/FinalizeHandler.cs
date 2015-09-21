using System;
using sharpbox.Dispatch.Model;
using sharpbox.WebLibrary.Core;
using sharpbox.WebLibrary.Web.Controllers;

namespace sharpbox.Bootstrap.Package.Mvc.Helpers.Handler
{
    using System.Web.Mvc;

    public class FinalizeHandler<T> : LifecycleHandler<T> where T : new()
    {
      public override void HandleRequest(WebContext<T> webContext, SharpboxController<T> controller)
    {
        if (webContext.WebResponse.DispatchResponse != null &&
            webContext.WebResponse.DispatchResponse.ResponseType != ResponseTypes.Error)
        {
          webContext.WebResponse.Instance = (T)webContext.WebResponse.DispatchResponse.Entity;
        }
    }
  }
}