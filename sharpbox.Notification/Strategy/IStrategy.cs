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
        List<BackLogItem> BackLog { get; set; }

        /// <summary>
        /// The Notification Client is setup to fire this *every* time an event happens in the dispatcher. Once it creates a backLogItem it will pass it to this meathod for further processing.
        /// This allows for your strategy to whateve additional tweaks (i.e. - a more defined message, process it immediately, etc) before having it be added to the running queue.
        /// </summary>
        void ProcessBackLogItem(BackLogItem backLogItem);

        /// <summary>
        /// If something wishes to notify a person immediately you can use this to answer the request.
        /// </summary>
        /// <param name="request">It's assumed that the Entity within the request will be a backLogItem from the BackLog. On notify the method should update the item in the backlog (possibly save/update)</param>
        /// <returns></returns>
        Response Notify(Request request);

    }
}
