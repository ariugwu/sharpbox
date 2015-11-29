using System;
using System.Collections.Generic;
using sharpbox.App;

namespace sharpbox.WebLibrary.Core.Wiring
{
    public interface IAppPersistence
    {
        AppContext AppContext { get; set; }

        T Add<T>(T instance) where T : new();

        T Update<T>(T instance) where T : new();

        List<T> UpdateAll<T>(List<T> items) where T : new();

        T Remove<T>(T instance) where T : new();

        List<T> Get<T>() where T : new();

        T GetById<T>(string id) where T : new();

        AppContext SaveEnvironment(AppContext appContext);

        AppContext SaveAvailableClaims(AppContext appContext);

        AppContext SaveAvailableUserRoles(AppContext appContext);

        AppContext SaveClaimsByRole(AppContext appContext);

        AppContext SaveUsersInRoles(AppContext appContext);

        AppContext SaveTextResources(AppContext appContext);

        AppContext LoadEnvironmentFromFile(AppContext appContext);

        AppContext LoadAvailableClaimsFromFile(AppContext appContext);

        AppContext LoadClaimsByRoleFromFile(AppContext appContext);

        AppContext LoadAvailableUserRolesFromFile(AppContext appContext);

        AppContext LoadUserInRolesFromFile(AppContext appContext);

        AppContext LoadTextResourcesFromFile(AppContext appContext);
    }
}