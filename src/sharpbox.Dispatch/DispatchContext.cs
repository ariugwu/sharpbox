using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using sharpbox.Localization.Model;

namespace sharpbox.Dispatch
{
    using Model;

    /// <summary>
    /// Provides a dispatcher to help decouple commands/requests from business logic.
    /// </summary>
    /// <exception cref="TargetInvocationException">A registered Action failed during either the Command Processing or the Event Broadcast.</exception>
    /// <exception cref="Exception">General uncaught exception from either Command Processing or an Event Broadcast</exception>
    [Serializable]
    public class DispatchContext : IDispatchContext
    {
        #region Constructor(s)

        public DispatchContext()
        {
            this.CommandHub = new Dictionary<CommandName, ICommandHubItem>();
            this.EventHub = new Dictionary<EventName, Queue<Action<IResponse>>>();
            this.EchoSubscribers = new Queue<Action<IResponse>>();
            this.RoutineHub = new Dictionary<RoutineName, Queue<IRoutineItem>>();
            this.QueryHub = new Dictionary<QueryName, Delegate>();
            this.CommandStream = new Queue<ICommandStreamItem>();
            this.QueryStream = new Queue<QueryName>();
            this.FeedbackHub = new Dictionary<string, Feedback>();
        }
        
        #endregion

        #region Field(s)

        private const string ResponseMessage = "[Message: {0}] [Method: {1}] [Entity: {2}] ";

        #endregion

        #region Properties
        /// <summary>
        /// The list of all commands processed by the Dispatcher.
        /// </summary>
        public Queue<ICommandStreamItem> CommandStream { get; private set; }

        /// <summary>
        /// List of all queries sent to the Dispatcher
        /// </summary>
        public Queue<QueryName> QueryStream { get; private set; }

        public Dictionary<CommandName, ICommandHubItem> CommandHub { get; }

        public Dictionary<EventName, Queue<Action<IResponse>>> EventHub { get; }

        public Dictionary<RoutineName, Queue<IRoutineItem>> RoutineHub { get; }

        public Dictionary<QueryName, Delegate> QueryHub { get; }

        public Dictionary<string, Feedback> FeedbackHub { get; set; } 

        public Queue<Action<IResponse>> EchoSubscribers { get; }

        #endregion

        #region Register

        public void Listen(EventName eventName, Action<IResponse> method)
        {
            this.EnsureEventSubscriberKey(eventName);

            this.EventHub[eventName].Enqueue(method);

        }

        /// <summary>
        /// Any method here will get called for every event.
        /// </summary>
        /// <param name="method">The callback method to target when an event is fired.</param>
        public void Echo(Action<IResponse> method)
        {
            this.EchoSubscribers.Enqueue(method);
        }

        /// <summary>
        /// Allows you to register a query by name and target
        /// </summary>
        /// <typeparam name="T">The entity to be returned</typeparam>
        /// <param name="queryName">The name of the query (i.e. - 'Get', 'GetXByZFromY', etc)</param>
        /// <param name="action">The target delegate that will return T</param>
        /// <param name="feedback"></param>
        public void Register<T>(QueryName queryName, Delegate action, Feedback feedback)
        {
            try
            {
                this.FeedbackHub.Add(queryName.Name, feedback);
                this.QueryHub.Add(queryName, action); // wireup the action associated with this command, and the event channel to broadcast to when this command is processed.
            }
            catch (Exception ex)
            {
                var msg = string.Format(ResponseMessage, "Query Registration failed with msg: " + ex.Message, action.Method.Name, typeof(T).Name);
                this.Broadcast(new Response { Entity = ex, Type = ex.GetType(), EventName = EventName.OnException, Message = msg });
            }
        }

        /// <summary>
        /// Register with the command hub, which is used for creating the 1-to-1 relationship between their commands name and event. i.e. - UpdateThing and OnThingUpdate you still need to register what callback you want to run on UpdateThing. Same with the event.
        /// </summary>
        /// <param name="commandName">The command to register</param>
        /// <param name="action">The target to invoke.</param>
        /// <param name="eventName">The channel to broadcast the response on.</param>
        /// <typeparam name="T">The entity to be returned</typeparam>
        public void Register<T>(CommandName commandName, Func<T, T> action, EventName eventName, Feedback feedback)
        {
            try
            {
                this.FeedbackHub.Add(commandName.Name, feedback);
                this.CommandHub.Add(commandName, new CommandHubItem { Action = action, EventName = eventName }); // wireup the action associated with this command, and the event channel to broadcast to when this command is processed.
            }
            catch (Exception ex)
            {
                var msg = string.Format(ResponseMessage, "Command Registration failed with msg: " + ex.Message, action.Method.Name, typeof(T).Name);
                this.Broadcast(new Response { Entity = ex, Type = ex.GetType(), EventName = EventName.OnException, Message = msg });
            }
        }

        /// <summary>
        /// Register a command name to a target method. This overload allows you to register a method that returns T but can take any number of arguments
        /// </summary>
        /// <param name="commandName">The command to register</param>
        /// <param name="action">The target to invoke.</param>
        /// <param name="eventName">The channel to broadcast the response on.</param>
        /// <typeparam name="T">The entity to be returned</typeparam>
        public void Register<T>(CommandName commandName, Delegate action, EventName eventName, Feedback feedback)
        {
            try
            {
                this.FeedbackHub.Add(commandName.Name, feedback);
                this.CommandHub.Add(commandName, new CommandHubItem { Action = action, EventName = eventName }); // wireup the action associated with this command, and the event channel to broadcast to when this command is processed.
            }
            catch (Exception ex)
            {
                var msg = string.Format(ResponseMessage, "Command Registration failed with msg: " + ex.Message, action.Method.Name, typeof(T).Name);
                this.Broadcast(new Response { Entity = ex, Type = ex.GetType(), EventName = EventName.OnException, Message = msg });
            }
        }

        /// <summary>
        /// Register with the routine hub, which is used to chain command actions and provide failover and rollback options
        /// </summary>
        /// <typeparam name="T">The type of the parameter which will be passed between actions.</typeparam>
        /// <param name="routineName">Name of the routine to associate this command name, event name, and action.</param>
        /// <param name="commandName">Name of the command to register</param>
        /// <param name="eventName">Broadcast channel</param>
        /// <param name="action">Called as the primary/preferred target</param>
        /// <param name="failOver">Optional. Will be called on error of the 'action'</param>
        /// <param name="rollBack">Optional. In the advent of an error that can't be solved by action or failover. Once this level is reached the command queue will stop processing.</param>
        public void Register<T>(RoutineName routineName, CommandName commandName, EventName eventName, Func<T, Feedback, T> action, Func<T, Feedback, T> failOver = null, Func<T, Feedback, T> rollBack = null)
        {
            try
            {
                this.EnsureRoutineHubKey(routineName);

                this.RoutineHub[routineName].Enqueue(new RoutineItem { CommandName = commandName, EventName = eventName, Action = action, FailOver = failOver, Rollback = rollBack }); // wireup the action associated with this command, and the event channel to broadcast to when this command is processed.
            }
            catch (Exception ex)
            {
                var msg = string.Format(ResponseMessage, "Registration failed with msg: " + ex.Message, action.Method.Name, typeof(T).Name);
                this.Broadcast(new Response { Entity = ex, Type = ex.GetType(), EventName = EventName.OnException, Message = msg });
            }
        }
        #endregion

        #region Broadcast
        /// <summary>
        /// Will await the registered method so the call returns immediately. Ensure the key exists, add the response to the Event stream. Cycle through all the subscribers and fire off the associated action.
        /// @SEE: For the apparoch to catch Target Invocation exceptions -> http://csharptest.net/350/throw-innerexception-without-the-loosing-stack-trace/
        /// </summary>
        /// <param name="response">The changed object, the original request, and other useful data is packaged in this object for easy sharing.</param>
        public void Broadcast(IResponse response)
        {

            // For the command response we want to loop through the echo subscribers first.
            // Go through each method that wants to 'trace' all events in the system.
            this.FireOffToEchoSubs(response);

            this.EnsureEventSubscriberKey(response.EventName);

            // Go through each subscriber to this event.
            // We'll also want to send this to all echo subscribers since technically it's tracable.
            foreach (var p in this.EventHub[response.EventName])
            {
                try
                {
                    p.Invoke(response);
                    var eventResponse = new Response(response.Request,
                        $"{response.EventName} Broadcast to method: {p.Method.Name}",
                        ResponseTypes.Info)
                    { EventName = response.EventName };

                    this.FireOffToEchoSubs(eventResponse);
                }
                catch (TargetInvocationException ex)
                {
                    this.BroadCast(ex, response.Request);
                }
            }
        }

        /// <summary>
        /// A factored out helper for anytime an exception needs to be broadcsat. Also adds the failed request/response to the command stream. NOTE: Should also be used PostProcess calls outside of the dispatcher to log and broadcast errors.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public IResponse BroadCast(Exception ex, IRequest request)
        {
            var response = new Response
            {
                Entity = ex,
                Type = ex.GetType(),
                EventName = EventName.OnException,
                Message = ex.ToString(),
                RequestId = request.RequestId,
                Request = request,
                ResponseType = ResponseTypes.Error
            };

            this.CommandStream.Enqueue(new CommandStreamItem() { Command = request.CommandName, Response = response });

            this.Broadcast(response);

            return response;
        }

        private void FireOffToEchoSubs(IResponse response)
        {
            // Go through each method that wants to 'trace' all events in the system
            foreach (var t in this.EchoSubscribers)
            {
                try
                {
                    t.Invoke(response);
                }
                catch (TargetInvocationException ex)
                {
                    this.BroadCast(ex, response.Request);
                }
            }
        }
        #endregion

        #region Execute

        public T RollBack<T>(RoutineName routineName, Feedback feedback, object[] args)
        {

            if (!this.RoutineHub.ContainsKey(routineName)) throw new ArgumentException("There is no routine by this name registered.");

            if (this.RoutineHub[routineName].FirstOrDefault(x => x.Rollback == null) != null) throw new InvalidDataException("There is a method in this routine without a RollBack function assigned. This routine is not eligable for RollBack.");

            var result = default(T);

            var reverse = this.RoutineHub[routineName].Reverse().ToList();

            for (var i = (reverse.Count - 1); i >= 0; i--)
            {
                var request = Request.Create(reverse[i].CommandName, reverse[i].BroadCastMessage + "[Rolling Back Routine: " + routineName + "][" + (i - 1) + "/" + reverse.Count() + "]", args);

                var response = new Response(
                    request,
                    request.Message + "[Routine: " + routineName + "] [Executing Rollback Method: "
                    + reverse[i].Rollback.Method.Name + "]",
                    ResponseTypes.Success) { EventName = reverse[i].EventName };

                try
                {
                    result = (T)reverse[i].Rollback.DynamicInvoke(args);
                    response.Entity = result;

                    // Broadcase the response to all listeners.
                    this.Broadcast(response);
                }
                catch (TargetInvocationException rollbackException)
                {
                    this.BroadCast(rollbackException, request);

                    response.Message = "[All is lost. RollBack method failed]" + response.Message;
                    response.ResponseType = ResponseTypes.Error;

                    this.CommandStream.Enqueue(new CommandStreamItem() { Command = reverse[i].CommandName, Response = response });

                    // Broadcase the response to all listeners.
                    this.Broadcast(response);

                    break;
                }
            }
            return result;
        }

        public T Run<T>(RoutineName routineName, Feedback feedback, object[] args)
        {
            var result = default(T);
            var routine = this.RoutineHub[routineName].ToList();

            for (var i = 0; i < routine.Count - 1; i++)
            {
                try
                {
                    result = this.Run<T>(routineName, routine[i], new Feedback() { Success = new Resource() {Value = "(Routine: " + routineName + ")(" + (i + 1) + "/" + routine.Count + ")"} }, args);

                    //Replace whatever T was that was passed in.
                    int index = Array.FindIndex(args, x => x.GetType() == typeof(T) || x.GetType().IsSubclassOf(typeof(T)));
                    args[index] = result;

                }
                catch (Exception ex)
                {
                    var request = Request.Create(routine[i].CommandName, routine[i].BroadCastMessage, args);
                    var exResponse = this.BroadCast(ex, request);

                    try
                    {
                        // If we have a fail over action then try it.
                        if (routine[i].FailOver != null)
                        {

                            try
                            {
                                result = this.FailOver<T>(routineName, request, exResponse.ResponseId, routine[i], args);
                            }
                            catch (Exception failOverException)
                            {
                                this.BroadCast(failOverException, request);
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
                        this.BroadCast(deepEx, request);
                        throw;
                    }
                }
            }

            return result;
        }

        public T Fetch<T>(QueryName queryName, object[] args)
        {
            var action = this.QueryHub[queryName];
            var result = action.DynamicInvoke(args);

            // Add the query name to the query stream
            this.QueryStream.Enqueue(queryName);

            return (T) result;
        }

        public IResponse Process<T>(CommandName commandName, object[] args)
        {
            var feedback = this.FeedbackHub[commandName.Name];
            var request = Request.Create(commandName, feedback.Info.Value, args);
            var response = new Response(request, request.Message, ResponseTypes.Success);

            request.Action = this.CommandHub[request.CommandName].Action;

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
                    response.Message = request.Message ?? String.Format(ResponseMessage, "N/A", request.Action.Method.Name, response.Type.Name);
                }

                response.EventName = this.CommandHub[request.CommandName].EventName; // Set the event name.

                // Add The incoming request and out going response to the command stream.
                this.CommandStream.Enqueue(new CommandStreamItem() { Command = request.CommandName, Response = response });

                // Broadcase the response to all listeners.
                this.Broadcast(response);

                return response;
            }
            catch (Exception ex)
            {

                var exResponseUniqueKey = this.BroadCast(ex, request);

                return new Response(request, String.Format("Command Failed: {0}. See Exception with Response (Unique) Key: {1}.", request.CommandName, exResponseUniqueKey), ResponseTypes.Error);
            }
        }

        private T Run<T>(RoutineName routineName, IRoutineItem r, Feedback feedback, object[] args)
        {
            r.BroadCastMessage = feedback.Success.Value; // Set the broadcast message. Other values are set during registration.

            var request = Request.Create(r.CommandName, r.BroadCastMessage, args);
            var response = new Response(request, request.Message + "[Routine: " + routineName + "]", ResponseTypes.Success);


            var result = (T)r.Action.DynamicInvoke(args);
            response.Entity = result;
            response.Type = result.GetType();
            response.EventName = r.EventName;
            response.Message = request.Message ?? String.Format(ResponseMessage, "N/A", r.Action.Method.Name, response.Type.Name);

            this.CommandStream.Enqueue(new CommandStreamItem() { Command = r.CommandName, Response = response });

            // Broadcase the response to all listeners.
            this.Broadcast(response);

            return result;
        }

        private T FailOver<T>(RoutineName routineName, Request request, Guid exResponseId, IRoutineItem r, object[] args)
        {
            request.Message = request.Message;
            var response = new Response(request, "", ResponseTypes.Success);

            var result = (T)r.FailOver.DynamicInvoke(args);
            response.Entity = result;
            response.Type = result.GetType();
            response.EventName = r.EventName;
            response.Message = "[Executing Failover Method: " + r.FailOver.Method.Name + "]" + String.Format(ResponseMessage, "N/A", r.Action.Method.Name, response.Type.Name);

            this.CommandStream.Enqueue(new CommandStreamItem()
            {
                Command = r.CommandName,
                Response = response
            });

            // Broadcase the response to all listeners.
            this.Broadcast(response);

            return result;
        }

        #endregion

        #region Helper(s)

        private void EnsureEventSubscriberKey(EventName eventName)
        {
            if (!this.EventHub.ContainsKey(eventName)) this.EventHub.Add(eventName, new Queue<Action<IResponse>>());
        }

        private void EnsureRoutineHubKey(RoutineName routineName)
        {
            if (!this.RoutineHub.ContainsKey(routineName)) this.RoutineHub.Add(routineName, new Queue<IRoutineItem>());
        }

        #endregion
    }
}
