using System.Collections.Generic;
using System.Net.Mail;
using sharpbox.Dispatch.Model;

namespace sharpbox.Cli.Model.Domain.AppContext
{
    public class ExampleMediator : Mediator
    {
        /// <summary>
        /// Extension of the AppContext which contains the dispatcher. All we've done is throw in some dispatcher friendly components.
        /// </summary>
        /// <param name="userIdentity">Example of something you might want encapulated and updated.</param>
        /// <param name="eventNames">Used to bootstrap Dispatch. this way things like Audit wire into whatever events are in a derived system as well as the default list. If not provided then at empty list is used.</param>
        /// <param name="smtpClient">Powers the email client.</param>
        public ExampleMediator(string userIdentity, List<EventNames> eventNames, SmtpClient smtpClient)
            : base()
        {
            UserId = userIdentity;

            Email = new Email.Client(smtpClient);
            File = new Io.Client(new Io.Strategy.Binary.BinaryStrategy());
            // Setup Logging
            Log = new Log.Client();

            // The following modules require persistence
            var dispatcher = Dispatch;

            // Setup auditing

            var filename = "AuditLog.dat";
            var persistenceStrategy = new Io.Strategy.Binary.BinaryStrategy();

            var auditStrategy = new Audit.Strategy.File.FileStrategy(dispatcher, persistenceStrategy, new Dictionary<string, object> {{"filePath", filename}});
            Audit = new Audit.Client(ref dispatcher, eventNames, auditStrategy); // This is passed as a ref because the audit class will register itself to various events depending on the audit level chosen.
        
            // Setup Notification
            filename = "NotificationLog.dat";
            var notificationStrategy = new Notification.Strategy.File.FileStrategy(persistenceStrategy,Email, new Dictionary<string, object> { { "filePath", filename } });
            Notification = new Notification.Client(ref dispatcher, eventNames, notificationStrategy);
            

        }

        public string UserId { get; set; }

        public Notification.Client Notification { get; set; } // A dispatch friendly notification system.
        public Email.Client Email { get; set; } // A dispatch friendly email client
        public Log.Client Log { get; set; } // A dispatch friendly logger
        public Audit.Client Audit { get; set; } // A dispatch friendly Auditor
        public Io.Client File { get; set; } // A dispatch friendly file client

        public Response ChangeUser(Request request)
        {
            var entity = (string)request.Entity;
            UserId = entity;
            
            return new Response(request, "User changed in the ExampleMediator",ResponseTypes.Success);
        }

        public Response BroadCastEventStream(Request request)
        {
            // Example of being able to bend the rules a little bit and simply use the system to kick off a response, as apposed to deliverying something to be processed.
            request.Entity = Dispatch.CommandStream;
            request.Type = typeof (Queue<CommandStreamItem>);
            return new Response(request, "Broadcasting current command stream.", ResponseTypes.Success);
        }

    }
}
