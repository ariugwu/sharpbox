using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharpbox.WebLibrary.Core.Wiring
{
    using sharpbox.Common.Dispatch.Model;

    public class BaseWiringCommands
    {
        #region Commands and Events

        public static QueryName Get = new QueryName("Get");
        public static QueryName GetById = new QueryName("GetId");

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
