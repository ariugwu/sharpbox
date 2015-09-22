using System.Web.Mvc;
using sharpbox;

namespace sharpbox.WebLibrary.Helpers.Extensions
{
  public static class WebViewPageExtensions
  {
    public static AppContext Site(this WebViewPage wvp)
    {
      return (AppContext)wvp.Session["Site"];
    }
  }
}