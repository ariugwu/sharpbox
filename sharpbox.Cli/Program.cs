using System.Diagnostics;
using sharpbox.Dispatch.Model;
using sharpbox.Model.Domain.Dispatch;

namespace sharpbox.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            // The benefit of the dispatcher is being able to see all subscribed events in one place at one time.
            // This is centeralization is put to use with the Audit component which, when set to AuditLevel = All, will make a entry for *every* registered system event.
            var app = new AppContext();

            app.Dispatch.Subscribe(PublisherNames.OnLogException, Booya);
            
            // Basic test of the dispatch. This says: TO anyone listen to 'OnLogException', here is a package.
            app.Dispatch.Publish(new Package(){ Message = "Test of anyone listening to OnLogException.", PublisherName = PublisherNames.OnLogException});

            // Test Email:
            //app.Email.Send();

            // Test logging
            //app.Log.Exception();

            // Test file operations
            // app.File
        }

        public static void Booya(Package package)
        {
            Debug.WriteLine("Booya worked.");
        }
    }
}
