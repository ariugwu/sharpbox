using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace sharpbox.WebLibrary.Core
{
    using App.Model;
    using Localization.Model;
    using Membership.Model;

    public static class DefaultAppContextFunctions
    {
        public static string EnivronmentFileName { get { return "Environment.dat";} }
        public static string AuditTrailFileName { get { return "AuditTrail.dat"; } }
        public static string AvailableClaimsFileName { get { return "AvailableClaims.dat"; } }
        public static string AvailableUserRolesFileName { get { return "AvailableUserRoles"; } }
        public static string ClaimsByRoleFileName { get { return "ClaimsByRole.dat"; } }
        public static string UsersInRolesFileName { get { return "UsersInRoles.dat"; } }
        public static string TextResourcesFileName { get { return "TextResources.dat"; } }


        public static T Add<T>(T instance)
        {
            throw new NotImplementedException();
        }

        public static T Update<T>(T instance)
        {
            throw new NotImplementedException();
        }

        public static T Remove<T>(T instance)
        {
            throw new NotImplementedException();
        }

        public static T GetById<T>(int id) where T : new()
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<T> GetAll<T>()
        {
            throw new NotImplementedException();
        }
        public static AppContext SaveEnvironment(AppContext appContext)
        {
            var path = Path.Combine(appContext.DataPath, EnivronmentFileName);
            appContext.File.Replace(path, appContext.Environment);

            return appContext;
        }

        public static AppContext SaveAuditTrail(AppContext appContext)
        {
            var path = Path.Combine(appContext.DataPath, AuditTrailFileName);
            var trail = appContext.Audit.Trail.Where(x => x.Type != typeof(AppContext) && !x.Type.IsSubclassOf(typeof(AppContext))).ToList();
            trail = trail.Where(x => x.Request.Type != typeof(AppContext) && !x.Request.Type.IsSubclassOf(typeof(AppContext))).ToList();
            appContext.File.Replace(path, trail);

            return appContext;
        }

        public static AppContext SaveAvailableClaims(AppContext appContext)
        {
            var path = Path.Combine(appContext.DataPath, AvailableClaimsFileName);
            appContext.File.Replace(path, appContext.AvailableClaims);

            return appContext;
        }

        public static AppContext SaveAvailableUserRoles(AppContext appContext)
        {
            var path = Path.Combine(appContext.DataPath, AvailableUserRolesFileName);
            appContext.File.Replace(path, appContext.AvailableUserRoles);

            return appContext;
        }

        public static AppContext SaveClaimsByRole(AppContext appContext)
        {
            var path = Path.Combine(appContext.DataPath, ClaimsByRoleFileName);
            appContext.File.Replace(path, appContext.ClaimsByUserRole);

            return appContext;
        }

        public static AppContext SaveUsersInRoles(AppContext appContext)
        {
            var path = Path.Combine(appContext.DataPath, UsersInRolesFileName);
            appContext.File.Replace(path, appContext.UsersInRoles);

            return appContext;
        }

        public static AppContext SaveTextResources(AppContext appContext)
        {
            var path = Path.Combine(appContext.DataPath, TextResourcesFileName);
            appContext.File.Replace(path, appContext.Resources);

            return appContext;
        }

        public static AppContext LoadEnvironmentFromFile(AppContext appContext)
        {
            var path = Path.Combine(appContext.DataPath, EnivronmentFileName);

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

            return appContext;
        }

        public static AppContext LoadAuditTrail(AppContext appContext)
        {
            var path = Path.Combine(appContext.DataPath, AuditTrailFileName);

            if (appContext.File.Exists(path))
            {
                appContext.Audit.Trail = appContext.File.Read<List<Dispatch.Model.Response>>(path);
            }
            else
            {
                appContext.Audit.Trail = appContext.Audit.Trail ?? new List<Dispatch.Model.Response>();

                appContext.File.Write(path, appContext.Audit.Trail);
            }

            return appContext;
        }

        public static AppContext LoadAvailableClaimsFromFile(AppContext appContext)
        {
            var path = Path.Combine(appContext.DataPath, AvailableClaimsFileName);

            if (appContext.File.Exists(path))
            {
                appContext.AvailableClaims = appContext.File.Read<List<Claim>>(path);
            }
            else
            {
                appContext.AvailableClaims = new List<Claim>();

                appContext.File.Write(path, appContext.AvailableClaims);
            }

            return appContext;
        }

        public static AppContext LoadClaimsByRoleFromFile(AppContext appContext)
        {
            var path = Path.Combine(appContext.DataPath, ClaimsByRoleFileName);

            if (appContext.File.Exists(path))
            {
                appContext.ClaimsByUserRole = appContext.File.Read<List<UserRoleClaim>>(path);
            }
            else
            {
                appContext.ClaimsByUserRole = new List<UserRoleClaim>();

                appContext.File.Write(path, appContext.ClaimsByUserRole);
            }

            return appContext;
        }

        public static AppContext LoadAvailableUserRolesFromFile(AppContext appContext)
        {
            var path = Path.Combine(appContext.DataPath, AvailableUserRolesFileName);

            if (appContext.File.Exists(path))
            {
                appContext.AvailableUserRoles = appContext.File.Read<List<UserRole>>(path);
            }
            else
            {
                appContext.AvailableUserRoles = new List<UserRole>();

                appContext.File.Write(path, appContext.AvailableUserRoles);
            }

            return appContext;
        }

        public static AppContext LoadUserInRolesFromFile(AppContext appContext)
        {
            var path = Path.Combine(appContext.DataPath, UsersInRolesFileName);

            if (appContext.File.Exists(path))
            {
                appContext.UsersInRoles = appContext.File.Read<List<UserUserRole>>(path);
            }
            else
            {
                appContext.UsersInRoles = new List<UserUserRole>();

                appContext.File.Write(path, appContext.UsersInRoles);
            }

            return appContext;
        }

        public static AppContext LoadTextResourcesFromFile(AppContext appContext)
        {
            var path = Path.Combine(appContext.DataPath, TextResourcesFileName);

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

            return appContext;
        }

    }
}