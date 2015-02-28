using System;
using System.Collections.Generic;

namespace sharpbox.Notification.Model
{
    [Serializable]
    public class BackLogItem
    {
        public BackLogItem() { }

        public Guid BackLogItemId { get; set; }
        public Guid RequestId { get; set; }
        public Guid ResponseId { get; set; }

        public string UserId { get; set; }

        public bool WasSent { get; set; }
        public DateTime? SentDate { get; set; }
        public int AttempNumber { get; set; }
        public DateTime? PreviousAttempTime { get; set; }
        public DateTime? NextAttempTime { get; set; }

        public List<string> To { get; set; }
        public string From { get; set; }
        public List<string> Cc { get; set; }
        public List<string> Bcc { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

    }
}
