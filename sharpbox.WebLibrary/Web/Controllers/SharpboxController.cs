using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.OData;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Query;
using System.Web.Mvc;
using FluentValidation;
using Newtonsoft.Json;
using NJsonSchema;

namespace sharpbox.WebLibrary.Web.Controllers
{
    using App;
    using Common.App;
    using Common.Data.Helpers;

    using Core;

    using sharpbox.Dispatch.Model;
    using sharpbox.WebLibrary.Web.Helpers;

    using WebLibrary.Helpers;
    
    public abstract class SharpboxController<T> : Controller, ISharpboxController<T>
        where T : class, new()
    {
        #region Properties

        public Dictionary<CommandName, Dictionary<ResponseTypes, string>> CommandMessageMap { get; set; }

        public WebContext<T> WebContext { get; set; }

        #endregion

        #region Field(s)
        private ODataConventionModelBuilder _odataModelbuilder = new ODataConventionModelBuilder();
        #endregion

        #region Constructor(s)

        protected SharpboxController(WebContext<T> webContext = null)
        {
            this.CommandMessageMap = new Dictionary<CommandName, Dictionary<ResponseTypes, string>>();

            // Populate what we need for OData
            //TODO: Not ideal to do this on every request.
            this._odataModelbuilder.EntitySet<T>($"{typeof(T).Name}s");
            this._odataModelbuilder.AddEntity(typeof(T));

            this.WebContext = webContext ?? new WebContext<T>();
            this.WebContext.User = this.User;
            this.WebContext.WebResponse = new WebResponse<T>()
                                              {
                                                  ModelErrors =
                                                      new Dictionary<string, Stack<ModelError>>()
                                              };

            this.WarmBootAppContext(this.WebContext);
        }

        #endregion

        #region MVC Override(s)
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            InitDuringAuthorization();
        }

        public virtual void InitDuringAuthorization()
        {
            this.WebContext.UploadPath = this.Server.MapPath("~/Upload/");
            this.WebContext.DataPath = this.WebContext.File.DataPath = this.Server.MapPath("~/App_Data/");
            this.WebContext.CurrentLogOn = this.User.Identity.Name;
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
        
        public virtual ActionResult Index()
        {
            return this.View("~/Sharpbox/Web/Views/Crud/Index.cshtml");
        }

        public ActionResult Detail()
        {
            return this.View("~/Sharpbox/Web/Views/Crud/Detail.cshtml");
        }

        #endregion

        #region API

        public virtual JsonResult MetaData()
        {
            var edm = new ODataQueryContext(this._odataModelbuilder.GetEdmModel(), typeof(T));
            var sElements = edm.Model.SchemaElements.ToList();
            var serializerSettings = new JsonSerializerSettings() { PreserveReferencesHandling = PreserveReferencesHandling.Objects }; // Prevent circular reference errors with EF objects and other one-to-many relationships
            return this.Json(JsonConvert.SerializeObject(sElements, serializerSettings), JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult Get()
        {
            var result = this.WebContext.Dispatch.Fetch<T>(AppContext.Get, null);

            if (!string.IsNullOrEmpty(this.Request.Url?.Query))
            {
                var options = new ODataQueryOptions<T>(new ODataQueryContext(this._odataModelbuilder.GetEdmModel(), typeof(T)), new HttpRequestMessage(HttpMethod.Get, this.Request.Url.AbsoluteUri));
                var queryResults = options.ApplyTo(result);

                result = (IQueryable<T>)queryResults;
            }

            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Duration = 3600, VaryByParam = "None")]
        public virtual JsonResult GetAsLookUpDictionary()
        {
            ODataQueryOptions<T> options = null;
            
            if (!string.IsNullOrEmpty(this.Request.Url?.Query))
            {
                options = new ODataQueryOptions<T>(new ODataQueryContext(this._odataModelbuilder.GetEdmModel(), typeof(T)), new HttpRequestMessage(HttpMethod.Get, this.Request.Url.AbsoluteUri));
            }

            var items = this.WebContext.Dispatch.Fetch<T>(AppContext.Get, new object[] { options });
            var dict = new Dictionary<string, string>();
            var type = typeof(T);

            foreach (var i in items)
            {
                var key = Common.Type.TypeInfoHelper.GetIdValueByConvention(i, type);
                var value = i.ToString(); //Common.Type.TypeInfoHelper.GetNameValueByConvention(i, type);

                dict.Add(key.ToString(), value);
            }

            return this.Json(dict, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult GetById(string id)
        {
            return this.Json((T)this.WebContext.Dispatch.Fetch<T>(AppContext.GetById, new object[] { id }), JsonRequestBehavior.AllowGet);
        }

        public JsonResult JsonSchema()
        {
            var schema = JsonSchema4.FromType<T>();
            var schemaJson = schema.ToJson();

            // Uses Json.Net Schema
            //var generator = new JSchemaGenerator();
            //JSchema schemaJson = generator.Generate(typeof(T));

            return this.Json(schemaJson, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DataflowMatrix()
        {
            // Build an object that shows what commands are pointed to what methods and what UiActions are mapped to what commands.
            // Also show routines w/ failover and rollback paths.
            // Show events that are listening.

            Dictionary<string, string> commandNameTarget = this.WebContext.Dispatch.CommandHub?.ToDictionary(x => x.Key.Name, y => y.Value.Action.Method.ToString());
            Dictionary<string, List<string>> eventNameTarget = this.WebContext.Dispatch.EventHub?.ToDictionary(x => x.Key.Name, y => y.Value.Select(x => x.Method.ToString()).ToList());
            Dictionary<string, List<Tuple<string, string, string>>> routineNameTarget = this.WebContext.Dispatch.RoutineHub?.ToDictionary(x => x.Key.Name, y => y.Value.Select(x => new Tuple<string, string, string>(x.Action?.Method.ToString(), x.FailOver?.Method.ToString(), x.Rollback?.Method.ToString())).ToList());

            return this.Json(new { commandNameTarget, eventNameTarget, routineNameTarget }, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult Query()
        {
            ODataQueryOptions<T> options = null;
            if (!string.IsNullOrEmpty(this.Request.Url?.Query))
            {
                options = new ODataQueryOptions<T>(new ODataQueryContext(this._odataModelbuilder.GetEdmModel(), typeof(T)), new HttpRequestMessage(HttpMethod.Get, this.Request.Url.AbsoluteUri));
            }

            var queryName = this.WebContext.Dispatch.QueryHub.First(x => x.Key.Name == this.Request.QueryString["QueryName"]).Key;

            return this.Json(this.WebContext.Dispatch.Fetch<T>(queryName, new object[] { options }), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Post(WebRequest<T> webRequest)
        {
            try
            {
                if (!this.IsModelStateValid())
                {
                    this.MigrateModelErrorsToWebContext();
                }
                else
                {
                    this.WebContext.ProcessRequest(webRequest, this);
                }

                var serializerSettings = new JsonSerializerSettings() { PreserveReferencesHandling = PreserveReferencesHandling.Objects }; // Prevent circular reference errors with EF objects and other one-to-many relationships
                return this.Json(JsonConvert.SerializeObject(this.WebContext.WebResponse, serializerSettings));
            }
            catch (Exception ex)
            {
                return this.HandleException(ex);
            }
        }

        public JsonResult PostWithAttachment(WebRequest<T> webRequest, HttpPostedFileBase file)
        {
            try
            {
                BinaryReader b = new BinaryReader(file.InputStream);
                byte[] binData = b.ReadBytes(file.ContentLength);

                webRequest.FileDetail = new Io.Model.FileDetail() { Data = binData, FilePath = file.FileName };
                this.WebContext.ProcessRequest(webRequest, this);

                var serializerSettings = new JsonSerializerSettings() { PreserveReferencesHandling = PreserveReferencesHandling.Objects }; // Prevent circular reference errors with EF objects and other one-to-many relationships

                return this.Json(JsonConvert.SerializeObject(this.WebContext.WebResponse, serializerSettings));
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

            return this.Json(JsonConvert.SerializeObject(this.WebContext.WebResponse, serializerSettings), JsonRequestBehavior.AllowGet);
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
        public virtual Dictionary<CommandName, Dictionary<ResponseTypes, string>> LoadCommandMessageMap()
        {
            this.CommandMessageMap.Add(AppContext.Add, new Dictionary<ResponseTypes, string>());
            this.CommandMessageMap[AppContext.Add].Add(ResponseTypes.Error, "Add failed.");
            this.CommandMessageMap[AppContext.Add].Add(ResponseTypes.Success, "Add success.");

            this.CommandMessageMap.Add(AppContext.Update, new Dictionary<ResponseTypes, string>());
            this.CommandMessageMap[AppContext.Update].Add(ResponseTypes.Error, "Update failed.");
            this.CommandMessageMap[AppContext.Update].Add(ResponseTypes.Success, "Update success.");

            this.CommandMessageMap.Add(AppContext.Remove, new Dictionary<ResponseTypes, string>());
            this.CommandMessageMap[AppContext.Remove].Add(ResponseTypes.Error, "Removal failed.");
            this.CommandMessageMap[AppContext.Remove].Add(ResponseTypes.Success, "Removal success.");
            return this.CommandMessageMap;
        }

        [System.Web.Http.NonAction]
        public virtual string FormatMessage(IResponse response, string message)
        {
            return message;
        }

        [System.Web.Http.NonAction]
        public virtual ActionCommandMap LoadCommandActionMap()
        {
            return new ActionCommandMap(useOneToOneMap: true);
        }

        #endregion

        #region .NET this Facade
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
            this.WireApplicationContext();
            this.WireDomain();
            this.WireDefaultRoutes();
        }
        /// <summary>
        /// The default wiring will use writing the file system. 
        /// </summary>
        public virtual void WireApplicationContext()
        {
            this.WebContext.AppWiring.WireContext<T>(this.WebContext.Dispatch);
        }

        /// <summary>
        /// Override this method to provide new CRUD and GET wiring
        /// </summary>
        public virtual void WireDefaultRoutes()
        {
            this.WebContext.AppWiring.WireDefaultRoutes<T>(this.WebContext.Dispatch);
        }

        /// <summary>
        /// Here to extend any methods outside both the application and domain wiring
        /// </summary>
        public virtual void WireDomain()
        {

        }

        #endregion

    }
}
