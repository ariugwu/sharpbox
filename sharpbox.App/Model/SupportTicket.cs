using System;
using System.Net.Mail;
using Newtonsoft.Json;

namespace sharpbox.App.Model
{
    [Serializable]
    public class SupportTicket
    {
        public SupportTicket(Exception ex)
        {
            SupportTicketId = Guid.NewGuid();
            SerializedException = JsonConvert.SerializeObject(ex);
            ExceptionMessage = ex.ToString(); // This will output all the inner exception messages as well. (Hopefully)
        }

        public SupportTicket()
        {
            SupportTicketId = Guid.NewGuid();
        }

        public Guid SupportTicketId { get; set; }
        public string Description { get; set; }
        public string ExceptionMessage { get; set; }
        public MailMessage Mail { get; set; }
        public string SerializedException { get; set; }
        public bool IsHighPriority { get; set; }
        public string SubmittedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ResponseId { get; set; }
        public int EnvironmentId { get; set; }

    }
}
