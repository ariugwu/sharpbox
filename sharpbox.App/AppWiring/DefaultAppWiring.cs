namespace sharpbox.App.AppWiring
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Common.App;

    using Dispatch.Model;

    public class DefaultAppWiring
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
            dispatchContext.Register<IQueryable<T>>(AppContext.Get, new Func<object, IQueryable<T>>(this.Crud.Get<T>));
            dispatchContext.Register<T>(AppContext.GetById, new Func<string, T>(this.Crud.GetById<T>));

            //Register Command(s)
            dispatchContext.Register<T>(AppContext.Add, this.Crud.Add, AppContext.OnAdd);
            dispatchContext.Register<T>(AppContext.Update, this.Crud.UpdateById, AppContext.OnUpdate);
            dispatchContext.Register<List<T>>(AppContext.UpdateAll, this.Crud.Update, AppContext.OnUpdateAll);
            dispatchContext.Register<T>(AppContext.Remove, this.Crud.Remove, AppContext.OnRemove);

        }

        public void WireContext<T>(IDispatchContext dispatchContext) where T : class, new()
        {
        }
        
    }
}