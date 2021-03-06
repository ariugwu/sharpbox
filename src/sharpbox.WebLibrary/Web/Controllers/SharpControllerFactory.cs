﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using sharpbox.App;

namespace sharpbox.WebLibrary.Web.Controllers
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.SessionState;

    public class SharpControllerFactory : DefaultControllerFactory
    {
        public SharpControllerFactory(App.AppContext defaultContext)
        {
            this.DefaultContext = defaultContext;
            this.PopulateDictionary();
        }

        public SharpControllerFactory()
        {
            this.PopulateDictionary();
        }

        public AppContext DefaultContext { get; set; }

        public Dictionary<string, Type> TypeDictionary { get; set; }

        //SEE: http://stackoverflow.com/questions/20043306/how-to-impelment-a-custom-controller-factory-asp-net-mvc
        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            try
            {
                ////@SEE: http://stackoverflow.com/a/3788316
                //var someType = this.GetType(controllerName.Trim().ToLower());
                //Type yourGenericType = null;

                //object instance = null;

                //if (this.DefaultContext != null && someType != null)
                //{
                //    yourGenericType = typeof(GenericController<>).MakeGenericType(someType);
                //    instance = Activator.CreateInstance(yourGenericType, this.DefaultContext);
                //    return (IController)instance;
                //}

                //if (someType != null)
                //{
                //    yourGenericType = typeof(GenericController<>).MakeGenericType(someType);
                //    instance = Activator.CreateInstance(yourGenericType);
                //    return (IController)instance;
                //}

                TextInfo ti = new CultureInfo("en-US", false).TextInfo;
                controllerName = ti.ToTitleCase(controllerName.Trim());

                if (this.TypeDictionary.ContainsKey(controllerName))
                {
                    var controllerType = this.TypeDictionary[controllerName];
                    var genericController = typeof(GenericController<>).MakeGenericType(controllerType);
                    var instance = Activator.CreateInstance(genericController);

                    return (IController)instance;
                }

                return base.CreateController(requestContext, controllerName);

            }
            catch
            {
                throw;
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


        #region Helper(s)

        private void PopulateDictionary()
        {
            this.TypeDictionary = new Dictionary<string, Type>();
                // Localization
                this.TypeDictionary.Add("Resource", typeof(Localization.Model.Resource));
                this.TypeDictionary.Add("ResourceType", typeof(Localization.Model.ResourceType));
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

        private Type GetType(string entityName)
        {
            Type type = Type.GetType(entityName);
            if (type != null) return type;

            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                entityName = char.ToUpper(entityName[0]) + entityName.Substring(1);
                var fn = asm.GetName().Name;
                Debug.WriteLine(fn);
                type = asm.GetType($"{fn}.Model.{entityName}");
                if (type != null)
                {
                    return type;
                }
            }
            return null;
        }

        #endregion
    }
}
