namespace sharpbox.WebLibrary.Web.Helpers.Handler
{
  using Core;
  using Controllers;
  public class FinalizeHandler<T> : LifecycleHandler<T> where T : new()
  {
    public override void HandleRequest(WebContext<T> webContext, ISharpboxController<T> controller)
    {


    }
  }
}