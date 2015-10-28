using System;
using System.IO;
using Environment = sharpbox.App.Model.Environment;

namespace sharpbox.Bootstrap.Package.Core.Extension
{
    public static class DefaultAppContextFunctions
    {
        public static void LoadEnvironmentFromXml(this AppContext appContext)
        {
            var path = Path.Combine(appContext.DataPath, "Environment.xml");

            if (appContext.File.Exists(path))
            {
                appContext.Environment = appContext.File.Read<App.Model.Environment>(path);
            }
            else
            {
                appContext.Environment = new Environment
                {
                    ApplicationId = Guid.NewGuid(), 
                    ApplicationName = "Sample Application"
                };
            }
        }

        public static void LoadAvailableClaims(this AppContext appContext) { }
        
        public static void LoadAvailableUserRoles(this AppContext appContext) { }
        
        public static void LoadCurrentUserRoles(this AppContext appContext) { }
        
        public static void LoadTextResources(this AppContext appContext) { }
    }
}