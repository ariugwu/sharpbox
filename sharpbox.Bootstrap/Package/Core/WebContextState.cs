using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sharpbox.Bootstrap.Package.Core
{
    public enum WebContextState
    {
        Waiting,
        ProcessingRequest,
        ResponseSet,
        ResponseProcessed,
        Faulted
    }
}