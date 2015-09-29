using sharpbox.Bootstrap.Package.Mvc.Helpers.Handler;
using sharpbox.WebLibrary.Mvc.Helpers.Handler;

namespace sharpbox.WebLibrary.Core
{
    using System.Security.Principal;

    using sharpbox.Bootstrap.Package.Core;
    using sharpbox.Dispatch.Model;
    using sharpbox.WebLibrary.Mvc.Controllers;

    public class WebContext<T> where T : new()
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

        private AuditTrailHandler<T> _auditTrailHandler = new AuditTrailHandler<T>();
        private LoadContextHandler<T> _loadContextHandler = new LoadContextHandler<T>();
        private ValidationHandler<T> _validationHandler = new ValidationHandler<T>();
        private ExecuteHandler<T> _executeHandler = new ExecuteHandler<T>();
        private FinalizeHandler<T> _finalizeHandler = new FinalizeHandler<T>();

        public void SetAuditTrailHandler(LifecycleHandler<T> handler)
        {
            _executeHandler.SetSuccessor(handler);
            handler.SetSuccessor(_finalizeHandler);
        }
        #endregion
        
        #endregion

        public void ProcessRequest(WebRequest<T> webRequest, SharpboxApiController<T> controller)
        {
            this.WebRequest = webRequest;
            this._handler.ProcessRequest(this, controller);
        }

        public void ProcessTempData(SharpboxApiController<T> controller)
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