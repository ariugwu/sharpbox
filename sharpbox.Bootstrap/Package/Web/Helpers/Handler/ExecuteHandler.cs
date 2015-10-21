using System.Web.Mvc;

namespace sharpbox.WebLibrary.Web.Helpers.Handler
{
  using Bootstrap.Package.Core;
  using Controllers;
  using Dispatch.Model;
  using Core;
  public class ExecuteHandler<T> : LifecycleHandler<T> where T : new()
  {
      public ExecuteHandler() : base(new LifeCycleHandlerName("Execute")) { } 

    public override void HandleRequest(WebContext<T> webContext, ISharpboxController<T> controller)
    {
      webContext.DispatchResponse = webContext.AppContext.Dispatch.Process<T>(webContext.WebRequest.CommandName, "Default Execution Message", new object[] { webContext.WebRequest.Instance });
      webContext.WebResponse.Instance = (T)webContext.DispatchResponse.Entity;
      webContext.WebResponse.ResponseType = webContext.DispatchResponse.ResponseType.ToString();
      webContext.WebContextState = WebContextState.ResponseSet;

      var messageMap = controller.LoadCommandMessageMap(webContext);
      var commandName = webContext.WebRequest.CommandName;
      var responseType = webContext.DispatchResponse.ResponseType;

      if (webContext.DispatchResponse.ResponseType == ResponseTypes.Error)
      {
        AddModelStateError(webContext, controller, "ProcessingError", new ModelError(webContext.DispatchResponse.Message));
      }

      // Doing this allows us to provide the controller with override authority. Kind of loopy but works.

      if (messageMap.ContainsKey(commandName) && messageMap[commandName].ContainsKey(responseType))
      {
        webContext.WebResponse.Message = messageMap[commandName][responseType];
      }
      else
      {
        webContext.WebResponse.Message = webContext.DispatchResponse.Message;
      }
    }
  }
}