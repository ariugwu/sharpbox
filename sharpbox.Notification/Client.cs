using System;
using System.Collections.Generic;
using System.Linq;
using sharpbox.Dispatch.Model;
using sharpbox.Notification.Model;

namespace sharpbox.Notification
{
    public class Client
    {
        #region Constructor(s)

        public Client(ref Dispatch.Client dispatcher, Email.Client emailClient, List<EventNames> availableEvents)
        {
            _emailClient = emailClient;
            ConfigureNotification(dispatcher, availableEvents);
        }

        public Client()
        {

        }
        #endregion

        #region Field(s)

        private Email.Client _emailClient;
        private Dictionary<EventNames, List<string>> _subscribers;
        private List<BackLogItem> _backLog;

        #endregion

        #region Properties

        public Dictionary<EventNames, List<string>> Subscribers { get { return _subscribers ?? (_subscribers = new Dictionary<EventNames, List<string>>()); } set { _subscribers = value; } }

        public List<BackLogItem> BackLog { get { return _backLog ?? (_backLog = new List<BackLogItem>()); } set { _backLog = value; } }

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
            if (!Subscribers.ContainsKey(response.EventName)) return; // Bail early if there are no subscribers.

            // Run through all of the subscribers for this publisher and generate a backlog item for them.
            foreach (var s in Subscribers[response.EventName])
            {
                // Add the backlog item
                var bli = new BackLogItem
                {
                    AttempNumber = 0,
                    BackLogItemId = Guid.NewGuid(),
                    NextAttempTime = null,
                    RequestId = response.RequestId,
                    ResponseId = response.ResponseId,
                    SentDate = null,
                    UserId = s,
                    WasSent = false,
                    Message = response.Message
                };

                _backLog.Add(bli);
            }
            
        }

        public Response Notify(Request request)
        {
            var bli = (BackLogItem)request.Entity;

            _emailClient.Send(bli.To, bli.From, bli.Subject, bli.Message);

            bli.WasSent = true;
            bli.SentDate = DateTime.Now;

            // If this doesn't exist then add it. If it does then update it.
            if (!_backLog.Exists(x => x.BackLogItemId.Equals(bli.BackLogItemId)))
            {
                _backLog.Add(bli);
            }
            else
            {
                // update the item in our backlog
                for (var i = 0; i < _backLog.Count; i++)
                {
                    if (_backLog[i].BackLogItemId != bli.BackLogItemId) continue;

                    _backLog[i] = bli;
                    break;
                }
            }

            return new Response(request, String.Format("Email sent to {0}. With Subject: {1}", String.Join(";", bli.To, bli.Subject)), ResponseTypes.Success);
        }


        public Response AddSub(Request request)
        {
            var sub = (Subscriber) request.Entity;

            if (!Subscribers.ContainsKey(sub.EventName)) Subscribers.Add(sub.EventName, new List<string>());
            Subscribers[sub.EventName].Add(sub.UserId);

            return new Response(request, String.Format("Added the following user '{0}' to the subscription for: {1}", sub.UserId, sub.EventName), ResponseTypes.Success);
        }

        #endregion
    }
}
