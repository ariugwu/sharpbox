using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        private const string ResponseMessage = "[Message: {0}] [Method: {1}] [Entity: {2}] ";

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
                var msg = String.Format(ResponseMessage, "Registration failed with msg: " + ex.Message, action.Method.Name, typeof(T).Name);
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
                _routineHub[routineName].Enqueue(new RoutineItem { CommandName = commandName, EventName = eventName, Action = action, FailOver = failOver, Rollback = rollBack }); // wireup the action associated with this command, and the event channel to broadcast to when this command is processed.
            }
            catch (Exception ex)
            {
                var msg = String.Format(ResponseMessage, "Registration failed with msg: " + ex.Message, action.Method.Name, typeof(T).Name);
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
        /// User beware. You can rollback an entire routine *if* you have rollback methods assigned to each RoutineItem in the queue. This method will reverse the queue and start with the last method and loop through roll backs. Passing the modified T to each in turn.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="routineName"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">If there is no routine found with the supplied name</exception>
        /// <exception cref="InvalidDataException">If there is a RoutineItem in the queue without a registered Rollback method.</exception>
        public T RollBack<T>(RoutineNames routineName, string message, object[] args)
        {

            if (!_routineHub.ContainsKey(routineName)) throw new ArgumentException("There is no routine by this name registered.");

            if (_routineHub[routineName].FirstOrDefault(x => x.Rollback == null) != null) throw new InvalidDataException("There is a method in this routine without a RollBack function assigned. This routine is not eligable for RollBack.");

            var result = default(T);

            var reverse = _routineHub[routineName].Reverse();

            foreach (var r in reverse)
            {
                var request = Request.Create(r.CommandName, r.BroadCastMessage + "[Routine: " + routineName + "]", args);

                var response = new Response(request, request.Message + "[Routine: " + routineName + "] [Executing Rollback Method: " + r.Rollback.Method.Name + "]", ResponseTypes.Success);
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
                    BroadCastExceptionResponse(rollbackException, request);

                    response.Message = "[All is lost. RollBack method failed]" + response.Message;
                    response.ResponseType = ResponseTypes.Error;

                    CommandStream.Enqueue(new CommandStreamItem() { Command = r.CommandName, Response = response });

                    // Broadcase the response to all listeners.
                    Broadcast(response);

                    break;
                }

                return result;
            }

            return result;
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
                    var request= Request.Create(r.CommandName, r.BroadCastMessage + "[Routine: " + routineName + "]", args);
                    var exResponseUniqueKey = BroadCastExceptionResponse(ex, request);

                    try
                    {
                        // If we have a fail over action then try it.
                        if (r.FailOver != null)
                        {

                            try
                            {
                                result = ProcessFailOver<T>(routineName, request, exResponseUniqueKey, r, args);
                            }
                            catch (Exception failOverException)
                            {
                                BroadCastExceptionResponse(failOverException, request);
                                throw;
                            }
                        }
                        else
                        {
                            throw;
                        }
                    }
                    catch (Exception deepEx)
                    {
                        BroadCastExceptionResponse(deepEx, request);
                        throw;
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
                    response.Message = String.Format(ResponseMessage, "N/A", request.Action.Method.Name, response.Type.Name);
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
                Message = string.Format("{0} [Request Id: {1} ] - '{2}'", ex.Message, request.RequestUniqueKey, request.Message),
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
            response.Type = result.GetType();
            response.EventName = r.EventName;
            response.Message = String.Format(ResponseMessage, "N/A", r.Action.Method.Name, response.Type.Name);

            CommandStream.Enqueue(new CommandStreamItem() { Command = r.CommandName, Response = response });

            // Broadcase the response to all listeners.
            Broadcast(response);

            return result;
        }

        private T ProcessFailOver<T>(RoutineNames routineName, Request request, Guid exResponseUniqueKey, RoutineItem r, object[] args)
        {

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
