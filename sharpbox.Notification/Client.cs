using System;
using System.Collections.Generic;
using System.Linq;
using sharpbox.Dispatch.Model;
using sharpbox.Notification.Model;
using sharpbox.Notification.Strategy;

namespace sharpbox.Notification
{
    public class Client
    {
        #region Constructor(s)

        public Client(Dispatch.Client dispatcher,List<EventNames> availableEvents, IStrategy strategy)
        {
            _strategy = strategy;
            ConfigureNotification(dispatcher, availableEvents);
        }

        public Client()
        {

        }
        #endregion

        #region Field(s)

        private IStrategy _strategy;

        #endregion

        #region Properties

        public Dictionary<EventNames, List<string>> Subscribers { get { return _strategy.Subscribers;  } set{ _strategy.Subscribers = value;} }
        public Queue<BackLogItem> BackLog { get { return _strategy.BackLog;  } }

        #endregion

        #region Client Method(s)

        public void ConfigureNotification(Dispatch.Client dispatcher, List<EventNames> availableEvents)
        {
            foreach (var p in availableEvents.Where(x => !x.ToString().ToLower().Contains("onnotification"))) // subscribe to everything but our own events
            {
                dispatcher.Listen(p, ProcessEvent);
            }
        }

        #endregion

        #region Strategy Method(s)

        /// <summary>
        /// Whenever the dispatcher publishes an event we create a message for it and stick it on the queue. Then we see if anyone is requesting notification and we create a backlog entry for them to be processed at a later date. Likely by a scheduled task or explict request.
        /// </summary>
        /// <param name="response"></param>
        public void ProcessEvent(Response response)
        {
            // Add a queue entry so we know all the system events regardless of whether anyone is subscribed to them.
            var entry = new Entry
            {
                CreatedDate = DateTime.Now,
                PublisherName = response.EventName,
                EntryId = Guid.NewGuid(),
                UserFriendlyMessage = response.Message
            };


            if (!Subscribers.ContainsKey(response.EventName)) return; // Bail early if there are no subscribers.

            // Run through all of the subscribers for this publisher and generate a backlog item for them.
            foreach (var s in Subscribers[response.EventName])
            {
                // Add the backlog item
                var bli = new BackLogItem
                {
                    AttempNumber = 0,
                    BackLogId = Guid.NewGuid(),
                    NextAttempTime = null,
                    EntryId = entry.EntryId,
                    SentDate = null,
                    UserId = s,
                    WasSent = false,
                    Message = entry.UserFriendlyMessage
                };

                bli = _strategy.ProcessBackLogItem(bli);

                BackLog.Enqueue(bli);
            }
            
        }

        public Response Notify(Request request)
        {
            return _strategy.Notify(request);
        }

        #endregion
    }
}
