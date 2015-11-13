using System.Web.Mvc;

namespace sharpbox.WebLibrary.Helpers.Extensions
{
    using Common.Data;
    using Core;

    public static class WebViewPageExtensions
    {
        public static WebContext<T> WebContext<T>(this WebViewPage wvp) where T : new()
        {
            return (WebContext<T>)wvp.Session["WebContext"];
        }
    }
}