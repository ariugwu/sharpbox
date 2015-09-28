using System.Collections.Generic;

namespace sharpbox.WebLibrary.Core
{
    using System.Web.Mvc;

    using sharpbox.Bootstrap.Package.Core;
    using sharpbox.Dispatch.Model;
    using sharpbox.WebLibrary.Mvc.Controllers;

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

        public void ProcessRequest(WebContext<T> webContext, SharpboxApiController<T> controller)
        {
            this.HandleRequest(webContext, controller);

            if (this._successor != null && controller.ModelState.IsValid)
            {
                this._successor.ProcessRequest(webContext, controller);
            }
        }

        public abstract void HandleRequest(WebContext<T> webContext, SharpboxApiController<T> controller);

        public static void AddModelStateError(WebContext<T> webContext, SharpboxApiController<T> controller, string key, ModelError modelError)
        {
            controller.ModelState.AddModelError(key, modelError.ErrorMessage);

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
    }
}
