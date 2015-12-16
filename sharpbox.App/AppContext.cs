using System;
using System.Net.Mail;

namespace sharpbox.App
{
    using AppWiring;
    using Common.App;
    using Common.Io;

    using Dispatch;
    using Io.Strategy.Binary;
    using Localization;

    using sharpbox.App.Model;
    using sharpbox.Dispatch.Model;
    using sharpbox.Email;
    using sharpbox.Membership;
    using sharpbox.Notification;

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
        public AppContext(string cultureCode = "en-us", SmtpClient smtpClient = null, IStrategy ioStrategy = null, string defaultConnectionStringName = "Sharpbox", DefaultAppWiring appWiring = null)
        {
            this.Dispatch = new DispatchContext();

            this.Email = new Client(smtpClient ?? new SmtpClient());
            this.File = new Io.Client(ioStrategy ?? new BinaryStrategy());
            this.Notification = new NotificationContext();
            this.Localization = new LocalizationContext(cultureCode);

            this.DefaultConnectionStringName = defaultConnectionStringName;

            this.AppWiring = appWiring ?? new DefaultAppWiring(new IoCrud() { File = this.File });

        }

        #region Wiring

        public DefaultAppWiring AppWiring { get; set; }

        #endregion

        #region Encapsulated Context(s)

        public DispatchContext Dispatch { get; set; }

        /// <summary>
        /// Handy encapsulation for resources you will/could/might use throughout the application
        /// </summary>
        public Environment Environment { get; set; }

        // Membership
        public IdentityContext IdentityContext { get; set; }

        public string CurrentLogOn { get; set; }

        // Localization
        public LocalizationContext Localization { get; set; }

        // Notification
        public NotificationContext Notification { get; set; } // A dispatch friendly notification system.

        // Email
        public Client Email { get; set; } // A dispatch friendly email client

        // File
        public Io.Client File { get; set; } // A dispatch friendly file client

        #endregion

        #region Properties
        public string DefaultConnectionStringName { get; set; }
        public string UploadPath { get; set; }
        public string DataPath { get; set; }
        #endregion

        #region Commands and Events

        public static CommandName Add = new CommandName("Add");

        public static CommandName Update = new CommandName("Update");

        public static CommandName UpdateAll = new CommandName("UpdateAll");

        public static CommandName Remove = new CommandName("Remove");

        public static EventName OnGet = new EventName("OnGet");
        public static EventName OnAdd = new EventName("OnAdd");
        public static EventName OnUpdate = new EventName("OnUpdate");
        public static EventName OnUpdateAll = new EventName("OnUpdateAll");
        public static EventName OnRemove = new EventName("OnRemove");
        public static EventName OnFrameworkCommand = new EventName("OnFrameworkCommand");

        public static QueryName Get = new QueryName("Get");

        public static QueryName GetById = new QueryName("GetId");

        #endregion
    }
}
