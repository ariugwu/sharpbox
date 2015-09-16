using System;
using sharpbox.WebLibrary.Core;

namespace sharpbox.WebLibrary.Mvc.Helpers.Handler
{
  public class LoadContextHandler<T> : LifecycleHandler<T>
  {
    public override void HandleRequest(WebContext<T> webContext)
    {
      throw new NotImplementedException();
    }
  }
}