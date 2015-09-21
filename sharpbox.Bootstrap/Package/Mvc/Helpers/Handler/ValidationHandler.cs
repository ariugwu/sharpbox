﻿using System.Linq;
using System.Web.Mvc;
using FluentValidation;
using FluentValidation.Results;
using sharpbox.WebLibrary.Core;
using sharpbox.WebLibrary.Web.Controllers;

namespace sharpbox.WebLibrary.Mvc.Helpers.Handler
{
  public class ValidationHandler<T> : LifecycleHandler<T> where T : new()
  {
    public ValidationResult ValidationResult { get; set; }

    public AbstractValidator<T> Validator { get; set; }

    public override void HandleRequest(WebContext<T> webContext, SharpboxController<T> controller)
    {
      if (!controller.ModelState.IsValid)
      {
        foreach (var v in controller.ModelState.Values)
        {
          if (v.Errors.Count <= 0) continue;

          foreach (var me in v.Errors)
          {
            AddModelStateError(webContext, v.ToString(), new ModelError(me.ErrorMessage));
          }

        }
        return;
      }

      this.Validator = controller.LoadValidatorByUiAction(webContext.WebRequest.UiAction);

      if (!this.Validate(webContext.WebRequest))
      {
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