using System;

namespace sharpbox.Notification.Model
{
    public class BackLog
    {
        public int BackLogId { get; set; }
        public int EntryId { get; set; }
        public string UserId { get; set; }
        public bool WasSent { get; set; }
        public DateTime? SentDate { get; set; }
        public int AttempNumber { get; set; }
        public DateTime? NextAttempTime { get; set; }
        public string Message { get; set; }
    }
}
