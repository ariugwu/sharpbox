using System.Collections.Generic;
using sharpbox.Dispatch.Model;
using sharpbox.Notification.Model;

namespace sharpbox.Notification
{
    public class Client
    {
        #region Constructor(s)

        public Client(Dispatch.Client dispatcher)
        {
            ConfigureNotification(dispatcher);
        }

        #endregion

        #region Properties

        public Dictionary<PublisherNames, List<QueueEntry>> Queue { get; set; }
        
        #endregion

        #region Client Method(s)

        private void ConfigureNotification(Dispatch.Client dispatcher)
        {

            foreach (var p in dispatcher.AvailablePublications)
            {
                dispatcher.Subscribe(p, ProcessQueue);
            }

        }

        #endregion

        #region Strategy Method(s)

        private void ProcessQueue(Dispatch.Client dispatcher, Package package)
        {
            foreach (var q in Queue[package.PublisherName])
            {

            }
        }

        private void LoadQueue()
        {
            
        }

        private void AddQueueEntry(QueueEntry entry)
        {
            
        }
        #endregion
    }
}
