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

        private Dictionary<EventNames, Queue<Action<Response>>> _eventSubscribers;

        private Queue<Action<Response>> _echoSubscribers;

        private Dictionary<CommandNames, CommandHubItem> _commandHub;

        private Dictionary<RoutineNames, Queue<RoutineItem>> _routineHub;

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
        /// Any method here will get called for every event.
        /// </summary>
        /// <param name="method"></param>
        public void Echo(Action<Response> method)
        {
            _echoSubscribers.Enqueue(method);
        }

        /// <summary>
        /// While the CommandEvent map is used for creating the 1-to-1 relationship between thier commands name and event. i.e. - UpdateReciepe and OnReceipeUpdate you still need to register what callback you want to run on UpdateReceipe. Same with the event.
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="action"></param>
        /// <param name="eventName"></param>
        public void Register<T>(CommandNames commandName, Func<T, T> action, EventNames eventName)
        {
            try
            {
                _commandHub.Add(commandName, new CommandHubItem { Action = action, EventName = eventName }); // wireup the action associated with this command, and the event channel to broadcast to when this command is processed.
            }
            catch (Exception ex)
            {
                Broadcast(new Response { Entity = ex, Type = ex.GetType(), EventName = EventNames.OnException, Message = "Dispatch failed to register the action", ResponseUniqueKey = Guid.NewGuid() });
            }
        }

        public void Register<T>(RoutineNames routineName, CommandNames commandName, EventNames eventName, Func<T, T> action, Func<T, T> failOver, Func<T, T> rollBack)
        {
            try
            {
                EnsureCommandHubKey(routineName);
                _routineHub[routineName].Enqueue(new RoutineItem {CommandName = commandName, EventName = eventName, Action = action, FailOver = failOver, Rollback = rollBack }); // wireup the action associated with this command, and the event channel to broadcast to when this command is processed.
            }
            catch (Exception ex)
            {
                Broadcast(new Response { Entity = ex, Type = ex.GetType(), EventName = EventNames.OnException, Message = "Dispatch failed to register the routine", ResponseUniqueKey = Guid.NewGuid() });
            }
        }

        /// <summary>
        /// Ensure the key exists, add the response to the Event stream. Cycle through all the subscribers and fire off the associated action.
        /// @SEE: For the apparoch to catch Target Invocation exceptions -> http://csharptest.net/350/throw-innerexception-without-the-loosing-stack-trace/
        /// </summary>
        /// <param name="response"></param>
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
                    var exResponse = new Response
                    {
                        Entity = ex,
                        Type = ex.GetType(),
                        EventName = EventNames.OnException,
                        Message = "Dispatch process failed to trace using the registered method( " + t.Method.Name + ") Request Id:" + response.RequestId + " for channel: " + response.EventName,
                        RequestId = response.RequestId,
                        ResponseUniqueKey = Guid.NewGuid(),
                        ResponseType = ResponseTypes.Error
                    };

                    Broadcast(exResponse);
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
                    var exResponse = new Response
                    {
                        Entity = ex,
                        Type = ex.GetType(),
                        EventName = EventNames.OnException,
                        Message = "Dispatch process failed to broadcast Request Id:" + response.RequestId + " on channel: " + response.EventName,
                        RequestId = response.RequestId,
                        ResponseUniqueKey = Guid.NewGuid(),
                        ResponseType = ResponseTypes.Error
                    };

                    Broadcast(exResponse);
                }
            }
        }

        public T Process<T>(RoutineNames routineName, string message, object[] args)
        {
            var result = default(T);
            Request request;
            Response response;

            foreach (var r in _routineHub[routineName])
            {
                try
                {
                    r.BroadCastMessage = message; // Se the broadcast message. Other values are set during registration.

                    request = Request.Create(r.CommandName, r.BroadCastMessage, args);
                    response = new Response(request, request.Message + "[Routine: " + routineName + "]", ResponseTypes.Success);

                    result = (T)r.Action.DynamicInvoke(args);
                    response.Entity = result;
                    response.EventName = r.EventName;
                    CommandStream.Enqueue(new CommandStreamItem() { Command = r.CommandName, Response = response });

                    // Broadcase the response to all listeners.
                    Broadcast(response);
                }
                catch (Exception ex)
                {
                    request = Request.Create(r.CommandName, r.BroadCastMessage + "[Routine: " + routineName+ "]", args);
                    var exResponseUniqueKey = BroadCastExceptionResponse(ex, request);

                    try
                    {
                        if (r.FailOver != null)
                        {
                            response = new Response(request, request.Message + "[Routine: " + routineName + "] [Exception Response Key: + " + exResponseUniqueKey + " [Executing Failover Method: " + r.FailOver.Method.Name+"]", ResponseTypes.Success);

                            try
                            {

                                result = (T)r.FailOver.DynamicInvoke(args);
                                response.Entity = result;
                                response.EventName = r.EventName;

                                CommandStream.Enqueue(new CommandStreamItem()
                                {
                                    Command = r.CommandName,
                                    Response = response
                                });

                                // Broadcase the response to all listeners.
                                Broadcast(response);

                            }
                            catch (Exception failOverException)
                            {
                                request = Request.Create(r.CommandName, r.BroadCastMessage + "[Routine: " + routineName + "]", args);
                                exResponseUniqueKey = BroadCastExceptionResponse(failOverException, request);

                                if (r.Rollback != null)
                                {
                                    response = new Response(request,
                                        request.Message + "[Routine: " + routineName + "] [Executing Rollback Method: " +
                                        r.Rollback.Method.Name + "]", ResponseTypes.Success);

                                    try
                                    {
                                        result = (T)r.Rollback.DynamicInvoke(args);
                                        response.Entity = result;
                                        response.EventName = r.EventName;

                                        // Broadcase the response to all listeners.
                                        Broadcast(response);
                                    }
                                    catch (Exception rollbackException)
                                    {
                                        exResponseUniqueKey = BroadCastExceptionResponse(rollbackException, request);
                                        response.ResponseType = ResponseTypes.Error;
                                        response.EventName = r.EventName;
                                        CommandStream.Enqueue(new CommandStreamItem() { Command = r.CommandName, Response = response });

                                        // Broadcase the response to all listeners.
                                        Broadcast(response);
                                    }
                                }

                                // You've failed the action, and failOver, and executed a rollback if available. Break out of the routine.
                                break;
                            }
                        }
                        else if (r.Rollback != null)
                        {
                            response = new Response(request, request.Message + "[Routine: " + routineName + "] [Executing Rollback Method: " + r.Rollback.Method.Name + "]", ResponseTypes.Success);
                            try
                            {
                                result = (T)r.Rollback.DynamicInvoke(args);
                                response.Entity = result;
                                response.EventName = r.EventName;

                                // Broadcase the response to all listeners.
                                Broadcast(response);
                            }
                            catch (Exception rollbackException)
                            {
                                exResponseUniqueKey = BroadCastExceptionResponse(rollbackException, request);
                                response.ResponseType = ResponseTypes.Error;
                                response.EventName = r.EventName;
                                CommandStream.Enqueue(new CommandStreamItem() { Command = r.CommandName, Response = response });

                                // Broadcase the response to all listeners.
                                Broadcast(response);
                            }

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

                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Ensure the key exists. Fire off the actio associated with this request's command.
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

        private Guid BroadCastExceptionResponse(Exception ex, Request request)
        {
            var exResponse = new Response
            {
                Entity = ex,
                Type = ex.GetType(),
                EventName = EventNames.OnException,
                Message = ex.Message + " [Request Id:" + request.RequestUniqueKey + "]",
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
