using System;
using System.Collections.Generic;
using sharpbox.Dispatch.Model;
using sharpbox.Notification.Model;

namespace sharpbox.Notification
{
    [Serializable]
    public class Client
    {
        public Client(Email.Client emailClient)
        {
            _emailClient = emailClient;
        }

        public Client()
        {

        }

        private Email.Client _emailClient;
        private Dictionary<string, List<string>> _subscribers;
        private List<BackLogItem> _backLog;

        /// <summary>
        /// Use the string value of the EventName as the key. The object is to complicated to ensure comparing against values easy. Specifically code first. (e.g - If the Id is set in one and not the other there's no match)
        /// </summary>
        public Dictionary<string, List<string>> Subscribers { get { return _subscribers ?? (_subscribers = new Dictionary<string, List<string>>()); } set { _subscribers = value; } }

        public List<BackLogItem> BackLog { get { return _backLog ?? (_backLog = new List<BackLogItem>()); } set { _backLog = value; } }


        /// <summary>
        /// Whenever the dispatcher publishes an event we create a message for it and stick it on the queue. Then we see if anyone is requesting notification and we create a backlog entry for them to be processed at a later date. Likely by a scheduled task or explict request.
        /// </summary>
        /// <param name="response"></param>
        public void ProcessEvent(Response response)
        {
            if (!Subscribers.ContainsKey(response.EventName.Name)) return; // Bail early if there are no subscribers.

            // Run through all of the subscribers for this publisher and generate a backlog item for them.
            foreach (var s in Subscribers[response.EventName.Name])
            {
                // Add the backlog item
                var bli = new BackLogItem
                {
                    AttempNumber = 0,
                    BackLogItemUniqueId = Guid.NewGuid(),
                    NextAttempTime = null,
                    RequestId = response.RequestId,
                    RequestUniqueKey = response.RequestUniqueKey,
                    ResponseId = response.ResponseId,
                    ResponseUniqueKey = response.ResponseUniqueKey,
                    SentDate = null,
                    UserId = s,
                    WasSent = false,
                    Message = response.Message
                };

                BackLog.Add(bli);
            }

        }

        public BackLogItem Notify(BackLogItem bli)
        {
            _emailClient.Send(bli.To, bli.From, bli.Subject, bli.Message);

            bli.WasSent = true;
            bli.SentDate = DateTime.Now;

            // If this doesn't exist then add it. If it does then update it.
            if (!BackLog.Exists(x => x.BackLogItemUniqueId.Equals(bli.BackLogItemUniqueId)))
            {
                BackLog.Add(bli);
            }
            else
            {
                // update the item in our backlog
                for (var i = 0; i < BackLog.Count; i++)
                {
                    if (BackLog[i].BackLogItemUniqueId != bli.BackLogItemUniqueId) continue;

                    BackLog[i] = bli;
                    break;
                }
            }

            return bli;
        }


        public Subscriber AddSub(Subscriber subscriber)
        {
            if (!Subscribers.ContainsKey(subscriber.EventName.Name)) Subscribers.Add(subscriber.EventName.Name, new List<string>());
            Subscribers[subscriber.EventName.Name].Add(subscriber.UserId);

            return subscriber;
        }

    }
}
