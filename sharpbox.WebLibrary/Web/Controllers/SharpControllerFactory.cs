using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharpbox.WebLibrary.Web.Controllers
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.SessionState;

    public class SharpControllerFactory : IControllerFactory
    {
        //SEE: http://stackoverflow.com/questions/20043306/how-to-impelment-a-custom-controller-factory-asp-net-mvc
        public IController CreateController(RequestContext requestContext, string controllerName)
        {
            switch (controllerName.Trim().ToLower())
            {
                case "environment":
                    return null;
                    break;
                default:
                    return null;
            }
        }

        public SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, string controllerName)
        {
            return SessionStateBehavior.Default;
        }

        public void ReleaseController(IController controller)
        {
            throw new NotImplementedException();
        }
    }
}
