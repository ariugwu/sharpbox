using System.Collections.Generic;
using System.Net.Mail;
using sharpbox.Dispatch.Model;
using sharpbox.Localization.Model;

namespace sharpbox.Intranet.Core.Domain.Dispatch
{
    public class BaseMediator : sharpbox.Mediator
    {
        public BaseMediator(List<EventNames> eventNames, SmtpClient smtpClient)
        {
            Email = new Email.Client(smtpClient);
            File = new Io.Client(new Io.Strategy.Binary.BinaryStrategy());

            // The following modules require persistence
            var dispatcher = Dispatch;

            // Setup auditing

            var filename = "AuditLog.dat";
            var persistenceStrategy = new Io.Strategy.Binary.BinaryStrategy();

            var auditStrategy = new Audit.Strategy.File.FileStrategy(dispatcher, persistenceStrategy, new Dictionary<string, object> { { "filePath", filename } });
            Audit = new Audit.Client(ref dispatcher, eventNames, auditStrategy); // This is passed as a ref because the audit class will register itself to various events depending on the audit level chosen.

            // Setup Notification
            filename = "NotificationLog.dat";
            var notificationStrategy = new Notification.Strategy.File.FileStrategy(persistenceStrategy, Email, new Dictionary<string, object> { { "filePath", filename } });
            Notification = new Notification.Client(ref dispatcher, eventNames, notificationStrategy);
        }

        public Dictionary<ResourceNames, string> Resources { get; set; }

        public Notification.Client Notification { get; set; } // A dispatch friendly notification system.
        public Email.Client Email { get; set; } // A dispatch friendly email client
        public Audit.Client Audit { get; set; } // A dispatch friendly Auditor
        public Io.Client File { get; set; } // A dispatch friendly file client

        #region Event Name(s)

        public static readonly EventNames OnBroadcastCommandStream = new EventNames("OnBroadcastCommandStream");
        public static readonly EventNames OnBroadcastEventStream = new EventNames("OnBroadcastEventStream");

        public static readonly EventNames OnAuditRecord = new EventNames("OnAuditRecord");

        public static readonly EventNames OnEmailSend = new EventNames("OnEmailSend");

        public static readonly EventNames OnFileCreate = new EventNames("OnFileCreate");
        public static readonly EventNames OnFileDelete = new EventNames("OnFileDelete");
        public static readonly EventNames OnFileAccess = new EventNames("OnFileAccess");

        public static readonly EventNames OnDataCreate = new EventNames("OnDataCreate");
        public static readonly EventNames OnDataUpdate = new EventNames("OnDataUpdate");
        public static readonly EventNames OnDataDelete = new EventNames("OnDataDelete");
        public static readonly EventNames OnDataGetById = new EventNames("OnDataGetById");
        public static readonly EventNames OnDataGetAll = new EventNames("OnDataGetAll");

        public static readonly EventNames OnNotificationNotify = new EventNames("OnNotificationNotify");

        public static readonly EventNames OnException = new EventNames("OnException");

        #endregion

        #region Command name(s)

        public static readonly CommandNames BroadcastCommandStream = new CommandNames("BroadcastCommandStream");
        public static readonly CommandNames BroadcastEventStream = new CommandNames("BroadcastEventStream");

        public static readonly CommandNames Notify = new CommandNames("Notify");
        public static readonly CommandNames AddNotificationSubscriber = new CommandNames("AddNotificationSubscriber");

        public static readonly CommandNames ProcessEmail = new CommandNames("ProcessEmail");

        public static readonly CommandNames FileCreate = new CommandNames("FileCreate");
        public static readonly CommandNames FileDelete = new CommandNames("FileDelete");
        public static readonly CommandNames FileAccess = new CommandNames("FileAccess");

        public static readonly CommandNames ChangeUser = new CommandNames("ChangeUser");

        #endregion

        #region Wireup a basic dispatch system

        public void Init()
        {
            // Email

            // IO
            Dispatch.Register(FileCreate, WriteFile, OnFileCreate);

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

        public static List<EventNames> BaseEventList()
        {
            return new List<EventNames>()
            {
                OnFileCreate,
                OnFileDelete,
                OnFileAccess,
                OnEmailSend,
                OnAuditRecord,
                OnNotificationNotify,
                OnBroadcastCommandStream,
                OnBroadcastEventStream,
                OnException
            };
        }

        public static List<CommandNames> BaseCommandList()
        {
            return new List<CommandNames>()
            {
                ChangeUser,
                BroadcastCommandStream,
                BroadcastEventStream,
                Notify,
                ProcessEmail,
                FileCreate,
                FileAccess,
                FileDelete,
                AddNotificationSubscriber
            };
        }

    }
}