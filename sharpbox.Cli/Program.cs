using System;
using System.Diagnostics;
using System.Net.Mail;
using sharpbox.Dispatch.Model;
using sharpbox.Cli.Model.Domain.Sharpbox;
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

      var example = new ExampleMediator("ugwua", smtpClient);

      //Create a response object we can repopulate after each request.
      Response response = null;

      // Give the notification a subscriber. Now whenever this then is broadcast a backlog message will be created for me.
      response = example.Dispatch.Process<Subscriber>(ExtendedCommandNames.AddNotificationSubscriber, "Adding a subcriber to OnException", new object[] { new Subscriber(ExampleMediator.OnUserChange, "ugwua") });
      
      // Next we're going to try the user change command we registered earlier.
      Console.WriteLine("Current UserId: " + example.UserId);
      Console.WriteLine("Please input a new User Id: ");

      var newUserId = Console.ReadLine();

      response = example.Dispatch.Process<String>(ExampleMediator.UserChange, "Changing the userid to lyleb", new object[] { newUserId });

      Console.WriteLine("Current UserId changed to: " + example.UserId);

      // Io: Test file operations. We pass in the dispatcher so everything threads back.
      example.File.Write("Test.xml", example.Notification.BackLog);

      // Notification: Fails and this is intentional as their isn't a proper email client, but shows us what happens when a command fails.
      // response = example.Dispatch.Process<List<BackLogItem>>(ExtendedCommandNames.SendNotification, "Sending out backlogitem", new object[] { example.Notification.BackLog.First() });
      
      // Notification
      Console.WriteLine("###Notification Info####");
      Console.WriteLine("Total subscribers: " + example.Notification.Subscribers.Count);
      Console.WriteLine("Total back log: " + example.Notification.BackLog.Count);

      example.OutPutCommandStream();

      // Audit: See the results in the audit trail
      Console.WriteLine("Audit Trail Count: " + example.Audit.Trail.Count);
        Console.ReadKey();

        // Process a routine
       var finalVersionOfUserId = example.Dispatch.Process<string>(RoutineNames.Example, "Changing the name using a routine.", new object[] {"johnsont"});
       example.OutPutCommandStream();
       Console.WriteLine("Audit Trail Count: " + example.Audit.Trail.Count);
      // Recommend using a 'Final command' to call at the end of each session as well as on exception.
      // This is so you can decide what to do with the backlog messages and audit trail you've collected.
      //example.Final();
       Console.ReadKey();
      // The end result of this demo should be the following:
      // Wired and functional: Logging, Email, and IO
      // Dispatch: A functional pub/sub system for broadcasting events and data changes.
      // Audit: A subscriber to all dispatch events that logs them using your chosen strategy (XML to filesystem by default)
      // Notification: Another subscriber to all dispatch events. Provides a 'queue' of system events and their user friendly messages (dies with session). A list of 'subscribers' to events. A 'backlog' of messages which are the intersection of published events and subscribers to them. Persists to the filesystem by default. 
      Console.ReadLine();
    }


  }
}
