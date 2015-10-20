using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace sharpbox.WebLibrary.Web.Helpers.Handler
{
    using Controllers;
    using Core;
    using WebLibrary.Helpers;

    public class LoadContextHandler<T> : LifecycleHandler<T> where T : new()
    {
        public ActionCommandMap ActionCommandMap { get; set; }

        public override void HandleRequest(WebContext<T> webContext, ISharpboxController<T> controller)
        {
            webContext.WebResponse = new WebResponse<T>() { ModelErrors = new Dictionary<string, Stack<ModelError>>() };

            try
            {
                this.ActionCommandMap = controller.LoadCommandActionMap();
                webContext.WebRequest.CommandName = this.ActionCommandMap.GetCommandByAction(webContext.AppContext,
                    webContext.WebRequest.UiAction);
            }
            catch (ArgumentNullException aex)
            {
                AddModelStateError(webContext, controller, aex.ParamName, new ModelError(aex.Message));
            }
            catch (InvalidOperationException iex)
            {
                AddModelStateError(webContext, controller, iex.Source, new ModelError(iex.Message));
            }
            catch (KeyNotFoundException knfe)
            {
                AddModelStateError(webContext, controller, knfe.Source, new ModelError(knfe.Message));
            }
            catch (Exception ex)
            {
                AddModelStateError(webContext, controller, ex.Source, new ModelError(ex.Message));
            }
        }
    }
}