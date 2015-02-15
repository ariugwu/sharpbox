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
            _availablePublications = publisherNames ?? PublisherNames.DefaultPubList();
        }

        #endregion

        #region Field(s)

        private Dictionary<PublisherNames, List<Action<Client,Package>>> _subscribers;
        private List<PublisherNames> _availablePublications;
        #endregion

        #region Properties

        public Dictionary<PublisherNames, List<Action<Client,Package>>> Subscribers
        {

            get
            {
                return _subscribers ?? (_subscribers = new Dictionary<PublisherNames, List<Action<Client,Package>>>());
            }

            set { _subscribers = value; }
        }

        /// <summary>
        /// Used almost exclusively by the AppContext so that it can extend and then allow the Auditor to loop and register will all. All internal modules will have access to the Dispatch dll directly.
        /// </summary>
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
        public void Subscribe(PublisherNames publisherName, Action<Client,Package> method)
        {
            EnsureSubscriberKey(publisherName);

            Subscribers[publisherName].Add(method);
        }

        /// <summary>
        /// Cycle through all the subscribers and fire off the associated action
        /// </summary>
        /// <param name="package"></param>
        public void Publish(Package package)
        {
            EnsureSubscriberKey(package.PublisherName);

            foreach (var p in Subscribers[package.PublisherName])
            {
                p.Invoke(this,package);
            }
        }

        #endregion

        #region Helper(s)

        private void EnsureSubscriberKey(PublisherNames publisherName)
        {
            if (!Subscribers.ContainsKey(publisherName)) Subscribers.Add(publisherName, new List<Action<Client,Package>>());

        }

        #endregion

    }
}
