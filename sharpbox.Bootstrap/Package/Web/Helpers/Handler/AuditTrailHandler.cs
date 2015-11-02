using sharpbox.Bootstrap.Package.Core;

namespace sharpbox.WebLibrary.Web.Helpers.Handler
{
    using Controllers;
    using Core;

    using sharpbox.WebLibrary.Data;

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