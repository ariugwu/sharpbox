using System;
using System.Collections.Generic;

namespace sharpbox.Notification.Model
{
    [Serializable]
    public class BackLogItem
    {
        public BackLogItem() { }

        public Guid BackLogId { get; set; }
        public Guid EntryId { get; set; }
        public string UserId { get; set; }
        public bool WasSent { get; set; }
        public DateTime? SentDate { get; set; }
        public int AttempNumber { get; set; }
        public DateTime? NextAttempTime { get; set; }

        public List<string> To { get; set; }
        public string From { get; set; }
        public List<string> Cc { get; set; }
        public List<string> Bcc { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

    }
}
