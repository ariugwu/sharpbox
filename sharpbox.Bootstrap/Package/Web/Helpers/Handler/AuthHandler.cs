
using System;
using sharpbox.WebLibrary.Core;

namespace sharpbox.WebLibrary.Mvc.Helpers.Handler
{
    using System.Web.Mvc;
    using sharpbox.WebLibrary.Mvc.Controllers;

    public class AuthHandler<T> : LifecycleHandler<T> where T : new()
    {
      public override void HandleRequest(WebContext<T> webContext, SharpboxApiController<T> controller)
    {

    }
  }
}