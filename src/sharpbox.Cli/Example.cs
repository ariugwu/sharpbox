using System;
using sharpbox.Dispatch.Model;
using sharpbox.Io.Model;

namespace sharpbox.Cli
{
    class Example
    {
        public static void Run()
        {
            // The benefit of the dispatcher is being able to see all subscribed events in one place at one time.
            // This centeralization is put to use with the Audit component which, when set to AuditLevel = All, will make a entry for *every* registered system event. We use basic since we're using xml and want to prevent event reflection. Audit saves file -> file generates audit message -> Audit saves file.
            // In this case we'll be using our extended list (defined in this project) and show how that can naturally hook into whatever events you want to register.
            var example = new Model.ExampleContext(LogOn: "ugwua");

            // Create a response object we can repopulate after each request.
            IResponse response = null;

            Console.WriteLine("Example 1: let's change the user id");
            Console.WriteLine(); Console.WriteLine(); Console.WriteLine();
            // Next we're going to try the user change command we registered in the mediator.
            Console.WriteLine("Current LogOn: " + example.LogOn);
            Console.WriteLine("Please input a new User Id: ");

            var newUserId = Console.ReadLine();
            Console.WriteLine();
            response = example.Dispatch.Process<String>(Model.ExampleContext.UserChange, new object[] { newUserId });

            Console.WriteLine("Current LogOn is now.: " + example.LogOn);
            Console.WriteLine(); Console.WriteLine(); Console.WriteLine();
            Console.WriteLine("Press any key to continue....");
            Console.ReadKey();
            Console.Clear();

            Console.WriteLine("Example 3: Next will test out writing some text to a file. ");
            Console.WriteLine("Please input a string to save to a file named 'foo.txt': ");

            var randomText = Console.ReadLine();
            Console.WriteLine(); Console.WriteLine();

            response = example.Dispatch.Process<FileDetail>(Model.ExampleContext.WriteARandomFile, new object[] { new FileDetail() { FilePath = "Random.txt", Data = System.Text.Encoding.UTF8.GetBytes(randomText) } });

            Console.WriteLine("The input was saved to a text file. We received a 'response' object back so we can get all the info needd to look up the action details in the logs.");
            Console.WriteLine("[Status:{0}] [Request Id: {1}] [ResponseId: {2}]", response.ResponseType, response.RequestId, response.ResponseId);
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

            // Process a routine
            Console.WriteLine("Example 7: Now we're going to process a routine. This is a chain of events that will run from start to finish syncronously. After which we'll out put the command stream again.");
            Console.WriteLine(); Console.WriteLine(); Console.WriteLine();
            Console.WriteLine("Press any key to continue....");
            Console.ReadKey();
            Console.Clear();

            Console.WriteLine("Note: We baked in an exception on purpose to test the ability register and automatically call failover methods. We're also firing on an event channel that has a notification subscriber so we can see how email templates work. The exception should display below:");
            Console.WriteLine(); Console.WriteLine();
            var finalVersionOfUserId = example.Dispatch.Run<string>(RoutineName.Example, App.AppWiring.DefaultAppWiring.GenericFeedback, new object[] { "johnsont" });
            Console.WriteLine("We chose to pass a user name through 3 different changes. One of them had an error and a failover method was executed.");
            Console.WriteLine("The final value returned is : \"{0}\" which should equal the current value in the Example class: \"{1}\"", finalVersionOfUserId, example.LogOn);
            Console.WriteLine(); Console.WriteLine(); Console.WriteLine();
            Console.WriteLine("Press any key to continue....");
            Console.ReadKey();
            Console.Clear();

            Console.WriteLine("Example 8: You also have the option to use an anonymous delegate. This allows you to pass values through the dispatcher solely to record them. Look at the registration in the context for more details. In this case we're just passing a string:");
            Console.WriteLine(); Console.WriteLine();
            response = example.Dispatch.Process<string>(Model.ExampleContext.DummyPassThroughCommand, new object[] { "Some text" });
            Console.WriteLine(); Console.WriteLine(); Console.WriteLine();
            Console.WriteLine("Press any key to continue....");
            Console.ReadKey();
            Console.Clear();

            Console.WriteLine("Example 9: You can register any method by passing in a func. However, The return type must always be specificed and can only be a single object.");
            Console.WriteLine(); Console.WriteLine();
            var commandname = new CommandName("SomethignInLine");
            example.Dispatch.Register<string>(commandname, new Func<int, string, string, string>(TestOfDelegateRegistration), new EventName("OnSomethingInLine"), App.AppWiring.DefaultAppWiring.GenericFeedback);
            response = example.Dispatch.Process<string>(commandname, new object[] { 1, "adfajd", "adfjsadl" });
            Console.WriteLine(); Console.WriteLine(); Console.WriteLine();
            Console.WriteLine("Press any key to continue....");
            Console.ReadKey();
            Console.Clear();

            Console.WriteLine("Press any key to see the new command stream. NOTE: Audit carries the same information in a different composition. The difference is the command stream is always avaialble regardless of audit implementation.");
            Console.ReadKey();
            Console.Clear();

            example.OutPutCommandStream();


            // Example 10: Notification: Fails and this is intentional as their isn't a proper email client, but shows us what happens when a command fails.
            // response = example.Dispatch.Process<List<BackLogItem>>(ExtendedCommandNames.SendNotification, "Sending out backlogitem", new object[] { example.Notification.BackLog.First() });


            // Recommend using a 'Final command' to call at the end of each session.
            // This is so you can decide what to do with the backlog messages and audit trail you've collected.
            example.Final();

            // The end result of this demo should be the following:
            // Wired and functional: Logging, Email, and IO
            // Dispatch: A functional pub/sub system for broadcasting events and data changes.
            // Audit: A subscriber to all dispatch events that logs them using your chosen strategy (XML to filesystem by default)
            // Notification: Another subscriber to all dispatch events. Provides a 'queue' of system events and their user friendly messages (dies with session). A list of 'subscribers' to events. A 'backlog' of messages which are the intersection of published events and subscribers to them. Persists to the filesystem by default. 
            Console.ReadLine();
        }

        public static string TestOfDelegateRegistration(int foo, string bar, string booya)
        {
            return $"{foo}-{bar}-{booya}";
        }
    }
}
