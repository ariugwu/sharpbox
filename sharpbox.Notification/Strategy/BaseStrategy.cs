using System;
using System.Collections.Generic;
using sharpbox.Dispatch.Model;
using sharpbox.Notification.Model;

namespace sharpbox.Notification.Strategy
{
    public class BaseStrategy : IStrategy
    {
        #region Constructor(s)

        public BaseStrategy()
        {
            LoadQueue();
        }

        #endregion

        public Dictionary<PublisherNames, List<QueueEntry>> Queue { get; set; }

        public void ProcessQueue(Dispatch.Client dispatcher, Package package)
        {
            foreach (var q in Queue[package.PublisherName])
            {
                Notify(q);
            }
        }

        public void Notify(QueueEntry entry)
        {
            throw new NotImplementedException();
        }

        public void LoadQueue()
        {
            throw new NotImplementedException();
        }

        public void AddQueueEntry(QueueEntry entry)
        {
            throw new NotImplementedException();
        }
    }
}
