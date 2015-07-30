using System;

namespace sharpbox.Dispatch.Model
{
    [Serializable]
    public class RoutineItem
    {
        public CommandName CommandName { get; set; }
        public EventName EventName { get; set; }
        public Delegate Action { get; set; }
        public Delegate FailOver { get; set; }
        public Delegate Rollback { get; set; }

        public string BroadCastMessage { get; set; }
    }
}
