
using System.Diagnostics;
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

            app.Dispatch.Publish(new Package());

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
