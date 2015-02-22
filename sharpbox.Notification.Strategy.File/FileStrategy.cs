using System;
using System.Collections.Generic;
using sharpbox.Dispatch.Model;
using sharpbox.Notification.Model;

namespace sharpbox.Notification.Strategy.File
{
    public class FileStrategy : IStrategy
    {
        private Io.Client _file;
        private Dictionary<string, object> _props;

        public FileStrategy(Dispatch.Client dispatcher, Io.Strategy.IStrategy persistenceStrategy, Dictionary<string, object> props)
        {
            _file = new Io.Client(persistenceStrategy);
            _props = props;

            LoadBacklog();
            LoadSubscribers();
        }
        
        private Dictionary<EventNames, List<string>> _subscribers;
        private List<BackLog> _queue;

        public Dictionary<EventNames, List<string>> Subscribers { get{ return _subscribers ?? (_subscribers = new Dictionary<EventNames, List<string>>());} set { _subscribers = value; } }

        public List<BackLog> Queue { get { return _queue ?? (_queue = new List<BackLog>()); } set { _queue = value;} }
 
        public void ProcessPackage(Package package)
        {
            // Add a queue entry so we know all the system events regardless of whether anyone is subscribed to them.
            var entry = new Entry
            {
                CreatedDate = DateTime.Now,
                PublisherName = package.EventName,
                EntryId = Guid.NewGuid(),
                UserFriendlyMessage = package.Message
            };


            if (!Subscribers.ContainsKey(package.EventName)) return; // Bail early if there are no subscribers.

            // Run through all of the subscribers for this publisher and generate a backlog item for them.
            foreach (var s in Subscribers[package.EventName])
            {
                // Add the backlog item
                AddBackLogItem(new BackLog
                {
                    AttempNumber = 0,
                    BackLogId = Guid.NewGuid(),
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

        public void LoadBacklog()
        {
            var filePath = _props["filePath"].ToString();
            if (!_file.Exists(filePath)) _file.Write(filePath, new List<BackLog>());
            Queue = _file.Read<List<BackLog>>(_props["filePath"].ToString());
        }

        public void SaveBackLog()
        {
            _file.Write(_props["filePath"].ToString(), Queue);
            LoadBacklog();
        }

        public void AddBackLogItem(BackLog backlog)
        {
            
            //Bail if there is already an entry for this user and this event.
            if (Queue.Exists(x => x.EntryId.Equals(backlog.EntryId) && x.UserId.Trim().ToLower().Equals(backlog.UserId.Trim().ToLower()))) return;

            Queue.Add(backlog);
            SaveBackLog();
        }


        public void LoadSubscribers()
        {
            _subscribers = new Dictionary<EventNames, List<string>>
            {
                {EventNames.OnLogException, new List<string>() {"ugwua"}}
            };
        }

        public void AddSubscriber(EventNames publisherName, string userId)
        {
            if (!Subscribers.ContainsKey(publisherName)) Subscribers.Add(publisherName, new List<string>());

            if (Subscribers[publisherName].Contains(userId.Trim().ToLower())) return;

            Subscribers[publisherName].Add(userId.Trim().ToLower());
        }
    }
}
