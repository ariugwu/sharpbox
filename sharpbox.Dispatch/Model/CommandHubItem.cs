using System;

namespace sharpbox.Dispatch.Model
{
    [Serializable]
    public class CommandHubItem : ICommandHubItem
    {
        public Delegate Action { get; set; }
        public EventName EventName { get; set; }
    }
}
