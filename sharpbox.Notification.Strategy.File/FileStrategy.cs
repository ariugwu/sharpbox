﻿using System;
using System.Collections.Generic;
using sharpbox.Dispatch.Model;
using sharpbox.Notification.Model;

namespace sharpbox.Notification.Strategy.File
{
    public class FileStrategy : IStrategy
    {
        private Io.Client _file;
        private string _filePath;

        public FileStrategy(Io.Strategy.IStrategy persistenceStrategy, Email.Client emailClient, string filePath)
        {
            _file = new Io.Client(persistenceStrategy);
            _filePath = filePath;

            _emailClient = emailClient;

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
        }

        public Response Notify(Request request)
        {
            var bli = (BackLogItem) request.Entity;

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

            SaveBackLog();

            return new Response(request, String.Format("Email sent to {0}. With Subject: {1}", String.Join(";", bli.To, bli.Subject)), ResponseTypes.Success);
        }

        public void LoadBacklog()
        {
            if (!_file.Exists(_filePath)) _file.Write(_filePath, new List<BackLogItem>());
            BackLog = _file.Read<List<BackLogItem>>(_filePath);
        }

        public void SaveBackLog()
        {
            _file.Write(_filePath, BackLog);
            LoadBacklog();
        }

    }
}
