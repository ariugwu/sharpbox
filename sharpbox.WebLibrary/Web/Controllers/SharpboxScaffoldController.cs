using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

using Newtonsoft.Json;

using NJsonSchema;
using sharpbox.App;

namespace sharpbox.WebLibrary.Web.Controllers
{
    using System.Linq;

    using Common.Dispatch.Model;

    using Core;

    using Dispatch.Model;

    using sharpbox.WebLibrary.Core.Wiring;
    using sharpbox.WebLibrary.Web.Helpers.AppWiring;

    public abstract class SharpboxScaffoldController<T> : SharpboxController<T>, ISharpboxScaffoldController<T>
        where T : new()
    {
        #region Properties

        public WebContext<T> WebContext { get; set; }
        public IAppWiring AppWiring { get; set; }
        public IAppPersistence AppPersistence { get; set; }
        #endregion

        #region Constructor(s)

        protected SharpboxScaffoldController(AppContext appContext, IAppWiring appWiring, IAppPersistence appPersistence)
        {
            // Only create a new WebContext if one doesn't already exist.
            this.WebContext = new WebContext<T>
                                  {
                                      AppContext = appContext,
                                      User = this.User,
                                      WebResponse =
                                          new WebResponse<T>()
                                              {
                                                  ModelErrors = new Dictionary<string, Stack<ModelError>>()
                                              }
                                  };
            
            this.AppPersistence = appPersistence;
            this.AppWiring = appWiring;
            this.AppWiring.AppPersistence = appPersistence;

            this.WarmBootAppContext(this.WebContext.AppContext);
        }

        protected SharpboxScaffoldController(AppContext appContext) : this(appContext, new DefaultAppWiring(), new IoAppPersistence()) { } 

        #endregion

        #region MVC Override(s)

        protected override void InitDuringAuthorization()
        {
            this.WebContext.AppContext.UploadPath = this.Server.MapPath("~/Upload/");
            this.WebContext.AppContext.DataPath = this.Server.MapPath("~/App_Data/");
            this.WebContext.AppContext.Dispatch.Process<AppContext>(BaseWiringCommands.RunLoadAppContextRoutine, "Loading AppContext in OnAuthorization override", new object[] { this.WebContext.AppContext });
            this.WebContext.AppContext.CurrentLogOn = User.Identity.Name;
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

        public virtual JsonResult Get()
        {
            return this.Json((List<T>)this.WebContext.AppContext.Dispatch.Process(BaseWiringCommands.Get, null), JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Duration = 20, VaryByParam = "None")]
        public virtual JsonResult GetAsLookUpDictionary()
        {
            var items = (List<T>)this.WebContext.AppContext.Dispatch.Process(BaseWiringCommands.Get, null);
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
            return this.Json((T)this.WebContext.AppContext.Dispatch.Process(BaseWiringCommands.GetById, new object[] { id }), JsonRequestBehavior.AllowGet);
        }

        public JsonResult JsonSchema()
        {
            // Uses NJsonSchema lib
            var schema = JsonSchema4.FromType<T>();
            var schemaJson = schema.ToJson();

            // Uses Json.Net Schema
            //var generator = new JSchemaGenerator();
            //    generator.GenerationProviders.Add(null);
            //JSchema jSchema = generator.Generate(typeof(T));

            //Try to grab the validator and poke around
            //var validator = this.LoadValidatorByUiAction(new UiAction("Update"));
            //var list = validator.ToList();
            //foreach (var v in list)
            //{
            //    foreach (var vv in v.Validators)
            //    {
            //        vv.
            //    }
            //}

            return this.Json(schemaJson, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DataflowMatrix()
        {
            // Build an object that shows what commands are pointed to what methods and what UiActions are mapped to what commands.
            // Also show routines w/ failover and rollback paths.
            // Show events that are listening.

            Dictionary<string, string> commandNameTest;
            Dictionary<string, string> eventNameTarget;
            Dictionary<string, string> commandNameTarget;
            Dictionary<string, List<string>> routineNameTarget;

            return this.Json(null, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Execute(WebRequest<T> webRequest)
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

        public JsonResult ExecuteWithAttachment(WebRequest<T> webRequest, HttpPostedFileBase file)
        {
            try
            {
                BinaryReader b = new BinaryReader(file.InputStream);
                byte[] binData = b.ReadBytes(file.ContentLength);

                webRequest.FileDetail = new Io.Model.FileDetail(){ Data = binData, FilePath = file.FileName};
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

            return this.Json(JsonConvert.SerializeObject(this.WebContext.WebResponse, serializerSettings));
        }

        #endregion

        #region CommandActionMapping

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

        #region Validation

        public virtual Dictionary<object, Attribute> GetDataAnnotations()
        {
            return new Dictionary<object, Attribute>(0);
        }

        public void AddDataAnotationsToValidator()
        {
            
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
            this.AppWiring.WireContext(this);
        }

        /// <summary>
        /// Override this method to provide new CRUD and GET wiring
        /// </summary>
        public virtual void WireDefaultRoutes()
        {
            this.AppWiring.WireDefaultRoutes(this);   
        }

        #endregion

        #endregion
    }
}
