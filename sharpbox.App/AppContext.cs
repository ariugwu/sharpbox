using System;
using System.Net.Mail;

namespace sharpbox.App
{
    using AppWiring;
    using Common.App;
    using Common.Io;

    using Dispatch;
    using Localization;

    using sharpbox.Io.Strategy.Binary;

    [Serializable]
    public class AppContext
    {
        /// <summary>
        /// A bit of a kitchen sink. Will instantiate Dispatch, Email, File, Audit, Notification, and map default commands and listeners.
        /// </summary>
        /// <param name="cultureCode"></param>
        /// <param name="smtpClient"></param>
        /// <param name="ioStrategy"></param>
        /// <param name="defaultConnectionStringName"></param>
        public AppContext(string cultureCode = "en-us", SmtpClient smtpClient = null, IStrategy ioStrategy = null, string defaultConnectionStringName = "Sharpbox", IAppWiring appWiring = null)
        {
            this.Dispatch = new DispatchContext();

            this.Email = new Email.Client(smtpClient ?? new SmtpClient());
            this.File = new Io.Client(ioStrategy ?? new BinaryStrategy());
            this.Notification = new Notification.NotificationContext();
            this.Localization = new LocalizationContext(cultureCode);

            this.DefaultConnectionStringName = defaultConnectionStringName;

            this.AppWiring = appWiring ?? new DefaultAppWiring(new IoCrud() { File = this.File });

        }

        #region Wiring

        public IAppWiring AppWiring { get; set; }

        #endregion

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
    }
}
