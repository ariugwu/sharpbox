using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Net.Mail;
using sharpbox.Dispatch.Model;
using sharpbox.EfCodeFirst.Audit;
using sharpbox.EfCodeFirst.Notification;
using sharpbox.Io.Model;
using sharpbox.Notification.Model;

namespace sharpbox.Cli.Model.Domain.Sharpbox
{
    [Serializable]
    public class ExampleMediator : BaseMediator
    {
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

            WireUpEvents();
        }

        public string UserId { get; set; }

        #region Domain Specific Event(s)
        public static readonly EventNames OnUserChange = new EventNames("OnUserChange");
        public static readonly EventNames Write = new EventNames("OnUserChange");
        public static readonly EventNames OnRandomFileWritten = new EventNames("OnRandomFileWritten");
        public static readonly EventNames OnWriteAuditTrailToDisk = new EventNames("OnWriteAuditTrailToDisk");
        #endregion

        #region Domain Specific Commands(s)
        public static readonly CommandNames UserChange = new CommandNames("ChangeUser");
        public static readonly CommandNames WriteARandomFile = new CommandNames("WriteARandomFile");
        public static readonly CommandNames WriteAuditTrailToDisk = new CommandNames("WriteAuditTrailToDisk");
        #endregion
                
        public void WireUpEvents()
        {
            // Setup what a command should do and who it should broadcast to when it's done
            Dispatch.Register<String>(ExampleMediator.UserChange, ChangeUser, ExampleMediator.OnUserChange);
            Dispatch.Register<BackLogItem>(ExtendedCommandNames.SendNotification, Notification.Notify, ExtendedEventNames.OnNotificationNotify);
            Dispatch.Register<Subscriber>(ExtendedCommandNames.AddNotificationSubscriber, Notification.AddSub, ExtendedEventNames.OnNotificationAddSubScriber);
            Dispatch.Register<MailMessage>(ExtendedCommandNames.SendEmail, SendEmail, ExtendedEventNames.OnEmailSend);
            Dispatch.Register<FileDetail>(WriteARandomFile, WriteRandomTxtFile, OnRandomFileWritten);
            Dispatch.Register<List<Response>>(WriteAuditTrailToDisk, StoreAuditTrailAsBinary, OnWriteAuditTrailToDisk);
            // Listen to an 'under the covers' system event
            Dispatch.Listen(EventNames.OnException, ExampleListener);

            // All of our internal stuff uses the broadcast system so we'll listen on exception and rethrow.
            // TODO: Does this hide the info? Is there any benefit to throwing it from the offending method/call?
            Dispatch.Listen(EventNames.OnException, FireOnException);

            Dispatch.Listen(ExtendedEventNames.OnNotificationAddSubScriber, ExampleListener);
            Dispatch.Listen(OnWriteAuditTrailToDisk, ExampleListener);

            // Let's try a routine
            // Our first routine item will feed a string argument to the UserChange method, broadcast the event through the OnUserChange channel
            Dispatch.Register<string>(RoutineNames.Example, ExampleMediator.UserChange, ExampleMediator.OnUserChange, ChangeUser, null, null);
            Dispatch.Register<string>(RoutineNames.Example, ExampleMediator.UserChange, ExampleMediator.OnUserChange, ChangeUserStep2,ChangeUserStep2FailOver, null);
            Dispatch.Register<string>(RoutineNames.Example, ExampleMediator.UserChange, ExampleMediator.OnUserChange, ChangeUserStep3, null, null);

            // Look at the concept of 'Echo'. We can setup a filter that will get call for all events. This is helpful for Audit and Notification subsystems.
            Dispatch.Echo(Notification.ProcessEvent);
            Dispatch.Echo(Audit.Record);
        }

        #region Event and Command Method(s)

        public static void ExampleListener(Response response)
        {
            Console.WriteLine("{0} broadcasts: {1}", response.EventName, response.Message);
        }

        public static void FireOnException(Response response)
        {
            var exception = response.Entity as Exception;
            if (exception != null) Console.WriteLine("The dispatch is designed to catch all exceptions. You can listen for them and do what you need with the exception itself. Ex Message:" + exception.Message);
        }

        public void OutPutCommandStream()
        {
            Console.WriteLine("### Event Stream Dump ###");
            foreach (var e in Dispatch.CommandStream)
            {
                Console.WriteLine("Command: " + e.Command + "");
                Console.WriteLine("     Request : " + e.Response.Request.Message);
                Console.WriteLine("     Response: " + e.Response.Message);
            }
        }

        public List<Response> StoreAuditTrailAsBinary(List<Response> trail)
        {
            File.Write("AuditTrail.dat", trail);

            return trail;
        } 

        public FileDetail WriteRandomTxtFile(FileDetail fileDetail)
        {
            File.Write(fileDetail);

            return fileDetail;
        }
        public string ChangeUser(string userId)
        {

            UserId = userId;

            return UserId;
        }

        public string ChangeUserStep2(string userId)
        {

            throw new NotImplementedException("Let's see if the app will failover.");
        }

        public string ChangeUserStep2FailOver(string userId)
        {

            UserId = userId + "-We changed this through the routine's Second (Failover) Step.";

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

        #endregion
    }
}
