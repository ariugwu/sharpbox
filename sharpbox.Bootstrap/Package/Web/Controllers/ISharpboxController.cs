using System.Collections.Generic;
using FluentValidation;

namespace sharpbox.WebLibrary.Web.Controllers
{
    using Dispatch.Model;
    using Core;
    using Data;
    using WebLibrary.Helpers;

    public interface ISharpboxController<T>
        where T : new()
    {
        #region Properties
        WebContext<T> WebContext { get; set; }

        IRepository<T> Repository { get; set; }

        IUnitOfWork<T> UnitOfWork { get; set; }

        Dictionary<CommandName, Dictionary<ResponseTypes, string>> CommandMessageMap { get; set; }

        #endregion

        #region Validation
        [System.Web.Http.NonAction]
        AbstractValidator<T> LoadValidatorByUiAction(UiAction uiAction);
        #endregion

        #region CommandActionMapping

        [System.Web.Http.NonAction]
        ActionCommandMap LoadCommandActionMap();

        [System.Web.Http.NonAction]
        Dictionary<CommandName, Dictionary<ResponseTypes, string>> LoadCommandMessageMap(WebContext<T> webContext);

        #endregion

      #region .NET Controller Facade
      void AddErrorToModelState(string key, string modelErrorMessage);

      bool IsModelStateValid();
      void MigrateModelErrorsToWebContext();

      #endregion
    }
}
