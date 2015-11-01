using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sharpbox.WebLibrary.Helpers.ControllerWiring
{
    public interface IAppWiring
    {
        void WireDefaultRoutes<T>(Web.Controllers.ISharpboxController<T> controller) where T : new();
        void WireContext<T>(Web.Controllers.ISharpboxController<T> controller) where T : new();

    }
}