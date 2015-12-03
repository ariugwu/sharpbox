using System.Collections.Generic;
using System.Web.Mvc;

namespace sharpbox.WebLibrary.Web.Helpers.Handler
{
    using Core;
    using Controllers;
    using Dispatch.Model;
    using Common.Data;

    using sharpbox.Common.Dispatch;

    public class ExecuteHandler<T> : LifecycleHandler<T> where T : class, new()
    {
        public ExecuteHandler()
            : base(new LifeCycleHandlerName("Execute"))
        {
        }

        public override void HandleRequest(WebContext<T> webContext, ISharpboxController<T> controller)
        {
            // Grab the instance
            var parameters = new List<object> { webContext.WebRequest.Instance };

            // If we are calling this handler or the entire chain out of band we might be declaring additional arguments. So we add them to the list.
            if (webContext.WebRequest.Args != null)
            {
                parameters.AddRange(webContext.WebRequest.Args);
            }

            // If this is a request with a file then add it to the parameters for execution
            if (webContext.WebRequest.FileDetail != null)
            {
                parameters.Add(webContext.WebRequest.FileDetail);
            }

            webContext.DispatchResponse = webContext.AppContext.Dispatch.Process<T>(webContext.WebRequest.CommandName, "Default Execution Message", parameters.ToArray());

            webContext.WebResponse.Instance = (T)webContext.DispatchResponse.Entity;
            webContext.WebResponse.ResponseType = webContext.DispatchResponse.ResponseType.ToString();

            webContext.WebContextState = WebContextState.ResponseSet;

            var messageMap = controller.LoadCommandMessageMap();
            var commandName = webContext.WebRequest.CommandName;
            var responseType = webContext.DispatchResponse.ResponseType;

            if (webContext.DispatchResponse.ResponseType == ResponseTypes.Error)
            {
                this.AddModelStateError(webContext, controller, "ProcessingError", new ModelError(webContext.DispatchResponse.Message));
            }

            // Check to see if the controller has a message for this command + response type. Else use the default dispatcher message.

            if (messageMap.ContainsKey(commandName) && messageMap[commandName].ContainsKey(responseType))
            {
                webContext.WebResponse.Message = messageMap[commandName][responseType];

            }
            else
            {
                webContext.WebResponse.Message = webContext.DispatchResponse.Message;
            }
            
            // Then we'll give the controller once last chance to modify or override the message. This is helpful for formatted messages.
            controller.FormatMessage(webContext.DispatchResponse, webContext.WebResponse.Message);
        }
    }
}