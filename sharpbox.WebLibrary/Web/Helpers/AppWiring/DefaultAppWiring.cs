namespace sharpbox.WebLibrary.Web.Helpers.AppWiring
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Common.Dispatch.Model;
    using Core.Wiring;
    using Dispatch.Model;

    public class DefaultAppWiring : IAppWiring
    {
        public IAppPersistence AppPersistence { get; set; }

        public DefaultAppWiring(IAppPersistence appPersistence)
        {
        }

        public DefaultAppWiring()
        {
            
        }

        public void WireDefaultRoutes<T>(Controllers.ISharpboxController<T> controller) where T : class, new()
        {
            var appContext = controller.WebContext.AppContext;
            this.AppPersistence.AppContext = appContext;

            //Register Queries
            appContext.Dispatch.Register<IQueryable<T>>(BaseQueryName.Get, new Func<object,IQueryable<T>>(this.AppPersistence.Get<T>));
            appContext.Dispatch.Register<T>(BaseQueryName.GetById, new Func<string, T>(this.AppPersistence.GetById<T>));

            //Register Command(s)
            appContext.Dispatch.Register<T>(BaseCommandName.Add, this.AppPersistence.Add, BaseEventName.OnAdd);
            appContext.Dispatch.Register<T>(BaseCommandName.Update, this.AppPersistence.Update, BaseEventName.OnUpdate);
            appContext.Dispatch.Register<List<T>>(BaseCommandName.UpdateAll, this.AppPersistence.UpdateAll, BaseEventName.OnUpdateAll);
            appContext.Dispatch.Register<T>(BaseCommandName.Remove, this.AppPersistence.Remove, BaseEventName.OnRemove);

            // Register the message map
            if (controller.CommandMessageMap == null)
            {
                controller.CommandMessageMap = new Dictionary<CommandName, Dictionary<ResponseTypes, string>>();
            }

            controller.CommandMessageMap.Add(BaseCommandName.Add, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[BaseCommandName.Add].Add(ResponseTypes.Error, "Add failed.");
            controller.CommandMessageMap[BaseCommandName.Add].Add(ResponseTypes.Success, "Add success.");

            controller.CommandMessageMap.Add(BaseCommandName.Update, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[BaseCommandName.Update].Add(ResponseTypes.Error, "Update failed.");
            controller.CommandMessageMap[BaseCommandName.Update].Add(ResponseTypes.Success, "Update success.");

            controller.CommandMessageMap.Add(BaseCommandName.Remove, new Dictionary<ResponseTypes, string>());
            controller.CommandMessageMap[BaseCommandName.Remove].Add(ResponseTypes.Error, "Removal failed.");
            controller.CommandMessageMap[BaseCommandName.Remove].Add(ResponseTypes.Success, "Removal success.");
        }

        public void WireContext<T>(Controllers.ISharpboxController<T> controller) where T : class, new()
        {
            var appContext = controller.WebContext.AppContext;
            this.AppPersistence.AppContext = appContext;

            //Populate Command-to-Message Map
            if (controller.CommandMessageMap == null)
            {
                controller.CommandMessageMap = new Dictionary<CommandName, Dictionary<ResponseTypes, string>>();
            }

        }
        
    }
}