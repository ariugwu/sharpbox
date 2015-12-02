using System.Collections.Generic;
using System.Web.Mvc;
using sharpbox.App;

namespace sharpbox.WebLibrary.Core
{
  using System.Security.Principal;
  using Web.Helpers.Handler;

  using Dispatch.Model;

  using Web.Controllers;

  public class WebContext<T> where T : class, new()
    {

    #region Constructor(s)

    public WebContext()
    {
      _handler = new AuthHandler<T>();

      _handler.SetSuccessor(_loadContextHandler);
      _loadContextHandler.SetSuccessor(_validationHandler);
      _validationHandler.SetSuccessor(_executeHandler);
      _executeHandler.SetSuccessor(_auditTrailHandler);
      _auditTrailHandler.SetSuccessor(_finalizeHandler);
    }

    #endregion

    #region Properties

    public AppContext AppContext { get; set; }
    public WebRequest<T> WebRequest { get; set; }
    public Response DispatchResponse { get; set; }
    public WebResponse<T> WebResponse { get; set; }
    public WebContextState WebContextState { get; set; }
    public IPrincipal User { get; set; }

    #region Chain of Responsibility

    // The base handler
    public LifecycleHandler<T> _handler;

    private LifecycleHandler<T> _loadContextHandler = new LoadContextHandler<T>();
    private LifecycleHandler<T> _validationHandler = new ValidationHandler<T>();
    private LifecycleHandler<T> _executeHandler = new ExecuteHandler<T>();
    private LifecycleHandler<T> _auditTrailHandler = new AuditTrailHandler<T>();
    private LifecycleHandler<T> _finalizeHandler = new FinalizeHandler<T>();

    public void SetAuthHandler(LifecycleHandler<T> handler)
    {
      _handler = handler;
    }

    public void SetLoadContextHandler(LifecycleHandler<T> handler)
    {
      _loadContextHandler = handler;
    }

    public void SetValidationHandler(LifecycleHandler<T> handler)
    {
      _validationHandler = handler;
    }

    public void SetExecutionHandler(LifecycleHandler<T> handler)
    {
      _executeHandler = handler;
    }

    public void SetAuditTrailHandler(LifecycleHandler<T> handler)
    {
      _auditTrailHandler = handler;
    }

    public void SetFianlizeHandler(LifecycleHandler<T> handler)
    {
      _finalizeHandler = handler;
    }

    #endregion

    #endregion

    public void ProcessRequest(WebRequest<T> webRequest, ISharpboxController<T> controller)
        {
      this.WebRequest = webRequest;
      this.WebResponse = new WebResponse<T>() { ModelErrors = new Dictionary<string, Stack<ModelError>>() };

      this._handler.ProcessRequest(this, controller);
    }

    public void ProcessTempData(SharpboxController<T> controller)
    {
      // If there's no response or it's valid (with no errors just bail)
      // The response will be set to null after this request is sent.
      if (this.WebResponse != null && this.WebResponse.ModelErrors != null)
      {

        foreach (var e in this.WebResponse.ModelErrors)
        {
          foreach (var me in e.Value)
          {
            controller.ModelState.AddModelError(e.Key, me.ErrorMessage);
          }
        }
      }

      // This ensures that WebResponse will be nulled on the next request and not loop.
      this.WebContextState = WebContextState.ResponseProcessed;
    }
  }
}