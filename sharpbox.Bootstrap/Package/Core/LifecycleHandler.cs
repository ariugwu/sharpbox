using System.Collections.Generic;
using sharpbox.WebLibrary.Web.Controllers;

namespace sharpbox.WebLibrary.Core
{
  using System.Web.Mvc;

  public abstract class LifecycleHandler<T> where T : new()
  {
    protected LifecycleHandler<T> _successor;

    /// <summary>
    /// The set successor.
    /// </summary>
    /// <param name="successor">
    /// The successor.
    /// </param>
    public void SetSuccessor(LifecycleHandler<T> successor)
    {
      this._successor = successor;
    }

    public void ProcessRequest(WebContext<T> webContext, SharpboxController<T> controller)
    {
      this.HandleRequest(webContext, controller);

      if (this._successor != null && controller.ModelState.IsValid)
      {
        this._successor.ProcessRequest(webContext, controller);
      }

    }

    public abstract void HandleRequest(WebContext<T> webContext, SharpboxController<T> controller);

    public static void AddModelStateError(WebContext<T> webContext, SharpboxController<T> controller, string key, ModelError modelError)
    {
      controller.ModelState.AddModelError(key, modelError.ErrorMessage);

      if (!webContext.WebResponse.ModelErrors.ContainsKey(key))
      {
        webContext.WebResponse.ModelErrors.Add(key, new Stack<ModelError>());
      } 

      webContext.WebResponse.ModelErrors[key].Push(modelError);
    }

    public static void AddModelStateError(WebContext<T> webContext, string key, ModelError modelError)
    {
      if (!webContext.WebResponse.ModelErrors.ContainsKey(key))
      {
        webContext.WebResponse.ModelErrors.Add(key, new Stack<ModelError>());
      }

      webContext.WebResponse.ModelErrors[key].Push(modelError);
    }
  }
}
