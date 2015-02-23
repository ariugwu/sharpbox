using System;
using System.Collections.Generic;
using System.Net.Mail;
using sharpbox.Dispatch.Model;

namespace sharpbox.Cli.Model.Domain.AppContext
{
    public class ExampleMediator : sharpbox.Mediator
    {
        /// <summary>
        /// Extension of the AppContext which contains the dispatcher. All we've done is throw in some dispatcher friendly components.
        /// </summary>
        /// <param name="userIdentity">Used to create a Dispatch instance which is then used to bootstrap Notification, Log, and Audit functionality.</param>
        /// <param name="eventNames">Used to bootstrap Dispatch. this way things like Audit wire into whatever events are in a derived system as well as the default list. If not provided then at empty list is used.</param>
        /// <param name="actionNames"></param>
        /// <param name="smtpClient">Powers the email client.</param>
        public ExampleMediator(string userIdentity, List<EventNames> eventNames, List<CommandNames> actionNames, SmtpClient smtpClient)
            : base(userIdentity, eventNames, actionNames)
        {
            // Append all the events and roles we're going to need.

            Email = new Email.Client(smtpClient);
            File = new Io.Client(new Io.Strategy.Xml.XmlStrategy());

            // The following modules require persistence
            var dispatcher = Dispatch;

            // Setup auditing

            var filename = "AuditLog.xml";
            var persistenceStrategy = new Io.Strategy.Xml.XmlStrategy();

            var auditStrategy = new Audit.Strategy.File.FileStrategy(dispatcher, persistenceStrategy, new Dictionary<string, object> {{"filePath", filename}});
            Audit = new Audit.Client(ref dispatcher, auditStrategy); // This is passed as a ref because the audit class will register itself to various events depending on the audit level chosen.
        
            // Setup Notification
            filename = "NotificationLog.xml";
            var notificationStrategy = new Notification.Strategy.File.FileStrategy(dispatcher, persistenceStrategy, new Dictionary<string, object> { { "filePath", filename } });
            Notification = new Notification.Client(Dispatch, notificationStrategy);
            
            // Setup Logging
            filename = "Log.xml";
            var fileStrategy = new Log.Strategy.File.FileStrategy(persistenceStrategy, new Dictionary<string, object> { { "filePath", filename } });
            Log = new Log.Client(fileStrategy);
        }

        public Feedback Feedback { get; set; } // Should be populated after every action request.

        public Notification.Client Notification { get; set; } // A dispatch friendly notification system.
        public Email.Client Email { get; set; } // A dispatch friendly email client
        public Log.Client Log { get; set; } // A dispatch friendly logger
        public Audit.Client Audit { get; set; } // A dispatch friendly Auditor
        public Io.Client File { get; set; } // A dispatch friendly file client

        public void ExampleProcessFeedback(Request request)
        {
            var feedback = (Feedback) request.Entity;
            Feedback = feedback;
        }

        public void ChangeUser(Request request)
        {
            var entity = (string)request.Entity;
            Dispatch.CurrentUserId = entity;
        }

    }
}
