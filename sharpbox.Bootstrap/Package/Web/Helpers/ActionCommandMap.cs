using System.Collections.Generic;
using System.Linq;

namespace sharpbox.WebLibrary.Helpers
{
    
    using Common.Dispatch.Model;

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

        public CommandName GetCommandByAction(AppContext appContext, UiAction uiAction)
        {
            return this.UseOneToOneMap ? appContext.Dispatch.CommandHub.First(x => x.Key.Name.Equals(uiAction.Name)).Key : _map[uiAction];
        }

    }
}
