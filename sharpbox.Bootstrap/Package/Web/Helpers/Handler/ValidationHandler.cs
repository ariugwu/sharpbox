using System.Web.Mvc;
using FluentValidation;
using FluentValidation.Results;
using sharpbox.Bootstrap.Package.Core;
using sharpbox.WebLibrary.Core;

namespace sharpbox.WebLibrary.Web.Helpers.Handler
{
    using Controllers;

    public class ValidationHandler<T> : LifecycleHandler<T> where T : new()
    {
        public ValidationHandler() : base(new LifeCycleHandlerName("Validation")) { } 

        public ValidationResult ValidationResult { get; set; }

        public AbstractValidator<T> Validator { get; set; }

        public override void HandleRequest(WebContext<T> webContext, ISharpboxController<T> controller)
        {
            if (!controller.IsModelStateValid())
            {
                controller.MigrateModelErrorsToWebContext();
                return;
            }

            this.Validator = controller.LoadValidatorByUiAction(webContext.WebRequest.UiAction);

            if (!this.Validate(webContext.WebRequest))
            {
                webContext.WebResponse.Instance = webContext.WebRequest.Instance;

                foreach (var e in this.ValidationResult.Errors)
                {
                    AddModelStateError(webContext, controller, e.PropertyName, new ModelError(e.ErrorMessage));
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