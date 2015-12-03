using System;
using sharpbox.Common.Dispatch.Model;

namespace sharpbox.Dispatch.Model
{
    using sharpbox.Common.Dispatch;

    [Serializable]
    public class CommandHubItem : ICommandHubItem
    {
        public Delegate Action { get; set; }
        public EventName EventName { get; set; }
    }
}
