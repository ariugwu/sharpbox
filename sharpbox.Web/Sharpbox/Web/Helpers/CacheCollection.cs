using System;
using System.Linq;
using System.Web.Caching;
using System.Collections.Generic;
using sharpbox.Localization.Model;

namespace sharpbox.Web.Sharpbox.Web.Helpers
{
  public static class CacheCollection
  {

    public static Dictionary<string, string> GetResourcesByCulture(Cache cache, string cultureInfo, Guid applicationId)
    {
      return GetAppAndGlobalResourcesByAppId(cache, applicationId).Where(x => x.CultureCode.Trim().ToLower().Equals(cultureInfo.Trim().ToLower())).ToDictionary(x => x.ResourceName.Name.Trim().ToLower(), x => x.Value);
    } 

    public static IEnumerable<Resource> GetResources(Cache cache)
    {
      var action = new Func<IEnumerable<Resource>>(Toolbox.Data.Localization.LocalizationRepository.GetResources);
      var things = ((IEnumerable<Resource>)BaseCache.GetInsertCacheItem(cache, BaseCacheNames.Resources.ToString(),action, null));

      return things;
    }

    public static IEnumerable<Resource> GetOnlyAppResourcesByAppId(Cache cache, Guid applicationId)
    {
      var action = new Func<Guid, IEnumerable<Resource>>(Toolbox.Data.Localization.LocalizationRepository.GetOnlyAppResources);
      var things = ((IEnumerable<Resource>)BaseCache.GetInsertCacheItem(cache, BaseCacheNames.OnlyAppResources.ToString(), action, new object[] { applicationId }));

      return things;
    }

    public static IEnumerable<Resource> GetAppAndGlobalResourcesByAppId(Cache cache, Guid applicationId)
    {
      var action = new Func<Guid, IEnumerable<Resource>>(Toolbox.Data.Localization.LocalizationRepository.GetAppAndGlobalResources);
      var things = ((IEnumerable<Resource>)BaseCache.GetInsertCacheItem(cache, BaseCacheNames.AppAndGlobalResources.ToString(), action, new object[] { applicationId }));

      return things;
    }

    //An example of caching.
    public static IEnumerable<string> GetDummyData(Cache cache)
    {
      var action = new Func<IEnumerable<string>>(ViewModel.DummyViewModel.DummyDataForCachingExample);
      var things = ((IEnumerable<string>)BaseCache.GetInsertCacheItem(cache, BaseCacheNames.DummyData.ToString(), action, null));

      return things;
    }

  }
}
