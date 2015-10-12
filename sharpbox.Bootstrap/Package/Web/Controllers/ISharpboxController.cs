using System.Collections.Generic;
using FluentValidation;
using sharpbox.Common.Data;

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

        #endregion

        #region Validation

      /// <summary>
      /// The default is no validation. However you may (should) choose to define which AbstractValidator of type T should be used for which UiAction.
      /// </summary>
      /// <param name="uiAction"></param>
      /// <returns></returns>
        [System.Web.Http.NonAction]
        AbstractValidator<T> LoadValidatorByUiAction(UiAction uiAction);
        #endregion

        #region CommandActionMapping

      /// <summary>
      /// This will typically default to a one to one mappng. Meaning that whatever UiAction you pass it will be assumed that a Command exists with the same name. Override to provide custom mapping.
      /// </summary>
      /// <returns></returns>
        [System.Web.Http.NonAction]
        ActionCommandMap LoadCommandActionMap();

        [System.Web.Http.NonAction]
        Dictionary<CommandName, Dictionary<ResponseTypes, string>> LoadCommandMessageMap(WebContext<T> webContext);

        /// <summary>
        /// Use this to provide messaging that overrides what is supplied to the Dispatcher during command registration.
        /// </summary>
        Dictionary<CommandName, Dictionary<ResponseTypes, string>> CommandMessageMap { get; set; }

        #endregion

      #region .NET Controller Facade
      void AddErrorToModelState(string key, string modelErrorMessage);

      bool IsModelStateValid();
      void MigrateModelErrorsToWebContext();

      #endregion
    }
}
