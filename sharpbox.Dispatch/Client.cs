using System;
using System.Collections.Generic;
using System.Reflection;
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
            _echoSubscribers = new Queue<Action<Response>>();
            _routineHub = new Dictionary<RoutineNames, Queue<RoutineItem>>();
            CommandStream = new Queue<CommandStreamItem>();
        }

        private const string ResponseMessage = "[Broadcast Event: {0}] [For Command: {1}] [Method: {2}] [Entity: {3}] [Request Id: {4}] [Response Id: {5}] [Message: {6}]";

        private Dictionary<EventNames, Queue<Action<Response>>> _eventSubscribers;

        private Queue<Action<Response>> _echoSubscribers;

        private Dictionary<CommandNames, CommandHubItem> _commandHub;

        private Dictionary<RoutineNames, Queue<RoutineItem>> _routineHub;

        /// <summary>
        /// The list of all commands processed by the Dispatcher.
        /// </summary>
        public Queue<CommandStreamItem> CommandStream { get; private set; }

        /// <summary>
        /// This method will take the action and append it to the list for the given publisher name. Whenever publish is called for that publisherName the associated methods will be invoked.
        /// </summary>
        /// <param name="eventName">The event you would like to subscribe to</param>
        /// <param name="method">The callback method to target when that event is fired.</param>
        public void Listen(EventNames eventName, Action<Response> method)
        {
            EnsureEventSubscriberKey(eventName);

            _eventSubscribers[eventName].Enqueue(method);
        }

        /// <summary>
        /// Any method here will get called for every event.
        /// </summary>
        /// <param name="method">The callback method to target when an event is fired.</param>
        public void Echo(Action<Response> method)
        {
            _echoSubscribers.Enqueue(method);
        }

        /// <summary>
        /// Register with the command hub, which is used for creating the 1-to-1 relationship between thier commands name and event. i.e. - UpdateReciepe and OnReceipeUpdate you still need to register what callback you want to run on UpdateReceipe. Same with the event.
        /// </summary>
        /// <param name="commandName">The command to register</param>
        /// <param name="action">The action to invoke.</param>
        /// <param name="eventName">The channel to broadcast the response on.</param>
        public void Register<T>(CommandNames commandName, Func<T, T> action, EventNames eventName)
        {
            try
            {
                _commandHub.Add(commandName, new CommandHubItem { Action = action, EventName = eventName }); // wireup the action associated with this command, and the event channel to broadcast to when this command is processed.
            }
            catch (Exception ex)
            {
                var msg = String.Format(ResponseMessage, eventName, commandName, action.Method.Name, null, "N/A","N/A", "Registration failed with msg: " + ex.Message);
                Broadcast(new Response { Entity = ex, Type = ex.GetType(), EventName = EventNames.OnException, Message = msg, ResponseUniqueKey = Guid.NewGuid() });
            }
        }

        /// <summary>
        /// Register with the routine hub, which is used to chain command actions and provide failover and rollback options
        /// </summary>
        /// <typeparam name="T">The type of the parameter which will be passed between actions.</typeparam>
        /// <param name="routineName"></param>
        /// <param name="commandName"></param>
        /// <param name="eventName"></param>
        /// <param name="action">Called as the primary/preferred target</param>
        /// <param name="failOver">Optional. Will be called on error of the 'action'</param>
        /// <param name="rollBack">Optional. In the advent of an error that can't be solved by action or failover. Once this level is reached the command queue will stop processing.</param>
        public void Register<T>(RoutineNames routineName, CommandNames commandName, EventNames eventName, Func<T, T> action, Func<T, T> failOver, Func<T, T> rollBack)
        {
            try
            {
                EnsureCommandHubKey(routineName);
                _routineHub[routineName].Enqueue(new RoutineItem {CommandName = commandName, EventName = eventName, Action = action, FailOver = failOver, Rollback = rollBack }); // wireup the action associated with this command, and the event channel to broadcast to when this command is processed.
            }
            catch (Exception ex)
            {
                var msg = String.Format(ResponseMessage, eventName, commandName, action.Method.Name, null, "N/A", "N/A", "Registration failed with msg: " + ex.Message);
                Broadcast(new Response { Entity = ex, Type = ex.GetType(), EventName = EventNames.OnException, Message = msg, ResponseUniqueKey = Guid.NewGuid() });
            }
        }

        /// <summary>
        /// Ensure the key exists, add the response to the Event stream. Cycle through all the subscribers and fire off the associated action.
        /// @SEE: For the apparoch to catch Target Invocation exceptions -> http://csharptest.net/350/throw-innerexception-without-the-loosing-stack-trace/
        /// </summary>
        /// <param name="response">The changed object, the original request, and other useful data is packaged in this object for easy sharing.</param>
        public void Broadcast(Response response)
        {

            // Go through each method that wants to 'trace' all events in the system
            foreach (var t in _echoSubscribers)
            {
                try
                {
                    t.Invoke(response);
                }
                catch (TargetInvocationException ex)
                {
                    BroadCastExceptionResponse(ex, response.Request);
                }
            }

            EnsureEventSubscriberKey(response.EventName);
            // Go through each subscriber to this event.
            foreach (var p in _eventSubscribers[response.EventName])
            {
                try
                {
                    p.Invoke(response);
                }
                catch (TargetInvocationException ex)
                {
                    BroadCastExceptionResponse(ex, response.Request);
                }
            }
        }

        /// <summary>
        /// Fires off the queue for the given routine in order. The argument should be the parameters for all registered actions since each target takes in and returns the same type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="routineName"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public T Process<T>(RoutineNames routineName, string message, object[] args)
        {
            var result = default(T);

            foreach (var r in _routineHub[routineName])
            {
                try
                {
                    result = ProcessRoutineAction<T>(routineName, r, message, args);
                }
                catch (Exception ex)
                {
                    try
                    {
                        // If we have a fail over action then try it. If it also fails or doesn't exist then try for a roll back.
                        if (r.FailOver != null)
                        {
                            
                            try
                            {
                                result = ProcessFailOver<T>(ex, routineName, r, args);
                            }
                            catch (Exception failOverException)
                            {
                                if (r.Rollback != null)
                                {
                                    result = ProcessRollBack<T>(failOverException, routineName, r, args);
                                }

                                // You've failed the action, and failOver, and executed a rollback if available. Break out of the routine.
                                break;
                            }
                        }
                        else if (r.Rollback != null)
                        {
                            result = ProcessRollBack<T>(ex, routineName, r, args);

                            //We've failed the action, there's no failOver and we've executed a rollback if available. Break out of the routine
                            break;
                        }
                        else
                        {
                            // Break out of the routine
                            break;
                        }
                    }
                    catch (Exception deepEx)
                    {
                        var request = Request.Create(r.CommandName, "All is lost. The action failed, then both failover, rollover, and all error handling failed.", args);
                        BroadCastExceptionResponse(deepEx, request);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Ensure the key exists. Fire off the action associated with this request's command.
        /// @SEE: For the apparoch to catch Target Invocation exceptions -> http://csharptest.net/350/throw-innerexception-without-the-loosing-stack-trace/
        /// </summary>
        /// <exception cref="KeyNotFoundException"></exception>
        public Response Process<T>(CommandNames commandName, string message, object[] args)
        {
            var request = Request.Create(commandName, message, args);
            var response = new Response(request, request.Message, ResponseTypes.Success);

            request.Action = _commandHub[request.CommandName].Action;

            try
            {
                if (request.Action.Method.ReturnType == typeof(void)) // @SEE: http://stackoverflow.com/questions/3456994/how-to-use-net-reflection-to-determine-method-return-type-including-void-and
                {
                    response.Message = "Target method returns void. No message will be populated";
                }
                else
                {
                    var result = (T)request.Action.DynamicInvoke(args);
                    response.Entity = result;
                    response.Type = result.GetType();
                }

                response.EventName = _commandHub[request.CommandName].EventName; // Set the event name.

                // Add The incoming request and out going response to the command stream.
                CommandStream.Enqueue(new CommandStreamItem() { Command = request.CommandName, Response = response });

                // Broadcase the response to all listeners.
                Broadcast(response);

                return response;
            }
            catch (Exception ex)
            {

                var exResponseUniqueKey = BroadCastExceptionResponse(ex, request);

                return new Response(request, String.Format("Command Failed: {0}. See Exception with Response (Unique) Key: {1}.", request.CommandName, exResponseUniqueKey), ResponseTypes.Error);
            }
        }

        /// <summary>
        /// A factored out helper for anytime an exception needs to be broadcsat. Used in methods that have a request available.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="request"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private Guid BroadCastExceptionResponse(Exception ex, Request request)
        {
            var exResponse = new Response
            {
                Entity = ex,
                Type = ex.GetType(),
                EventName = EventNames.OnException,
                Message = string.Format("{0} [Request Id: {1} ] - '{2}'", ex.Message,request.RequestUniqueKey, request.Message),
                RequestId = request.RequestId,
                RequestUniqueKey = request.RequestUniqueKey,
                Request = request,
                ResponseUniqueKey = Guid.NewGuid(),
                ResponseType = ResponseTypes.Error
            };

            CommandStream.Enqueue(new CommandStreamItem() { Command = request.CommandName, Response = exResponse });

            Broadcast(exResponse);

            return exResponse.RequestUniqueKey;

        }

        private T ProcessRoutineAction<T>(RoutineNames routineName, RoutineItem r, string message, object[] args)
        {
            r.BroadCastMessage = message; // Set the broadcast message. Other values are set during registration.

            var request = Request.Create(r.CommandName, r.BroadCastMessage, args);
            var response = new Response(request, request.Message + "[Routine: " + routineName + "]", ResponseTypes.Success);

            var result = (T)r.Action.DynamicInvoke(args);
            response.Entity = result;
            response.EventName = r.EventName;
            CommandStream.Enqueue(new CommandStreamItem() { Command = r.CommandName, Response = response });

            // Broadcase the response to all listeners.
            Broadcast(response);

            return result;
        }

        private T ProcessFailOver<T>(Exception ex, RoutineNames routineName, RoutineItem r, object[] args)
        {
            var request = Request.Create(r.CommandName, r.BroadCastMessage, args);
            var exResponseUniqueKey = BroadCastExceptionResponse(ex, request);
            var response = new Response(request, request.Message + "[Routine: " + routineName + "] [Exception Response Key: + " + exResponseUniqueKey + "] [Executing Failover Method: " + r.FailOver.Method.Name + "]", ResponseTypes.Success);

            var result = (T)r.FailOver.DynamicInvoke(args);
            response.Entity = result;
            response.EventName = r.EventName;

            CommandStream.Enqueue(new CommandStreamItem()
            {
                Command = r.CommandName,
                Response = response
            });

            // Broadcase the response to all listeners.
            Broadcast(response);

            return result;
        }

        private T ProcessRollBack<T>(Exception ex, RoutineNames routineName, RoutineItem r, object[] args)
        {
            var result = default(T);
            var request = Request.Create(r.CommandName, r.BroadCastMessage + "[Routine: " + routineName + "]", args);
            var exResponseUniqueKey = BroadCastExceptionResponse(ex, request);
            
            var response = new Response(request, request.Message + "[Routine: " + routineName + "] [Exception Response Key: + " + exResponseUniqueKey + " ] [Executing Rollback Method: " + r.Rollback.Method.Name + "]", ResponseTypes.Success);
                response.EventName = r.EventName;

            try
            {
                result = (T)r.Rollback.DynamicInvoke(args);
                response.Entity = result;

                // Broadcase the response to all listeners.
                Broadcast(response);
            }
            catch (Exception rollbackException)
            {
                exResponseUniqueKey = BroadCastExceptionResponse(rollbackException, request);

                response.Message = "[All is lost. RollBack method failed]" + response.Message;
                response.ResponseType = ResponseTypes.Error;

                CommandStream.Enqueue(new CommandStreamItem() { Command = r.CommandName, Response = response });

                // Broadcase the response to all listeners.
                Broadcast(response);
            }

            return result;
        }

        private void EnsureEventSubscriberKey(EventNames eventName)
        {
            if (!_eventSubscribers.ContainsKey(eventName)) _eventSubscribers.Add(eventName, new Queue<Action<Response>>());
        }

        private void EnsureCommandHubKey(RoutineNames routineName)
    {
        if (!_routineHub.ContainsKey(routineName)) _routineHub.Add(routineName, new Queue<RoutineItem>());
    }

    }
}
