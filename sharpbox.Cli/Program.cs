using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using sharpbox.Cli.Model.Domain.AppContext;
using sharpbox.Dispatch.Model;
using sharpbox.Cli.Model.Domain.Dispatch;
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
            var example = new ExampleMediator("ugwua", EventNamesExtension.ExtendedPubList, smtpClient);
            
            example = WireUpEvents(example); // Wire the commands, and listeners.

            // Now we're set to actually use the application.
            var feedback = new Feedback { ActionName = CommandNames.ChangeUser, Message = "Meaningless message", Successful = true };

            try {
                    example.Dispatch.Process(new Request
                    {
                        CommandName = CommandNames.SetFeedback,
                        Message = "Testing the feedback system.",
                        Entity = feedback,
                        RequestId = Guid.NewGuid(),
                        Type = typeof(Feedback)
                    });  
            }
            catch (TargetInvocationException tEx)
            {
                example.Log.Exception(example.Dispatch, tEx.Message);
                example.Dispatch.Broadcast(new Response { ResponseId = Guid.NewGuid(), Message = tEx.Message, EventName = EventNames.OnLogException, Entity = tEx, Type = tEx.GetType()});
            }
            catch (Exception ex)
            {
                example.Log.Exception(example.Dispatch, ex.Message);
                // Basic test of the dispatch. This says: To anyone listen to 'OnLogException', here is a response.
                example.Dispatch.Broadcast(new Response { ResponseId = Guid.NewGuid(), Message = ex.Message, EventName = EventNames.OnLogException, Entity = ex, Type = ex.GetType()});
            }

            // Another test from the subscription we set a few lines above.
            example.Dispatch.Broadcast(new Response { ResponseId = Guid.NewGuid(), Message = "Test of anyone listening to Example Extended publisher.", EventName = EventNamesExtension.ExampleExtendedPublisher});

            // Next we're going to try the user change command we registered earlier.
            Debug.WriteLine("Current UserId: " + example.UserId);
            example.Dispatch.Process(new Request { RequestId = Guid.NewGuid(), CommandName = CommandNames.ChangeUser, Message = "Changing the userid to lyleb", Entity = "lyleb", Type = null});
            Debug.WriteLine("Current UserId: " + example.UserId);

            // Notification
            Debug.WriteLine("###Notification Info####");
            Debug.WriteLine("Total subscribers: " + example.Notification.Subscribers.Count);
            Debug.WriteLine("Total back log: " + example.Notification.BackLog.Count);

            // Email: Test Email:
            try
            {
                // We know this will fail because the smtp client isn't fully configured and the emails are bad
                example.Email.Send(new List<string> { "test@testy.com" }, "foo.bar@gmail.com","This is a test email from my framework", "Testing is good for you.");
            }
            catch (Exception ex)
            {
                example.Log.Exception(ex, ex.Message);
                // Basic test of the dispatch. This says: To anyone listen to 'OnLogException', here is a response.
                example.Dispatch.Broadcast(new Response { ResponseId = Guid.NewGuid(), Entity = ex, Message = ex.Message, EventName = EventNames.OnLogException,});
            }

            // Log: Test logging
            example.Log.Info(example.Dispatch, "Test of the info logging!");

            // Io: Test file operations. We pass in the dispatcher so everything threads back.
            example.File.Write("Test.xml", example.Notification.BackLog);

            // Broad Cast the command stream to test usefulness.
            try
            {
                example.Dispatch.Process(new Request
                {
                    RequestId = Guid.NewGuid(),
                    CommandName = CommandNames.BroadcastCommandStream,
                    Message = "Request to broadcast command stream",
                    Entity = null,
                    Type = null
                });
            }
            catch (Exception ex)
            {
                example.Dispatch.Broadcast(new Response { Entity = ex, Type = ex.GetType(), EventName = EventNames.OnException, Message = "Failed our example broadcast in Progam.cs", ResponseId = Guid.NewGuid() });
            }

            // Notification
            example.Dispatch.Process(new Request { Entity = example.Notification.BackLog.First(), Type = typeof(BackLogItem), CommandName = CommandNames.SendNotification, Message= "Sending out backlogitem", RequestId = Guid.NewGuid()});

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
            example.Dispatch.Register(CommandNames.SetFeedback, example.ExampleProcessFeedback, EventNames.OnFeedbackSet);
            example.Dispatch.Register(CommandNames.ChangeUser, example.ChangeUser, EventNames.OnUserChange);
            example.Dispatch.Register(CommandNames.BroadcastCommandStream, example.BroadCastEventStream, EventNames.OnBroadcastCommandStream);
            example.Dispatch.Register(CommandNames.SendNotification, example.Notification.Notify, EventNames.OnNotificationNotify);

            // Add some listeners to those broadcasts. NOTE: This is a queue so things will be fired in FIFO order.
            example.Dispatch.Listen(EventNames.OnUserChange, ExampleListener);
            example.Dispatch.Listen(EventNames.OnFeedbackSet, ExampleListener);
            example.Dispatch.Listen(EventNames.OnBroadcastCommandStream, OutPutCommandStream);
            // Listen to an 'under the covers' system event
            example.Dispatch.Listen(EventNames.OnLogException, ExampleListener);

            // Give the notification a subscriber.
            example.Notification.AddSub(EventNames.OnUserChange, "ugwua");

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
            if (exception != null) throw exception;
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
