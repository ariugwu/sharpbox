using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using sharpbox.App;
using sharpbox.Dispatch.Model;
using sharpbox.Localization.Model;
using sharpbox.Membership.Model;
using Environment = sharpbox.App.Model.Environment;

namespace sharpbox.WebLibrary.Core.Extension
{
    using System.Reflection;

    public class DefaultAppPersistence : IAppPersistence
    {
        public AppContext AppContext { get; set; }

        public string EnivronmentFileName => "Environment.dat";
        public string AuditTrailFileName => "AuditTrail.dat";
        public string AvailableClaimsFileName => "AvailableClaims.dat";
        public string AvailableUserRolesFileName => "AvailableUserRoles";
        public string ClaimsByRoleFileName => "ClaimsByRole.dat";
        public string UsersInRolesFileName => "UsersInRoles.dat";
        public string TextResourcesFileName => "TextResources.dat";

        public T Add<T>(T instance) where T : new()
        {
            List<T> things = this.Get<T>();
            things.Add(instance);

            return instance;
        }

        public List<T> UpdateAll<T>(List<T> items) where T : new()
        {
            var path = Path.Combine(this.AppContext.DataPath, $"{typeof(T).Name}.dat");

            if (this.AppContext.File.Exists(path))
            {
                this.AppContext.File.Replace(path, items);
            }
            else
            {
                items = new List<T>();

                this.AppContext.File.Write(path, items);
            }

            return items;
        }

        public T Update<T>(T instance) where T : new()
        {
            PropertyInfo idInfo = this.GetIdPropertyByConvention(typeof(T));

            if (idInfo != null)
            {
                List<T> things = this.Get<T>();

                //@SEE: http://stackoverflow.com/a/28658501
                var item = things.FirstOrDefault(x => idInfo.GetValue(x).ToString() == idInfo.GetValue(instance).ToString());

                if (item != null)
                {
                    things[things.IndexOf(item)] = instance;
                }
                else
                {
                    things.Add(instance);
                }

                var path = Path.Combine(this.AppContext.DataPath, string.Format("{0}.dat", typeof(T).Name));

                if (this.AppContext.File.Exists(path))
                {
                    this.AppContext.File.Replace(path, things);
                }
                else
                {
                    this.AppContext.File.Write(path, things);
                }

                return instance;
            }
            throw new ArgumentException("This object has no id that follows the connvention 'ObjectNameId'");
        }

        public T Remove<T>(T instance) where T : new()
        {
            instance = new T();

            return this.Update(instance);
        }

        public T GetById<T>(string id) where T : new()
        {
            PropertyInfo idInfo = this.GetIdPropertyByConvention(typeof(T));

            if (idInfo != null)
            {
                List<T> things = this.Get<T>();

                //@SEE: http://stackoverflow.com/a/28658501
                return things.FirstOrDefault(x => idInfo.GetValue(x).ToString() == id);
            }

            throw new ArgumentException("This object has no id that follows the connvention 'ObjectNameId'");
        }

        public List<T> Get<T>() where T : new()
        {
            List<T> things;

            var path = Path.Combine(this.AppContext.DataPath, string.Format("{0}.dat", typeof(T).Name));

            if (this.AppContext.File.Exists(path))
            {
                things = this.AppContext.File.Read<List<T>>(path);
            }
            else
            {
                things = new List<T>();

                this.AppContext.File.Write(path, things);
            }

            return things;
        }

        public AppContext SaveEnvironment(AppContext appContext)
        {
            var path = Path.Combine(appContext.DataPath, EnivronmentFileName);

            var envs = new List<Environment> { appContext.Environment };

            appContext.File.Replace(path, envs);

            return appContext;
        }

        public AppContext SaveAvailableClaims(AppContext appContext)
        {
            var path = Path.Combine(appContext.DataPath, AvailableClaimsFileName);
            //appContext.File.Replace(path, appContext.AvailableClaims);

            return appContext;
        }

        public AppContext SaveAvailableUserRoles(AppContext appContext)
        {
            var path = Path.Combine(appContext.DataPath, AvailableUserRolesFileName);
            //appContext.File.Replace(path, appContext.AvailableUserRoles);

            return appContext;
        }

        public AppContext SaveClaimsByRole(AppContext appContext)
        {
            var path = Path.Combine(appContext.DataPath, ClaimsByRoleFileName);
            //appContext.File.Replace(path, appContext.ClaimsByUserRole);

            return appContext;
        }

        public AppContext SaveUsersInRoles(AppContext appContext)
        {
            var path = Path.Combine(appContext.DataPath, UsersInRolesFileName);
            //appContext.File.Replace(path, appContext.UsersInRoles);

            return appContext;
        }

        public AppContext SaveTextResources(AppContext appContext)
        {
            var path = Path.Combine(appContext.DataPath, TextResourcesFileName);
            appContext.File.Replace(path, appContext.Resources);

            return appContext;
        }

        public AppContext LoadEnvironmentFromFile(AppContext appContext)
        {
            var path = Path.Combine(appContext.DataPath, EnivronmentFileName);

            if (appContext.File.Exists(path))
            {
                var envs = appContext.File.Read<List<Environment>>(path);
                appContext.Environment = envs.First();
            }
            else
            {
                appContext.Environment = new Environment
                {
                    EnvironmentId = 1,
                    ApplicationName = "Sample Application"
                };
                var envs = new List<Environment> { appContext.Environment };
                appContext.File.Write(path, envs);
            }

            return appContext;
        }

        public AppContext LoadAvailableClaimsFromFile(AppContext appContext)
        {
            var path = Path.Combine(appContext.DataPath, AvailableClaimsFileName);

            if (appContext.File.Exists(path))
            {
                //appContext.AvailableClaims = appContext.File.Read<List<Claim>>(path);
            }
            else
            {
                //appContext.AvailableClaims = new List<Claim>();

               // appContext.File.Write(path, appContext.AvailableClaims);
            }

            return appContext;
        }

        public AppContext LoadClaimsByRoleFromFile(AppContext appContext)
        {
            var path = Path.Combine(appContext.DataPath, ClaimsByRoleFileName);

            if (appContext.File.Exists(path))
            {
               // appContext.ClaimsByUserRole = appContext.File.Read<List<UserRoleClaim>>(path);
            }
            else
            {
                //appContext.ClaimsByUserRole = new List<UserRoleClaim>();

                //appContext.File.Write(path, appContext.ClaimsByUserRole);
            }

            return appContext;
        }

        public AppContext LoadAvailableUserRolesFromFile(AppContext appContext)
        {
            var path = Path.Combine(appContext.DataPath, AvailableUserRolesFileName);

            if (appContext.File.Exists(path))
            {
                //appContext.AvailableUserRoles = appContext.File.Read<List<Role>>(path);
            }
            else
            {
                //appContext.AvailableUserRoles = new List<Role>();

                //appContext.File.Write(path, appContext.AvailableUserRoles);
            }

            return appContext;
        }

        public AppContext LoadUserInRolesFromFile(AppContext appContext)
        {
            var path = Path.Combine(appContext.DataPath, UsersInRolesFileName);

            if (appContext.File.Exists(path))
            {
               // appContext.UsersInRoles = appContext.File.Read<List<UserUserRole>>(path);
            }
            else
            {
                //appContext.UsersInRoles = new List<UserUserRole>();

                //appContext.File.Write(path, appContext.UsersInRoles);
            }

            return appContext;
        }

        public AppContext LoadTextResourcesFromFile(AppContext appContext)
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

        private PropertyInfo GetIdPropertyByConvention(Type type)
        {
            string idName = $"{type.Name}Id";

            return type.GetProperty(idName);
        }

    }
}