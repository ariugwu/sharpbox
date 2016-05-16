using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Caching;

namespace sharpbox.Common.Data.Helpers
{
  public static class GenericCacheCollection<T> where T : class
  {
    public static IEnumerable<T> GetAll(Cache cache, DbContext dbContext)
    {
      var action = new Func<IEnumerable<T>>(dbContext.Set<T>);
      var things = ((IEnumerable<T>)BaseCache.GetInsertCacheItem(cache, typeof(T).Name, action, null));

      return things;
    }
  }
}