using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData;
using System.Collections.Generic;
using System.Web.Mvc;

using sharpbox.Dispatch.Model;
using sharpbox.EfCodeFirst.Audit;
using sharpbox.WebLibrary.Core;
using sharpbox.WebLibrary.Core.Strategies;
using sharpbox.WebLibrary.Data;
using sharpbox.WebLibrary.Helpers;

using FluentValidation;

namespace sharpbox.WebLibrary.Mvc.Controllers
{
    using System.Web.Http.Results;

    using Newtonsoft.Json;

    using sharpbox.Bootstrap.Package.Data;

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
    public abstract class SharpboxApiController<T> : ApiController
        where T : new()
    {
        #region Properties
        public WebContext<T> WebContext { get; set; }

        public IRepository<T> Repository { get; set; }
 
        #endregion

        #region Constructor(s)

        protected SharpboxApiController(AppContext appContext, IMediator<T> mediator, IRepository<T> repository)
        {
            // Only create a new WebContext if one doesn't already exist.
            this.WebContext = new WebContext<T> { AppContext = appContext, Mediator = mediator, User = this.User };
            this.Repository = repository;
        }

        protected SharpboxApiController(AppContext appContext, IUnitOfWork<T> unitOfWork, IRepository<T> repository)
          : this(appContext, new DefaultMediator<T>(appContext, unitOfWork), repository)
        {
        }

        protected SharpboxApiController(AppContext appContext, IRepository<T> repository)
          : this(appContext, new DefaultMediator<T>(appContext, new DefaultUnitOfWork<T>(appContext.File)), repository)
        {
        }

        protected SharpboxApiController(AppContext appContext)
          : this(appContext, new DefaultMediator<T>(appContext, new DefaultUnitOfWork<T>(appContext.File)), new DefaultRepository<T>(appContext.File))
        {
        }

        #endregion

        [Queryable]
        public IHttpActionResult GetById(int id)
        {
            return this.Ok(this.Repository.Get(id));
        }

        [Queryable]
        public IHttpActionResult Get()
        {
            return this.Ok(this.Repository.Get());
        }

        // PUT odata/SharpboxApi(5)
        public IHttpActionResult Put(WebRequest<T> webRequest)
        {
            try
            {
                this.WebContext.ProcessRequest(webRequest, this);
                return this.Ok(this.WebContext.WebResponse);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex);
            }
        }

        // POST odata/SharpboxApi
        public JsonResult<WebResponse<T>>  Post(WebRequest<T> webRequest)
        {
            try
            {
                this.WebContext.ProcessRequest(webRequest, this);
                return this.Json(this.WebContext.WebResponse);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex);
            }
        }

        // PATCH odata/SharpboxApi(5)
        [System.Web.Http.AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch(int key, WebRequest<T> webRequest)
        {
            try
            {
                this.WebContext.ProcessRequest(webRequest, this);
                return this.Ok(this.WebContext.WebResponse);
            }
            catch (Exception ex)
            {
                return this.HandleException(ex);
            }
        }

        // DELETE odata/SharpboxApi(5)
        public IHttpActionResult Delete(WebRequest<T> webRequest)
        {
            try
            {
                if (webRequest != null && webRequest.UiAction == null)
                {
                    webRequest.UiAction = new UiAction("Delete");    
                }

                this.WebContext.ProcessRequest(webRequest, this);
                return this.NotFound();
            }
            catch (Exception ex)
            {
                return this.HandleException(ex);
            }
        }


        private JsonResult<WebResponse<T>> HandleException(Exception ex)
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
            return this.WebContext.Mediator.CommandMessageMap;
        }

        #endregion
    }
}
