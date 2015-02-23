using System;
using System.Collections.Generic;
using System.Linq;
using sharpbox.Dispatch.Model;

namespace sharpbox.Dispatch
{
    /// <summary>
    /// 
    /// </summary>
    /// <exception cref="System.Reflection.TargetInvocationException">A registered Action failed during either the Command Processing or the Event Broadcast.</exception>
    /// <exception cref="System.Exception">General uncaught exception from either Command Processing or an Event Broadcast</exception>
    public class Client
    {

        #region Constructor(s)

        public Client(string userId, List<EventNames> eventNames = null, List<CommandNames> commandNames = null)
        {
            CurrentUserId = userId;
            _availableEvents = eventNames ?? EventNames.DefaultPubList();
            _availableCommands = commandNames ?? CommandNames.DefaultActionList();
        }

        #endregion

        #region Field(s)

        private Dictionary<EventNames, List<Action<Package>>> _eventSubscribers;
        private Dictionary<CommandNames, List<Action<Request>>> _commandSubscribers;
        private List<EventNames> _availableEvents;
        private List<CommandNames> _availableCommands;
        private List<Package> _eventStream = new List<Package>();
        private List<Request> _commandStream = new List<Request>();
        private Dictionary<CommandNames, EventNames> _commandEventMap = new Dictionary<CommandNames, EventNames>();

        #endregion

        #region Properties

        public string CurrentUserId { get; set; }

        public Dictionary<EventNames, List<Action<Package>>> EventSubscribers
        {

            get
            {
                return _eventSubscribers ?? (_eventSubscribers = new Dictionary<EventNames, List<Action<Package>>>());
            }

            set { _eventSubscribers = value; }
        }

        public Dictionary<CommandNames, List<Action<Request>>> CommandSubscribers
        {

            get
            {
                return _commandSubscribers ?? (_commandSubscribers = new Dictionary<CommandNames, List<Action<Request>>>());
            }

            set { _commandSubscribers = value; }
        }

        public Dictionary<CommandNames, EventNames> CommandEventMap { get { return _commandEventMap; } set { _commandEventMap = value; } }

        /// <summary>
        /// Used almost exclusively by the AppContext so that it can extended and then allow the Auditor to loop and listen to all. Note: All internal modules will have access to the Dispatch dll directly.
        /// </summary>
        public List<EventNames> AvailableEvents
        {
            get { return _availableEvents ?? (_availableEvents = new List<EventNames>()); }
        }

        /// <summary>
        /// The list of commands that have been registered and can be called at any given time. Also looped through by the Auditor.
        /// </summary>
        public List<CommandNames> AvailableCommands
        {
            get { return _availableCommands ?? (_availableCommands = new List<CommandNames>()); }
        }

        public List<Package> EventStream { get { return _eventStream ?? (_eventStream = new List<Package>()); } }

        public List<Request> CommandStream { get { return _commandStream ?? (_commandStream = new List<Request>()); } }

        #endregion

        #region Method(s)
        /// <summary>
        /// This method will take the action and append it to the list for the given publisher name. Whenever publish is called for that publisherName the associated methods will be invoked.
        /// </summary>
        /// <param name="publisherName"></param>
        /// <param name="method"></param>
        public void Listen(EventNames publisherName, Action<Package> method)
        {
            EnsureEventSubscriberKey(publisherName);

            EventSubscribers[publisherName].Add(method);
        }

        /// <summary>
        /// While the CommandEvent map is used for creating the 1-to-1 relationship between thier commands name and event. i.e. - UpdateReciepe and OnReceipeUpdate you still need to register what callback you want to run on UpdateReceipe. Same with the event.
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="method"></param>
        public void Register(CommandNames commandName, Action<Request> method)
        {
            if (!CommandSubscribers.ContainsKey(CommandNames.SetFeedback) && commandName != CommandNames.SetFeedback) throw new Exception("A 'SetFeedback' action must be registered before using any action registration!");

            EnsureCommandSubscriberKey(commandName);

            CommandSubscribers[commandName].Add(method);
        }

        /// <summary>
        /// Ensure the key exists, add the package to the Event stream. Cycle through all the subscribers and fire off the associated action.
        /// </summary>
        /// <param name="package"></param>
        public void Broadcast(Package package)
        {
            EnsureEventSubscriberKey(package.EventName);

            _eventStream.Add(package);

            foreach (var p in EventSubscribers[package.EventName])
            {

                p.Invoke(package);

            }
        }

        /// <summary>
        /// Ensure the key exists. Add request to the Command stream. Cycle through all the subscribers and fire off the associated action.
        /// </summary>
        /// <param name="request"></param>
        public void Process(Request request)
        {
            EnsureCommandSubscriberKey(request.CommandName);

            _commandStream.Add(request);

            foreach (var a in CommandSubscribers[request.CommandName])
            {
                a.Invoke(request);

                //Auto broadcast to the command's primary event
                if (CommandEventMap.ContainsKey(request.CommandName)) Broadcast(new Package { Entity = request.Entity, EventName = CommandEventMap[request.CommandName], Message = String.Format("Processed Request : {0}", request.RequestId), PackageId = Guid.NewGuid(), UserId = CurrentUserId });
            }
        }

        #endregion

        #region Private Helper(s)

        private void EnsureEventSubscriberKey(EventNames eventName)
        {
            if (!EventSubscribers.ContainsKey(eventName)) EventSubscribers.Add(eventName, new List<Action<Package>>());

        }

        private void EnsureCommandSubscriberKey(CommandNames commandName)
        {
            if (!CommandSubscribers.ContainsKey(commandName)) CommandSubscribers.Add(commandName, new List<Action<Request>>());

        }

        #endregion

        #region Public Helper(s)

        public void ExtendAvailableEvents(List<EventNames> eventNames)
        {
            foreach (var e in eventNames.Where(e => !AvailableEvents.Contains(e)))
            {
                AvailableEvents.Add(e);
            }
        }

        public void ExtendAvailableCommands(List<CommandNames> actionNames)
        {
            foreach (var a in actionNames.Where(a => !AvailableCommands.Contains(a)))
            {
                AvailableCommands.Add(a);
            }
        }

        #endregion

    }
}
