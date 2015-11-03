using System;
using System.Collections.Generic;
using System.Web.Mvc;
using FluentValidation;
using Newtonsoft.Json;

namespace sharpbox.WebLibrary.Web.Controllers
{
    using Common.Data.Helpers;
    using Common.Dispatch.Model;
    using Dispatch.Model;

    public abstract class SharpboxController<T> : Controller, ISharpboxController<T>
        where T : new()
    {
        #region Properties

        public AppContext AppContext { get; set; }

        public Dictionary<CommandName, Dictionary<ResponseTypes, string>> CommandMessageMap { get; set; }

        #endregion

        #region Constructor(s)

        protected SharpboxController(AppContext appContext)
        {
            this.WarmBootAppContext(this.AppContext);
        }

        protected SharpboxController() { } 

        #endregion

        #region MVC Override(s)

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            this.AppContext.UploadPath = this.Server.MapPath("~/Upload/");
            this.AppContext.DataPath = this.Server.MapPath("~/App_Data/");
            this.AppContext.CurrentLogOn = null;
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


        public ActionResult Execute(T instance, UiAction action)
        {
            try
            {
                var command = this.LoadCommandActionMap().GetCommandByAction(this.AppContext,action);
                var response = this.AppContext.Dispatch.Process<T>(command, "Processing Web Request", new object[] { instance });


            }
            catch (Exception ex)
            {
                return null;
            }

            return null;
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
        public virtual Dictionary<CommandName, Dictionary<ResponseTypes, string>> LoadCommandMessageMap()
        {
            return this.CommandMessageMap;
        }

        #region Bootstrap methods

        /// <summary>
        /// Every time a request comes in we want pull the latest application info into our AppContext
        /// </summary>
        /// <param name="appContext">The AppContext which handles each request.</param>
        private void WarmBootAppContext(AppContext appContext)
        {
            this.WireDomain();
        }

        /// <summary>
        /// Here to extend any methods outside both the application and domain wiring
        /// </summary>
        public virtual void WireDomain()
        {
            
        }

        #endregion

        #endregion
    }
}
