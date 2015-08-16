using Microsoft.AspNet.Mvc;

namespace sharpbox.Web.Sharpbox.Web.Controllers
{
    using System.Security.Cryptography.X509Certificates;

    using FluentValidation;

    using sharpbox.Web.Sharpbox.Web.Helpers;

    public interface ISharpboxController<T>
    {
        #region Constructor(s)

        #endregion

        #region Properties

        AbstractValidator<T> Validator { get; set; }

        sharpbox.AppContext AppContext { get; set; }

        T Instance { get; }

        #endregion

        #region Action(s)
        IActionResult Execute(T instance, UiAction uiAction);

        JsonResult GetJsonModel();
        #endregion

        #region Validation

        void SetValidator(UiAction uiAction);

        #endregion

        #region Chain of Responsibility

        // Auth
        // Load AppContext
        // Execute Action using Mediator
        // Check ModelState
        // Set Feedback
        // Return T
        // Record audit trail
        // Flush state info (audit trail, notification, etc) for this request

        #endregion
    }
}