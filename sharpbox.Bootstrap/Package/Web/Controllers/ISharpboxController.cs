using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharpbox.WebLibrary.Mvc.Controllers
{
    using FluentValidation;

    using sharpbox.Dispatch.Model;
    using sharpbox.WebLibrary.Core;
    using sharpbox.WebLibrary.Data;
    using sharpbox.WebLibrary.Helpers;

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
    }
}
