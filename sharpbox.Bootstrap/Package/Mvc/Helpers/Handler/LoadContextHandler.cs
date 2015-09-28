using System.Collections.Generic;
using System.Web.Mvc;
using sharpbox.WebLibrary.Core;
using sharpbox.WebLibrary.Helpers;


namespace sharpbox.WebLibrary.Mvc.Helpers.Handler
{
    using sharpbox.WebLibrary.Mvc.Controllers;

    public class LoadContextHandler<T> : LifecycleHandler<T> where T : new()
  {
    public ActionCommandMap ActionCommandMap { get; set; }

    public override void HandleRequest(WebContext<T> webContext, SharpboxApiController<T> controller)
    {
      this.ActionCommandMap = controller.LoadCommandActionMap();

      webContext.WebRequest.CommandName = this.ActionCommandMap.GetCommandByAction(webContext.AppContext, webContext.WebRequest.UiAction);

      webContext.WebResponse = new WebResponse<T>() { ModelErrors = new Dictionary<string, Stack<ModelError>>() };
    }
  }
}