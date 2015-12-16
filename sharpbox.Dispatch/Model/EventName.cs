namespace sharpbox.Dispatch.Model
{
    using System;

    /// <summary>
    /// As a rule there are events and requests. "Events" begin with "On" and have a One-To-Many relationship. Anything else is a request and should generally have a One-to-One relationship
    /// </summary>
    [Serializable]
    public class EventName : EnumPattern
    {

        public static readonly EventName OnException = new EventName("OnException");

        public EventName(string value) : base(value)
        {
            this.EventNameId = Guid.NewGuid();
            this.Name = value;
        }

        public EventName()
        {
            this.EventNameId = Guid.NewGuid();
        }

        public Guid EventNameId { get; set; }
        public string Name { get; set; }

        public Guid? EnvironmentId { get; set; }
    }
}
