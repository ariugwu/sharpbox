using System;
using System.Web.Mvc;
using System.Collections.Generic;
using sharpbox.Common.Data;

namespace sharpbox.WebLibrary.Core
{
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

        public void ProcessRequest(WebContext<T> webContext, ISharpboxScaffoldController<T> controller)
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

                this.AddModelStateError(webContext, controller, "ExecutionError", new ModelError(ex, ex.Message));
            }

            if (this._successor != null && controller.IsModelStateValid())
            {
                this._successor.ProcessRequest(webContext, controller);
            }
        }

        /// <summary>
        /// This will: (1) Call the controller facade pattern to add this error to the Controller which in turn sets !IsModelValid. (2) Add the error to the WebResponse using the overload method and (3) set the WebContext as faulted. 
        /// </summary>
        /// <param name="webContext"></param>
        /// <param name="controller"></param>
        /// <param name="key"></param>
        /// <param name="modelError"></param>
        public void AddModelStateError(WebContext<T> webContext, ISharpboxScaffoldController<T> controller, string key, ModelError modelError)
        {
            controller.AddErrorToModelState(key, modelError.ErrorMessage);

            this.AddModelStateError(webContext, key, modelError);
        }

        public void AddModelStateError(WebContext<T> webContext, string key, ModelError modelError)
        {
            // We'll keep track of all the model errors but the LifeCycleTrail below will likely be more helpful
            if (!webContext.WebResponse.ModelErrors.ContainsKey(key))
            {
                webContext.WebResponse.ModelErrors.Add(key, new Stack<ModelError>());
            }

            webContext.WebResponse.ModelErrors[key].Push(modelError);

            // We want to know not just what errors occur but also where is the cycle they occur
            webContext.WebResponse.AddLifeCycleTrailItem(this.Name, LifeCycleHandlerState.Error, modelError.ErrorMessage);

            webContext.WebContextState = WebContextState.Faulted;
            webContext.WebResponse.ResponseType = ResponseTypes.Error.ToString();
        }

        /// <summary>
        /// Used internally for non error lifecycle events
        /// </summary>
        /// <param name="webContext"></param>
        /// <param name="state"></param>
        /// <param name="message"></param>
        protected void PopulateLifeCycleTrail(WebContext<T> webContext, LifeCycleHandlerState state = null, string message = null)
        {
            webContext.WebResponse.AddLifeCycleTrailItem(this.Name, state ?? LifeCycleHandlerState.Success, message ?? string.Empty);
        }

        public abstract void HandleRequest(WebContext<T> webContext, ISharpboxScaffoldController<T> controller);

    }
}
