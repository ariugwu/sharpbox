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
      var preValidationHandler = new PreValidationHandler<T>();
      var executeHandler = new ExecuteHandler<T>();
      var postValidationHandler = new PostValidationHandler<T>();
      var auditTrailHandler = new AuditTrailHandler<T>();
      var finalizeHandler = new FinalizeHandler<T>();

      _handler = new AuthHandler<T>();

      _handler.SetSuccessor(loadContextHandler);
      loadContextHandler.SetSuccessor(preValidationHandler);
      preValidationHandler.SetSuccessor(executeHandler);
      executeHandler.SetSuccessor(postValidationHandler);
      postValidationHandler.SetSuccessor(auditTrailHandler);
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

    public void ProcessRequest()
    {
      _handler.HandleRequest(this);
    }

  }
}