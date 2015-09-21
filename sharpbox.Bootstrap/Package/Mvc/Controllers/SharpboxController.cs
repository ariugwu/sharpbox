using System;
using System.Collections.Generic;
using System.Web.Mvc;
using FluentValidation;
using sharpbox.Bootstrap.Package.Core;
using sharpbox.WebLibrary.Core;
using sharpbox.WebLibrary.Core.Strategies;
using sharpbox.WebLibrary.Data;
using sharpbox.WebLibrary.Web.Helpers;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace sharpbox.WebLibrary.Web.Controllers
{
  public abstract class SharpboxController<T> : Controller, ISharpboxController<T> where T : new()
  {

    #region Override(s)
    #endregion

    #region Properties
    public WebContext<T> WebContext { get; set; }

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
        if (this.WebContext.WebResponse != null && !this.WebContext.WebResponse.IsValid)
        {
          foreach (var e in this.WebContext.WebResponse.ModelErrors)
          {
            foreach (var me in e.Value)
            {
              this.ModelState.AddModelError(e.Key, me.ErrorMessage);
            }
          }
        }
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
    public abstract AbstractValidator<T> LoadValidatorByUiAction(UiAction uiAction);
    #endregion

    #region CommandActionMapping
    public abstract ActionCommandMap LoadCommandActionMap();
    #endregion

    #region Action(s)

    [HttpPost]
    public ActionResult Execute(WebRequest<T> webRequest)
    {
      try
      {
        this.WebContext.ProcessRequest(webRequest, this); // Pass the current controller and the webRequest
      }
      catch (Exception ex)
      {
        if (WebContext.WebResponse == null) WebContext.WebResponse = new WebResponse<T>() { ModelErrors = new Dictionary<string, Stack<ModelError>>() };

        LifecycleHandler<T>.AddModelStateError(this.WebContext, this, "ExecutionError", new ModelError(ex, ex.Message));
      }

      this.TempData["WebContext"] = this.WebContext;

      return this.Redirect(this.ControllerContext.HttpContext.Request.UrlReferrer.ToString());
    }

    public void GeneratePdf(string url)
    {

    }

    public JsonResult GetJsonModel()
    {
      return this.Json(new T());
    }

    #endregion
  }
}
