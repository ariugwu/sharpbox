using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Antlr.Runtime.Misc;
using sharpbox.Bootstrap.Package.Core;

namespace sharpbox.Common.Data.Core
{
    [Serializable]
    public class WebResponse<T>
    {

        public WebResponse()
        {
            LifeCycleTrail = new Dictionary<LifeCycleHandlerName, List<Tuple<LifeCycleHandlerState, string>>>();
        }

        public T Instance { get; set; }

        public Dictionary<string, Stack<ModelError>> ModelErrors { get; set; }

        public string ResponseType { get; set; }

        public string Message { get; set; }

        // Keep track of LifeCycle and record which (if any) throw an error
        public Dictionary<LifeCycleHandlerName, List<Tuple<LifeCycleHandlerState, string>>> LifeCycleTrail { get; set; }

        public void AddLifeCycleTrailItem(LifeCycleHandlerName name, LifeCycleHandlerState state, string message)
        {
            if (!this.LifeCycleTrail.ContainsKey(name))
            {
                this.LifeCycleTrail[name] = new ListStack<Tuple<LifeCycleHandlerState, string>>();
            }

            this.LifeCycleTrail[name].Add(new Tuple<LifeCycleHandlerState, string>(state, message));
        }
    }
}