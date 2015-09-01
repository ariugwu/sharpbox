using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;
using sharpbox.Web.Sharpbox.Core;
using sharpbox.Web.Sharpbox.Core.Strategies;
using sharpbox;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace sharpbox.Web.Sharpbox.Web.Controllers
{
    using System;
    using System.Linq;

    using FluentValidation;
    using FluentValidation.Results;

    using sharpbox.Web.Sharpbox.Web.Helpers;

    using AppContext = sharpbox.AppContext;

    public abstract class SharpboxController<T> : Controller, ISharpboxController<T>
    {

        #region Chain of Responsibility (Stack Overrides)

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }

        #endregion

        #region Constructor(s)

        public SharpboxController(IMediator<T> mediator, ActionCommandMap actionCommandMap)
        {
            this.Mediator = mediator;
            this.ActionCommandMap = actionCommandMap;
        }

        public SharpboxController(AppContext appContext, IDispatchStrategy<T> dispatchStrategy, ActionCommandMap actionCommandMap)
            : this(new DefaultMediator<T>(appContext, dispatchStrategy), actionCommandMap)
        {
        }

        public SharpboxController(AppContext appContext, IDispatchStrategy<T> dispatchStrategy)
    : this(new DefaultMediator<T>(appContext, dispatchStrategy), new ActionCommandMap(useOneToOneMap: true))
        {
        }

        public SharpboxController(AppContext appContext)
            : this(new DefaultMediator<T>(appContext, new DefaultDispatchStrategy<T>()), new ActionCommandMap(useOneToOneMap: true))
        {

        }

        #endregion

        #region Properties

        public AppContext AppContext { get; set; }

        public AbstractValidator<T> Validator { get; set; }

        public T Instance { get; set; }

        public IMediator<T> Mediator { get; set; }

        public Feedback<T> Feedback { get; set; }

        public ActionCommandMap ActionCommandMap { get; set; }

        public ValidationResult ValidationResult { get; set; }

        #endregion

        #region Validation

        public bool Validate()
        {
            this.ValidationResult = this.Validator.Validate(this.Instance);

            return this.ValidationResult.IsValid;
        }

        public void SetValidator(UiAction uiAction)
        {
            this.Validator = this.LoadValidatorByUiAction(uiAction);
        }

        public abstract AbstractValidator<T> LoadValidatorByUiAction(UiAction uiAction);

        #endregion

        #region Action(s)

        public IActionResult Execute(T instance, UiAction uiAction)
        {
            this.Process(instance, uiAction);

            // Do temp stuff here.
            // 1. If the ModelState is invalid OR the feedback is error then return the view
            if (!this.ModelState.IsValid || this.Feedback.FeedbackType == FeedbackType.Error)
            {

            }
            else
            {
                // 2. If the ModelState and Feedback are good then redirect back to the requestor.
            }

            //TODO: Test and finalize

            throw new NotImplementedException();
        }

        [NonAction]
        public void Process(T instance, UiAction uiAction)
        {
            this.Validator = this.LoadValidatorByUiAction(uiAction);

            var command = this.ActionCommandMap.GetCommandByAction(this.AppContext, uiAction);

            if (!this.Validate())
            {
                foreach (var e in this.ValidationResult.Errors)
                {
                  this.ModelState.AddModelError(e.PropertyName, e.ErrorMessage);
                }
            }
            else
            {
                try
                {
                    this.Feedback = this.Mediator.Process(this.AppContext, this.Instance, command);
                }
                catch (Exception ex)
                {
                    this.ModelState.AddModelError("Exception", ex.Message);
                }
            }
        }

        public void GeneratePdf(string url)
        {
            
        }

        public JsonResult GetJsonModel()
        {
            return this.Json(this.Instance);
        }

        #endregion
    }
}
