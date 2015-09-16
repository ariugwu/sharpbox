using System.Web.Mvc;
using sharpbox.WebLibrary.Web.Helpers;
using FluentValidation;
using sharpbox.WebLibrary.Core;

namespace sharpbox.WebLibrary.Web.Controllers
{
  public interface ISharpboxController<T>
  {

    #region Properties
    WebContext<T> WebContext { get; set; }

    ActionCommandMap ActionCommandMap { get; set; }
    #endregion

    #region Action(s)
    ActionResult Execute(T instance, UiAction uiAction);

    #endregion

    #region Validation

    void SetValidator(UiAction uiAction);

    AbstractValidator<T> LoadValidatorByUiAction(UiAction uiAction);
    #endregion

  }
}