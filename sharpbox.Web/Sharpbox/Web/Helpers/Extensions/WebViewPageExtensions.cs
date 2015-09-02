using System.Web.Mvc;
using Toolbox.Core.App;

namespace sharpbox.Web.Sharpbox.Web.Helpers.Extensions
{
  public static class WebViewPageExtensions
  {
    public static AppContext Site(this WebViewPage wvp)
    {
      return (AppContext)wvp.Session["Site"];
    }
  }
}