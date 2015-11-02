using sharpbox.Bootstrap.Package.Core;

namespace sharpbox.Common.Data.Web.Helpers.Handler
{
    using Controllers;
    using Core;

    using sharpbox.Common.Data;

    public class AuditTrailHandler<T> : LifecycleHandler<T> where T : ISharpThing<T>, new()
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