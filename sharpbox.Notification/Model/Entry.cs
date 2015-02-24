using System;
using sharpbox.Dispatch.Model;

namespace sharpbox.Notification.Model
{
    /// <summary>
    /// The Notification Entry extends the package and allows for a more robust message to be created.
    /// </summary>
    [Serializable]
    public class Entry
    {
        public Entry()
        {
        }

        public Guid EntryId { get; set; }
        public EventNames PublisherName { get; set; }
        public string UserFriendlyMessage { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid PackageId { get; set; }
        public Response Response { get; set; }
    }
}
