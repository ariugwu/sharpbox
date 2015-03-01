using System.Collections.Generic;
using System.Net.Mail;
using sharpbox.Dispatch;
using sharpbox.Dispatch.Model;
using sharpbox.Localization.Model;

namespace sharpbox
{
    public class BaseMediator
    {
        public BaseMediator(List<EventNames> eventNames, SmtpClient smtpClient, Io.Strategy.IStrategy ioStrategy, Audit.Strategy.IStrategy auditStrategy, Notification.Strategy.IStrategy notificationStrategy)
        {
            Dispatch = new Client();
            Email = new Email.Client(smtpClient);
            File = new Io.Client(ioStrategy);

            // The following modules require persistence
            var dispatcher = Dispatch;

            // Setup auditing
            Audit = new Audit.Client(ref dispatcher, eventNames, auditStrategy); // This is passed as a ref because the audit class will register itself to various events depending on the audit level chosen.

            // Setup Notification
            Notification = new Notification.Client(ref dispatcher, eventNames, notificationStrategy);
        }

        public BaseMediator()
        {
            Dispatch = new Client();
        } 

        public Dictionary<ResourceNames, string> Resources { get; set; }

        public Client Dispatch { get; set; }
        public Notification.Client Notification { get; set; } // A dispatch friendly notification system.
        public Email.Client Email { get; set; } // A dispatch friendly email client
        public Audit.Client Audit { get; set; } // A dispatch friendly Auditor
        public Io.Client File { get; set; } // A dispatch friendly file client

        #region Wireup a basic dispatch system

        public void Init()
        {
            // Email

            // IO
            Dispatch.Register(BaseCommandNames.FileCreate, WriteFile, BaseEventNames.OnFileCreate);

            // Notification
            //Dispatch.Register(AddNotificationSubscriber, Notification.));
        }
        #endregion


        #region Email Call(s)

        public Response SendEmail(Request request) { }

        #endregion

        #region IO Call(s)

        public Response WriteFile(Request request) { }
        public Response ReadFile(Request request) { }
        public Response DeleteFile(Request request) { }

        #endregion

        #region Localization Call(s)

        #endregion

    }
}
