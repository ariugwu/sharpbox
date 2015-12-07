using sharpbox.Common.Notification;

namespace sharpbox.Notification.Model
{
    using System;
    using System.Collections.Generic;

    using Common.Data;

    [Serializable]
    [EmailTemplate]
    public class BackLogItem
    {
        public BackLogItem()
        {
            this.BackLogItemId = Guid.NewGuid();
        }

        public Guid BackLogItemId { get; set; }
        public Guid RequestId { get; set; }
        public Guid ResponseId { get; set; }
        public string LogOn { get; set; }
        public bool WasSent { get; set; }
        public DateTime? SentDate { get; set; }
        public int AttempNumber { get; set; }
        public string AttemptMessage { get; set; }
        public DateTime? PreviousAttempTime { get; set; }
        public DateTime? NextAttempTime { get; set; }

        public List<string> To { get; set; }
        public string From { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

        public Guid? EnvironmentId { get; set; }

    }
}
