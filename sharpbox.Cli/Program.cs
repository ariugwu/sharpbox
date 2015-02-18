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
            // This is centeralization is put to use with the Audit component which, when set to AuditLevel = All, will make a entry for *every* registered system event.
            // In this case we'll be using our extended list (defined in this project) and show how that can naturally hook into whatever events you want to register.
            var smtpClient = new SmtpClient("smtp.google.com", 587);
            var app = new ConsoleContext("ugwua", smtpClient, PublicationNamesExtension.ExtendedPubList);

            app.Dispatch.Subscribe(PublicationNamesExtension.ExampleExtendedPublisher, Booya);
            
            // Basic test of the dispatch. This says: TO anyone listen to 'OnLogException', here is a package.
            app.Dispatch.Publish(new Package() { Message = "Test of anyone listening to OnLogException.", PublisherName = PublisherNames.OnLogException });

            // Another test from the subscription we set a few lines above.
            app.Dispatch.Publish(new Package() { Message = "Test of anyone listening to Example Extended publisher.", PublisherName = PublicationNamesExtension.ExampleExtendedPublisher });

            // Next we're going to try the built in user change event.
            Debug.WriteLine(app.Dispatch.CurrentUserId);
            app.Dispatch.Publish(new Package{ PublisherName = PublisherNames.OnUserChange, Message = "Changing the userid to lyleb", UserId = "lyleb"});
            Debug.WriteLine(app.Dispatch.CurrentUserId);


            // Notification
            Debug.WriteLine("###Notification Info####");
            Debug.WriteLine("Total subscribers: " + app.Notification.Subscribers.Count);
            Debug.WriteLine("Total queue: " + app.Notification.Subscribers.Count);

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
        }

        public static void Booya(Dispatch.Client dispatcher, Package package)
        {
            Debug.WriteLine(string.Format("{0} broadcasts: {1}", package.PublisherName, package.Message));
        }
    }
}
