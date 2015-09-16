using System;
using System.Linq;
using System.Web.Caching;
using System.Collections.Generic;
using sharpbox.Localization.Model;

namespace Toolbox.Helpers
{
  public static class CacheCollection
  {

    //An example of caching.
    public static IEnumerable<string> GetDummyData(Cache cache)
    {
      var action = new Func<IEnumerable<string>>(() => { return new List<string>() { "foo", "bar", "lipsum" }; });
      var things = ((IEnumerable<string>)BaseCache.GetInsertCacheItem(cache, BaseCacheNames.DummyData.ToString(), action, null));

      return things;
    }

  }
}
