using System;
using System.Collections.Generic;
using System.IO;

namespace sharpbox.Common.Dispatch
{
    using Model;

    public interface IDispatchContext
    {
        /// <summary>
        /// The list of all commands processed by the Dispatcher.
        /// </summary>
        Queue<ICommandStreamItem> CommandStream { get; }

        /// <summary>
        /// List of all queries sent to the Dispatcher
        /// </summary>
        Queue<QueryName> QueryStream { get; }

        Dictionary<CommandName, ICommandHubItem> CommandHub { get; }

        Dictionary<EventName, Queue<Action<IResponse>>> EventHub { get; }

        Dictionary<RoutineName, Queue<IRoutineItem>> RoutineHub { get; }

        Dictionary<QueryName, Delegate> QueryHub { get; }

        /// <summary>
        /// This method will take the action and append it to the list for the given publisher name. Whenever publish is called for that publisherName the associated methods will be invoked.
        /// </summary>
        /// <param name="eventName">The event you would like to subscribe to</param>
        /// <param name="method">The callback method to target when that event is fired.</param>
        void Listen(EventName eventName, Action<IResponse> method);

        /// <summary>
        /// Any method here will get called for every event.
        /// </summary>
        /// <param name="method">The callback method to target when an event is fired.</param>
        void Echo(Action<IResponse> method);

        /// <summary>
        /// Allows you to register a query by name and target
        /// </summary>
        /// <typeparam name="T">The entity to be returned</typeparam>
        /// <param name="queryName">The name of the query (i.e. - 'Get', 'GetXByZFromY', etc)</param>
        /// <param name="target">The method to be invoked</param>
        void Register<T>(QueryName queryName, Delegate action);

        /// <summary>
        /// Register with the command hub, which is used for creating the 1-to-1 relationship between thier commands name and event. i.e. - UpdateReciepe and OnReceipeUpdate you still need to register what callback you want to run on UpdateReceipe. Same with the event.
        /// </summary>
        /// <param name="commandName">The command to register</param>
        /// <param name="action">The action to invoke.</param>
        /// <param name="eventName">The channel to broadcast the response on.</param>
        void Register<T>(CommandName commandName, Func<T, T> action, EventName eventName);

        void Register<T>(CommandName commandName, Delegate action, EventName eventName);

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
        void Register<T>(RoutineName routineName, CommandName commandName, EventName eventName, Func<T, T> action, Func<T, T> failOver = null, Func<T, T> rollBack = null);

        /// <summary>
        /// Will await the registered method so the call returns immediately. Ensure the key exists, add the response to the Event stream. Cycle through all the subscribers and fire off the associated action.
        /// @SEE: For the apparoch to catch Target Invocation exceptions -> http://csharptest.net/350/throw-innerexception-without-the-loosing-stack-trace/
        /// </summary>
        /// <param name="response">The changed object, the original request, and other useful data is packaged in this object for easy sharing.</param>
        void Broadcast(IResponse response);

        /// <summary>
        /// A factored out helper for anytime an exception needs to be broadcsat. Also adds the failed request/response to the command stream. NOTE: Should also be used PostProcess calls outside of the dispatcher to log and broadcast errors.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        IResponse BroadCastExceptionResponse(Exception ex, IRequest request);

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
        T RollBack<T>(RoutineName routineName, string message, object[] args);

        /// <summary>
        /// Fires off the queue for the given routine in order. The argument should be the parameters for all registered actions since each target takes in and returns the same type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="routineName"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        T Run<T>(RoutineName routineName, string message, object[] args);

        /// <summary>
        /// Simply put: pass in some arguments that your target method will understand and get some variant of T back
        /// </summary>
        /// <typeparam name="T">The type result of the request</typeparam>
        /// <param name="queryName">The name of the query</param>
        /// <param name="args">The arguments to be processed</param>
        /// <returns>The return T</returns>
        object Fetch(QueryName queryName, object[] args);

        /// <summary>
        /// Fires off the action associated with this command. T is the returning type. You can register your command with a Func that takes and returns T, or an inline func that takes any number of parameters to be passed in here. However, the return time must match T (always).
        /// </summary>
        /// <typeparam name="T">The return type and possibly also the sole parameter.</typeparam>
        /// <param name="commandName">The command name is look up the registered delegate/func/action with.</param>
        /// <param name="message">The message you would like associated with this requested and stored in the audit log.</param>
        /// <param name="args">The arguments to pass to your registered delegate/func/action.</param>
        /// <returns></returns>
        IResponse Process<T>(CommandName commandName, string message, object[] args);
    }
}
