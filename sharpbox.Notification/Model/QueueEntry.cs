using System;
using sharpbox.Dispatch.Model;

namespace sharpbox.Notification.Model
{
    public class QueueEntry
    {
        public int QueueEntryId { get; set; }
        public PublisherNames PublisherName { get; set; }
        public string Message { get; set; }
        public bool WasSent { get; set; }
        public DateTime SentDate { get; set; }
        public int AttempNumber { get; set; }
        public DateTime NextAttempTime { get; set; }
        public string UserId { get; set; }
    }
}
