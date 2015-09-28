using System.Web.Mvc;
using sharpbox.WebLibrary.Core;

namespace sharpbox.WebLibrary.Helpers.Extensions
{
    public static class WebViewPageExtensions
    {
        public static WebContext<T> WebContext<T>(this WebViewPage wvp) where T : new()
        {
            return (WebContext<T>)wvp.Session["WebContext"];
        }
    }
}