using System;
using System.IO;

namespace sharpbox.WebLibrary.Core
{
    using System.Collections.Generic;
    using System.Linq;

    using App.Model;

    using Membership.Model;

    using sharpbox.Localization.Model;

    public static class DefaultAppContextFunctions
    {
        public static void LoadEnvironmentFromFile(this AppContext appContext)
        {
            var path = Path.Combine(appContext.DataPath, "Environment.dat");

            if (appContext.File.Exists(path))
            {
                appContext.Environment = appContext.File.Read<Environment>(path);
            }
            else
            {
                appContext.Environment = new Environment
                {
                    ApplicationId = Guid.NewGuid(), 
                    ApplicationName = "Sample Application"
                };

                appContext.File.Write(path, appContext.Environment);
            }
        }

        public static void LoadAvailableClaimsFromFile(this AppContext appContext)
        {
            var path = Path.Combine(appContext.DataPath, "AvailableClaims.dat");

            if (appContext.File.Exists(path))
            {
                appContext.AvailableClaims = appContext.File.Read<List<Claim>>(path);
            }
            else
            {
                appContext.AvailableClaims = new List<Claim>();

                appContext.File.Write(path, appContext.AvailableClaims);
            }
        }

        public static void LoadClaimsByRoleFromFile(this AppContext appContext)
        {
            var path = Path.Combine(appContext.DataPath, "ClaimsByRole.dat");

            if (appContext.File.Exists(path))
            {
                appContext.ClaimsByUserRole = appContext.File.Read<List<UserRoleClaim>>(path);
            }
            else
            {
                appContext.ClaimsByUserRole = new List<UserRoleClaim>();

                appContext.File.Write(path, appContext.ClaimsByUserRole);
            }
        }

        public static void LoadAvailableUserRolesFromFile(this AppContext appContext)
        {
            var path = Path.Combine(appContext.DataPath, "AvailableUserRoles.dat");

            if (appContext.File.Exists(path))
            {
                appContext.AvailableUserRoles = appContext.File.Read<List<UserRole>>(path);
            }
            else
            {
                appContext.AvailableUserRoles = new List<UserRole>();

                appContext.File.Write(path, appContext.AvailableUserRoles);
            }
        }

        public static void LoadUserInRolesFromFile(this AppContext appContext)
        {
            var path = Path.Combine(appContext.DataPath, "UsersInRoles.dat");

            if (appContext.File.Exists(path))
            {
                appContext.UsersInRoles = appContext.File.Read<List<UserUserRole>>(path);
            }
            else
            {
                appContext.UsersInRoles = new List<UserUserRole>();

                appContext.File.Write(path, appContext.UsersInRoles);
            }
        }

        public static void LoadTextResourcesFromFile(this AppContext appContext)
        {
            var path = Path.Combine(appContext.DataPath, "Resources.dat");

            if (appContext.File.Exists(path))
            {
                var allResources = appContext.File.Read<List<Resource>>(path);
                appContext.Resources = allResources.ToDictionary(x => x.ResourceName, x => x.Value);
            }
            else
            {
                appContext.Resources = new Dictionary<ResourceName, string>();

                appContext.File.Write(path, new List<Resource>());
            }
        }

    }
}