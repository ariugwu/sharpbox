using System;
using sharpbox.Common.Type;

namespace sharpbox.Dispatch.Model
{
    [Serializable]
    public class RoutineName : EnumPattern
    {

        public static readonly RoutineName Example = new RoutineName("Example");

        public RoutineName(string value)
            : base(value)
        {
            Name = value;
        }

        public RoutineName() { }

        public int EventNameId { get; set; }
        public string Name { get; set; }
    }
}
