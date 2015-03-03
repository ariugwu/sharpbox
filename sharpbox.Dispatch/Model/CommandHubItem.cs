using System;

namespace sharpbox.Dispatch.Model
{
    [Serializable]
    public class CommandHubItem
    {
        public Delegate Action { get; set; }
        public EventNames EventName { get; set; }
    }
}
