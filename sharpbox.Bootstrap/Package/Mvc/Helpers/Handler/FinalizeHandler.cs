using System;
using System.Web.Mvc;
using sharpbox.WebLibrary.Mvc.Controllers;
using sharpbox.Dispatch.Model;
using sharpbox.WebLibrary.Core;

namespace sharpbox.Bootstrap.Package.Mvc.Helpers.Handler
{
    public class FinalizeHandler<T> : LifecycleHandler<T> where T : new()
    {
        public override void HandleRequest(WebContext<T> webContext, SharpboxApiController<T> controller)
        {

        }
    }
}