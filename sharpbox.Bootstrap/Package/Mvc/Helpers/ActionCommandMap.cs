using System.Collections.Generic;
using sharpbox.Dispatch.Model;

namespace sharpbox.WebLibrary.Helpers
{
    using System.Linq;

    public class ActionCommandMap
    {
        public ActionCommandMap(Dictionary<UiAction, CommandName> map)
        {
            this._map = map;
        }

        public ActionCommandMap(bool useOneToOneMap = true) : this(new Dictionary<UiAction, CommandName>())
        {
            UseOneToOneMap = useOneToOneMap;
        }

        private Dictionary<UiAction, CommandName> _map;
         
        public bool UseOneToOneMap { get; private set; }

        public CommandName GetCommandByAction(AppContext appContext, UiAction uiAction)
        {
            return this.UseOneToOneMap ? appContext.Dispatch.CommandHub.First(x => x.Key.Name.Equals(uiAction.Name)).Key : _map[uiAction];
        }
    }
}
