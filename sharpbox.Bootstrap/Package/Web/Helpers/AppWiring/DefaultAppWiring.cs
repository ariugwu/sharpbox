using System;
using System.Collections.Generic;

namespace sharpbox.WebLibrary.Helpers.ControllerWiring
{
    using Common.Data;
    using Common.Data.Helpers.ControllerWiring;
    using Common.Dispatch.Model;
    using Core;
    using Dispatch.Model;

    public class DefaultAppWiring : IAppWiring
    {
        private IAppPersistence _appPersistence;

        public DefaultAppWiring(IAppPersistence appPersistence)
        {
            _appPersistence = appPersistence;
        }

        public void WireDefaultRoutes<T>(WebLibrary.Web.Controllers.ISharpboxScaffoldController<T> controller) where T : ISharpThing<T>, new()
        {
            var appContext = controller.WebContext.AppContext;
            this._appPersistence.AppContext = appContext;

            //Register Queries
            appContext.Dispatch.Register<List<T>>(Get, new Func<List<T>>(this._appPersistence.Get<T>));
            appContext.Dispatch.Register<T>(GetBySharpId, new Func<Guid, T>(this._appPersistence.GetBySharpId<T>));

            //Register Command(s)
            appContext.Dispatch.Register<T>(Add, this._appPersistence.Add, OnAdd);
            appContext.Dispatch.Register<T>(Update, this._appPersistence.Update, OnUpdate);
            appContext.Dispatch.Register<List<T>>(UpdateAll, this._appPersistence.UpdateAll, OnUpdateAll);
            appContext.Dispatch.Register<T>(Remove, this._appPersistence.Remove, OnRemove);
        }

        public void WireContext<T>(WebLibrary.Web.Controllers.ISharpboxScaffoldController<T> controller) where T : ISharpThing<T>, new()
        {
            var appContext = controller.WebContext.AppContext;
            this._appPersistence.AppContext = appContext;

            // The Load AppContext Routine which fires on each request.
            appContext.Dispatch.Register<AppContext>(RunLoadAppContextRoutine, LoadEnvironment, OnFrameworkCommand, this._appPersistence.LoadEnvironmentFromFile);
            //appContext.Dispatch.Register<AppContext>(RunLoadAppContextRoutine, LoadAuditTrail, OnFrameworkCommand, this._appPersistence.LoadAuditTrail);
            appContext.Dispatch.Register<AppContext>(RunLoadAppContextRoutine, LoadAvailableClaims, OnFrameworkCommand, this._appPersistence.LoadAvailableClaimsFromFile);
            appContext.Dispatch.Register<AppContext>(RunLoadAppContextRoutine, LoadAvailableUserRoles, OnFrameworkCommand, this._appPersistence.LoadAvailableUserRolesFromFile);
            appContext.Dispatch.Register<AppContext>(RunLoadAppContextRoutine, LoadClaimsByRole, OnFrameworkCommand, this._appPersistence.LoadClaimsByRoleFromFile);
            appContext.Dispatch.Register<AppContext>(RunLoadAppContextRoutine, LoadUserInRoles, OnFrameworkCommand, this._appPersistence.LoadUserInRolesFromFile);
            appContext.Dispatch.Register<AppContext>(RunLoadAppContextRoutine, LoadTextResources, OnFrameworkCommand, this._appPersistence.LoadTextResourcesFromFile);

            appContext.Dispatch.Register<AppContext>(SaveEnvironment, this._appPersistence.SaveEnvironment, OnFrameworkCommand);
            appContext.Dispatch.Register<AppContext>(SaveAuditTrail, this._appPersistence.SaveAuditTrail, OnFrameworkCommand);
            appContext.Dispatch.Register<AppContext>(SaveAvailableClaims, this._appPersistence.SaveAvailableClaims, OnFrameworkCommand);
            appContext.Dispatch.Register<AppContext>(SaveAvailableUserRoles, this._appPersistence.SaveAvailableUserRoles, OnFrameworkCommand);
            appContext.Dispatch.Register<AppContext>(SaveClaimsByRole, this._appPersistence.SaveClaimsByRole, OnFrameworkCommand);
            appContext.Dispatch.Register<AppContext>(SaveUserInRoles, this._appPersistence.SaveUsersInRoles, OnFrameworkCommand);
            appContext.Dispatch.Register<AppContext>(SaveTextResources, this._appPersistence.SaveTextResources, OnFrameworkCommand);

            //Register Listener(s)

            //Populate Command-to-Message Map
            controller.CommandMessageMap = new Dictionary<CommandName, Dictionary<ResponseTypes, string>>();

            controller.CommandMessageMap.Add(Add, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[Add].Add(ResponseTypes.Error, "Add failed.");
            controller.CommandMessageMap[Add].Add(ResponseTypes.Success, "Add success.");

            controller.CommandMessageMap.Add(Update, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[Update].Add(ResponseTypes.Error, "Update failed.");
            controller.CommandMessageMap[Update].Add(ResponseTypes.Success, "Update success.");

            controller.CommandMessageMap.Add(Remove, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[Remove].Add(ResponseTypes.Error, "Removal failed.");
            controller.CommandMessageMap[Remove].Add(ResponseTypes.Success, "Removal success.");

            controller.CommandMessageMap.Add(LoadEnvironment, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[LoadEnvironment].Add(ResponseTypes.Info, "Loading Environment. MVC OnAuthorization.");

            controller.CommandMessageMap.Add(LoadAvailableClaims, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[LoadAvailableClaims].Add(ResponseTypes.Info, "Loading Available Claims. MVC OnAuthorization.");
            controller.CommandMessageMap.Add(LoadAuditTrail, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[LoadAuditTrail].Add(ResponseTypes.Info, "Loading Audit trail. MVC OnAuthorization.");
            controller.CommandMessageMap.Add(LoadAvailableUserRoles, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[LoadAvailableUserRoles].Add(ResponseTypes.Info, "Loading Available User Roles. MVC OnAuthorization.");
            controller.CommandMessageMap.Add(LoadClaimsByRole, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[LoadClaimsByRole].Add(ResponseTypes.Info, "Loading Claims By Role. MVC OnAuthorization.");
            controller.CommandMessageMap.Add(LoadTextResources, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[LoadTextResources].Add(ResponseTypes.Info, "Loading Text Resources. MVC OnAuthorization.");
            controller.CommandMessageMap.Add(LoadUserInRoles, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[LoadUserInRoles].Add(ResponseTypes.Info, "Loading User In Roles. MVC OnAuthorization.");
            controller.CommandMessageMap.Add(SaveEnvironment, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[SaveEnvironment].Add(ResponseTypes.Info, "Saving Environment.");
            controller.CommandMessageMap.Add(SaveAuditTrail, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[SaveAuditTrail].Add(ResponseTypes.Info, "Saving Audit Trail.");
            controller.CommandMessageMap.Add(SaveAvailableClaims, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[SaveAvailableClaims].Add(ResponseTypes.Info, "Saving Available Claims.");
            controller.CommandMessageMap.Add(SaveAvailableUserRoles, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[SaveAvailableUserRoles].Add(ResponseTypes.Info, "Saving Available User Roles.");
            controller.CommandMessageMap.Add(SaveClaimsByRole, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[SaveClaimsByRole].Add(ResponseTypes.Info, "Save Claims By Role.");
            controller.CommandMessageMap.Add(SaveTextResources, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[SaveTextResources].Add(ResponseTypes.Info, "Save Text Resources.");
            controller.CommandMessageMap.Add(SaveUserInRoles, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[SaveUserInRoles].Add(ResponseTypes.Info, "Save Users In Roles.");
        }

        #region Commands and Events

        public static QueryName Get = new QueryName("Get");
        public static QueryName GetBySharpId = new QueryName("GetBySharpId");

        public static CommandName Add = new CommandName("Add");
        public static CommandName Update = new CommandName("Update");
        public static CommandName UpdateAll = new CommandName("UpdateAll");
        public static CommandName Remove = new CommandName("Remove");

        public static RoutineName RunLoadAppContextRoutine = new RoutineName("RunLoadAppContextRoutine");

        public static CommandName LoadEnvironment = new CommandName("LoadEnvironment");
        public static CommandName LoadAvailableClaims = new CommandName("LoadAvailableClaims");
        public static CommandName LoadAuditTrail = new CommandName("LoadAuditTrail");
        public static CommandName LoadAvailableUserRoles = new CommandName("LoadAvailableUserRoles");
        public static CommandName LoadClaimsByRole = new CommandName("LoadClaimsByRole");
        public static CommandName LoadUserInRoles = new CommandName("LoadUserInRoles");
        public static CommandName LoadTextResources = new CommandName("LoadTextResources");

        public static CommandName SaveEnvironment = new CommandName("SaveEnvironment");
        public static CommandName SaveAuditTrail = new CommandName("SaveAuditTrail");
        public static CommandName SaveAvailableClaims = new CommandName("SaveAvailableClaims");
        public static CommandName SaveAvailableUserRoles = new CommandName("SaveAvailableUserRoles");
        public static CommandName SaveClaimsByRole = new CommandName("SaveClaimsByRole");
        public static CommandName SaveUserInRoles = new CommandName("SaveUserInRoles");
        public static CommandName SaveTextResources = new CommandName("SaveTextResources");

        public static EventName OnGet = new EventName("OnGet");
        public static EventName OnAdd = new EventName("OnAdd");
        public static EventName OnUpdate = new EventName("OnUpdate");
        public static EventName OnUpdateAll = new EventName("OnUpdateAll");
        public static EventName OnRemove = new EventName("OnRemove");
        public static EventName OnFrameworkCommand = new EventName("OnFrameworkCommand");

        #endregion
    }
}