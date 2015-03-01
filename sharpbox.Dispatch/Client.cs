using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
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
            _commandHub = new Dictionary<CommandNames, CommandHubItem>();
            _eventSubscribers = new Dictionary<EventNames, Queue<Action<Response>>>();
            CommandStream = new Queue<CommandStreamItem>();
        }

        private Dictionary<EventNames, Queue<Action<Response>>> _eventSubscribers;

        private Dictionary<CommandNames, CommandHubItem> _commandHub;

        public Queue<CommandStreamItem> CommandStream { get; private set; }

        /// <summary>
        /// This method will take the action and append it to the list for the given publisher name. Whenever publish is called for that publisherName the associated methods will be invoked.
        /// </summary>
        /// <param name="publisherName"></param>
        /// <param name="method"></param>
        public void Listen(EventNames publisherName, Action<Response> method)
        {
            EnsureEventSubscriberKey(publisherName);

            _eventSubscribers[publisherName].Enqueue(method);
        }

        /// <summary>
        /// While the CommandEvent map is used for creating the 1-to-1 relationship between thier commands name and event. i.e. - UpdateReciepe and OnReceipeUpdate you still need to register what callback you want to run on UpdateReceipe. Same with the event.
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="action"></param>
        /// <param name="eventName"></param>
        public void Register(CommandNames commandName, Func<Request, Response> action, EventNames eventName)
        {
            try
            {
                _commandHub.Add(commandName, new CommandHubItem { Action = action, EventName = eventName }); // wireup the action associated with this command, and the event channel to broadcast to when this command is processed.
            }
            catch (Exception ex)
            {
                Broadcast(new Response { Entity = ex, Type = ex.GetType(), EventName = EventNames.OnException, Message = "Dispatch failed to register the action", ResponseId = Guid.NewGuid() });
            }
        }

        /// <summary>
        /// Ensure the key exists, add the response to the Event stream. Cycle through all the subscribers and fire off the associated action.
        /// @SEE: For the apparoch to catch Target Invocation exceptions -> http://csharptest.net/350/throw-innerexception-without-the-loosing-stack-trace/
        /// </summary>
        /// <param name="response"></param>
        public void Broadcast(Response response)
        {
            EnsureEventSubscriberKey(response.EventName);

            foreach (var p in _eventSubscribers[response.EventName])
            {
                try
                {
                    p.Invoke(response);
                }
                catch (TargetInvocationException ex)
                {
                    var exResponse = new Response
                    {
                        Entity = ex,
                        Type = ex.GetType(),
                        EventName = EventNames.OnException,
                        Message = "Dispatch process failed to broadcast Request Id:" + response.RequestId + " on channel: " + response.EventName,
                        RequestId = response.RequestId,
                        ResponseId = Guid.NewGuid()
                    };

                    Broadcast(exResponse);
                }
            }
        }

        /// <summary>
        /// Ensure the key exists. Fire off the actio associated with this request's command.
        /// @SEE: For the apparoch to catch Target Invocation exceptions -> http://csharptest.net/350/throw-innerexception-without-the-loosing-stack-trace/
        /// </summary>
        /// <param name="request"></param>
        /// <exception cref=""></exception>
        public Response Process(Request request)
        {
            try
            {
                // Get the response from the registered action.
                var response = _commandHub[request.CommandName].Action.Invoke(request);
                response.EventName = _commandHub[request.CommandName].EventName; // Set the event name.

                // Add The incoming request and out going response to the command stream.
                CommandStream.Enqueue(new CommandStreamItem() { Command = request.CommandName, Request = request, Response = response });

                // Broadcase the response to all listeners.
                Broadcast(response);

                return response;
            }
            catch (Exception ex)
            {
                var exResponse = new Response
                {
                    Entity = ex,
                    Type = ex.GetType(),
                    EventName = EventNames.OnException,
                    Message = "Dispatch process failed for Request Id:" + request.RequestId,
                    RequestId = request.RequestId,
                    ResponseId = Guid.NewGuid()
                };

                CommandStream.Enqueue(new CommandStreamItem() { Command = request.CommandName, Request = request, Response = exResponse });

                Broadcast(exResponse);

                return new Response(request, String.Format("Command Failed: {0}. See Exception with Response Id: {1}", request.CommandName, exResponse.ResponseId), ResponseTypes.Error);
            }
        }

        private void EnsureEventSubscriberKey(EventNames eventName)
        {
            if (!_eventSubscribers.ContainsKey(eventName)) _eventSubscribers.Add(eventName, new Queue<Action<Response>>());
        }
    }
}
