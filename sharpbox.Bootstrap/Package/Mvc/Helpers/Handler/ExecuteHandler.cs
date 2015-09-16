using sharpbox.WebLibrary.Core;

namespace sharpbox.WebLibrary.Mvc.Helpers.Handler
{
  public class ExecuteHandler<T> : LifecycleHandler<T>
  {
    public override void HandleRequest(WebContext<T> webContext)
    {
      webContext.Response = webContext.AppContext.Dispatch.Process<T>(webContext.WebRequest.CommandName, webContext.WebRequest.CommandName.Name, new object[] { webContext.WebRequest.Instance });
    }
  }
}