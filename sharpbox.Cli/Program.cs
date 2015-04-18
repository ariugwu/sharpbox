using System;
using System.Collections.Generic;
using System.Net.Mail;
using sharpbox.Dispatch.Model;
using sharpbox.Cli.Model.Domain.Sharpbox;
using sharpbox.EfCodeFirst.Audit;
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

            var example = new ExampleContext("ugwua", smtpClient);

            // Create a response object we can repopulate after each request.
            Response response = null;

            var commandname = new CommandNames("SomethignInLine");
            example.Dispatch.Register<string>(commandname, new Func<int, string, string, string>(TestOfDelegateRegistration), new EventNames("OnSomethingInLine"));
            response = example.Dispatch.Process<string>(commandname, "TEST", new object[] {1, "adfajd", "adfjsadl"});
            
            Console.WriteLine("Our first task will be to add a listener to the 'OnUserChange' event. This means that whenver the user is changed within the system our subscriber will get a message added to the backlog.");
            Console.WriteLine(); Console.WriteLine(); Console.WriteLine();
            Console.WriteLine("Press any key to continue....");
            Console.ReadKey();
            Console.Clear();

            Console.WriteLine("Example 1: let's change the user id");
            Console.WriteLine(); Console.WriteLine(); Console.WriteLine();
            // Next we're going to try the user change command we registered in the mediator.
            Console.WriteLine("Current UserId: " + example.UserId);
            Console.WriteLine("Please input a new User Id: ");

            var newUserId = Console.ReadLine();
            Console.WriteLine();
            response = example.Dispatch.Process<String>(ExampleContext.UserChange, "Changing the userid", new object[] { newUserId });

            Console.WriteLine("Current UserId is now.: " + example.UserId);
            Console.WriteLine(); Console.WriteLine(); Console.WriteLine();
            Console.WriteLine("Press any key to continue....");
            Console.ReadKey();
            Console.Clear();

            // Give the notification a subscriber. Now whenever this event is broadcast a backlog message will be created for me.
            Console.WriteLine("Example 2: In our configuration we added a listener to the 'AddNotificationSubscriber' event. So we're able to have Notification listen to all events, but also add other listers to specific Notification events:");
            Console.WriteLine(); Console.WriteLine(); Console.WriteLine();
            response = example.Dispatch.Process<Subscriber>(Notification.Domain.Dispatch.NotificationCommands.AddNotificationSubscriber, "Adding a subcriber to OnUserChange.", new object[] { new Subscriber(ExampleContext.OnUserChange, "ugwua") });
            Console.WriteLine(); Console.WriteLine(); Console.WriteLine();
            Console.WriteLine("Press any key to continue....");
            Console.ReadKey();
            Console.Clear();

            Console.WriteLine("Example 3: Next will test out writing some text to a file. ");
            Console.WriteLine("Please input a string to save to a file named 'foo.txt': ");

            var randomText = Console.ReadLine();
            Console.WriteLine(); Console.WriteLine();

            response = example.Dispatch.Process<FileDetail>(ExampleContext.WriteARandomFile, "Example from the CLI project of writing a file.", new object[] { new FileDetail() { FilePath = "Random.txt", Data = System.Text.Encoding.UTF8.GetBytes(randomText) } });

            // Notification: Fails and this is intentional as their isn't a proper email client, but shows us what happens when a command fails.
            // response = example.Dispatch.Process<List<BackLogItem>>(ExtendedCommandNames.SendNotification, "Sending out backlogitem", new object[] { example.Notification.BackLog.First() });
            Console.WriteLine("The input was saved to a text file. We received a 'response' object back so we can get all the info needd to look up the action details in the audit log.");
            Console.WriteLine("[Status:{0}] [Request Id: {1}] [ResponseId: {2}]", response.ResponseType, response.RequestUniqueKey, response.ResponseUniqueKey);
            Console.WriteLine(); Console.WriteLine(); Console.WriteLine();
            Console.WriteLine("Press any key to continue....");
            Console.ReadKey();
            Console.Clear();

            Console.WriteLine("Example 4: Example of writing an object to a file. In this case we've decided to persist our current audit trail. We should see a broad cast anouncement below:");
            Console.WriteLine(); Console.WriteLine();

            var trail = new List<Response>();
            trail.AddRange(example.Audit.Trail);
            example.Dispatch.Process<List<Response>>(ExampleContext.WriteAuditTrailToDisk, "Writing the current audit trail to a binary file", new object[] { trail });
            Console.WriteLine(); Console.WriteLine(); Console.WriteLine();
            Console.WriteLine("Press any key to continue....");
            Console.ReadKey();
            Console.Clear();

            Console.WriteLine("Example 5: Okay. Now let's output the command stream and see how it what it looks like.");
            Console.WriteLine(); Console.WriteLine(); Console.WriteLine();
            Console.WriteLine("Press any key to continue....");
            Console.ReadKey();
            Console.Clear();
            example.OutPutCommandStream();

            Console.WriteLine("Press any key to continue....");
            Console.ReadKey();
            Console.Clear();

            Console.WriteLine("Example 6: Next we'll take a look at what notification/audit information we've collected. Remember that subscriber we added earlier to OnUserChange?");
            Console.WriteLine(); Console.WriteLine(); Console.WriteLine();
            Console.WriteLine("Press any key to continue....");
            Console.ReadKey();
            Console.Clear();

            // Notification
            Console.WriteLine("###Notification Info####");
            Console.WriteLine("Total subscribers: " + example.Notification.Subscribers.Count);
            Console.WriteLine("Total back log: " + example.Notification.BackLog.Count);
            // Audit: See the results in the audit trail
            Console.WriteLine("###Audit Info####");
            Console.WriteLine("Audit Trail Count: " + example.Audit.Trail.Count);
            Console.WriteLine(); Console.WriteLine(); Console.WriteLine();
            Console.WriteLine("Press any key to continue....");
            Console.ReadKey();
            Console.Clear();

            // Process a routine
            Console.WriteLine("Example 7: Now we're going to process a routine. This is a chain of events that will run from start to finish syncronously. After which we'll out put the command stream again.");
            Console.WriteLine(); Console.WriteLine(); Console.WriteLine();
            Console.WriteLine("Press any key to continue....");
            Console.ReadKey();
            Console.Clear();

            Console.WriteLine("Note: We baked in an exception on purpose to test the ability register and automatically call failover methods. The exceptio should display below:");
            Console.WriteLine(); Console.WriteLine();
            var finalVersionOfUserId = example.Dispatch.Process<string>(RoutineNames.Example, "Changing the name using a routine.", new object[] { "johnsont" });
            Console.WriteLine(); Console.WriteLine();

            Console.WriteLine(); Console.WriteLine();
            Console.WriteLine("We chose to pass a user name through 3 different changes. One of them had an error and a failover method was executed.");
            Console.WriteLine(); Console.WriteLine();
            Console.WriteLine("The final value returned is : {0} which should equal the current value in the Example class: '{1}'", finalVersionOfUserId, example.UserId);
            Console.WriteLine(); Console.WriteLine(); Console.WriteLine();

            Console.WriteLine("Example 8: You also have the option to use a built in stub method for legacy code. This allows you to pass values through the dispatcher solely to record them. In this case we're just passing a string:");
            Console.WriteLine(); Console.WriteLine();
            response = example.Dispatch.Process<string>(ExampleContext.DummyPassThroughCommand, "Just passing a value I want recorded in context of the command/event stream", new object[] { "Some text" });
            Console.WriteLine(); Console.WriteLine();

            Console.WriteLine("Press any key to see the new command stream. NOTE: Audit carries the same information in a different composition. The difference is the command stream is always avaialble regardless of audit implementation.");
            Console.ReadKey();
            Console.Clear();

            example.OutPutCommandStream();

            // Recommend using a 'Final command' to call at the end of each session.
            // This is so you can decide what to do with the backlog messages and audit trail you've collected.
           //example.Final();

            // The end result of this demo should be the following:
            // Wired and functional: Logging, Email, and IO
            // Dispatch: A functional pub/sub system for broadcasting events and data changes.
            // Audit: A subscriber to all dispatch events that logs them using your chosen strategy (XML to filesystem by default)
            // Notification: Another subscriber to all dispatch events. Provides a 'queue' of system events and their user friendly messages (dies with session). A list of 'subscribers' to events. A 'backlog' of messages which are the intersection of published events and subscribers to them. Persists to the filesystem by default. 
            Console.ReadLine();
        }

        public static string TestOfDelegateRegistration(int foo, string bar, string booya)
        {
            return String.Format("{0}-{1}-{2}", foo, bar, booya);
        }

    }
}
