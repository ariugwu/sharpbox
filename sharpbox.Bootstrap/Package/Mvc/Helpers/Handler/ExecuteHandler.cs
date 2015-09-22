﻿using System.Web.Mvc;
using sharpbox.Dispatch.Model;
using sharpbox.WebLibrary.Core;
using sharpbox.WebLibrary.Web.Controllers;

namespace sharpbox.WebLibrary.Mvc.Helpers.Handler
{
    public class ExecuteHandler<T> : LifecycleHandler<T> where T : new()
    {
      public override void HandleRequest(WebContext<T> webContext, SharpboxController<T> controller)
        {
            webContext.WebResponse.DispatchResponse = webContext.AppContext.Dispatch.Process<T>(webContext.WebRequest.CommandName, webContext.WebRequest.CommandName.Name, new object[] { webContext.WebRequest.Instance });
            
        if (webContext.WebResponse.DispatchResponse.ResponseType == ResponseTypes.Error)
            {
              AddModelStateError(webContext, controller, "ProcessingError", new ModelError(webContext.WebResponse.DispatchResponse.Message));
            }
        }
    }
}