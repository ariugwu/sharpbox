using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using sharpbox.Dispatch.Model;
using sharpbox.Cli.Model.Domain.Sharpbox;
using sharpbox.EfCodeFirst.Audit;
using sharpbox.Notification.Model;

namespace sharpbox.Cli
{
  class Program
  {
    static void Main(string[] args)
    {
      // The benefit of the dispatcher is being able to see all subscribed events in one place at one time.
      // This centeralization is put to use with the Audit component which, when set to AuditLevel = All, will make a entry for *every* registered system event. We use basic since we're using xml and want to prevent event reflection. Audit saves file -> file generates audit message -> Audit saves file.
      // In this case we'll be using our extended list (defined in this project) and show how that can naturally hook into whatever events you want to register.
      var smtpClient = new SmtpClient("smtp.google.com", 587);

      AuditContext auditDb = new AuditContext();

      // We need to create a list of events we want the auditor and notification to listen for.
      var possibleEvents = new List<EventNames>()
            {
                ExtendedEventNames.OnFileCreate,
                ExtendedEventNames.OnFileDelete,
                ExtendedEventNames.OnFileAccess,
                ExtendedEventNames.OnEmailSend,
                ExtendedEventNames.OnNotificationNotify,
                ExtendedEventNames.OnNotificationBacklogPersisted,
                ExtendedEventNames.OnNotificationAddSubScriber,
                EventNames.OnException,
                ExampleMediator.OnUserChange
            };

      var example = new ExampleMediator("ugwua", possibleEvents, smtpClient);

      example = WireUpEvents(example); // Wire the commands, and listeners.

      //Create a response object we can repopulate after each request.
      Response response = null;

      // Give the notification a subscriber. Now whenever this then is broadcast a backlog message will be created for me.
      response = example.Dispatch.Process<Subscriber>(ExtendedCommandNames.AddNotificationSubscriber, "Adding a subcriber to OnException", new object[] { new Subscriber(ExampleMediator.OnUserChange, "ugwua") });

      // Next we're going to try the user change command we registered earlier.
      Debug.WriteLine("Current UserId: " + example.UserId);
      response = example.Dispatch.Process<String>(ExampleMediator.UserChange, "Changing the userid to lyleb", new object[] { "lyleb" });
      Debug.WriteLine("Current UserId: " + example.UserId);

      // Io: Test file operations. We pass in the dispatcher so everything threads back.
      example.File.Write("Test.xml", example.Notification.BackLog);

      // Request a broadcast of the command stream to test usefulness.
      response = example.Dispatch.Process<Queue<CommandStreamItem>>(CommandNames.BroadcastCommandStream, "Request to broadcast command stream", new object[] { example.Dispatch.CommandStream});

      // Notification
      response = example.Dispatch.Process<List<BackLogItem>>(ExtendedCommandNames.SendNotification, "Sending out backlogitem", new object[] { example.Notification.BackLog.First() });

      // Notification
      Debug.WriteLine("###Notification Info####");
      Debug.WriteLine("Total subscribers: " + example.Notification.Subscribers.Count);
      Debug.WriteLine("Total back log: " + example.Notification.BackLog.Count);

      // Audit: See the results in the audit trail
      var trail = example.Audit.Trail;
      Debug.WriteLine(trail.Count);

      // The end result of this demo should be the following:
      // Wired and functional: Logging, Email, and IO
      // Dispatch: A functional pub/sub system for broadcasting events and data changes.
      // Audit: A subscriber to all dispatch events that logs them using your chosen strategy (XML to filesystem by default)
      // Notification: Another subscriber to all dispatch events. Provides a 'queue' of system events and their user friendly messages (dies with session). A list of 'subscribers' to events. A 'backlog' of messages which are the intersection of published events and subscribers to them. Persists to the filesystem by default. 
      Console.ReadLine();
    }

    public static ExampleMediator WireUpEvents(ExampleMediator example)
    {
      // Setup what a command should do and who it should broadcast to when it's done
      example.Dispatch.Register<String>(ExampleMediator.UserChange, example.ChangeUser, ExampleMediator.OnUserChange);
      example.Dispatch.Register<Queue<CommandStreamItem>>(CommandNames.BroadcastCommandStream, example.HandleBroadCastCommandStream, ExtendedEventNames.OnBroadcastCommandStream);
      example.Dispatch.Register<Queue<CommandStreamItem>>(CommandNames.BroadcastCommandStreamAfterError, example.HandleBroadCastCommandStream, ExtendedEventNames.OnBroadcastCommandStreamAfterError);
      example.Dispatch.Register<BackLogItem>(ExtendedCommandNames.SendNotification, example.Notification.Notify, ExtendedEventNames.OnNotificationNotify);
      example.Dispatch.Register<Subscriber>(ExtendedCommandNames.AddNotificationSubscriber, example.Notification.AddSub, ExtendedEventNames.OnNotificationAddSubScriber);
      example.Dispatch.Register<List<Response>>(CommandNames.PersistAuditTrail, example.PersistAuditTrail, ExtendedEventNames.OnAuditTrailPersisted);
      example.Dispatch.Register<MailMessage>(ExtendedCommandNames.SendEmail, example.SendEmail, ExtendedEventNames.OnEmailSend);

      // Add some listeners to those broadcasts. NOTE: This is a queue so things will be fired in FIFO order.
      example.Dispatch.Listen(ExtendedEventNames.OnBroadcastCommandStream, OutPutCommandStream);

      // Listen to an 'under the covers' system event
      example.Dispatch.Listen(EventNames.OnException, ExampleListener);

      example.Dispatch.Listen(ExtendedEventNames.OnNotificationAddSubScriber, ExampleMediator.SaveResponseToAuditDatabase);
      // All of our internal stuff uses the broadcast system so we'll listen on exception and rethrow.
      // TODO: Does this hide the info? Is there any benefit to throwing it from the offending method/call?
      example.Dispatch.Listen(EventNames.OnException, FireOnException);

      return example;
    }

    public static void ExampleListener(Response response)
    {
      Debug.WriteLine("{0} broadcasts: {1}", response.EventName, response.Message);
    }

    public static void FireOnException(Response response)
    {
      var exception = response.Entity as Exception;
      if (exception != null) Debug.WriteLine("The dispatch is designed to catch all exceptions. You can listen for them and do what you need with the exception itself. Ex Message:" + exception.Message);
    }

    public static void OutPutCommandStream(Response response)
    {
      Debug.WriteLine("### Event Stream Dump ###");
      foreach (var e in (Queue<CommandStreamItem>)response.Entity)
      {
        Debug.WriteLine("Command:{0} | Request Msg: {1} | Response Msg: '{2}' | Response Channel: {3}", e.Command, e.Request.Message, e.Response.Message, e.Response.EventName);
      }
    }
  }
}
