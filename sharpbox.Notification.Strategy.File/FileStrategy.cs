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

        public FileStrategy(Io.Strategy.IStrategy persistenceStrategy, Dictionary<string, object> props)
        {
            _file = new Io.Client(persistenceStrategy);
            _props = props;

            _emailClient = (Email.Client) props["emailClient"];

            LoadBacklog();

        }

        private Email.Client _emailClient;
        private Dictionary<EventNames, List<string>> _subscribers;
        private List<BackLogItem> _backLog;

        public Dictionary<EventNames, List<string>> Subscribers { get{ return _subscribers ?? (_subscribers = new Dictionary<EventNames, List<string>>());} set { _subscribers = value; } }

        public List<BackLogItem> BackLog { get { return _backLog ?? (_backLog = new List<BackLogItem>()); } set { _backLog = value; } }

        public void ProcessBackLogItem(BackLogItem backLogItem)
        {
            _backLog.Add(backLogItem);
            SaveBackLog();
            return backLogItem;
        }

        public Response Notify(Request request)
        {
            var bli = (BackLogItem) request.Entity;

            _emailClient.Send(bli.To, bli.From, bli.Subject, bli.Message, bli.Cc ?? new List<string>(1), bli.Bcc ?? new List<string>(1), new Dictionary<string, byte[]>(1));

            bli.WasSent = true;
            bli.SentDate = DateTime.Now;

            // update the item in our backlog
            for (var i = 0; i < _backLog.Count; i++)
            {
                if (_backLog[i].BackLogItemId != bli.BackLogItemId) continue;

                _backLog[i] = bli;
                break;
            }

            SaveBackLog();

            return new Response(request, String.Format("Email sent to {0}. With Subject: {1}", String.Join(";", bli.To),bli.Subject));
        }

        public void LoadBacklog()
        {
            var filePath = _props["filePath"].ToString();
            if (!_file.Exists(filePath)) _file.Write(filePath, new List<BackLogItem>());
            BackLog = _file.Read<List<BackLogItem>>(_props["filePath"].ToString());
        }

        public void SaveBackLog()
        {
            _file.Write(_props["filePath"].ToString(), BackLog);
            LoadBacklog();
        }

    }
}
