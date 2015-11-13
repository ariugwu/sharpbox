namespace sharpbox.Notification.Model
{
    using System;
    using System.Collections.Generic;

    using Common.Data;
    using Util.Notification;

    [Serializable]
    [EmailTemplate]
    public class BackLogItem
    {
        public BackLogItem() { }
        public int BackLogItemId { get; set; }
        public int RequestId { get; set; }
        public int ResponseId { get; set; }
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
