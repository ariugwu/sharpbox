using System.Diagnostics;
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
            // This is centeralization is put to use with the Audit component which, when set to AuditLevel = All, will make a entry for *every* registered system event.
            // In this case we'll be using our extended list (defined in this project) and show how that can naturally hook into whatever events you want to register.
            var smtpClient = new SmtpClient("gmail.com", 25);
            var app = new ConsoleContext("ugwua", smtpClient, PublicationNamesExtension.ExtendedPubList);

            app.Dispatch.Subscribe(PublicationNamesExtension.ExampleExtendedPublisher, Booya);
            
            // Basic test of the dispatch. This says: TO anyone listen to 'OnLogException', here is a package.
            app.Dispatch.Publish(new Package() { Message = "Test of anyone listening to OnLogException.", PublisherName = PublicationNamesExtension.ExampleExtendedPublisher });

            // Next we're going to try the built in user change event.
            Debug.WriteLine(app.Dispatch.CurrentUserId);
            app.Dispatch.Publish(new Package{ PublisherName = PublicationNamesExtension.OnUserChange, Message = "Changing the userid to lyleb", UserId = "lyleb"});
            Debug.WriteLine(app.Dispatch.CurrentUserId);

            // Email: Test Email:
            //app.Email.Send();

            // Log: Test logging
            //app.Log.Exception();

            // Io: Test file operations
            // app.File

            // Audit: See the results in the audit trail
            var trail = app.Audit.Trail;
                Debug.WriteLine(trail.Count);

            // Membership:
        }

        public static void Booya(Dispatch.Client dispatcher, Package package)
        {
            Debug.WriteLine("Booya worked.");
        }
    }
}
