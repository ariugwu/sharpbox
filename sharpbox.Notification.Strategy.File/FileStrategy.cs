using System;
using System.Collections.Generic;
using System.Linq;
using sharpbox.Dispatch.Model;
using sharpbox.Notification.Model;

namespace sharpbox.Notification.Strategy.File
{
    public class FileStrategy : IStrategy
    {
        private Io.Client _file;
        private Dictionary<string, object> _props;

        public FileStrategy(Io.Strategy.IStrategy persistenceStrategy, Dictionary<string, object> props)
        {
            _file = new Io.Client(persistenceStrategy);
            _props = props;

            _emailClient = (Email.Client) props["emailClient"];

            LoadBacklog();
            LoadSubscribers();
        }

        private Email.Client _emailClient;
        private Dictionary<EventNames, List<string>> _subscribers;
        private Queue<BackLogItem> _backLog;

        public Dictionary<EventNames, List<string>> Subscribers { get{ return _subscribers ?? (_subscribers = new Dictionary<EventNames, List<string>>());} set { _subscribers = value; } }

        public Queue<BackLogItem> BackLog { get { return _backLog ?? (_backLog = new Queue<BackLogItem>()); } set { _backLog = value; } }

        public BackLogItem ProcessBackLogItem(BackLogItem backLogItem)
        {
            return backLogItem;
        }

        public Response Notify(Request request)
        {
            var bli = (BackLogItem) request.Entity;

            _emailClient.Send(bli.To, bli.From, bli.Subject, bli.Message, bli.Cc ?? new List<string>(1), bli.Bcc ?? new List<string>(1), new Dictionary<string, byte[]>(1));

            return new Response(request, String.Format("Email sent to {0}. With Subject: {1}", String.Join(";", bli.To),bli.Subject));
        }

        public void LoadBacklog()
        {
            var filePath = _props["filePath"].ToString();
            if (!_file.Exists(filePath)) _file.Write(filePath, new Queue<BackLogItem>());
            BackLog = _file.Read<Queue<BackLogItem>>(_props["filePath"].ToString());
        }

        public void SaveBackLog()
        {
            _file.Write(_props["filePath"].ToString(), BackLog);
            LoadBacklog();
        }

        public void AddBackLogItem(BackLogItem backlog)
        {
            
            //Bail if there is already an entry for this user and this event.
            if (BackLog.FirstOrDefault(x => x.Equals(backlog.EntryId) && x.UserId.Trim().ToLower().Equals(backlog.UserId.Trim().ToLower())) != null) return;

            BackLog.Enqueue(backlog);
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
