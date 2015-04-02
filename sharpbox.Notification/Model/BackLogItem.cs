using System;
using System.Collections.Generic;

namespace sharpbox.Notification.Model
{
    [Serializable]
    public class BackLogItem
    {
        public BackLogItem() { }

        public int BackLogItemId { get; set; }

        public Guid BackLogItemUniqueId { get; set; }
        public int RequestId { get; set; }
        public Guid RequestUniqueKey { get; set; }
        public int ResponseId { get; set; }
        public Guid ResponseUniqueKey { get; set; }
        public string UserId { get; set; }

        public bool WasSent { get; set; }
        public DateTime? SentDate { get; set; }
        public int AttempNumber { get; set; }
        public DateTime? PreviousAttempTime { get; set; }
        public DateTime? NextAttempTime { get; set; }

        public List<string> To { get; set; }
        public string From { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

        public Guid ApplicationId { get; set; }

    }
}
