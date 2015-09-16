using System.Web.Mvc;
using sharpbox;

namespace Toolbox.Helpers.Extensions
{
  public static class WebViewPageExtensions
  {
    public static AppContext Site(this WebViewPage wvp)
    {
      return (AppContext)wvp.Session["Site"];
    }
  }
}