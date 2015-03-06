using System;

namespace sharpbox.Dispatch.Model
{
    public class RoutineItem
    {
        public CommandNames CommandName { get; set; }
        public EventNames EventName { get; set; }
        public Delegate Action { get; set; }
        public Delegate FailOver { get; set; }
        public Delegate Rollback { get; set; }

        public string BroadCastMessage { get; set; }
    }
}
