using System;
using System.Collections.Generic;
using System.Net.Mail;
using sharpbox.Dispatch.Model;
using sharpbox.Cli.Model.Domain.Sharpbox;
using sharpbox.Io.Model;
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

            // Create a response object we can repopulate after each request.
            Response response = null;

            // Give the notification a subscriber. Now whenever this event is broadcast a backlog message will be created for me.
            response = example.Dispatch.Process<Subscriber>(ExtendedCommandNames.AddNotificationSubscriber, "Adding a subcriber to OnUserChange.", new object[] { new Subscriber(ExampleMediator.OnUserChange, "ugwua") });

            // Next we're going to try the user change command we registered in the mediator.
            Console.WriteLine("Current UserId: " + example.UserId);
            Console.WriteLine("Please input a new User Id: ");

            var newUserId = Console.ReadLine();

            response = example.Dispatch.Process<String>(ExampleMediator.UserChange, "Changing the userid to lyleb", new object[] { newUserId });

            Console.WriteLine("Please input a string to save to a file named 'foo.txt': ");

            var randomText = Console.ReadLine();
            example.Dispatch.Process<FileDetail>(ExampleMediator.WriteARandomFile, "Example from the CLI project of writing a file.", new object[] { new FileDetail() { FilePath = "Random.txt", Data = System.Text.Encoding.UTF8.GetBytes(randomText) } });

            // Notification: Fails and this is intentional as their isn't a proper email client, but shows us what happens when a command fails.
            // response = example.Dispatch.Process<List<BackLogItem>>(ExtendedCommandNames.SendNotification, "Sending out backlogitem", new object[] { example.Notification.BackLog.First() });

            Console.WriteLine("Example of writing an object to a file. We should see a broad cast anouncement below:");
            var trail = new List<Response>();
            trail.AddRange(example.Audit.Trail);
            example.Dispatch.Process<List<Response>>(ExampleMediator.WriteAuditTrailToDisk, "Writing the current audit trail to a binary file", new object[] { trail });

            Console.WriteLine("Press any key to continue....");
            Console.ReadKey();

            Console.Clear();
            Console.WriteLine("Okay. Now let's output the command stream and see how it what it looks like.");
            Console.WriteLine("Press any key to continue....");
            Console.ReadKey();
            example.OutPutCommandStream();
            Console.WriteLine("Press any key to continue....");
            Console.ReadKey();

            Console.WriteLine("Next we'll take a look at what notification/audit information we've collected. Remember that subscriber we added earlier to OnUserChange?");
            Console.WriteLine("Press any key to continue....");
            Console.ReadKey();
            // Notification
            Console.WriteLine("###Notification Info####");
            Console.WriteLine("Total subscribers: " + example.Notification.Subscribers.Count);
            Console.WriteLine("Total back log: " + example.Notification.BackLog.Count);
            // Audit: See the results in the audit trail
            Console.WriteLine("###Audit Info####");
            Console.WriteLine("Audit Trail Count: " + example.Audit.Trail.Count);
            Console.WriteLine("Press any key to continue....");
            Console.ReadKey();

            Console.Clear();
            // Process a routine
            Console.WriteLine("Now we're going to process a routine. This is a chain of events that will run from start to finish syncronously. After which we'll out put the command stream again.");
            Console.WriteLine("Press any key to continue....");
            Console.ReadKey();

            var finalVersionOfUserId = example.Dispatch.Process<string>(RoutineNames.Example, "Changing the name using a routine.", new object[] { "johnsont" });
            Console.WriteLine("We chose to pass a user name through 3 different changes. One of them had an error and a failover method was executed.");
            Console.WriteLine("The final value returned is : {0} which should equal the current value in the Example class: '{1}'", finalVersionOfUserId, example.UserId);
            Console.WriteLine("Press any key to see the new command stream. NOTE: Audit carries the same information in a different composition. The difference is the command stream is always avaialble regardless of audit implementation.");
            Console.ReadKey();

            example.OutPutCommandStream();

            // Recommend using a 'Final command' to call at the end of each session as well as on exception.
            // This is so you can decide what to do with the backlog messages and audit trail you've collected.
            //example.Final();

            // The end result of this demo should be the following:
            // Wired and functional: Logging, Email, and IO
            // Dispatch: A functional pub/sub system for broadcasting events and data changes.
            // Audit: A subscriber to all dispatch events that logs them using your chosen strategy (XML to filesystem by default)
            // Notification: Another subscriber to all dispatch events. Provides a 'queue' of system events and their user friendly messages (dies with session). A list of 'subscribers' to events. A 'backlog' of messages which are the intersection of published events and subscribers to them. Persists to the filesystem by default. 
            Console.ReadLine();
        }


    }
}
