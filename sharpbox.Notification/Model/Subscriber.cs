using System;
using sharpbox.Dispatch.Model;

namespace sharpbox.Notification.Model
{
    [Serializable]
    public class Subscriber
    {
        public EventNames EventName { get; set; }
        public string UserId { get; set; }
    }
}
