﻿using sharpbox.Bootstrap.Package.Core;
using sharpbox.WebLibrary.Core;

namespace sharpbox.WebLibrary.Web.Helpers.Handler
{
    using Controllers;

    using sharpbox.WebLibrary.Data;

    public class AuthHandler<T> : LifecycleHandler<T> where T : ISharpThing<T>, new()
    {
        public AuthHandler()
            : base(new LifeCycleHandlerName("Auth"))
        {
        }

        public override void HandleRequest(WebContext<T> webContext, ISharpboxController<T> controller)
        {
        }
    }
}