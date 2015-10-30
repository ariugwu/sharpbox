using System.Collections.Generic;
using sharpbox.Common.Dispatch.Model;
using sharpbox.Dispatch.Model;
using sharpbox.WebLibrary.Core;

namespace sharpbox.WebLibrary.Helpers.ControllerWiring
{
    public class SharpboxControllerWiring
    {
        public static void WireContext<T>(Web.Controllers.ISharpboxController<T> controller) where T : new()
        {
            DefaultAppContextFunctions.AppContext = controller.WebContext.AppContext; // Give our default class access to the context so it can use the built in tools.

            var appContext = controller.WebContext.AppContext;
            //Register Command(s)
            appContext.Dispatch.Register<T>(Add, DefaultAppContextFunctions.Add, OnAdd);
            appContext.Dispatch.Register<T>(Update, DefaultAppContextFunctions.Update, OnUpdate);
            appContext.Dispatch.Register<T>(Remove, DefaultAppContextFunctions.Remove, OnRemove);

            // The Load AppContext Routine which fires on each request.
            appContext.Dispatch.Register<AppContext>(RunLoadAppContextRoutine, LoadEnvironment, OnFrameworkCommand, DefaultAppContextFunctions.LoadEnvironmentFromFile);
            //appContext.Dispatch.Register<AppContext>(RunLoadAppContextRoutine, LoadAuditTrail, OnFrameworkCommand, DefaultAppContextFunctions.LoadAuditTrail);
            appContext.Dispatch.Register<AppContext>(RunLoadAppContextRoutine, LoadAvailableClaims, OnFrameworkCommand, DefaultAppContextFunctions.LoadAvailableClaimsFromFile);
            appContext.Dispatch.Register<AppContext>(RunLoadAppContextRoutine, LoadAvailableUserRoles, OnFrameworkCommand, DefaultAppContextFunctions.LoadAvailableUserRolesFromFile);
            appContext.Dispatch.Register<AppContext>(RunLoadAppContextRoutine, LoadClaimsByRole, OnFrameworkCommand, DefaultAppContextFunctions.LoadClaimsByRoleFromFile);
            appContext.Dispatch.Register<AppContext>(RunLoadAppContextRoutine, LoadUserInRoles, OnFrameworkCommand, DefaultAppContextFunctions.LoadUserInRolesFromFile);
            appContext.Dispatch.Register<AppContext>(RunLoadAppContextRoutine, LoadTextResources, OnFrameworkCommand, DefaultAppContextFunctions.LoadTextResourcesFromFile);

            appContext.Dispatch.Register<AppContext>(SaveEnvironment, DefaultAppContextFunctions.SaveEnvironment, OnFrameworkCommand);
            appContext.Dispatch.Register<AppContext>(SaveAuditTrail, DefaultAppContextFunctions.SaveAuditTrail, OnFrameworkCommand);
            appContext.Dispatch.Register<AppContext>(SaveAvailableClaims, DefaultAppContextFunctions.SaveAvailableClaims, OnFrameworkCommand);
            appContext.Dispatch.Register<AppContext>(SaveAvailableUserRoles, DefaultAppContextFunctions.SaveAvailableUserRoles, OnFrameworkCommand);
            appContext.Dispatch.Register<AppContext>(SaveClaimsByRole, DefaultAppContextFunctions.SaveClaimsByRole, OnFrameworkCommand);
            appContext.Dispatch.Register<AppContext>(SaveUserInRoles, DefaultAppContextFunctions.SaveUsersInRoles, OnFrameworkCommand);
            appContext.Dispatch.Register<AppContext>(SaveTextResources, DefaultAppContextFunctions.SaveTextResources, OnFrameworkCommand);

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

        public static CommandName GetAll = new CommandName("Get");
        public static CommandName GetSingleById = new CommandName("GetSingleById");

        public static CommandName Add = new CommandName("Add");
        public static CommandName Update = new CommandName("Update");
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
        public static EventName OnAdd = new EventName("OnAdd");
        public static EventName OnUpdate = new EventName("OnUpdate");
        public static EventName OnRemove = new EventName("OnRemove");
        public static EventName OnFrameworkCommand = new EventName("OnFrameworkCommand");

        #endregion
    }
}