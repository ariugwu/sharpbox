﻿using System;
using System.Collections.Generic;
using sharpbox.App;
using sharpbox.Dispatch;
using sharpbox.Dispatch.Model;
using sharpbox.Io.Model;

namespace sharpbox.Cli.Model
{
    [Serializable]
    public class ExampleContext : App.AppContext
    {
        /// <summary>
        /// Extension of the DefaultContext which contains the dispatcher. All we've done is throw in some dispatcher friendly components.
        /// </summary>
        /// <param name="LogOn">Example of something you might want encapulated and updated.</param>
        public ExampleContext(string LogOn)
          : base()
        {
            this.Dispatch = new DispatchContext();

            this.LogOn = LogOn;
            
            this.File = new Io.Client(new Io.Strategy.Binary.BinaryStrategy());

            //Wire up the dispatch items
            this.WireUpListeners();
            this.WireUpRoutines();
            this.WireUpCommandHubItems();

        }

        public string LogOn { get; set; }

        #region Domain Specific Event(s)
        public static readonly EventName OnUserChange = new EventName("OnUserChange");
        public static readonly EventName Write = new EventName("OnUserChange");
        public static readonly EventName OnRandomFileWritten = new EventName("OnRandomFileWritten");
        public static readonly EventName OnWriteAuditTrailToDisk = new EventName("OnWriteAuditTrailToDisk");
        public static readonly EventName OnDummyPassThroughCommand = new EventName("OnDummyPassThroughCommand");
        #endregion

        #region Domain Specific Commands(s)
        public static readonly CommandName UserChange = new CommandName("ChangeUser");
        public static readonly CommandName WriteARandomFile = new CommandName("WriteARandomFile");
        public static readonly CommandName WriteAuditTrailToDisk = new CommandName("WriteAuditTrailToDisk");
        public static readonly CommandName DummyPassThroughCommand = new CommandName("DummyPassThroughCommand");
        #endregion

        public void WireUpCommandHubItems()
        {
            // Setup what a command should do and who it should broadcast to when it's done
            this.Dispatch.Register<String>(ExampleContext.UserChange, CapitalizeUser, ExampleContext.OnUserChange, App.AppWiring.DefaultAppWiring.GenericFeedback);

            // We use this command to showcase how you can wire up existing code that you want audited, or otherwise a part of the command stream but not necessarily processed.
            this.Dispatch.Register<String>(ExampleContext.DummyPassThroughCommand, (value) => value, ExampleContext.OnDummyPassThroughCommand, App.AppWiring.DefaultAppWiring.GenericFeedback);

            this.Dispatch.Register<FileDetail>(WriteARandomFile, WriteRandomTxtFile, OnRandomFileWritten, App.AppWiring.DefaultAppWiring.GenericFeedback);
            this.Dispatch.Register<List<Response>>(WriteAuditTrailToDisk, StoreAuditTrailAsBinary, OnWriteAuditTrailToDisk, App.AppWiring.DefaultAppWiring.GenericFeedback);

        }

        public void WireUpRoutines()
        {
            // Let's try a routine
            // Our first routine item will feed a string argument to the UserChange method, broadcast the event through the OnUserChange channel
            this.Dispatch.Register<string>(RoutineName.Example, ExampleContext.UserChange, ExampleContext.OnUserChange, ChangeUser, null, null);
            this.Dispatch.Register<string>(RoutineName.Example, ExampleContext.UserChange, ExampleContext.OnUserChange, ChangeUserStep2, ChangeUserStep2FailOver, null);
            this.Dispatch.Register<string>(RoutineName.Example, ExampleContext.UserChange, ExampleContext.OnUserChange, ChangeUserStep3, null, null);
        }

        public void WireUpListeners()
        {
            // Listen to an 'under the covers' system event
            Dispatch.Listen(EventName.OnException, ExampleListener);

            // All of our internal stuff uses the broadcast system so we'll listen on exception and rethrow.
            // TODO: Does this hide the info? Is there any benefit to throwing it from the offending method/call?
            Dispatch.Listen(EventName.OnException, FireOnException);

            //Dispatch.Listen(NotificationEvents.OnNotificationAddSubScriber, ExampleListener);
            Dispatch.Listen(OnWriteAuditTrailToDisk, ExampleListener);

            // Look at the concept of 'Echo'. We can setup a filter that will get call for all events. This is helpful for Audit and Notification subsystems.
            Dispatch.Echo((response) => { Console.WriteLine("Echo out every message: {0}",response.Message); });
        }

        #region Event and Command Method(s)

        public static void ExampleListener(IResponse response)
        {
            Console.WriteLine("{0} broadcasts: {1}", response.EventName, response.Message);
        }

        public static void FireOnException(IResponse response)
        {
            var exception = response.Entity as Exception;
            if (exception != null) Console.WriteLine("The dispatch is designed to catch all exceptions. You can listen for them and do what you need with the exception itself. Ex Message:" + exception.Message);
        }

        public void OutPutCommandStream()
        {
            Console.WriteLine("### Command Stream Dump ###");
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
        public string ChangeUser(string userId, Feedback feedback)
        {

            LogOn = userId;

            return LogOn;
        }

        public string CapitalizeUser(string userId)
        {

            LogOn = userId.ToUpper();

            return LogOn;
        }

        public string ChangeUserStep2(string userId, Feedback feedback)
        {

            throw new NotImplementedException("Let's see if the app will failover.");
        }

        public string ChangeUserStep2FailOver(string userId, Feedback feedback)
        {

            LogOn = userId + "-We changed this through the routine's Second (Failover) Step.";

            return LogOn;
        }

        public string ChangeUserStep3(string userId, Feedback feedback)
        {

            LogOn = userId + "-We changed this through the routine's Third Step.";

            return LogOn;
        }

        public void Final(Response response)
        {
            Final();
        }

        public void Final()
        {
        }

        #endregion
    }
}
