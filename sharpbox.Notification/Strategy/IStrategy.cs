using System.Collections.Generic;
using sharpbox.Dispatch.Model;
using sharpbox.Notification.Model;

namespace sharpbox.Notification.Strategy
{
    public interface IStrategy
    {

        Dictionary<PublisherNames, List<QueueEntry>> Queue { get; set; }

        void ProcessQueue(Dispatch.Client dispatcher, Package package);

        void Notify(QueueEntry entry);

        void LoadQueue();

        void AddQueueEntry(QueueEntry entry);

    }
}
