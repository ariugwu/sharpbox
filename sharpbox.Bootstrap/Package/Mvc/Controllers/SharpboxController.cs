using System;
using System.Web.Mvc;
using FluentValidation;
using sharpbox.WebLibrary.Core;
using sharpbox.WebLibrary.Core.Strategies;
using sharpbox.Dispatch.Model;
using sharpbox.WebLibrary.Data;
using sharpbox.WebLibrary.Web.Helpers;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace sharpbox.WebLibrary.Web.Controllers
{
    using System.Linq;

    public abstract class SharpboxController<T> : Controller, ISharpboxController<T>
    {

        #region Override(s)
        #endregion

        #region Properties
        public WebContext<T> WebContext { get; set; }

        public ActionCommandMap ActionCommandMap { get; set; }
        #endregion

        #region Constructor(s)
        protected SharpboxController(AppContext appContext, IMediator<T> mediator)
        {
            if (this.TempData["WebContext"] == null)
            {
                this.WebContext = new WebContext<T> { AppContext = appContext, Mediator = mediator };
            }
            else
            {
                this.WebContext = (WebContext<T>)TempData["WebContext"];
            }
            
        }

        protected SharpboxController(AppContext appContext, IUnitOfWork<T> unitOfWork)
            : this(appContext, new DefaultMediator<T>(appContext, unitOfWork))
        {
        }

        protected SharpboxController(AppContext appContext)
            : this(appContext, new DefaultMediator<T>(appContext, new DefaultUnitOfWork<T>()))
        {
        }

        #endregion

        #region Validation

        public void SetValidator(UiAction uiAction)
        {
            this.WebContext.Validator = this.LoadValidatorByUiAction(uiAction); // Abstract method to be set by implementing class
        }

        public abstract AbstractValidator<T> LoadValidatorByUiAction(UiAction uiAction);

        #endregion

        #region CommandActionMapping 

        public void SetCommandActionMap()
        {
            this.ActionCommandMap = this.LoadCommandActionMap();
        }

        public abstract ActionCommandMap LoadCommandActionMap();

        #endregion

        #region Action(s)

        [HttpPost]
        public ActionResult Execute(T instance, UiAction uiAction)
        {
            try
            {
                this.WebContext.Instance = instance;
                this.SetValidator(uiAction);
                this.SetCommandActionMap();
                this.WebContext.WebRequest = new WebRequest<T>() { Instance = this.WebContext.Instance, CommandName = this.ActionCommandMap.GetCommandByAction(this.WebContext.AppContext, uiAction) };
                this.WebContext.ProcessRequest(this); // Pass the current controller
            }
            catch (Exception ex)
            {
                this.WebContext.Response = new Response() { Entity = this.WebContext.Instance, Message = ex.Message };
                this.ModelState.AddModelError("Exception", ex.Message);
            }

            this.TempData["WebContext"] = this.WebContext;

            return this.Redirect(this.ControllerContext.HttpContext.Request.UrlReferrer.ToString());
        }

        public void GeneratePdf(string url)
        {

        }

        public JsonResult GetJsonModel()
        {
            return this.Json(this.WebContext.Instance);
        }

        #endregion
    }
}
