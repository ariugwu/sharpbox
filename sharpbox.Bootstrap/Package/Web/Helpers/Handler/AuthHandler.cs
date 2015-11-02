using sharpbox.Bootstrap.Package.Core;
using sharpbox.Common.Data.Core;

namespace sharpbox.Common.Data.Web.Helpers.Handler
{
    using Controllers;

    using sharpbox.Common.Data.Core;
    using sharpbox.Common.Data;

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