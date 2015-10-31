using System;
using System.Linq;
using System.Web.Http;
using System.Collections.Generic;
using System.Web.Mvc;
using FluentValidation;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using NJsonSchema;

namespace sharpbox.WebLibrary.Web.Controllers
{
    using Common.Dispatch.Model;
    using Core;
    using Dispatch.Model;

    using WebLibrary.Helpers;
    using WebLibrary.Helpers.ControllerWiring;

    public abstract class SharpboxController<T> : Controller, ISharpboxController<T>
        where T : new()
    {
        #region Properties

        public WebContext<T> WebContext { get; set; }

        public Dictionary<CommandName, Dictionary<ResponseTypes, string>> CommandMessageMap { get; set; }

        #endregion

        #region Constructor(s)

        protected SharpboxController(AppContext appContext)
        {
            // Only create a new WebContext if one doesn't already exist.
            this.WebContext = new WebContext<T> { AppContext = appContext, User = this.User };
            this.WarmBootAppContext(this.WebContext.AppContext);
        }

        #endregion

        #region MVC Override(s)

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            this.WebContext.AppContext.UploadPath = this.Server.MapPath("~/Upload/");
            this.WebContext.AppContext.DataPath = this.Server.MapPath("~/App_Data/");
            this.WebContext.AppContext.Dispatch.Process<AppContext>(SharpboxControllerWiring.RunLoadAppContextRoutine, "Loading AppContext in OnAuthorization override", new object[] { this.WebContext.AppContext });
            this.WebContext.AppContext.CurrentLogOn = null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }

            base.Dispose(disposing);
        }

        #endregion

        #region Action(s)

        [Queryable]
        public virtual JsonResult Get()
        {
            return Json(this.WebContext.AppContext.Dispatch.Process<T>(SharpboxControllerWiring.Get, null), JsonRequestBehavior.AllowGet);
        }

        public JsonResult JsonSchema()
        {
            // Uses NJsonSchema lib
            var schema = JsonSchema4.FromType<T>();
            var schemaJson = schema.ToJson();

            // Uses Json.Net Schema
            //var generator = new JSchemaGenerator();
            //JSchema schema = generator.Generate(typeof(T));

            return Json(schemaJson, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Execute(WebRequest<T> webRequest)
        {
            try
            {
                this.WebContext.ProcessRequest(webRequest, this);

                var serializerSettings = new JsonSerializerSettings() { PreserveReferencesHandling = PreserveReferencesHandling.Objects }; // Prevent circular reference errors with EF objects and other one-to-many relationships
                return Json(JsonConvert.SerializeObject(this.WebContext.WebResponse, serializerSettings));
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

            this.WebContext._handler.AddModelStateError(this.WebContext, this, "ExecutionError", new ModelError(ex, ex.Message));
            var serializerSettings = new JsonSerializerSettings() { PreserveReferencesHandling = PreserveReferencesHandling.Objects }; // Prevent circular reference errors with EF objects and other one-to-many relationships
            return Json(JsonConvert.SerializeObject(this.WebContext.WebResponse, serializerSettings));
        }

        #endregion

        #region Validation

        [System.Web.Http.NonAction]
        public virtual AbstractValidator<T> LoadValidatorByUiAction(UiAction uiAction)
        {
            var validator = new InlineValidator<T>();
            return validator;
        }
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
                    this.WebContext._handler.AddModelStateError(this.WebContext, v.ToString(), new ModelError(me.ErrorMessage));
                }
            }
        }

        #endregion

        #region Bootstrap methods

        /// <summary>
        /// Every time a request comes in we want pull the latest application info into our AppContext
        /// </summary>
        /// <param name="appContext">The AppContext which handles each request.</param>
        private void WarmBootAppContext(AppContext appContext)
        {
            SharpboxControllerWiring.WireContext(this);
            WireApplication();
        }


        public virtual void WireApplication()
        {
            
        }

        #endregion

        #endregion
    }
}
