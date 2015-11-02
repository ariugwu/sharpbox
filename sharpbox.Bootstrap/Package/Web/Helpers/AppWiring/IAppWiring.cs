using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sharpbox.Common.Data.Helpers.ControllerWiring
{
    using sharpbox.Common.Data;

    public interface IAppWiring
    {
        void WireDefaultRoutes<T>(Web.Controllers.ISharpboxController<T> controller) where T : ISharpThing<T>, new();

        void WireContext<T>(Web.Controllers.ISharpboxController<T> controller) where T : ISharpThing<T>, new();

    }
}