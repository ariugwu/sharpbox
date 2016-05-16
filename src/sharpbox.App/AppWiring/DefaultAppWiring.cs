using sharpbox.Localization.Model;

namespace sharpbox.App.AppWiring
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Common.App;

    using Dispatch.Model;

    public class DefaultAppWiring
    {
        public static Feedback GenericFeedback = new Feedback()
        {
            Error = new Resource(new ResourceName("Error"), ResourceType.Feedback, "An error has occured {0}", "en-us", Guid.NewGuid()),
            Warning = new Resource(new ResourceName("Warning"), ResourceType.Feedback, "An error has occured {0}", "en-us", Guid.NewGuid()),
            Info = new Resource(new ResourceName("Info"), ResourceType.Feedback, "An error has occured {0}", "en-us", Guid.NewGuid()),
            Success = new Resource(new ResourceName("Success"), ResourceType.Feedback, "An error has occured {0}", "en-us", Guid.NewGuid())
        };

        public ICrud Crud { get; set; }

        public DefaultAppWiring(ICrud crud)
        {
            this.Crud = crud;
        }

        public DefaultAppWiring()
        {
            
        }

        public void WireDefaultRoutes<T>(IDispatchContext dispatchContext) where T : class, new()
        {
            //Register Queries
            dispatchContext.Register<IList<T>>(App.AppContext.Get, new Func<IList<T>>(this.Crud.Get<T>), GenericFeedback);
            dispatchContext.Register<T>(App.AppContext.GetById, new Func<string, T>(this.Crud.GetById<T>), GenericFeedback);

            //Register Command(s)
            dispatchContext.Register<T>(App.AppContext.Add, this.Crud.Add, App.AppContext.OnAdd, GenericFeedback);
            dispatchContext.Register<T>(App.AppContext.Update, this.Crud.UpdateById, App.AppContext.OnUpdate, GenericFeedback);
            dispatchContext.Register<IList<T>>(App.AppContext.UpdateAll, new Func<List<T>, IList<T>>(this.Crud.Update<T>), App.AppContext.OnUpdateAll, GenericFeedback);
            dispatchContext.Register<T>(App.AppContext.Remove, this.Crud.Remove, App.AppContext.OnRemove, GenericFeedback);

        }
    }
}