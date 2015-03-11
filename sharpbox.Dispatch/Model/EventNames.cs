using System;
using sharpbox.Util.Enum;

namespace sharpbox.Dispatch.Model
{
    /// <summary>
    /// As a rule there are events and requests. "Events" begin with "On" and have a One-To-Many relationship. Anything else is a request and should generally have a One-to-One relationship
    /// </summary>
    [Serializable]
    public class EventNames : EnumPattern
    {

        public static readonly EventNames OnException = new EventNames("OnException");

        public EventNames(string value) : base(value)
        {
            Name = value;
        }

        public EventNames() { }

        public int EventNameId { get; set; }
        public string Name { get; set; }
    }
}
