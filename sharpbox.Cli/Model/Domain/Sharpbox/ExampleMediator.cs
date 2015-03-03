using System.Collections.Generic;
using System.Net.Mail;
using sharpbox.Dispatch.Model;

namespace sharpbox.Cli.Model.Domain.Sharpbox
{
    public class ExampleMediator : BaseMediator
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

            // The following modules require persistence
            var dispatcher = Dispatch;

            Audit = new Audit.Client(ref dispatcher, eventNames); // This is passed as a ref because the audit class will register itself to various events depending on the audit level chosen.
        
            Notification = new Notification.Client(ref dispatcher, Email, eventNames);
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

    }
}
