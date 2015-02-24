using System;

namespace sharpbox.Notification.Model
{
    [Serializable]
    public class BackLog
    {
        public BackLog() { }

        public Guid BackLogId { get; set; }
        public Guid EntryId { get; set; }
        public string UserId { get; set; }
        public bool WasSent { get; set; }
        public DateTime? SentDate { get; set; }
        public int AttempNumber { get; set; }
        public DateTime? NextAttempTime { get; set; }
        public string Message { get; set; }
    }
}
