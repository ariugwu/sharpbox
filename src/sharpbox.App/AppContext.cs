using System;
using System.Net.Mail;

namespace sharpbox.App
{
    using AppWiring;
    using Common.Io;

    using Dispatch;
    using Io.Strategy.Binary;
    using Localization;

    using Dispatch.Model;

    [Serializable]
    public class AppContext
    {
        /// <summary>
        /// A bit of a kitchen sink. Will instantiate Dispatch, Email, File, Audit, Notification, and map default commands and listeners.
        /// </summary>
        /// <param name="cultureCode"></param>
        /// <param name="ioStrategy"></param>
        /// <param name="appWiring"></param>
        public AppContext(string cultureCode = "en-us", IStrategy ioStrategy = null, DefaultAppWiring appWiring = null)
        {
            this.Dispatch = new DispatchContext();
            
            this.File = new Io.Client(ioStrategy ?? new BinaryStrategy());

            this.Localization = new LocalizationContext(cultureCode);

            this.AppWiring = appWiring ?? new DefaultAppWiring(new IoCrud() { File = this.File });

        }

        #region Wiring

        public DefaultAppWiring AppWiring { get; set; }

        #endregion

        #region Encapsulated Context(s)

        public DispatchContext Dispatch { get; set; }

        // Localization
        public LocalizationContext Localization { get; set; }

        // File
        public Io.Client File { get; set; } // A dispatch friendly file client

        #endregion

        #region Properties
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
