namespace sharpbox.WebLibrary.Web.Helpers.AppWiring
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
            appContext.Dispatch.Register<IQueryable<T>>(BaseQueryName.Get, new Func<object,IQueryable<T>>(this.AppPersistence.Get<T>));
            appContext.Dispatch.Register<T>(BaseQueryName.GetById, new Func<string, T>(this.AppPersistence.GetById<T>));

            //Register Command(s)
            appContext.Dispatch.Register<T>(BaseCommandName.Add, this.AppPersistence.Add, BaseEventName.OnAdd);
            appContext.Dispatch.Register<T>(BaseCommandName.Update, this.AppPersistence.Update, BaseEventName.OnUpdate);
            appContext.Dispatch.Register<List<T>>(BaseCommandName.UpdateAll, this.AppPersistence.UpdateAll, BaseEventName.OnUpdateAll);
            appContext.Dispatch.Register<T>(BaseCommandName.Remove, this.AppPersistence.Remove, BaseEventName.OnRemove);

            // Register the message map
            if (controller.CommandMessageMap == null)
            {
                controller.CommandMessageMap = new Dictionary<CommandName, Dictionary<ResponseTypes, string>>();
            }

            controller.CommandMessageMap.Add(BaseCommandName.Add, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[BaseCommandName.Add].Add(ResponseTypes.Error, "Add failed.");
            controller.CommandMessageMap[BaseCommandName.Add].Add(ResponseTypes.Success, "Add success.");

            controller.CommandMessageMap.Add(BaseCommandName.Update, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[BaseCommandName.Update].Add(ResponseTypes.Error, "Update failed.");
            controller.CommandMessageMap[BaseCommandName.Update].Add(ResponseTypes.Success, "Update success.");

            controller.CommandMessageMap.Add(BaseCommandName.Remove, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[BaseCommandName.Remove].Add(ResponseTypes.Error, "Removal failed.");
            controller.CommandMessageMap[BaseCommandName.Remove].Add(ResponseTypes.Success, "Removal success.");
        }

        public void WireContext<T>(WebLibrary.Web.Controllers.ISharpboxScaffoldController<T> controller) where T : new()
        {
            var appContext = controller.WebContext.AppContext;
            this.AppPersistence.AppContext = appContext;

            // The Load AppContext Routine which fires on each request.
            appContext.Dispatch.Register<AppContext>(BaseRoutineName.RunLoadAppContextRoutine, BaseCommandName.LoadEnvironment, BaseEventName.OnFrameworkCommand, this.AppPersistence.LoadEnvironmentFromFile);
            appContext.Dispatch.Register<AppContext>(BaseRoutineName.RunLoadAppContextRoutine, BaseCommandName.LoadAvailableClaims, BaseEventName.OnFrameworkCommand, this.AppPersistence.LoadAvailableClaimsFromFile);
            appContext.Dispatch.Register<AppContext>(BaseRoutineName.RunLoadAppContextRoutine, BaseCommandName.LoadAvailableUserRoles, BaseEventName.OnFrameworkCommand, this.AppPersistence.LoadAvailableUserRolesFromFile);
            appContext.Dispatch.Register<AppContext>(BaseRoutineName.RunLoadAppContextRoutine, BaseCommandName.LoadClaimsByRole, BaseEventName.OnFrameworkCommand, this.AppPersistence.LoadClaimsByRoleFromFile);
            appContext.Dispatch.Register<AppContext>(BaseRoutineName.RunLoadAppContextRoutine, BaseCommandName.LoadUserInRoles, BaseEventName.OnFrameworkCommand, this.AppPersistence.LoadUserInRolesFromFile);
            appContext.Dispatch.Register<AppContext>(BaseRoutineName.RunLoadAppContextRoutine, BaseCommandName.LoadTextResources, BaseEventName.OnFrameworkCommand, this.AppPersistence.LoadTextResourcesFromFile);

            appContext.Dispatch.Register<AppContext>(BaseCommandName.SaveEnvironment, this.AppPersistence.SaveEnvironment, BaseEventName.OnFrameworkCommand);
            appContext.Dispatch.Register<AppContext>(BaseCommandName.SaveAvailableClaims, this.AppPersistence.SaveAvailableClaims, BaseEventName.OnFrameworkCommand);
            appContext.Dispatch.Register<AppContext>(BaseCommandName.SaveAvailableUserRoles, this.AppPersistence.SaveAvailableUserRoles, BaseEventName.OnFrameworkCommand);
            appContext.Dispatch.Register<AppContext>(BaseCommandName.SaveClaimsByRole, this.AppPersistence.SaveClaimsByRole, BaseEventName.OnFrameworkCommand);
            appContext.Dispatch.Register<AppContext>(BaseCommandName.SaveUserInRoles, this.AppPersistence.SaveUsersInRoles, BaseEventName.OnFrameworkCommand);
            appContext.Dispatch.Register<AppContext>(BaseCommandName.SaveTextResources, this.AppPersistence.SaveTextResources, BaseEventName.OnFrameworkCommand);

            //Register Listener(s)

            //Populate Command-to-Message Map
            if (controller.CommandMessageMap == null)
            {
                controller.CommandMessageMap = new Dictionary<CommandName, Dictionary<ResponseTypes, string>>();
            }

            controller.CommandMessageMap.Add(BaseCommandName.LoadEnvironment, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[BaseCommandName.LoadEnvironment].Add(ResponseTypes.Info, "Loading Environment. MVC OnAuthorization.");

            controller.CommandMessageMap.Add(BaseCommandName.LoadAvailableClaims, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[BaseCommandName.LoadAvailableClaims].Add(ResponseTypes.Info, "Loading Available Claims. MVC OnAuthorization.");
            controller.CommandMessageMap.Add(BaseCommandName.LoadAuditTrail, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[BaseCommandName.LoadAuditTrail].Add(ResponseTypes.Info, "Loading Audit trail. MVC OnAuthorization.");
            controller.CommandMessageMap.Add(BaseCommandName.LoadAvailableUserRoles, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[BaseCommandName.LoadAvailableUserRoles].Add(ResponseTypes.Info, "Loading Available User Roles. MVC OnAuthorization.");
            controller.CommandMessageMap.Add(BaseCommandName.LoadClaimsByRole, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[BaseCommandName.LoadClaimsByRole].Add(ResponseTypes.Info, "Loading Claims By Role. MVC OnAuthorization.");
            controller.CommandMessageMap.Add(BaseCommandName.LoadTextResources, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[BaseCommandName.LoadTextResources].Add(ResponseTypes.Info, "Loading Text Resources. MVC OnAuthorization.");
            controller.CommandMessageMap.Add(BaseCommandName.LoadUserInRoles, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[BaseCommandName.LoadUserInRoles].Add(ResponseTypes.Info, "Loading User In Roles. MVC OnAuthorization.");
            controller.CommandMessageMap.Add(BaseCommandName.SaveEnvironment, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[BaseCommandName.SaveEnvironment].Add(ResponseTypes.Info, "Saving Environment.");
            controller.CommandMessageMap.Add(BaseCommandName.SaveAuditTrail, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[BaseCommandName.SaveAuditTrail].Add(ResponseTypes.Info, "Saving Audit Trail.");
            controller.CommandMessageMap.Add(BaseCommandName.SaveAvailableClaims, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[BaseCommandName.SaveAvailableClaims].Add(ResponseTypes.Info, "Saving Available Claims.");
            controller.CommandMessageMap.Add(BaseCommandName.SaveAvailableUserRoles, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[BaseCommandName.SaveAvailableUserRoles].Add(ResponseTypes.Info, "Saving Available User Roles.");
            controller.CommandMessageMap.Add(BaseCommandName.SaveClaimsByRole, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[BaseCommandName.SaveClaimsByRole].Add(ResponseTypes.Info, "Save Claims By Role.");
            controller.CommandMessageMap.Add(BaseCommandName.SaveTextResources, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[BaseCommandName.SaveTextResources].Add(ResponseTypes.Info, "Save Text Resources.");
            controller.CommandMessageMap.Add(BaseCommandName.SaveUserInRoles, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[BaseCommandName.SaveUserInRoles].Add(ResponseTypes.Info, "Save Users In Roles.");
        }
        
    }
}