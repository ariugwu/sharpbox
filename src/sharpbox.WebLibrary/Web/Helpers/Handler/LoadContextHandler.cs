﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace sharpbox.WebLibrary.Web.Helpers.Handler
{
    using Common.Data.Helpers;
    using Controllers;
    using Core;

    public class LoadContextHandler<T> : LifecycleHandler<T> where T : class, new()
    {
        public LoadContextHandler()
            : base(new LifeCycleHandlerName("LoadContext"))
        {
        }

        public ActionCommandMap ActionCommandMap { get; set; }

        public override void HandleRequest(WebContext<T> webContext, ISharpboxController<T> controller)
        {
            try
            {
                this.ActionCommandMap = controller.LoadCommandActionMap();
                webContext.WebRequest.CommandName = this.ActionCommandMap.GetCommandByAction(webContext,webContext.WebRequest.UiAction);

                webContext.WebResponse.AddLifeCycleTrailItem(this.Name, LifeCycleHandlerState.Success, string.Empty);
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