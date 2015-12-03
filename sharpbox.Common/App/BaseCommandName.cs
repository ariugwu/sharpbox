namespace sharpbox.Common.App
{
    using sharpbox.Common.Dispatch.Model;

    public class BaseCommandName
    {
        #region Commands and Events

        public static CommandName Add = new CommandName("Add");
        public static CommandName Update = new CommandName("Update");
        public static CommandName UpdateAll = new CommandName("UpdateAll");
        public static CommandName Remove = new CommandName("Remove");

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

        #endregion
    }
}
