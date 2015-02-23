using System.Collections.Generic;
using sharpbox.Dispatch.Model;
using sharpbox.Notification.Model;

namespace sharpbox.Notification.Strategy
{
    public interface IStrategy
    {
        /// <summary>
        /// EventSubscribers are most likely loaded from an external system. These are the users who wish to have messages sent to them for certain events.
        /// </summary>
        Dictionary<EventNames, List<string>> Subscribers { get; set; }

        // The backlog is the list of messages that have been or need to be sent to subscribers. Usually persisted to an outside system.
        List<BackLog> Queue { get; set; }

        void ProcessPackage(Response response);

        /// <summary>
        /// Use whatever the strategy decides is the notification for backlog items. Upload the backlog as needed after the attempt
        /// </summary>
        /// <param name="backLog">The message you would like to process off the backlog</param>
        void Notify(BackLog backLog);

        /// <summary>
        /// Load the backlog.
        /// </summary>
        void LoadBacklog();

        /// <summary>
        /// Persist the backlog.
        /// </summary>
        void SaveBackLog();

        /// <summary>
        /// Add a item to backlog
        /// </summary>
        /// <param name="backlog">Queue item represents a message which needs or has been sent to a use.</param>
        void AddBackLogItem(BackLog backlog);

        void LoadSubscribers();

        void AddSubscriber(EventNames publisherName, string userId);

    }
}
