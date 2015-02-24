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
        public Client()
        {
            CommandHub = new Dictionary<CommandNames, CommandHubItem>();
            EventSubscribers = new Dictionary<EventNames, Queue<Action<Response>>>();
            CommandStream = new Queue<CommandStreamItem>();
        }

        public Dictionary<EventNames, Queue<Action<Response>>> EventSubscribers { get; set; }

        public Dictionary<CommandNames, CommandHubItem> CommandHub { get; set; }

        public Queue<CommandStreamItem> CommandStream { get; set; }


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
        /// <param name="action"></param>
        /// <param name="eventName"></param>
        public void Register(CommandNames commandName, Func<Request, Request> action, EventNames eventName)
        {
            try
            {
                CommandHub.Add(commandName, new CommandHubItem { Action = action, EventName = eventName }); // wireup the action associated with this command, and the event channel to broadcast to when this command is processed.
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Ensure the key exists, add the response to the Event stream. Cycle through all the subscribers and fire off the associated action.
        /// </summary>
        /// <param name="response"></param>
        public void Broadcast(Response response)
        {
            EnsureEventSubscriberKey(response.EventName);

            foreach (var p in EventSubscribers[response.EventName])
            {
                p.Invoke(response);
            }
        }

        /// <summary>
        /// Ensure the key exists. Fire off the actio associated with this request's command.
        /// </summary>
        /// <param name="request"></param>
        /// <exception cref=""></exception>
        public void Process(Request request)
        {
            try
            {
                request = CommandHub[request.CommandName].Action.Invoke(request);

                //Auto broadcast to the command's primary event
                var response = new Response
                {
                    Entity = request.Entity,
                    EventName = CommandHub[request.CommandName].EventName,
                    Message = String.Format("{0} | RequestId : {1}", request.Message, request.RequestId),
                    ResponseId = Guid.NewGuid(),
                    RequestId = request.RequestId
                };

                // Add The incoming request and out going response to the command stream.
                CommandStream.Enqueue(new CommandStreamItem() { Command = request.CommandName, Request = request, Response = response });

                // Broadcase the response to all listeners.
                Broadcast(response);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void EnsureEventSubscriberKey(EventNames eventName)
        {
            if (!EventSubscribers.ContainsKey(eventName)) EventSubscribers.Add(eventName, new Queue<Action<Response>>());
        }
    }
}
