using System;
using System.Globalization;
using System.Web.Mvc;

namespace sharpbox.Web.Sharpbox.Web.Helpers
{
  public static class Format
  {
    public static MvcHtmlString Resources(this HtmlHelper helper,AppContext app, string key)
    {
      return MvcHtmlString.Create(Resources(app,helper.ViewContext.HttpContext.Cache, key));
    }

    public static string Resources(AppContext app, System.Web.Caching.Cache cache, string key)
    {
      var culture = CultureInfo.CurrentCulture.Name;
      var resources = CacheCollection.GetResourcesByCulture(cache, culture, (Guid)app.Environment.ApplicationId);
      return resources.ContainsKey(key.Trim().ToLower()) ? resources[key.Trim().ToLower()] : String.Format("<p><strong>Not Resouces could be found for the key: \"{0}\" ({1})</strong></p>", key, culture);
    }
  }
}