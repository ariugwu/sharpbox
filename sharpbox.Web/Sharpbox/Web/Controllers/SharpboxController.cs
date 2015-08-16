using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;
using sharpbox.Web.Sharpbox.Core;
using sharpbox.Web.Sharpbox.Core.Strategies;
using sharpbox;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace sharpbox.Web.Sharpbox.Web.Controllers
{
    using System;

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

        public IActionResult Execute(T instance, UiAction uiAction)
        {
            throw new NotImplementedException();
        }

        [NonAction]
        public void Process(T instance, UiAction uiAction)
        {
            Validator = LoadValidatorByUiAction(uiAction);

            var command = this.ActionCommandMap.GetCommandByAction(this.AppContext, uiAction);

            if (!Validate())
            {

            }
            else
            {
                try
                {
                    var response = AppContext.Dispatch.Process<T>(command, "hmm", new object[] { this.Instance });

                    Instance = (T)response.Entity;

                }
                catch (Exception ex)
                {

                }
            }
        }

        public JsonResult GetJsonModel()
        {
            return Json(Instance);
        }
    }
}
