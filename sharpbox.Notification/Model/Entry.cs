using System;
using sharpbox.Dispatch.Model;

namespace sharpbox.Notification.Model
{
    /// <summary>
    /// The Notification Entry extends the package and allows for a more robust message to be created.
    /// </summary>
    public class Entry
    {
        public int EntryId { get; set; }
        public EventNames PublisherName { get; set; }
        public string SystemMessage { get; set; }
        public string UserFriendlyMessage { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
