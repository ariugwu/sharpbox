using System.Collections.Generic;
using sharpbox.Dispatch.Model;
using sharpbox.Notification.Model;

namespace sharpbox.Notification.Strategy
{
    public interface IStrategy
    {
        /// <summary>
        /// As the system fires events the Notification strategy will create a list a list of entries for each publisher 
        /// </summary>
        Dictionary<EventNames, List<Entry>> Queue { get; set; }

        /// <summary>
        /// EventSubscribers are most likely loaded from an external system. These are the users who wish to have messages sent to them for certain events.
        /// </summary>
        Dictionary<EventNames, List<string>> Subscribers { get; set; }

        // The backlog is the list of messages that have been or need to be sent to subscribers. Usually persisted to an outside system.
        List<BackLog> PendingMessages { get; set; }

        void ProcessPackage(Dispatch.Client dispatcher, Package package);

        /// <summary>
        /// Use whatever the strategy decides is the notification for backlog items. Upload the backlog as needed after the attempt
        /// </summary>
        /// <param name="backLog">The message you would like to process off the backlog</param>
        void Notify(BackLog backLog);

        /// <summary>
        /// Load the backlog.
        /// </summary>
        void LoadBacklog(Dispatch.Client dispatcher);

        /// <summary>
        /// Persist the backlog.
        /// </summary>
        void SaveBackLog(Dispatch.Client dispatcher);

        /// <summary>
        /// Add a item to backlog
        /// </summary>
        /// <param name="dispatcher"></param>
        /// <param name="backlog">PendingMessages item represents a message which needs or has been sent to a use.</param>
        void AddBackLogItem(Dispatch.Client dispatcher, BackLog backlog);

        void AddQueueEntry(Entry entry);

        void LoadSubscribers(Dispatch.Client dispatcher);

        void AddSubscriber(EventNames publisherName, string userId);

    }
}
