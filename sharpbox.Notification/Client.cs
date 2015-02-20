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

        public Client(Dispatch.Client dispatcher, IStrategy strategy = null, Dictionary<string, object> props = null)
        {
            _strategy = strategy ?? new BaseStrategy(dispatcher, props ?? new Dictionary<string, object> { { "xmlPath", "NotificationXmlRepository.xml" } });
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

        public Dictionary<EventNames, List<Entry>> Queue { get { return _strategy.Queue; } }
        public Dictionary<EventNames, List<string>> Subscribers { get { return _strategy.Subscribers;  } set{ _strategy.Subscribers = value;} }
        public List<BackLog> Backlog { get { return _strategy.Backlog;  } }

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
        /// <param name="dispatcher"></param>
        /// <param name="package"></param>
        public void ProcessPackage(Dispatch.Client dispatcher, Package package)
        {
            _strategy.ProcessPackage(dispatcher, package);
        }

        public void LoadBacklog(Dispatch.Client dispatcher)
        {
            _strategy.LoadBacklog(dispatcher);
        }

        public void AddQueueEntry(Entry entry)
        {
            _strategy.AddQueueEntry(entry);
        }

        public void Notify(BackLog backLog)
        {
           _strategy.Notify(backLog);
        }

        public void SaveBackLog(Dispatch.Client dispatcher)
        {
            _strategy.SaveBackLog(dispatcher);
        }

        public void AddBackLogItem(Dispatch.Client dispatcher, BackLog backlog)
        {
            _strategy.AddBackLogItem(dispatcher, backlog);
        }

        public void LoadSubscribers(Dispatch.Client dispatcher)
        {
            _strategy.LoadSubscribers(dispatcher);
        }

        public void AddSubscriber(EventNames publisherName, string userId)
        {
            _strategy.AddSubscriber(publisherName, userId);
        }

        #endregion
    }
}
