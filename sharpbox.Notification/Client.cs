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

        public Client(Dispatch.Client dispatcher, IStrategy strategy)
        {
            _strategy = strategy;
            ConfigureNotification(dispatcher);
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
        public List<BackLog> Queue { get { return _strategy.Queue;  } }

        #endregion

        #region Client Method(s)

        public void ConfigureNotification(Dispatch.Client dispatcher)
        {
            foreach (var p in dispatcher.AvailableEvents.Where(x => !x.ToString().ToLower().Contains("onnotification"))) // subscribe to everything but our own events
            {
                dispatcher.Listen(p, ProcessPackage);
            }
        }

        #endregion

        #region Strategy Method(s)

        /// <summary>
        /// Whenever the dispatcher publishes an event we create a message for it and stick it on the queue. Then we see if anyone is requesting notification and we create a backlog entry for them to be processed at a later date. Likely by a scheduled task or explict request.
        /// </summary>
        /// <param name="response"></param>
        public void ProcessPackage(Response response)
        {
            _strategy.ProcessPackage(response);
        }

        public void LoadBacklog()
        {
            _strategy.LoadBacklog();
        }

        public void Notify(BackLog backLog)
        {
           _strategy.Notify(backLog);
        }

        public void SaveBackLog()
        {
            _strategy.SaveBackLog();
        }

        public void AddBackLogItem(Dispatch.Client dispatcher, BackLog backlog)
        {
            _strategy.AddBackLogItem(backlog);
        }

        public void LoadSubscribers(Dispatch.Client dispatcher)
        {
            _strategy.LoadSubscribers();
        }

        public void AddSubscriber(EventNames publisherName, string userId)
        {
            _strategy.AddSubscriber(publisherName, userId);
        }

        #endregion
    }
}
