using System.Collections.Generic;
using System.Web.Mvc;
using sharpbox.Bootstrap.Package.Core;
using sharpbox.WebLibrary.Core;
using sharpbox.WebLibrary.Web.Controllers;
using sharpbox.WebLibrary.Web.Helpers;

namespace sharpbox.WebLibrary.Mvc.Helpers.Handler
{
  public class LoadContextHandler<T> : LifecycleHandler<T>
  {
    public ActionCommandMap ActionCommandMap { get; set; }
    public override void HandleRequest(WebContext<T> webContext, SharpboxController<T> controller)
    {
      this.ActionCommandMap = controller.LoadCommandActionMap();

      webContext.WebRequest.CommandName = this.ActionCommandMap.GetCommandByAction(webContext.AppContext, webContext.WebRequest.UiAction);

      webContext.WebResponse = new WebResponse<T>(){ ModelErrors = new Stack<ModelError>()};
    }
  }
}