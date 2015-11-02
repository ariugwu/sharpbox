using System.Web.Mvc;
using sharpbox.WebLibrary.Core;

namespace sharpbox.WebLibrary.Helpers.Extensions
{
    using sharpbox.WebLibrary.Data;

    public static class WebViewPageExtensions
    {
        public static WebContext<T> WebContext<T>(this WebViewPage wvp) where T : ISharpThing<T>, new()
        {
            return (WebContext<T>)wvp.Session["WebContext"];
        }
    }
}