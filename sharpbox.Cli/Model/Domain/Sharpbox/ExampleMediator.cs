using System.Data.Entity.Migrations;
using System.Net.Mail;
using sharpbox.Dispatch.Model;
using sharpbox.EfCodeFirst.Audit;
using sharpbox.EfCodeFirst.Notification;

namespace sharpbox.Cli.Model.Domain.Sharpbox
{
    public class ExampleMediator : BaseMediator
    {
        private AuditContext auditDb = new AuditContext();

        /// <summary>
        /// Extension of the AppContext which contains the dispatcher. All we've done is throw in some dispatcher friendly components.
        /// </summary>
        /// <param name="userIdentity">Example of something you might want encapulated and updated.</param>
        /// <param name="smtpClient">Powers the email client.</param>
        public ExampleMediator(string userIdentity, SmtpClient smtpClient)
            : base()
        {
            UserId = userIdentity;

            Email = new Email.Client(smtpClient);
            File = new Io.Client(new Io.Strategy.Binary.BinaryStrategy());

            Audit = new Audit.Client(); // This is passed as a ref because the audit class will register itself to various events depending on the audit level chosen.

            Notification = new Notification.Client(Email);
        }

        public string UserId { get; set; }

        #region Domain Specific Event(s)
        public static readonly EventNames OnUserChange = new EventNames("OnUserChange");
        public static readonly EventNames Write = new EventNames("OnUserChange");
        #endregion

        #region Domain Specific Commands(s)
        public static readonly CommandNames UserChange = new CommandNames("ChangeUser");
        #endregion

        public string ChangeUser(string userId)
        {
            
            UserId = userId;

          return UserId;
        }

        public string ChangeUserStep2(string userId)
        {

            UserId = userId + "-We changed this through the routine's Second Step";

            return UserId;
        }

        public string ChangeUserStep3(string userId)
        {

            UserId = userId + "-We changed this through the routine's Third Step.";

            return UserId;
        }

        public void Final(Response response)
        {
            Final();
        }

        public void Final()
        {
            using (var db = new AuditContext())
            {
                foreach (var a in Audit.Trail)
                {
                    db.Responses.AddOrUpdate(a);
                    db.SaveChanges();
                }
            }

            using (var db = new NotificationContext())
            {
                foreach (var b in Notification.BackLog)
                {
                    db.BackLogItems.AddOrUpdate(b);
                }
            }
        }

    }
}
