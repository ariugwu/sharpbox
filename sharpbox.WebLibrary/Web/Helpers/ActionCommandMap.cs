using System;
using System.Collections.Generic;
using System.Linq;
using sharpbox.App;
using sharpbox.WebLibrary.Helpers;

namespace sharpbox.Common.Data.Helpers
{
    
    using Dispatch.Model;

    public class ActionCommandMap
    {
        public ActionCommandMap(Dictionary<UiAction, CommandName> map, bool useOneToOneMap = false)
        {
            this._map = map;
            UseOneToOneMap = useOneToOneMap;
        }

        public ActionCommandMap(bool useOneToOneMap = true) : this(new Dictionary<UiAction, CommandName>(), useOneToOneMap)
        {
        }

        private Dictionary<UiAction, CommandName> _map;
         
        public bool UseOneToOneMap { get; private set; }

        /// <summary>
        /// When a UiAction is passed in we need to find the corresponding CommandName so that the request can be constructed and passed to the Dispatcher for processing.
        /// </summary>
        /// <param name="appContext">AppContext is required to check for command names registered in the Dispatch CommandHub.</param>
        /// <param name="uiAction">The UiAction to lookup. If 'UseOneToOneMap' was defined then this lookup will be in the Dispatch CommandHub, otherwise in the local Map that was provided.</param>
        /// <returns>The mapped CommandName</returns>
        /// <exception cref="ArgumentNullException">Possible null candidates: UiAction, AppContext, AppContext.Dispatch, appContext.Dispatch.CommandHub</exception>
        /// <exception cref="InvalidOperationException">Could be thrown by the CommandHub trying to find the CommandName that matches the UiAction</exception>
        /// <exception cref="KeyNotFoundException">Could be thrown by the UiAction = CommandName map not having the requested key.</exception>
        public CommandName GetCommandByAction(AppContext appContext, UiAction uiAction)
        {
            if (uiAction == null) { throw new ArgumentNullException("uiAction","The UiAction passed in is null"); }

            if (appContext.Dispatch == null || appContext.Dispatch.CommandHub == null) { throw new ArgumentNullException("appContext", "One of these values is null: appContext, appContext.Dispatch, or appContext.Dispatch.CommandHub"); }

            return this.UseOneToOneMap ? appContext.Dispatch.CommandHub.First(x => x.Key.Name.Equals(uiAction.Name)).Key : _map[uiAction];
      
        }

    }
}
