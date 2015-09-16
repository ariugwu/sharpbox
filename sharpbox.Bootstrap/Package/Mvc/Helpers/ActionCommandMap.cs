using System.Collections.Generic;
using sharpbox.Dispatch.Model;

namespace sharpbox.WebLibrary.Web.Helpers
{
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
            return this.UseOneToOneMap ? new CommandName(uiAction.ToString()) : _map[uiAction];
        }
    }
}
