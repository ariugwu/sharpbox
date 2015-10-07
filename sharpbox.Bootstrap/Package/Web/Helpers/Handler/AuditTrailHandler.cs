using System;
using sharpbox.WebLibrary.Core;

namespace sharpbox.WebLibrary.Web.Helpers.Handler
{
  using Controllers;

  public class AuditTrailHandler<T> : LifecycleHandler<T> where T : new()
  {
    public override void HandleRequest(WebContext<T> webContext, ISharpboxController<T> controller)
    {

    }

  }
}