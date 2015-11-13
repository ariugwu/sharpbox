using System.Collections.Generic;
using sharpbox.WebLibrary.Core;

namespace sharpbox.WebLibrary.Web.Controllers
{
    using Common.Data;
    using Common.Data.Helpers.ControllerWiring;
    using Common.Dispatch.Model;
    using Dispatch.Model;

    public interface ISharpboxScaffoldController<T> : ISharpboxController<T> where T : new()
    {
        #region Properties

        WebContext<T> WebContext { get; set; }

        IAppWiring AppWiring { get; set; }

        #endregion

        #region CommandActionMapping

        [System.Web.Http.NonAction]
        Dictionary<CommandName, Dictionary<ResponseTypes, string>> LoadCommandMessageMap(WebContext<T> webContext);

        #endregion

        #region .NET Controller Facade
        void AddErrorToModelState(string key, string modelErrorMessage);

        bool IsModelStateValid();
        void MigrateModelErrorsToWebContext();

        #endregion

        #region Validation

        void AddDataAnotationsToValidator();

        #endregion
    }
}
