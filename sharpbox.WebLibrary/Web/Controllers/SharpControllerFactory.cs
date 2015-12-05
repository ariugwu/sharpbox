using System;
using sharpbox.App;

namespace sharpbox.WebLibrary.Web.Controllers
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.SessionState;

    public class SharpControllerFactory : DefaultControllerFactory
    {
        public SharpControllerFactory(AppContext appContext)
        {
            this.AppContext = appContext;
        }
        public AppContext AppContext { get; set; }

        //SEE: http://stackoverflow.com/questions/20043306/how-to-impelment-a-custom-controller-factory-asp-net-mvc
        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            try
            {
                string controllername = requestContext.RouteData.Values["controller"].ToString();
                //@SEE: http://stackoverflow.com/a/3788316
                var someType = this.GetType(controllerName.Trim().ToLower());
                var yourGenericType = typeof(GenericController<>).MakeGenericType(someType);
                var instance = Activator.CreateInstance(yourGenericType, this.AppContext);

                return (IController) instance;

            }
            catch (Exception ex)
            {
                return new ErrorController();
            }
        }

        public override void ReleaseController(IController controller)
        {
            IDisposable dispose = controller as IDisposable;

            dispose?.Dispose();
        }

        public SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, string controllerName)
        {
            return SessionStateBehavior.Default;
        }

        private object GetInstance(string strFullyQualifiedName)
        {
            Type type = Type.GetType(strFullyQualifiedName);
            if (type != null)
                return Activator.CreateInstance(type);
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = asm.GetType(strFullyQualifiedName);
                if (type != null)
                    return Activator.CreateInstance(type);
            }
            return null;
        }

        private Type GetType(string strFullyQualifiedName)
        {
            Type type = Type.GetType(strFullyQualifiedName);
            if (type != null) return type;

            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = asm.GetType(strFullyQualifiedName);
                if (type != null)
                    return type;
            }
            return null;
        }
    }
}
