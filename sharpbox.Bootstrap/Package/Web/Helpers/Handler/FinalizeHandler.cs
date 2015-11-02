using sharpbox.Bootstrap.Package.Core;

namespace sharpbox.Common.Data.Web.Helpers.Handler
{
    using Core;
    using Controllers;

    using sharpbox.Common.Data;

    public class FinalizeHandler<T> : LifecycleHandler<T> where T : ISharpThing<T>, new()
    {

        public FinalizeHandler()
            : base(new LifeCycleHandlerName("Finalize"))
        {
        }
        public override void HandleRequest(WebContext<T> webContext, ISharpboxController<T> controller)
        {

        }
    }
}