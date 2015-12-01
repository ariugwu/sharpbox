namespace sharpbox.WebLibrary.Web.Helpers.AppWiring
{
    using System;
    using System.Collections.Generic;

    using sharpbox.App;
    using sharpbox.Common.Dispatch.Model;
    using sharpbox.Dispatch.Model;
    using sharpbox.WebLibrary.Core.Wiring;

    public class DefaultAppWiring : IAppWiring
    {
        public IAppPersistence AppPersistence { get; set; }

        public DefaultAppWiring(IAppPersistence appPersistence)
        {
        }

        public DefaultAppWiring()
        {
            
        }
        public void WireDefaultRoutes<T>(WebLibrary.Web.Controllers.ISharpboxScaffoldController<T> controller) where T : new()
        {
            var appContext = controller.WebContext.AppContext;
            this.AppPersistence.AppContext = appContext;

            //Register Queries
            appContext.Dispatch.Register<List<T>>(BaseWiringCommands.Get, new Func<object,List<T>>(this.AppPersistence.Get<T>));
            appContext.Dispatch.Register<T>(BaseWiringCommands.GetById, new Func<string, T>(this.AppPersistence.GetById<T>));

            //Register Command(s)
            appContext.Dispatch.Register<T>(BaseWiringCommands.Add, this.AppPersistence.Add, BaseWiringCommands.OnAdd);
            appContext.Dispatch.Register<T>(BaseWiringCommands.Update, this.AppPersistence.Update, BaseWiringCommands.OnUpdate);
            appContext.Dispatch.Register<List<T>>(BaseWiringCommands.UpdateAll, this.AppPersistence.UpdateAll, BaseWiringCommands.OnUpdateAll);
            appContext.Dispatch.Register<T>(BaseWiringCommands.Remove, this.AppPersistence.Remove, BaseWiringCommands.OnRemove);
        }

        public void WireContext<T>(WebLibrary.Web.Controllers.ISharpboxScaffoldController<T> controller) where T : new()
        {
            var appContext = controller.WebContext.AppContext;
            this.AppPersistence.AppContext = appContext;

            // The Load AppContext Routine which fires on each request.
            appContext.Dispatch.Register<AppContext>(BaseWiringCommands.RunLoadAppContextRoutine, BaseWiringCommands.LoadEnvironment, BaseWiringCommands.OnFrameworkCommand, this.AppPersistence.LoadEnvironmentFromFile);
            appContext.Dispatch.Register<AppContext>(BaseWiringCommands.RunLoadAppContextRoutine, BaseWiringCommands.LoadAvailableClaims, BaseWiringCommands.OnFrameworkCommand, this.AppPersistence.LoadAvailableClaimsFromFile);
            appContext.Dispatch.Register<AppContext>(BaseWiringCommands.RunLoadAppContextRoutine, BaseWiringCommands.LoadAvailableUserRoles, BaseWiringCommands.OnFrameworkCommand, this.AppPersistence.LoadAvailableUserRolesFromFile);
            appContext.Dispatch.Register<AppContext>(BaseWiringCommands.RunLoadAppContextRoutine, BaseWiringCommands.LoadClaimsByRole, BaseWiringCommands.OnFrameworkCommand, this.AppPersistence.LoadClaimsByRoleFromFile);
            appContext.Dispatch.Register<AppContext>(BaseWiringCommands.RunLoadAppContextRoutine, BaseWiringCommands.LoadUserInRoles, BaseWiringCommands.OnFrameworkCommand, this.AppPersistence.LoadUserInRolesFromFile);
            appContext.Dispatch.Register<AppContext>(BaseWiringCommands.RunLoadAppContextRoutine, BaseWiringCommands.LoadTextResources, BaseWiringCommands.OnFrameworkCommand, this.AppPersistence.LoadTextResourcesFromFile);

            appContext.Dispatch.Register<AppContext>(BaseWiringCommands.SaveEnvironment, this.AppPersistence.SaveEnvironment, BaseWiringCommands.OnFrameworkCommand);
            appContext.Dispatch.Register<AppContext>(BaseWiringCommands.SaveAvailableClaims, this.AppPersistence.SaveAvailableClaims, BaseWiringCommands.OnFrameworkCommand);
            appContext.Dispatch.Register<AppContext>(BaseWiringCommands.SaveAvailableUserRoles, this.AppPersistence.SaveAvailableUserRoles, BaseWiringCommands.OnFrameworkCommand);
            appContext.Dispatch.Register<AppContext>(BaseWiringCommands.SaveClaimsByRole, this.AppPersistence.SaveClaimsByRole, BaseWiringCommands.OnFrameworkCommand);
            appContext.Dispatch.Register<AppContext>(BaseWiringCommands.SaveUserInRoles, this.AppPersistence.SaveUsersInRoles, BaseWiringCommands.OnFrameworkCommand);
            appContext.Dispatch.Register<AppContext>(BaseWiringCommands.SaveTextResources, this.AppPersistence.SaveTextResources, BaseWiringCommands.OnFrameworkCommand);

            //Register Listener(s)

            //Populate Command-to-Message Map
            controller.CommandMessageMap = new Dictionary<CommandName, Dictionary<ResponseTypes, string>>();

            controller.CommandMessageMap.Add(BaseWiringCommands.Add, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[BaseWiringCommands.Add].Add(ResponseTypes.Error, "Add failed.");
            controller.CommandMessageMap[BaseWiringCommands.Add].Add(ResponseTypes.Success, "Add success.");

            controller.CommandMessageMap.Add(BaseWiringCommands.Update, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[BaseWiringCommands.Update].Add(ResponseTypes.Error, "Update failed.");
            controller.CommandMessageMap[BaseWiringCommands.Update].Add(ResponseTypes.Success, "Update success.");

            controller.CommandMessageMap.Add(BaseWiringCommands.Remove, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[BaseWiringCommands.Remove].Add(ResponseTypes.Error, "Removal failed.");
            controller.CommandMessageMap[BaseWiringCommands.Remove].Add(ResponseTypes.Success, "Removal success.");

            controller.CommandMessageMap.Add(BaseWiringCommands.LoadEnvironment, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[BaseWiringCommands.LoadEnvironment].Add(ResponseTypes.Info, "Loading Environment. MVC OnAuthorization.");

            controller.CommandMessageMap.Add(BaseWiringCommands.LoadAvailableClaims, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[BaseWiringCommands.LoadAvailableClaims].Add(ResponseTypes.Info, "Loading Available Claims. MVC OnAuthorization.");
            controller.CommandMessageMap.Add(BaseWiringCommands.LoadAuditTrail, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[BaseWiringCommands.LoadAuditTrail].Add(ResponseTypes.Info, "Loading Audit trail. MVC OnAuthorization.");
            controller.CommandMessageMap.Add(BaseWiringCommands.LoadAvailableUserRoles, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[BaseWiringCommands.LoadAvailableUserRoles].Add(ResponseTypes.Info, "Loading Available User Roles. MVC OnAuthorization.");
            controller.CommandMessageMap.Add(BaseWiringCommands.LoadClaimsByRole, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[BaseWiringCommands.LoadClaimsByRole].Add(ResponseTypes.Info, "Loading Claims By Role. MVC OnAuthorization.");
            controller.CommandMessageMap.Add(BaseWiringCommands.LoadTextResources, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[BaseWiringCommands.LoadTextResources].Add(ResponseTypes.Info, "Loading Text Resources. MVC OnAuthorization.");
            controller.CommandMessageMap.Add(BaseWiringCommands.LoadUserInRoles, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[BaseWiringCommands.LoadUserInRoles].Add(ResponseTypes.Info, "Loading User In Roles. MVC OnAuthorization.");
            controller.CommandMessageMap.Add(BaseWiringCommands.SaveEnvironment, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[BaseWiringCommands.SaveEnvironment].Add(ResponseTypes.Info, "Saving Environment.");
            controller.CommandMessageMap.Add(BaseWiringCommands.SaveAuditTrail, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[BaseWiringCommands.SaveAuditTrail].Add(ResponseTypes.Info, "Saving Audit Trail.");
            controller.CommandMessageMap.Add(BaseWiringCommands.SaveAvailableClaims, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[BaseWiringCommands.SaveAvailableClaims].Add(ResponseTypes.Info, "Saving Available Claims.");
            controller.CommandMessageMap.Add(BaseWiringCommands.SaveAvailableUserRoles, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[BaseWiringCommands.SaveAvailableUserRoles].Add(ResponseTypes.Info, "Saving Available User Roles.");
            controller.CommandMessageMap.Add(BaseWiringCommands.SaveClaimsByRole, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[BaseWiringCommands.SaveClaimsByRole].Add(ResponseTypes.Info, "Save Claims By Role.");
            controller.CommandMessageMap.Add(BaseWiringCommands.SaveTextResources, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[BaseWiringCommands.SaveTextResources].Add(ResponseTypes.Info, "Save Text Resources.");
            controller.CommandMessageMap.Add(BaseWiringCommands.SaveUserInRoles, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[BaseWiringCommands.SaveUserInRoles].Add(ResponseTypes.Info, "Save Users In Roles.");
        }
        
    }
}