using System.Collections.Generic;

namespace sharpbox.WebLibrary.Core
{
  using System.Web.Mvc;

  using Bootstrap.Package.Core;
  using Dispatch.Model;
  using Web.Controllers;

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

    public void ProcessRequest(WebContext<T> webContext, ISharpboxController<T> controller)
    {
      this.HandleRequest(webContext, controller);

      if (this._successor != null && controller.IsModelStateValid())
      {
        this._successor.ProcessRequest(webContext, controller);
      }
    }

    /// <summary>
    /// Will (1) Call the controller facade pattern to add this error to the Controller which in turn sets !IsModelValid. (2) add the error to the WebResponse. (3) Set the WebContext as faulted. 
    /// </summary>
    /// <param name="webContext"></param>
    /// <param name="controller"></param>
    /// <param name="key"></param>
    /// <param name="modelError"></param>
    public static void AddModelStateError(WebContext<T> webContext, ISharpboxController<T> controller, string key, ModelError modelError)
    {
      controller.AddErrorToModelState(key, modelError.ErrorMessage);

      if (!webContext.WebResponse.ModelErrors.ContainsKey(key))
      {
        webContext.WebResponse.ModelErrors.Add(key, new Stack<ModelError>());
      }

      webContext.WebResponse.ModelErrors[key].Push(modelError);
      webContext.WebContextState = WebContextState.Faulted;
      webContext.WebResponse.ResponseType = ResponseTypes.Error.ToString();
    }

    public static void AddModelStateError(WebContext<T> webContext, string key, ModelError modelError)
    {
      if (!webContext.WebResponse.ModelErrors.ContainsKey(key))
      {
        webContext.WebResponse.ModelErrors.Add(key, new Stack<ModelError>());
      }

      webContext.WebResponse.ModelErrors[key].Push(modelError);
      webContext.WebContextState = WebContextState.Faulted;
      webContext.WebResponse.ResponseType = ResponseTypes.Error.ToString();
    }

    public abstract void HandleRequest(WebContext<T> webContext, ISharpboxController<T> controller);

  }
}
