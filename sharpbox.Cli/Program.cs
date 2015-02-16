using System;
using System.Diagnostics;
using System.Threading;
using sharpbox.Dispatch.Model;

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
