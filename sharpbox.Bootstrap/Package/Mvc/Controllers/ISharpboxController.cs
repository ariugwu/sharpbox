using System.Web.Mvc;
using sharpbox.WebLibrary.Web.Helpers;
using FluentValidation;
using sharpbox.WebLibrary.Core;

namespace sharpbox.WebLibrary.Web.Controllers
{
  public interface ISharpboxController<T> where T : new()
  {

    #region Properties
    WebContext<T> WebContext { get; set; }

    #endregion

    #region Action(s)
    ActionResult Execute(WebRequest<T> webRequest);

    #endregion

    #region Validation

    AbstractValidator<T> LoadValidatorByUiAction(UiAction uiAction);
    #endregion

  }
}