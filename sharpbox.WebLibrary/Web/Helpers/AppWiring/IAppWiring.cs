using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sharpbox.Common.Data.Helpers.ControllerWiring
{
    using sharpbox.Common.Data;

    public interface IAppWiring
    {
        void WireDefaultRoutes<T>(WebLibrary.Web.Controllers.ISharpboxScaffoldController<T> controller) where T : new();

        void WireContext<T>(WebLibrary.Web.Controllers.ISharpboxScaffoldController<T> controller) where T : new();

    }
}