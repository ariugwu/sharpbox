using System;

namespace sharpbox.Dispatch.Model
{
    [Serializable]
    public class CommandHubItem
    {
        public Func<Request, Request> Action { get; set; }
        public EventNames EventName { get; set; }
    }
}
