using System.Linq;
using System.Web.Mvc;
using FluentValidation;
using FluentValidation.Results;
using sharpbox.WebLibrary.Core;
using sharpbox.WebLibrary.Web.Controllers;

namespace sharpbox.WebLibrary.Mvc.Helpers.Handler
{
  public class ValidationHandler<T> : LifecycleHandler<T>
  {
    public ValidationResult ValidationResult { get; set; }

    public AbstractValidator<T> Validator { get; set; }

    public override void HandleRequest(WebContext<T> webContext, SharpboxController<T> controller)
    {
      if (!controller.ModelState.IsValid)
      {
        foreach (var e in controller.ModelState.Values.SelectMany(x => x.Errors))
        {
          webContext.WebResponse.ModelErrors.Push(e);
        }
        return;
      }

      this.Validator = controller.LoadValidatorByUiAction(webContext.WebRequest.UiAction);

      if (!this.Validate(webContext.WebRequest))
      {
        foreach (var e in this.ValidationResult.Errors)
        {
          controller.ModelState.AddModelError(e.PropertyName, e.ErrorMessage);
          webContext.WebResponse.ModelErrors.Push(new ModelError(e.ErrorMessage));
        }
      }
    }

    public bool Validate(WebRequest<T> webRequest)
    {
      this.ValidationResult = this.Validator.Validate(webRequest.Instance);

      return this.ValidationResult.IsValid;
    }
  }
}