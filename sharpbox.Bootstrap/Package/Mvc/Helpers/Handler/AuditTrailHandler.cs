using System;
using sharpbox.WebLibrary.Core;
using sharpbox.WebLibrary.Web.Controllers;

namespace sharpbox.WebLibrary.Mvc.Helpers.Handler
{
    using System.Web.Mvc;

    public class AuditTrailHandler<T> : LifecycleHandler<T> where T : new()
    {
      public override void HandleRequest(WebContext<T> webContext, SharpboxController<T> controller)
    {

    }
  }
}