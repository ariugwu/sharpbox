﻿using sharpbox.WebLibrary.Core;

namespace sharpbox.WebLibrary.Web.Helpers.Handler
{
    using Common.Data;
    using Controllers;

    public class AuditTrailHandler<T> : LifecycleHandler<T> where T : class, new()
    {
        public AuditTrailHandler()
            : base(new LifeCycleHandlerName("AuditTrail"))
        {
        }

        public override void HandleRequest(WebContext<T> webContext, ISharpboxController<T> controller)
        {
        }
    }
}