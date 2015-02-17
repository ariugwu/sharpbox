using System;
using System.Collections.Generic;
using System.Linq;
using sharpbox.Data;
using sharpbox.Dispatch.Model;
using sharpbox.Notification.Model;

namespace sharpbox.Notification.Strategy
{
    public class BaseStrategy : IStrategy
    {
        public BaseStrategy(Dispatch.Client dispatcher, Dictionary<string, object> auxInfo)
        {
            AuxInfo = auxInfo;
            _xmlPath = (string)AuxInfo["xmlPath"];
            _repository = new Repository<BackLog>(dispatcher, auxInfo: auxInfo);

            LoadBacklog(dispatcher);
            LoadSubscribers(dispatcher);
        }
        
        private string _xmlPath;
        private Repository<BackLog> _repository;

        private Dictionary<PublisherNames, List<Entry>> _queue;
        private Dictionary<PublisherNames, List<string>> _subscribers;
        private List<BackLog> _backLog;
 
        public Repository<BackLog> Repository
        {
            get { return _repository; }
            set
            {
                _repository = value;
            }
        }

        public Dictionary<string, object> AuxInfo { get; set; }

        public Dictionary<PublisherNames, List<Entry>> Queue { get { return _queue ?? (_queue = new Dictionary<PublisherNames, List<Entry>>());} set { _queue = value;} }
        public Dictionary<PublisherNames, List<string>> Subscribers { get{ return _subscribers ?? (_subscribers = new Dictionary<PublisherNames, List<string>>());} set { _subscribers = value; } }

        public List<BackLog> Backlog { get { return _backLog ?? (_backLog = new List<BackLog>()); } set { _backLog = value;} }
 
        public void ProcessPackage(Dispatch.Client dispatcher, Package package)
        {
            if(!Queue.ContainsKey(package.PublisherName)) Queue.Add(package.PublisherName, new List<Entry>());

            // Add a queue entry so we know all the system events regardless of whether anyone is subscribed to them.
            var entry = new Entry
            {
                CreatedDate = DateTime.Now,
                PublisherName = package.PublisherName,
                EntryId = Queue.Count + 1,
                SystemMessage = package.Message,
                UserFriendlyMessage = package.Message
            };

            // Add the Entry
            AddQueueEntry(entry);

            if (!Subscribers.ContainsKey(package.PublisherName)) return; // Bail early if there are no subscribers.

            // Run through all of the subscribers for this publisher and generate a backlog item for them.
            foreach (var s in Subscribers[package.PublisherName])
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
                    WasSent = false
                });
            }

        }

        public void Notify(BackLog backLog)
        {
            throw new NotImplementedException();
        }

        public void LoadBacklog(Dispatch.Client dispatcher)
        {
            _backLog = Repository.All(dispatcher).ToList();
        }

        public void SaveBackLog(Dispatch.Client dispatcher)
        {
            Repository.UpdateAll(dispatcher, Backlog);
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
            _subscribers = new Dictionary<PublisherNames, List<string>>
            {
                {PublisherNames.OnLogException, new List<string>() {"ugwua"}}
            };

            dispatcher.Publish(new Package{ Entity = null, Message = "The base strategy for Notfication does not have a way to persist users", PublisherName = PublisherNames.OnNotificationAddQueueEntry});
        }

        public void AddSubscriber(PublisherNames publisherName, string userId)
        {
            if (!Subscribers.ContainsKey(publisherName)) Subscribers.Add(publisherName, new List<string>());

            if (Subscribers[publisherName].Contains(userId.Trim().ToLower())) return;

            Subscribers[publisherName].Add(userId.Trim().ToLower());
        }
    }
}
