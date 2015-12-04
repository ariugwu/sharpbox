namespace sharpbox.App.AppWiring
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using sharpbox.Common.App;
    using sharpbox.Common.Dispatch;

    public class DefaultAppWiring : IAppWiring
    {
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
            dispatchContext.Register<IQueryable<T>>(BaseQueryName.Get, new Func<object, IQueryable<T>>(this.Crud.Get<T>));
            dispatchContext.Register<T>(BaseQueryName.GetById, new Func<string, T>(this.Crud.GetById<T>));

            //Register Command(s)
            dispatchContext.Register<T>(BaseCommandName.Add, this.Crud.Add, BaseEventName.OnAdd);
            dispatchContext.Register<T>(BaseCommandName.Update, this.Crud.UpdateById, BaseEventName.OnUpdate);
            dispatchContext.Register<List<T>>(BaseCommandName.UpdateAll, this.Crud.Update, BaseEventName.OnUpdateAll);
            dispatchContext.Register<T>(BaseCommandName.Remove, this.Crud.Remove, BaseEventName.OnRemove);

        }

        public void WireContext<T>(IDispatchContext dispatchContext) where T : class, new()
        {
        }
        
    }
}