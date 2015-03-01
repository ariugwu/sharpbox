using System;
using sharpbox.Dispatch.Model;

namespace sharpbox.Notification.Model
{
    [Serializable]
    public class Subscriber
    {
        public Subscriber(EventNames eventName, string userId)
        {
            EventName = eventName;
            UserId = userId;
        }

        public EventNames EventName { get; set; }
        public string UserId { get; set; }
    }
}
