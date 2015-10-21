using sharpbox.Bootstrap.Package.Core;

namespace sharpbox.WebLibrary.Web.Helpers.Handler
{
  using Core;
  using Controllers;
  public class FinalizeHandler<T> : LifecycleHandler<T> where T : new()
  {

    public FinalizeHandler() : base(new LifeCycleHandlerName("Finalize")) { } 

    public override void HandleRequest(WebContext<T> webContext, ISharpboxController<T> controller)
    {

    }
  }
}