using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sharpbox.WebLibrary.Web.Helpers
{
    public class UiAction : Util.Enum.EnumPattern
    {
        public UiAction(string value) : base(value) { }

        public static UiAction Update = new UiAction("Update");
        public static UiAction Insert = new UiAction("Insert");
        public static UiAction Delete = new UiAction("Delete");
    }
}
