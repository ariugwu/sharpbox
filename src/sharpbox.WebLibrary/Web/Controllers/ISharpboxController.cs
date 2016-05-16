using System.Collections.Generic;
using FluentValidation;
using sharpbox.WebLibrary.Core;
using sharpbox.WebLibrary.Helpers;
using sharpbox.WebLibrary.Web.Helpers.TypeScript;

namespace sharpbox.WebLibrary.Web.Controllers
{
    using Dispatch.Model;

    using Helpers;

    public interface ISharpboxController<T> : IDispatchMetadata where T : class, new()
    {
        #region Properties

        WebContext<T> WebContext { get; set; }

        #endregion

        #region CommandActionMapping

        [System.Web.Http.NonAction]
        Dictionary<CommandName, Dictionary<ResponseTypes, string>> LoadCommandMessageMap();

        #endregion

        #region .NET Controller Facade
        void AddErrorToModelState(string key, string modelErrorMessage);

        bool IsModelStateValid();
        void MigrateModelErrorsToWebContext();

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
        string FormatMessage(IResponse response, string message);

        /// <summary>
        /// Use this to provide messaging that overrides what is supplied to the Dispatcher during command registration.
        /// </summary>
        Dictionary<CommandName, Dictionary<ResponseTypes, string>> CommandMessageMap { get; set; }

        #endregion
    }
}
