using System.Data.Entity;
using System.Web.Http.OData.Query;

namespace sharpbox.WebLibrary.Web.Helpers.AppWiring
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using App;
    using Localization.Model;
    using Core.Wiring;

    public class IoAppPersistence : IAppPersistence
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
            return this.Update(instance);
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
            PropertyInfo idInfo = Common.Type.TypeInfoHelper.GetIdPropertyByConvention(typeof(T));

            if (idInfo != null)
            {
                List<T> things = this.Get<T>(null).ToList();

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
            PropertyInfo idInfo = Common.Type.TypeInfoHelper.GetIdPropertyByConvention(typeof(T));
            
            if (idInfo != null)
            {
                List<T> things = this.Get<T>(null).ToList();

                //@SEE: http://stackoverflow.com/a/28658501
                return things.FirstOrDefault(x => idInfo.GetValue(x).ToString() == id);
            }

            throw new ArgumentException("This object has no id that follows the connvention 'ObjectNameId'");
        }

        public IQueryable<T> Get<T>(object arg) where T : new()
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

            IQueryable<T> results;

            if (arg != null)
            {
                var oDataOptions = (ODataQueryOptions<T>) arg;
                var queryResults = oDataOptions.ApplyTo(things.AsQueryable());

                results = (IQueryable<T>)queryResults;

            }
            else
            {
                results = things.AsQueryable();
            }

            return results;
        }

        public AppContext SaveEnvironment(AppContext appContext)
        {
            var path = Path.Combine(appContext.DataPath, this.EnivronmentFileName);

            var envs = new List<App.Model.Environment> { appContext.Environment };

            appContext.File.Replace(path, envs);

            return appContext;
        }

        public AppContext SaveAvailableClaims(AppContext appContext)
        {
            var path = Path.Combine(appContext.DataPath, this.AvailableClaimsFileName);
            //appContext.File.Replace(path, appContext.AvailableClaims);

            return appContext;
        }

        public AppContext SaveAvailableUserRoles(AppContext appContext)
        {
            var path = Path.Combine(appContext.DataPath, this.AvailableUserRolesFileName);
            //appContext.File.Replace(path, appContext.AvailableUserRoles);

            return appContext;
        }

        public AppContext SaveClaimsByRole(AppContext appContext)
        {
            var path = Path.Combine(appContext.DataPath, this.ClaimsByRoleFileName);
            //appContext.File.Replace(path, appContext.ClaimsByUserRole);

            return appContext;
        }

        public AppContext SaveUsersInRoles(AppContext appContext)
        {
            var path = Path.Combine(appContext.DataPath, this.UsersInRolesFileName);
            //appContext.File.Replace(path, appContext.UsersInRoles);

            return appContext;
        }

        public AppContext SaveTextResources(AppContext appContext)
        {
            var path = Path.Combine(appContext.DataPath, this.TextResourcesFileName);
            appContext.File.Replace(path, appContext.Localization.Resources);

            return appContext;
        }

        public AppContext LoadEnvironmentFromFile(AppContext appContext)
        {
            var path = Path.Combine(appContext.DataPath, this.EnivronmentFileName);

            if (appContext.File.Exists(path))
            {
                var envs = appContext.File.Read<List<App.Model.Environment>>(path);
                appContext.Environment = envs.First();
            }
            else
            {
                appContext.Environment = new App.Model.Environment
                {
                    EnvironmentId = 1,
                    ApplicationName = "Sample Application"
                };
                var envs = new List<App.Model.Environment> { appContext.Environment };
                appContext.File.Write(path, envs);
            }

            return appContext;
        }

        public AppContext LoadAvailableClaimsFromFile(AppContext appContext)
        {
            var path = Path.Combine(appContext.DataPath, this.AvailableClaimsFileName);

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
            var path = Path.Combine(appContext.DataPath, this.ClaimsByRoleFileName);

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
            var path = Path.Combine(appContext.DataPath, this.AvailableUserRolesFileName);

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
            var path = Path.Combine(appContext.DataPath, this.UsersInRolesFileName);

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
            var path = Path.Combine(appContext.DataPath, this.TextResourcesFileName);

            if (appContext.File.Exists(path))
            {
                var allResources = appContext.File.Read<List<Resource>>(path);
                appContext.Localization.Resources = allResources.ToDictionary(x => x.ResourceName, x => x.Value);
            }
            else
            {
                appContext.Localization.Resources = new Dictionary<ResourceName, string>();

                appContext.File.Write(path, new List<Resource>());
            }

            return appContext;
        }
    }
}