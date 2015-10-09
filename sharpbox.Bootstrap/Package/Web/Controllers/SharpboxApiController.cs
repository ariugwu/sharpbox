using System;
using System.Web.Http;
using System.Collections.Generic;
using System.Web.Mvc;

using FluentValidation;

namespace sharpbox.WebLibrary.Web.Controllers
{
  using Dispatch.Model;
  using Core;
  using Data;
  using WebLibrary.Helpers;
  /*
  To add a route for this controller, merge these statements into the Register method of the WebApiConfig class. Note that OData URLs are case sensitive.

  using System.Web.Http.OData.Builder;
  using sharpbox.Dispatch.Model;
  ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
  builder.EntitySet<DispatchResponse>("SharpboxApi");
  builder.EntitySet<EventName>("EventNames"); 
  builder.EntitySet<Request>("Requests"); 
  config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
  */
  public abstract class SharpboxApiController<T> : Controller, ISharpboxController<T>
      where T : new()
  {
    #region Properties

    public WebContext<T> WebContext { get; set; }

    public IRepository<T> Repository { get; set; }

    public IUnitOfWork<T> UnitOfWork { get; set; }

    public Dictionary<CommandName, Dictionary<ResponseTypes, string>> CommandMessageMap { get; set; }

    #endregion

    #region Constructor(s)

    protected SharpboxApiController(AppContext appContext, IRepository<T> repository, IUnitOfWork<T> unitOfWork)
    {
      // Only create a new WebContext if one doesn't already exist.
      this.WebContext = new WebContext<T> { AppContext = appContext, User = this.User };
      this.Repository = repository;
      this.UnitOfWork = unitOfWork;
    }

    protected SharpboxApiController(AppContext appContext, IRepository<T> repository)
      : this(appContext, repository, new DefaultUnitOfWork<T>(appContext.File))
    {
    }


    protected SharpboxApiController(AppContext appContext)
      : this(appContext, new DefaultRepository<T>(appContext.File), new DefaultUnitOfWork<T>(appContext.File))
    {
    }

    #endregion

    [Queryable]
    public JsonResult GetById(int id)
    {
      return Json(this.Repository.Get(id));
    }

    [Queryable]
    public JsonResult Get()
    {
      return Json(this.Repository.Get());
    }

    public JsonResult Execute(WebRequest<T> webRequest)
    {
      try
      {
        this.WebContext.ProcessRequest(webRequest, this);
        return Json(this.WebContext.WebResponse);
      }
      catch (Exception ex)
      {
        return this.HandleException(ex);
      }
    }

    private JsonResult HandleException(Exception ex)
    {
      if (this.WebContext.WebResponse == null)
      {
        this.WebContext.WebResponse = new WebResponse<T>() { ModelErrors = new Dictionary<string, Stack<ModelError>>() };
      }

      LifecycleHandler<T>.AddModelStateError(this.WebContext, this, "ExecutionError", new ModelError(ex, ex.Message));
      return this.Json(this.WebContext.WebResponse);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
      }
      base.Dispose(disposing);
    }

    #region Validation
    [System.Web.Http.NonAction]
    public abstract AbstractValidator<T> LoadValidatorByUiAction(UiAction uiAction);
    #endregion

    #region CommandActionMapping
    [System.Web.Http.NonAction]
    public virtual ActionCommandMap LoadCommandActionMap()
    {
      return new ActionCommandMap(useOneToOneMap: true);
    }

    [System.Web.Http.NonAction]
    public virtual Dictionary<CommandName, Dictionary<ResponseTypes, string>> LoadCommandMessageMap(WebContext<T> webContext)
    {
      return this.CommandMessageMap;
    }

    #region .NET Controller Facade
    public void AddErrorToModelState(string key, string modelErrorMessage)
    {
      this.ModelState.AddModelError(key, modelErrorMessage);
    }

    public bool IsModelStateValid()
    {
      return this.ModelState.IsValid;
    }

    public void MigrateModelErrorsToWebContext()
    {
      foreach (var v in this.ModelState.Values)
      {
        if (v.Errors.Count <= 0) continue;

        foreach (var me in v.Errors)
        {
         LifecycleHandler<T>.AddModelStateError(this.WebContext, v.ToString(), new ModelError(me.ErrorMessage));
        }

      }
    }

    #endregion

    #region Bootstrap methods

    /// <summary>
    /// Ideally this will help cut down on some wiring time for apps which only need basic CRUD functionality.
    /// </summary>
    /// <param name="appContext"></param>
    public virtual void BootstrapCrudCommands(AppContext appContext)
    {
      //Register Command(s)
      appContext.Dispatch.Register<T>(this.Insert, this.UnitOfWork.Insert, this.OnInsert);
      appContext.Dispatch.Register<T>(this.Update, this.UnitOfWork.Update, this.OnUpdate);
      appContext.Dispatch.Register<T>(this.Remove, this.UnitOfWork.Delete, this.OnRemove);

      //Register Listener(s)

      //Populate Command DispatchResponse Map
      this.CommandMessageMap = new Dictionary<CommandName, Dictionary<ResponseTypes, string>>();

      this.CommandMessageMap.Add(this.Insert, new Dictionary<ResponseTypes, string>());
      this.CommandMessageMap[this.Insert].Add(ResponseTypes.Error, "Insert failed.");
      this.CommandMessageMap[this.Insert].Add(ResponseTypes.Success, "Insert success.");

      this.CommandMessageMap.Add(this.Update, new Dictionary<ResponseTypes, string>());
      this.CommandMessageMap[this.Update].Add(ResponseTypes.Error, "Update failed.");
      this.CommandMessageMap[this.Update].Add(ResponseTypes.Success, "Update success.");

      this.CommandMessageMap.Add(this.Remove, new Dictionary<ResponseTypes, string>());
      this.CommandMessageMap[this.Remove].Add(ResponseTypes.Error, "Removal failed.");
      this.CommandMessageMap[this.Remove].Add(ResponseTypes.Success, "Removal success.");

    }

    #region CRUD Commands and Events

    public CommandName Insert = new CommandName("Insert");
    public CommandName Update = new CommandName("Update");
    public CommandName Remove = new CommandName("Remove");

    public EventName OnInsert = new EventName("OnInsert");
    public EventName OnUpdate = new EventName("OnUpdate");
    public EventName OnRemove = new EventName("OnRemove");

    #endregion

    #endregion

    #endregion
  }
}
