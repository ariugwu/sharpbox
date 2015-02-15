using System;
using System.Collections.Generic;
using sharpbox.Dispatch.Model;

namespace sharpbox.Dispatch
{
    public class Client
    {

        #region Constructor(s)

        public Client(List<PublisherNames> publisherNames = null)
        {
            if (publisherNames != null) _availablePublications = publisherNames;
        }

        #endregion

        #region Field(s)

        private Dictionary<PublisherNames, List<Action<PublisherNames>>> _subscribers;
        private List<PublisherNames> _availablePublications;
        #endregion

        #region Properties

        public Dictionary<PublisherNames, List<Action<PublisherNames>>> Subscribers
        {

            get
            {
                return _subscribers ?? (_subscribers = new Dictionary<PublisherNames, List<Action<PublisherNames>>>());
            }

            set { _subscribers = value; }
        }

        public List<PublisherNames> AvailablePublications
        {
            get { return _availablePublications ?? (_availablePublications = new List<PublisherNames>()); }
            set { _availablePublications = value; }
        }

        #endregion

        #region Method(s)
        /// <summary>
        /// This method will take the action and append it to the list for the given publisher name. Whenever publish is called for that publisherName the associated methods will be invoked.
        /// </summary>
        /// <param name="publisherName"></param>
        /// <param name="method"></param>
        public void Subscribe(PublisherNames publisherName, Action<PublisherNames> method)
        {
            EnsureSubscriberKey(publisherName);

            Subscribers[publisherName].Add(method);
        }

        /// <summary>
        /// Cycle through all the subscribers and fire off the associated action
        /// </summary>
        /// <param name="publisherName"></param>
        public void Publish(PublisherNames publisherName)
        {
            EnsureSubscriberKey(publisherName);

            foreach (var p in Subscribers[publisherName])
            {
                p.Invoke(publisherName);
            }
        }

        #endregion

        #region Helper(s)

        private void EnsureSubscriberKey(PublisherNames publisherName)
        {
            if (!Subscribers.ContainsKey(publisherName)) Subscribers.Add(publisherName, new List<Action<PublisherNames>>());

        }

        #endregion

    }
}
