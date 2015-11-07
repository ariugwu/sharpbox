﻿

namespace sharpbox.WebLibrary.Web.Helpers.Handler
{  
    using Common.Data;
    using Controllers;
    using Core;

    public class AuthHandler<T> : LifecycleHandler<T> where T : ISharpThing<T>, new()
    {
        public AuthHandler()
            : base(new LifeCycleHandlerName("Auth"))
        {
        }

        public override void HandleRequest(WebContext<T> webContext, ISharpboxScaffoldController<T> controller)
        {
        }
    }
}