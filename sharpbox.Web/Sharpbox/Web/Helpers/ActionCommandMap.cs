using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sharpbox.Web.Sharpbox.Web.Helpers
{
    using sharpbox.Dispatch.Model;

    public class ActionCommandMap
    {
        public ActionCommandMap(Dictionary<UiAction, CommandNames> map)
        {
            this._map = map;
        }

        public ActionCommandMap(bool useOneToOneMap = true) : this(new Dictionary<UiAction, CommandNames>())
        {
            UseOneToOneMap = useOneToOneMap;
        }

        private Dictionary<UiAction, CommandNames> _map;
         
        public bool UseOneToOneMap { get; private set; }

        public CommandNames GetCommandByAction(AppContext appContext, UiAction uiAction)
        {
            var list = new List<CommandNames>();
            if (this.UseOneToOneMap)
            {
                list.Select(x => x.Equals(uiAction));
            }

            return _map[uiAction];
        }
    }
}
