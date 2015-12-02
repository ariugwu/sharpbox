using System;
using System.Net.Mail;

namespace sharpbox.App
{
    using Common.Email;
    using Common.Notification;
    using Dispatch;
    using Localization;
    using Notification.Model;

    [Serializable]
    public class AppContext
    {
        /// <summary>
        /// A bit of a kitchen sink. Will instantiate Dispatch, Email, File, Audit, Notification, and map default commands and listeners.
        /// </summary>
        /// <param name="smtpClient"></param>
        /// <param name="ioStrategy"></param>
        /// <param name="defaultConnectionStringName"></param>
        public AppContext(string cultureCode, SmtpClient smtpClient, Io.Strategy.IStrategy ioStrategy, string defaultConnectionStringName = "Sharpbox")
        {
            this.Dispatch = new DispatchContext();

            this.Email = new Email.Client(smtpClient);
            this.File = new Io.Client(ioStrategy);
            this.Notification = new Notification.NotificationContext();
            this.Localization = new LocalizationContext(cultureCode);

            this.DefaultConnectionStringName = defaultConnectionStringName;
            
            this.RegisterCommands();
            this.MapListeners();
        }

        /// <summary>
        /// Do all the wiring yourself
        /// </summary>
        public AppContext()
        {
        }

        #region Encapsulated Context(s)
        public DispatchContext Dispatch { get; set; }

        /// <summary>
        /// Handy encapsulation for resources you will/could/might use throughout the application
        /// </summary>
        public Model.Environment Environment { get; set; }

        // Membership
        public Membership.IdentityContext IdentityContext { get; set; }

        public string CurrentLogOn { get; set; }

        // Localization
        public LocalizationContext Localization { get; set; }

        // Notification
        public Notification.NotificationContext Notification { get; set; } // A dispatch friendly notification system.

        // Email
        public Email.Client Email { get; set; } // A dispatch friendly email client

        // File
        public Io.Client File { get; set; } // A dispatch friendly file client

        #endregion

        #region Properties
        public string DefaultConnectionStringName { get; set; }
        public string UploadPath { get; set; }
        public string DataPath { get; set; }
        #endregion

        #region Init Method(s)
        /// <summary>
        /// Map our actions and listeners to the dispatch
        /// </summary>
        public void RegisterCommands()
        {
            // Dispatch

            // Email
            this.Dispatch.Register<MailMessage>(EmailCommands.SendEmail, this.SendEmail, EmailEvents.OnEmailSend);

            // IO
            //Dispatch.Register(ExtendedCommandNames.FileCreate, WriteFile, ExtendedEventNames.OnFileCreate);

            // Notification
            this.Dispatch.Register<BackLogItem>(NotificationCommands.SendNotification, (bli) => this.Notification.Notify(bli, this.IdentityContext.UserManger.EmailService), NotificationEvents.OnNotificationNotify);
            this.Dispatch.Register<Subscriber>(NotificationCommands.AddNotificationSubscriber, new Func<Subscriber, Type, Subscriber>(this.Notification.AddSub), NotificationEvents.OnNotificationAddSubScriber);

        }

        public void MapListeners()
        {
            // Look at the concept of 'EchoAllEventsTo'. We can setup a filter that will get call for all events. This is helpful for Audit and Notification subsystems.
            this.Dispatch.Echo(Notification.ProcessEvent);
        }

        public virtual MailMessage SendEmail(MailMessage mail)
        {
            this.Email.Send(mail);
            return mail;
        }
        #endregion
    }
}
