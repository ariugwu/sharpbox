using System;
using sharpbox.Util.Enum;

namespace sharpbox.Dispatch.Model
{
    [Serializable]
    public class RoutineNames : EnumPattern
    {

        public static readonly RoutineNames Example = new RoutineNames("Example");

        public RoutineNames(string value)
            : base(value)
        {
        }

        public RoutineNames() { }

        public int EventNameId { get; set; }
        public string Value { get { return _value; } }
    }
}
