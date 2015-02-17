using System.Collections.Generic;
using sharpbox.Dispatch.Model;
using sharpbox.Notification.Model;
using sharpbox.Notification.Strategy;

namespace sharpbox.Notification
{
    public class Client
    {
        #region Constructor(s)

        public Client(Dispatch.Client dispatcher)
        {
            ConfigureNotification(dispatcher);
        }

        public Client()
        {

        }
        #endregion

        #region Field(s)

        private IStrategy _strategy;

        #endregion

        #region Properties

        public Dictionary<PublisherNames, List<QueueEntry>> Queue { get { return _strategy.Queue; } }
        
        #endregion

        #region Client Method(s)

        public void ConfigureNotification(Dispatch.Client dispatcher)
        {

            foreach (var p in dispatcher.AvailablePublications)
            {
                dispatcher.Subscribe(p, ProcessQueue);
            }

        }

        #endregion

        #region Strategy Method(s)

        public void ProcessQueue(Dispatch.Client dispatcher, Package package)
        {
            _strategy.ProcessQueue(dispatcher, package);
        }

        public void LoadQueue()
        {
            _strategy.LoadQueue();
        }

        public void AddQueueEntry(QueueEntry entry)
        {
            _strategy.AddQueueEntry(entry);
        }

        #endregion
    }
}
