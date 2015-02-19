using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mail;
using System.Text;
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
            // This centeralization is put to use with the Audit component which, when set to AuditLevel = All, will make a entry for *every* registered system event.
            // In this case we'll be using our extended list (defined in this project) and show how that can naturally hook into whatever events you want to register.
            var smtpClient = new SmtpClient("smtp.google.com", 587);
            var app = new ConsoleContext("ugwua", PublicationNamesExtension.ExtendedPubList,smtpClient);

            app.Dispatch.Subscribe(PublicationNamesExtension.ExampleExtendedPublisher, Booya);
            
            // Basic test of the dispatch. This says: To anyone listen to 'OnLogException', here is a package.
            app.Dispatch.Publish(new Package() { Message = "Test of anyone listening to OnLogException.", PublisherName = PublisherNames.OnLogException, UserId = app.Dispatch.CurrentUserId});

            // Another test from the subscription we set a few lines above.
            app.Dispatch.Publish(new Package() { Message = "Test of anyone listening to Example Extended publisher.", PublisherName = PublicationNamesExtension.ExampleExtendedPublisher, UserId = app.Dispatch.CurrentUserId });

            // Next we're going to try the built in user change event.
            Debug.WriteLine("Current UserId: " + app.Dispatch.CurrentUserId);
            app.Dispatch.Publish(new Package{ PublisherName = PublisherNames.OnUserChange, Message = "Changing the userid to lyleb", Entity = "lyleb", Type = null,  UserId = app.Dispatch.CurrentUserId});
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
            Io.Client.Save(app.Dispatch, "text.txt", Encoding.ASCII.GetBytes(String.Format("This is a test string for fun. : {0}", DateTime.Now.ToLongDateString())));

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

        public static void Booya(Dispatch.Client dispatcher, Package package)
        {
            Debug.WriteLine(string.Format("{0} broadcasts: {1}", package.PublisherName, package.Message));
        }
    }
}
