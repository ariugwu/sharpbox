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
  public abstract class SharpboxController<T> : Controller, ISharpboxController<T>
  {

    #region Properties
    public WebContext<T> WebContext { get; set; }

    public ActionCommandMap ActionCommandMap { get; set; }
    #endregion

    #region Constructor(s)
    protected SharpboxController(IMediator<T> mediator, ActionCommandMap actionCommandMap)
    {
      WebContext = new WebContext<T> {Mediator = mediator};
      ActionCommandMap = actionCommandMap;

    }

    protected SharpboxController(IDispatchStrategy<T> dispatchStrategy, ActionCommandMap actionCommandMap)
    {
      WebContext = new WebContext<T>();
      WebContext.Mediator = new DefaultMediator<T>(WebContext, dispatchStrategy);
      ActionCommandMap = actionCommandMap;
    }

    protected SharpboxController(IDispatchStrategy<T> dispatchStrategy)
    {
      WebContext = new WebContext<T>();
      WebContext.Mediator = new DefaultMediator<T>(WebContext, dispatchStrategy);
      ActionCommandMap = new ActionCommandMap(useOneToOneMap: true);
    }

    protected SharpboxController(IUnitOfWork<T> unitOfWork)
    {
      WebContext = new WebContext<T>();
      WebContext.Mediator = new DefaultMediator<T>(WebContext, new DefaultDispatchStrategy<T>(unitOfWork));
      ActionCommandMap = new ActionCommandMap(useOneToOneMap: true);
    }

    protected SharpboxController()
    {
      WebContext = new WebContext<T>();
      WebContext.Mediator = new DefaultMediator<T>(WebContext, new DefaultDispatchStrategy<T>(new DefaultUnitOfWork<T>()));
      ActionCommandMap = new ActionCommandMap(useOneToOneMap: true);
    }

    #endregion

    #region Validation

    public void SetValidator(UiAction uiAction)
    {
      WebContext.Validator = this.LoadValidatorByUiAction(uiAction);
    }

    public abstract AbstractValidator<T> LoadValidatorByUiAction(UiAction uiAction);
    #endregion

    #region Action(s)

    public ActionResult Execute(T instance, UiAction uiAction)
    {

      SetValidator(uiAction);

      if (!this.ModelState.IsValid)
      {

        //TODO: temp stuff here: Store feedback in session and pull it out as part of the CoR

      }

      this.Process(instance, uiAction);

      if (!this.ModelState.IsValid)
      {

        //TODO: We check the model state to see if trying to process threw an error.

      }
      //TODO: Test and finalize

      throw new NotImplementedException();
    }

    [NonAction]
    public void Process(T instance, UiAction uiAction)
    {
      try
      {
        WebContext.Instance = instance;

        WebContext.Validator = this.LoadValidatorByUiAction(uiAction);

        if (!WebContext.Validate())
        {
          foreach (var e in WebContext.ValidationResult.Errors)
          {
            this.ModelState.AddModelError(e.PropertyName, e.ErrorMessage);
          }
        }
        else
        {
          var webRequest = new WebRequest<T>() { Instance = WebContext.Instance, CommandName = this.ActionCommandMap.GetCommandByAction(WebContext.AppContext, uiAction) };

          WebContext.ProcessRequest();

          if (WebContext.Response.ResponseType == ResponseTypes.Error)
          {
            this.ModelState.AddModelError("Processing Error", WebContext.Response.Message);
          }
        }
      }
      catch (Exception ex)
      {
        WebContext.Response = new Response() { Entity = WebContext.Instance, Message = ex.Message };
        this.ModelState.AddModelError("Exception", ex.Message);
      }
    }

    public void GeneratePdf(string url)
    {

    }

    public JsonResult GetJsonModel()
    {
      return this.Json(WebContext.Instance);
    }

    #endregion
  }
}
