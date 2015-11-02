﻿using System.Collections.Generic;
using FluentValidation;

namespace sharpbox.Common.Data.Web.Controllers
{
    using Bootstrap.Package.Web.Helpers.TypeScript;
    using Common.Dispatch.Model;
    using Core;

    using sharpbox.Common.Data;
    using sharpbox.Common.Data.Helpers;
    using sharpbox.Common.Data.Helpers.ControllerWiring;
    using sharpbox.Dispatch.Model;

    public interface ISharpboxController<T> : IDispatchMetadata
         where T : ISharpThing<T>, new()
    {
        #region Properties

        WebContext<T> WebContext { get; set; }

        IAppWiring AppWiring { get; set; }

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
