using System;
using sharpbox.WebLibrary.Core;
using sharpbox.WebLibrary.Web.Controllers;

namespace sharpbox.Bootstrap.Package.Mvc.Helpers.Handler
{
    using System.Web.Mvc;

    public class FinalizeHandler<T> : LifecycleHandler<T>
  {
      public override void HandleRequest(WebContext<T> webContext, SharpboxController<T> controller)
    {
    }
  }
}