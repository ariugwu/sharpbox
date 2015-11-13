using sharpbox.WebLibrary.Core;

namespace sharpbox.WebLibrary.Web.Helpers.Handler
{
    using Common.Data;
    using Controllers;

    public class FinalizeHandler<T> : LifecycleHandler<T> where T : new()
    {

        public FinalizeHandler()
            : base(new LifeCycleHandlerName("Finalize"))
        {
        }
        public override void HandleRequest(WebContext<T> webContext, ISharpboxScaffoldController<T> controller)
        {

        }
    }
}