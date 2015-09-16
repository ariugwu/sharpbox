using System;
using sharpbox.WebLibrary.Core;

namespace sharpbox.Bootstrap.Package.Mvc.Helpers.Handler
{
  public class FinalizeHandler<T> : LifecycleHandler<T>
  {
    public override void HandleRequest(WebContext<T> webContext)
    {
      throw new NotImplementedException();
    }
  }
}