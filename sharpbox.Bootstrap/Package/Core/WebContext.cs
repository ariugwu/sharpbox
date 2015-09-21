using sharpbox.Bootstrap.Package.Core;
using sharpbox.Bootstrap.Package.Mvc.Helpers.Handler;
using sharpbox.WebLibrary.Mvc.Helpers.Handler;
using sharpbox.WebLibrary.Web.Controllers;

namespace sharpbox.WebLibrary.Core
{
  public class WebContext<T> where T : new()
  {

    #region Constructor(s)

    public WebContext()
    {
      var loadContextHandler = new LoadContextHandler<T>();
      var validationHandler = new ValidationHandler<T>();
      var executeHandler = new ExecuteHandler<T>();
      var auditTrailHandler = new AuditTrailHandler<T>();
      var finalizeHandler = new FinalizeHandler<T>();

      _handler = new AuthHandler<T>();

      _handler.SetSuccessor(loadContextHandler);
      loadContextHandler.SetSuccessor(validationHandler);
      validationHandler.SetSuccessor(executeHandler);
      executeHandler.SetSuccessor(auditTrailHandler);
      auditTrailHandler.SetSuccessor(finalizeHandler);
    }

    #endregion

    #region Properties

    public AppContext AppContext { get; set; }

    public IMediator<T> Mediator { get; set; }

    public WebRequest<T> WebRequest { get; set; }

    public WebResponse<T> WebResponse { get; set; } 

    #region Chain of Responsibility

    // The base handler
    public LifecycleHandler<T> _handler;

    #endregion

    #endregion
    public void ProcessRequest(WebRequest<T> webRequest, SharpboxController<T> controller)
    {
      this.WebRequest = webRequest;
      _handler.ProcessRequest(this, controller);
    }

    public void ProcessTempData(SharpboxController<T> controller)
    {
      if (this.WebResponse == null || this.WebResponse.IsValid)
      {
        return;
      }

      foreach (var e in this.WebResponse.ModelErrors)
      {
        foreach (var me in e.Value)
        {
          controller.ModelState.AddModelError(e.Key, me.ErrorMessage);
        }
      }
    }
  }
}