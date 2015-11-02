using System.Web.Mvc;
using sharpbox.Common.Data.Core;

namespace sharpbox.Common.Data.Helpers.Extensions
{
    using sharpbox.Common.Data.Core;
    using sharpbox.Common.Data;

    public static class WebViewPageExtensions
    {
        public static WebContext<T> WebContext<T>(this WebViewPage wvp) where T : ISharpThing<T>, new()
        {
            return (WebContext<T>)wvp.Session["WebContext"];
        }
    }
}