using System;
using System.Collections.Generic;
using System.Reflection;
using sharpbox.Dispatch.Model;

namespace sharpbox.Dispatch
{
    public class Client
    {

        #region Constructor(s)

        public Client(string userId, List<EventNames> eventNames = null, List<ActionNames> actionNames = null)
        {
            CurrentUserId = userId;
            _availableEvents = eventNames ?? EventNames.DefaultPubList();
            _availableActions = actionNames ?? ActionNames.DefaultActionList();
        }

        #endregion

        #region Field(s)

        private Dictionary<EventNames, List<Action<Client, Package>>> _eventSubscribers;
        private Dictionary<ActionNames, List<Action<Client, Request>>> _actionSubscribers;
        private List<EventNames> _availableEvents;
        private List<ActionNames> _availableActions;
        #endregion

        #region Properties

        public string CurrentUserId { get; set; }

        public Dictionary<EventNames, List<Action<Client, Package>>> EventSubscribers
        {

            get
            {
                return _eventSubscribers ?? (_eventSubscribers = new Dictionary<EventNames, List<Action<Client, Package>>>());
            }

            set { _eventSubscribers = value; }
        }

        public Dictionary<ActionNames, List<Action<Client, Request>>> ActionSubscribers
        {

            get
            {
                return _actionSubscribers ?? (_actionSubscribers = new Dictionary<ActionNames, List<Action<Client, Request>>>());
            }

            set { _actionSubscribers = value; }
        }

        /// <summary>
        /// Used almost exclusively by the AppContext so that it can extend and then allow the Auditor to loop and register will all. All internal modules will have access to the Dispatch dll directly.
        /// </summary>
        public List<EventNames> AvailableEvents
        {
            get { return _availableEvents ?? (_availableEvents = new List<EventNames>()); }
            set { _availableEvents = value; }
        }

        public List<ActionNames> AvailableActions
        {
            get { return _availableActions ?? (_availableActions = new List<ActionNames>()); }
            set { _availableActions = value; }
        }

        #endregion

        #region Method(s)
        /// <summary>
        /// This method will take the action and append it to the list for the given publisher name. Whenever publish is called for that publisherName the associated methods will be invoked.
        /// </summary>
        /// <param name="publisherName"></param>
        /// <param name="method"></param>
        public void Listen(EventNames publisherName, Action<Client, Package> method)
        {
            EnsureEventSubscriberKey(publisherName);

            EventSubscribers[publisherName].Add(method);
        }

        public void Register(ActionNames actionName, Action<Client, Request> method)
        {
            if (!ActionSubscribers.ContainsKey(ActionNames.SetFeedback) && actionName != ActionNames.SetFeedback) throw new Exception("A 'SetFeedback' action must be registered before using any action registration!");

            EnsureActionSubscriberKey(actionName);

            ActionSubscribers[actionName].Add(method);
        }

        /// <summary>
        /// Cycle through all the subscribers and fire off the associated action
        /// </summary>
        /// <param name="package"></param>
        public void Broadcast(Package package)
        {
            EnsureEventSubscriberKey(package.EventName);

            foreach (var p in EventSubscribers[package.EventName])
            {
                try
                {
                    p.Invoke(this, package);
                }
                catch (TargetInvocationException tEx)
                {
                    this.Process(new Request{ ActionName = ActionNames.LogException, Entity = tEx, Message = String.Format("The following method failed: {0}", p.Method.Name), RequestId = 0, Type = tEx.GetType(), UserId = CurrentUserId});
                }
                catch (Exception ex)
                {
                    this.Process(new Request { ActionName = ActionNames.LogException, Entity = ex, Message = String.Format("The following method failed: {0}", p.Method.Name), RequestId = 0, Type = ex.GetType(), UserId = CurrentUserId });
                }
            }
        }


        public void Process(Request request)
        {
            foreach (var a in ActionSubscribers[request.ActionName])
            {
                try
                {
                    a.Invoke(this, request);
                }
                catch (TargetInvocationException tEx)
                {
                    this.Process(new Request { ActionName = ActionNames.LogException, Entity = tEx, Message = String.Format("The following method failed: {0}", a.Method.Name), RequestId = 0, Type = tEx.GetType(), UserId = CurrentUserId });
                }
                catch (Exception ex)
                {
                    this.Process(new Request { ActionName = ActionNames.LogException, Entity = ex, Message = String.Format("The following method failed: {0}", a.Method.Name), RequestId = 0, Type = ex.GetType(), UserId = CurrentUserId });
                }
            }
        }

        #endregion

        #region Helper(s)

        private void EnsureEventSubscriberKey(EventNames eventName)
        {
            if (!EventSubscribers.ContainsKey(eventName)) EventSubscribers.Add(eventName, new List<Action<Client, Package>>());

        }

        private void EnsureActionSubscriberKey(ActionNames actionName)
        {
            if (!ActionSubscribers.ContainsKey(actionName)) ActionSubscribers.Add(actionName, new List<Action<Client, Request>>());

        }
        #endregion

    }
}
