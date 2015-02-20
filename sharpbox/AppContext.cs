using System.Collections.Generic;
using System.Net.Mail;
using sharpbox.Dispatch;
using sharpbox.Dispatch.Model;

namespace sharpbox
{
    public abstract class AppContext
    {
        /// <summary>
        /// This constructor will do some wiring for you.
        /// </summary>
        /// <param name="userIdentity">Used to create a Dispatch instance which is then used to bootstrap Notification, Log, and Audit functionality.</param>
        /// <param name="extendedPublisherNames">Used to bootstrap Dispatch. this way things like Audit wire into whatever events are in a derived system as well as the default list. If not provided the default list will be used.</param>
        protected AppContext(string userIdentity, List<PublisherNames> extendedPublisherNames = null)
        {
            // Pub/Sub System(s)
            Dispatch = new Client(userIdentity, extendedPublisherNames ?? new List<PublisherNames>());
        }

        protected AppContext() { }

        public Dispatch.Client Dispatch { get; set; }

    }
}
