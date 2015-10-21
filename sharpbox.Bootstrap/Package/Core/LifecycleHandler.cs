using System;
using System.Web.Mvc;
using System.Collections.Generic;

namespace sharpbox.WebLibrary.Core
{
    using Bootstrap.Package.Core;
    using Dispatch.Model;
    using Web.Controllers;

    public abstract class LifecycleHandler<T> where T : new()
    {
        protected LifecycleHandler(LifeCycleHandlerName name)
        {
            Name = name;
        }

        protected LifecycleHandler() { }

        protected LifecycleHandler<T> _successor;

        public LifeCycleHandlerName Name { get; set; }

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
            try
            {
                this.HandleRequest(webContext, controller);
                this.PopulateLifeCycleTrail(webContext);
            }
            catch (Exception ex)
            {
                if (webContext.WebResponse == null)
                {
                    webContext.WebResponse = new WebResponse<T>() { ModelErrors = new Dictionary<string, Stack<ModelError>>() };
                }

                webContext._handler.AddModelStateError(webContext, controller, "ExecutionError", new ModelError(ex, ex.Message));
            }

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
        public void AddModelStateError(WebContext<T> webContext, ISharpboxController<T> controller, string key, ModelError modelError)
        {
            controller.AddErrorToModelState(key, modelError.ErrorMessage);

            if (!webContext.WebResponse.ModelErrors.ContainsKey(key))
            {
                webContext.WebResponse.ModelErrors.Add(key, new Stack<ModelError>());
            }

            webContext.WebResponse.ModelErrors[key].Push(modelError);

            webContext.WebResponse.AddLifeCycleTrailItem(this.Name, LifeCycleHandlerState.Error, modelError.ErrorMessage);

            webContext.WebContextState = WebContextState.Faulted;
            webContext.WebResponse.ResponseType = ResponseTypes.Error.ToString();
        }

        public void AddModelStateError(WebContext<T> webContext, string key, ModelError modelError)
        {
            if (!webContext.WebResponse.ModelErrors.ContainsKey(key))
            {
                webContext.WebResponse.ModelErrors.Add(key, new Stack<ModelError>());
            }

            webContext.WebResponse.ModelErrors[key].Push(modelError);

            webContext.WebResponse.AddLifeCycleTrailItem(this.Name, LifeCycleHandlerState.Error, modelError.ErrorMessage);

            webContext.WebContextState = WebContextState.Faulted;
            webContext.WebResponse.ResponseType = ResponseTypes.Error.ToString();
        }

        protected void PopulateLifeCycleTrail(WebContext<T> webContext, LifeCycleHandlerState state = null, string message = null)
        {
            webContext.WebResponse.AddLifeCycleTrailItem(this.Name, state ?? LifeCycleHandlerState.Success, message ?? string.Empty);
        }

        public abstract void HandleRequest(WebContext<T> webContext, ISharpboxController<T> controller);

    }
}
