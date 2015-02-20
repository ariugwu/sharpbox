using System;
using System.Collections.Generic;
using sharpbox.Dispatch.Model;
using sharpbox.Notification.Model;

namespace sharpbox.Notification.Strategy.Xml
{
    public class XmlStrategy : IStrategy
    {
        private Io.Client _file;
        private Dictionary<string, object> _props;

        public XmlStrategy(Dispatch.Client dispatcher, Dictionary<string, object> props)
        {
            _file = new Io.Client(new Io.Strategy.Xml.XmlStrategy());
            _props = props;

            LoadBacklog(dispatcher);
            LoadSubscribers(dispatcher);
        }
        
        private Dictionary<EventNames, List<Entry>> _queue;
        private Dictionary<EventNames, List<string>> _subscribers;
        private List<BackLog> _backLog;

        public Dictionary<EventNames, List<Entry>> Queue { get { return _queue ?? (_queue = new Dictionary<EventNames, List<Entry>>());} set { _queue = value;} }
        public Dictionary<EventNames, List<string>> Subscribers { get{ return _subscribers ?? (_subscribers = new Dictionary<EventNames, List<string>>());} set { _subscribers = value; } }

        public List<BackLog> Backlog { get { return _backLog ?? (_backLog = new List<BackLog>()); } set { _backLog = value;} }
 
        public void ProcessPackage(Dispatch.Client dispatcher, Package package)
        {
            if(!Queue.ContainsKey(package.EventName)) Queue.Add(package.EventName, new List<Entry>());

            // Add a queue entry so we know all the system events regardless of whether anyone is subscribed to them.
            var entry = new Entry
            {
                CreatedDate = DateTime.Now,
                PublisherName = package.EventName,
                EntryId = Queue.Count + 1,
                SystemMessage = package.Message,
                UserFriendlyMessage = package.Message
            };

            // Add the Entry
            AddQueueEntry(entry);

            if (!Subscribers.ContainsKey(package.EventName)) return; // Bail early if there are no subscribers.

            // Run through all of the subscribers for this publisher and generate a backlog item for them.
            foreach (var s in Subscribers[package.EventName])
            {
                // Add the backlog item
                AddBackLogItem(dispatcher, new BackLog
                {
                    AttempNumber = 0,
                    BackLogId = Backlog.Count + 1,
                    NextAttempTime = null,
                    EntryId = entry.EntryId,
                    SentDate = null,
                    UserId = s,
                    WasSent = false,
                    Message = entry.UserFriendlyMessage
                });
            }

        }

        public void Notify(BackLog backLog)
        {
            throw new NotImplementedException();
        }

        public void LoadBacklog(Dispatch.Client dispatcher)
        {
            var filePath = _props["filePath"].ToString();
            if (!_file.Exists(filePath)) _file.Write(dispatcher, filePath, new List<BackLog>());
            Backlog = _file.Read<List<BackLog>>(dispatcher, _props["filePath"].ToString());
        }

        public void SaveBackLog(Dispatch.Client dispatcher)
        {
            _file.Write(dispatcher, _props["filePath"].ToString(), Backlog);
            LoadBacklog(dispatcher);
        }

        public void AddBackLogItem(Dispatch.Client dispatcher, BackLog backlog)
        {
            
            //Bail if there is already an entry for this user and this event.
            if (Backlog.Exists(x => x.EntryId.Equals(backlog.EntryId) && x.UserId.Trim().ToLower().Equals(backlog.UserId.Trim().ToLower()))) return;

            Backlog.Add(backlog);
            SaveBackLog(dispatcher);
        }

        public void AddQueueEntry(Entry entry)
        {
            if (!Queue.ContainsKey(entry.PublisherName)) Queue.Add(entry.PublisherName, new List<Entry>());
            Queue[entry.PublisherName].Add(entry);
        }

        public void LoadSubscribers(Dispatch.Client dispatcher)
        {
            _subscribers = new Dictionary<EventNames, List<string>>
            {
                {EventNames.OnLogException, new List<string>() {"ugwua"}}
            };

            dispatcher.Broadcast(new Package{ Entity = null, Message = "The base strategy for Notfication does not have a way to persist users", EventName = EventNames.OnNotificationAddQueueEntry, UserId = "system"});
        }

        public void AddSubscriber(EventNames publisherName, string userId)
        {
            if (!Subscribers.ContainsKey(publisherName)) Subscribers.Add(publisherName, new List<string>());

            if (Subscribers[publisherName].Contains(userId.Trim().ToLower())) return;

            Subscribers[publisherName].Add(userId.Trim().ToLower());
        }
    }
}
