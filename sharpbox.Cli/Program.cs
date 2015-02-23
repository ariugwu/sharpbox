using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mail;
using System.Reflection;
using sharpbox.Cli.Model.Domain.AppContext;
using sharpbox.Dispatch.Model;
using sharpbox.Cli.Model.Domain.Dispatch;

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
            var example = new ExampleContext("ugwua", EventNamesExtension.ExtendedPubList, CommandNames.DefaultActionList(), smtpClient);

            // Use this to 1-to-1 map an action to an event. Every action should have on primary "I'm done." broadcast with the updated data.
            example.Dispatch.CommandEventMap.Add(CommandNames.ChangeUser, EventNames.OnUserChange);
            example.Dispatch.CommandEventMap.Add(CommandNames.SetFeedback, EventNames.OnFeedbackSet);

            // Now we need to tell the various commands what to actually do. NOTE: We have the condition that a setfeedback action be the first thing you set!
            example.Dispatch.Register(CommandNames.SetFeedback, example.ExampleProcessFeedback);
            example.Dispatch.Register(CommandNames.ChangeUser, example.ChangeUser);

            // Piggy back additional listeners.
            example.Dispatch.Listen(EventNames.OnUserChange, ExampleListener);
            example.Dispatch.Listen(EventNames.OnFeedbackSet, ExampleListener);

            // Listen to an under the covers 'system' event
            example.Dispatch.Listen(EventNames.OnLogException, OnExceptionDumpEventStream);

            // Now we're set to actually use the application.
            var feedback = new Feedback { ActionName = CommandNames.ChangeUser, Message = "Meaningless message", Successful = true };

            try
            {
                example.Dispatch.Process(new Request
                {
                    CommandName = CommandNames.SetFeedback,
                    Message = "A test to set the feedback",
                    Entity = feedback,
                    RequestId = Guid.NewGuid(),
                    Type = typeof(Feedback),
                    UserId = example.Dispatch.CurrentUserId
                });
            }
            catch (TargetInvocationException tEx)
            {
                example.Log.Exception(example.Dispatch, tEx.Message);
                example.Dispatch.Broadcast(new Package { PackageId = Guid.NewGuid(), Message = tEx.Message, EventName = EventNames.OnLogException, Entity = tEx, Type = tEx.GetType(), UserId = example.Dispatch.CurrentUserId });
            }
            catch (Exception ex)
            {
                example.Log.Exception(example.Dispatch, ex.Message);
                // Basic test of the dispatch. This says: To anyone listen to 'OnLogException', here is a package.
                example.Dispatch.Broadcast(new Package { PackageId = Guid.NewGuid(), Message = ex.Message, EventName = EventNames.OnLogException, Entity = ex, Type = ex.GetType(), UserId = example.Dispatch.CurrentUserId });
            }

            // Another test from the subscription we set a few lines above.
            example.Dispatch.Broadcast(new Package { PackageId = Guid.NewGuid(), Message = "Test of anyone listening to Example Extended publisher.", EventName = EventNamesExtension.ExampleExtendedPublisher, UserId = example.Dispatch.CurrentUserId });

            // Next we're going to try the user change command we registered earlier.
            Debug.WriteLine("Current UserId: " + example.Dispatch.CurrentUserId);
            example.Dispatch.Process(new Request { RequestId = Guid.NewGuid(), CommandName = CommandNames.ChangeUser, Message = "Changing the userid to lyleb", Entity = "lyleb", Type = null, UserId = example.Dispatch.CurrentUserId });
            Debug.WriteLine("Current UserId: " + example.Dispatch.CurrentUserId);

            // Notification
            Debug.WriteLine("###Notification Info####");
            Debug.WriteLine("Total subscribers: " + example.Notification.Subscribers.Count);
            Debug.WriteLine("Total back log: " + example.Notification.Queue.Count);

            // Email: Test Email:
            try
            {
                // We know this will fail because the smtp client isn't fully configured and the emails are bad
                example.Email.Send(new List<string> { "test@testy.com" }, "foo.bar@gmail.com",
                    "This is a test email from my framework", "Testing is good for you.");
            }
            catch (Exception ex)
            {
                example.Log.Exception(example.Dispatch, ex.Message);
                // Basic test of the dispatch. This says: To anyone listen to 'OnLogException', here is a package.
                example.Dispatch.Broadcast(new Package { PackageId = Guid.NewGuid(), Message = "Test of anyone listening to OnLogException.", EventName = EventNames.OnLogException, UserId = example.Dispatch.CurrentUserId });
            }

            // Log: Test logging
            example.Log.Info(example.Dispatch, "Test of the info logging!");

            // Io: Test file operations. We pass in the dispatcher so everything threads back.
            example.File.Write("Test.xml", example.Notification.Queue);

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

        public static void ExampleListener(Package package)
        {
            Debug.WriteLine("{0} broadcasts: {1}", package.EventName, package.Message);
        }

        public static void OnExceptionDumpEventStream(Package package)
        {
            Debug.WriteLine("### Event Stream Dump ###");
            Debug.WriteLine("TODO: Would like to pass the EventStream on exception but the serializer for the xml audit isn't quite smart enough to write the full thing to file.");
            //foreach (var e in (List<Package>) package.Entity)
            //{
            //    Debug.WriteLine("{0}: {1} - {2}", e.EventName, e.Message, e.UserId);
            //}
        }

    }
}
