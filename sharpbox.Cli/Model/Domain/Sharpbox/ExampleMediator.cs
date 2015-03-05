using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net.Mail;
using sharpbox.Dispatch.Model;
using sharpbox.EfCodeFirst.Audit;

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

            // The following modules require persistence
            var dispatcher = Dispatch;

            Audit = new Audit.Client(); // This is passed as a ref because the audit class will register itself to various events depending on the audit level chosen.

            Audit.Trail = auditDb.Responses.ToList();
            Notification = new Notification.Client(Email);
        }

        public string UserId { get; set; }

        #region Domain Specific Event(s)
        public static readonly EventNames OnUserChange = new EventNames("OnUserChange");
        #endregion

        #region Domain Specific Commands(s)
        public static readonly CommandNames UserChange = new CommandNames("ChangeUser");
        #endregion

        public string ChangeUser(string userId)
        {
            
            UserId = userId;

          return UserId;
        }

        public Queue<CommandStreamItem> HandleBroadCastCommandStream(Queue<CommandStreamItem> stream)
        {
          return stream;
        }

        public void SaveResponseToAuditDatabase(Response response)
        {
            using (var db = new AuditContext())
            {
                db.Responses.AddOrUpdate(response);
                db.SaveChanges();
            }

            Dispatch.Broadcast(new Response(){ Message = "Audit record saved to databse. See request object for trigger", RequestUniqueKey = response.RequestUniqueKey, Entity = null, EventName = ExtendedEventNames.OnAuditResponseSaved, ResponseUniqueKey = Guid.NewGuid(), ResponseType = ResponseTypes.Success});
        }

    }
}
