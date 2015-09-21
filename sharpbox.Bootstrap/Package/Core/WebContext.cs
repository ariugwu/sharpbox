using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FluentValidation;
using FluentValidation.Results;
using sharpbox.Bootstrap.Package.Mvc.Helpers.Handler;
using sharpbox.Dispatch.Model;
using sharpbox.WebLibrary.Mvc.Helpers.Handler;

namespace sharpbox.WebLibrary.Core
{
    public class WebContext<T>
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

        public AbstractValidator<T> Validator { get; set; }

        public T Instance { get; set; }

        public IMediator<T> Mediator { get; set; }

        public WebRequest<T> WebRequest { get; set; }

        public Response Response { get; set; }

        public ValidationResult ValidationResult { get; set; }

        public IEnumerable<ModelError> ModelStateErrors { get; set; }

        public bool IsModelStateValid
        {
            get
            {
                return !(this.ModelStateErrors != null && this.ModelStateErrors.Any());
            }
        }

        public bool IsWebRequestValid
        {
            get
            {
                return this.Response != null && this.Response.ResponseType != ResponseTypes.Error;
            }
        }

        #region Chain of Responsibility

        // The base handler
        public LifecycleHandler<T> _handler;

        #endregion

        #endregion

        #region Validation

        public bool Validate()
        {
            this.ValidationResult = this.Validator.Validate(this.Instance);

            return this.ValidationResult.IsValid;
        }

        #endregion

        public void ProcessRequest(Controller controller)
        {
            _handler.ProcessRequest(this, controller);
        }

    }
}