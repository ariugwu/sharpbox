using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Mail;
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
            var app = new ConsoleContext("ugwua", PublicationNamesExtension.ExtendedPubList, ActionNames.DefaultActionList(), smtpClient);

            app.Dispatch.Register(ActionNames.SetFeedback, app.ExampleProcessFeedback);

            var feedback = new Feedback{ ActionName = ActionNames.ChangeUser, Message = "Meaningless message", Successful = true};
            app.Dispatch.Process(new Request{ ActionName = ActionNames.SetFeedback, Message = "A test to set the feedback", Entity = feedback, RequestId = 0, Type = typeof(Feedback), UserId = app.Dispatch.CurrentUserId});

            app.Dispatch.Listen(PublicationNamesExtension.ExampleExtendedPublisher, ExampleListener);
            
            // Basic test of the dispatch. This says: To anyone listen to 'OnLogException', here is a package.
            app.Dispatch.Broadcast(new Package() { Message = "Test of anyone listening to OnLogException.", EventName = EventNames.OnLogException, UserId = app.Dispatch.CurrentUserId});

            // Another test from the subscription we set a few lines above.
            app.Dispatch.Broadcast(new Package() { Message = "Test of anyone listening to Example Extended publisher.", EventName = PublicationNamesExtension.ExampleExtendedPublisher, UserId = app.Dispatch.CurrentUserId });

            app.Dispatch.Register(ActionNames.ChangeUser, app.ChangeUser);

            // Next we're going to try the built in user change event.
            Debug.WriteLine("Current UserId: " + app.Dispatch.CurrentUserId);
            app.Dispatch.Process(new Request{ ActionName = ActionNames.ChangeUser, Message = "Changing the userid to lyleb", Entity = "lyleb", Type = null,  UserId = app.Dispatch.CurrentUserId});
            Debug.WriteLine("Current UserId: " + app.Dispatch.CurrentUserId);


            // Notification
            Debug.WriteLine("###Notification Info####");
            Debug.WriteLine("Total subscribers: " + app.Notification.Subscribers.Count);
            Debug.WriteLine("Total queue: " + app.Notification.Queue.Count);

            // Email: Test Email:
            try
            {
                // We know this will fail because the smtp client isn't fully configured and the emails are bad
                app.Email.Send(app.Dispatch, new List<string> {"test@testy.com"}, "foo.bar@gmail.com",
                    "This is a test email from my framework", "Testing is good for you.");
            }
            catch (Exception ex)
            {
                app.Log.Exception(app.Dispatch, ex.Message);
            }

            // Log: Test logging
            app.Log.Info(app.Dispatch, "Test of the info logging!");

            // Io: Test file operations. We pass in the dispatcher so everything threads back.
            app.File.Write<List<Notification.Model.BackLog>>(app.Dispatch, "Test.xml", app.Notification.Backlog);

            // Audit: See the results in the audit trail
            var trail = app.Audit.Trail;
                Debug.WriteLine(trail.Count);

            // The end result of this demo should be the following:
            // Wired and functional: Logging, Email, and IO
            // Dispatch: A functional pub/sub system for broadcasting events and data changes.
            // Audit: A subscriber to all dispatch events that logs them using your chosen strategy (XML to filesystem by default)
            // Notification: Another subscriber to all dispatch events. Provides a 'queue' of system events and their user friendly messages (dies with session). A list of 'subscribers' to events. A 'backlog' of messages which are the intersection of published events and subscribers to them. Persists to the filesystem by default. 
            Console.ReadLine();
        }

        public static void ExampleListener(Dispatch.Client dispatcher, Package package)
        {
            Debug.WriteLine("{0} broadcasts: {1}", package.EventName, package.Message);
        }

    }
}
