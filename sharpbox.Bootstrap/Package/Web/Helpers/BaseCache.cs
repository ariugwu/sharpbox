using System;
using System.Web.Mvc;

namespace sharpbox.WebLibrary.Helpers
{
  public static class BaseCache
  {
    public static object GetInsertCacheItem(HtmlHelper helper, BaseCacheNames key, Delegate action, object[] args, bool flushCache = false)
    {
      return GetInsertCacheItem(helper.ViewContext.HttpContext.Cache, key.ToString(), action, args, flushCache);
    }

    public static object GetInsertCacheItem(System.Web.Caching.Cache cache, string key, Delegate action, object[] args, bool flushCache = false)
    {
      var cacheItem = cache[key];
      if (cacheItem != null && !flushCache) return cacheItem;

      cacheItem = action.DynamicInvoke(args);
      cache.Insert(key, cacheItem);

      return cacheItem;
    }

    public static void RemoveCacheItem(HtmlHelper helper, BaseCacheNames key)
    {
      RemoveCacheItem(helper.ViewContext.HttpContext.Cache, key);
    }

    public static void RemoveCacheItem(System.Web.Caching.Cache cache, BaseCacheNames key)
    {
      cache.Remove(key.ToString());
    }
  }
}