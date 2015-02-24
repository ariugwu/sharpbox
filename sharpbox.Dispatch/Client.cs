using System;
using System.Collections.Generic;
using sharpbox.Dispatch.Model;

namespace sharpbox.Dispatch
{
    /// <summary>
    /// 
    /// </summary>
    /// <exception cref="System.Reflection.TargetInvocationException">A registered Action failed during either the Command Processing or the Event Broadcast.</exception>
    /// <exception cref="System.Exception">General uncaught exception from either Command Processing or an Event Broadcast</exception>
    [Serializable]
    public class Client
    {
        public Client() { }

        public Client(string userId, List<EventNames> eventNames = null, List<CommandNames> commandNames = null)
        {
            CurrentUserId = userId;
            AvailableEvents = eventNames ?? EventNames.DefaultPubList();
            AvailableCommands = commandNames ?? CommandNames.DefaultActionList();

            EventSubscribers = new Dictionary<EventNames, Queue<Action<Response>>>();
            CommandSubscribers = new Dictionary<CommandNames, Queue<Action<Request>>>();
            CommandEventMap = new Dictionary<CommandNames, EventNames>();

            CommandStream = new Queue<Request>();
            EventStream = new Queue<Response>();
        }

        public string CurrentUserId { get; set; }

        public Dictionary<EventNames, Queue<Action<Response>>> EventSubscribers { get; set; }

        public Dictionary<CommandNames, Queue<Action<Request>>> CommandSubscribers { get; set; }

        public Dictionary<CommandNames, EventNames> CommandEventMap { get; set; }

        /// <summary>
        /// Used almost exclusively by the AppContext so that it can extended and then allow the Auditor to loop and listen to all. Note: All internal modules will have access to the Dispatch dll directly.
        /// </summary>
        public List<EventNames> AvailableEvents { get; set; }
        /// <summary>
        /// The list of commands that have been registered and can be called at any given time. Also looped through by the Auditor.
        /// </summary>
        public List<CommandNames> AvailableCommands { get; set; }
        public Queue<Response> EventStream { get; set; }

        public Queue<Request> CommandStream { get; set; }


        /// <summary>
        /// This method will take the action and append it to the list for the given publisher name. Whenever publish is called for that publisherName the associated methods will be invoked.
        /// </summary>
        /// <param name="publisherName"></param>
        /// <param name="method"></param>
        public void Listen(EventNames publisherName, Action<Response> method)
        {
            EnsureEventSubscriberKey(publisherName);

            EventSubscribers[publisherName].Enqueue(method);
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

            CommandSubscribers[commandName].Enqueue(method);
        }

        /// <summary>
        /// Ensure the key exists, add the response to the Event stream. Cycle through all the subscribers and fire off the associated action.
        /// </summary>
        /// <param name="response"></param>
        public void Broadcast(Response response)
        {
            EnsureEventSubscriberKey(response.EventName);

            EventStream.Enqueue(response);

            foreach (var p in EventSubscribers[response.EventName])
            {
                p.Invoke(response);
            }
        }

        /// <summary>
        /// Ensure the key exists. Add request to the Command stream. Cycle through all the subscribers and fire off the associated action.
        /// </summary>
        /// <param name="request"></param>
        public void Process(Request request)
        {
            EnsureCommandSubscriberKey(request.CommandName);

            CommandStream.Enqueue(request);

            foreach (var a in CommandSubscribers[request.CommandName])
            {
                a.Invoke(request);

                //Auto broadcast to the command's primary event
                if (CommandEventMap.ContainsKey(request.CommandName)) Broadcast(new Response { Entity = request.Entity, EventName = CommandEventMap[request.CommandName], Message = String.Format("{0} | RequestId : {1}", request.Message, request.RequestId), ResponseId = Guid.NewGuid(), UserId = CurrentUserId });
            }
        }

        private void EnsureEventSubscriberKey(EventNames eventName)
        {
            if (!EventSubscribers.ContainsKey(eventName)) EventSubscribers.Add(eventName, new Queue<Action<Response>>());
        }

        private void EnsureCommandSubscriberKey(CommandNames commandName)
        {
            if (!CommandSubscribers.ContainsKey(commandName)) CommandSubscribers.Add(commandName, new Queue<Action<Request>>());
        }
    }
}
