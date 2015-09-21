using System;
using sharpbox.WebLibrary.Core;

namespace sharpbox.WebLibrary.Mvc.Helpers.Handler
{
    using System.Web.Mvc;

    public class AuditTrailHandler<T> : LifecycleHandler<T>
  {
    public override void HandleRequest(WebContext<T> webContext, Controller controller)
    {

    }
  }
}