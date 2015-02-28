using System;
using System.Collections.Generic;
using sharpbox.Dispatch.Model;
using sharpbox.Notification.Model;

namespace sharpbox.Notification.Strategy
{
    public class InMemoryStrategy : IStrategy
    {
        private Dictionary<EventNames, List<string>> _subscribers;
        private List<BackLogItem> _backLog;

        public Dictionary<EventNames, List<string>> Subscribers
        {
            get { return _subscribers ?? (_subscribers = new Dictionary<EventNames, List<string>>()); }
            set { _subscribers = value; }
        }

        public List<BackLogItem> BackLog
        {
            get { return _backLog ?? (_backLog = new List<BackLogItem>()); }
            set { _backLog = value; }
        }

        public void ProcessBackLogItem(BackLogItem backLogItem)
        {
            throw new NotImplementedException();
        }

        public Response Notify(Request request)
        {
            throw new NotImplementedException();
        }
    }
}
