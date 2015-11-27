using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace sharpbox.App
{
    using Common.Email;
    using Common.Notification;
    using Dispatch;
    using Localization.Model;
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
        public AppContext(SmtpClient smtpClient, Io.Strategy.IStrategy ioStrategy, string defaultConnectionStringName = "Sharpbox")
        {
            this.Dispatch = new Client();
            this.Email = new Email.Client(smtpClient);
            this.File = new Io.Client(ioStrategy);
            this.Notification = new Notification.Client(this.Email);
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

        /// <summary>
        /// Handy encapsulation for resources you will/could/might use throughout the application
        /// </summary>
        public Model.Environment Environment { get; set; }

        // Membership
        public Membership.IdentityContext IdentityContext { get; set; }

        public string CurrentLogOn { get; set; }

        // Text Resources
        public Dictionary<ResourceName, string> Resources { get; set; }

        public Client Dispatch { get; set; }
        public Notification.Client Notification { get; set; } // A dispatch friendly notification system.
        public Email.Client Email { get; set; } // A dispatch friendly email client
        public Io.Client File { get; set; } // A dispatch friendly file client

        public string DefaultConnectionStringName { get; set; }
        public string UploadPath { get; set; }
        public string DataPath { get; set; }

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
            this.Dispatch.Register<BackLogItem>(NotificationCommands.SendNotification, this.Notification.Notify, NotificationEvents.OnNotificationNotify);
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
    }
}
