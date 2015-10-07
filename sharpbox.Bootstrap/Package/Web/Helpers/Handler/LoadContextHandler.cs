using System.Collections.Generic;
using System.Web.Mvc;

namespace sharpbox.WebLibrary.Web.Helpers.Handler
{
  using Controllers;
  using Core;
  using WebLibrary.Helpers;
  public class LoadContextHandler<T> : LifecycleHandler<T> where T : new()
  {
    public ActionCommandMap ActionCommandMap { get; set; }

    public override void HandleRequest(WebContext<T> webContext, ISharpboxController<T> controller)
    {
      this.ActionCommandMap = controller.LoadCommandActionMap();

      webContext.WebRequest.CommandName = this.ActionCommandMap.GetCommandByAction(webContext.AppContext, webContext.WebRequest.UiAction);

      webContext.WebResponse = new WebResponse<T>() { ModelErrors = new Dictionary<string, Stack<ModelError>>() };
    }
  }
}