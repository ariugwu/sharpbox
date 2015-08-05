using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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
      _commandHub = new Dictionary<CommandName, CommandHubItem>();
      _eventSubscribers = new Dictionary<EventName, Queue<Action<Response>>>();
      _echoSubscribers = new Queue<Action<Response>>();
      _routineHub = new Dictionary<RoutineName, Queue<RoutineItem>>();
      CommandStream = new Queue<CommandStreamItem>();
    }

    #region Field(s)


    private const string ResponseMessage = "[Message: {0}] [Method: {1}] [Entity: {2}] ";

    private Dictionary<EventName, Queue<Action<Response>>> _eventSubscribers;

    private Queue<Action<Response>> _echoSubscribers;

    private Dictionary<CommandName, CommandHubItem> _commandHub;

    private Dictionary<RoutineName, Queue<RoutineItem>> _routineHub;

    #endregion

    /// <summary>
    /// The list of all commands processed by the Dispatcher.
    /// </summary>
    public Queue<CommandStreamItem> CommandStream { get; private set; }

    public Dictionary<CommandName, CommandHubItem> CommandHub { get { return _commandHub; } }

    /// <summary>
    /// This method will take the action and append it to the list for the given publisher name. Whenever publish is called for that publisherName the associated methods will be invoked.
    /// </summary>
    /// <param name="eventName">The event you would like to subscribe to</param>
    /// <param name="method">The callback method to target when that event is fired.</param>
    public void Listen(EventName eventName, Action<Response> method)
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
    public void Register<T>(CommandName commandName, Func<T, T> action, EventName eventName)
    {
      try
      {
        _commandHub.Add(commandName, new CommandHubItem { Action = action, EventName = eventName }); // wireup the action associated with this command, and the event channel to broadcast to when this command is processed.
      }
      catch (Exception ex)
      {
        var msg = String.Format(ResponseMessage, "Registration failed with msg: " + ex.Message, action.Method.Name, typeof(T).Name);
        Broadcast(new Response { Entity = ex, Type = ex.GetType(), EventName = EventName.OnException, Message = msg, ResponseUniqueKey = Guid.NewGuid() });
      }
    }

    public void Register<T>(CommandName commandName, Delegate action, EventName eventName)
    {
      try
      {
        _commandHub.Add(commandName, new CommandHubItem { Action = action, EventName = eventName }); // wireup the action associated with this command, and the event channel to broadcast to when this command is processed.
      }
      catch (Exception ex)
      {
        var msg = String.Format(ResponseMessage, "Registration failed with msg: " + ex.Message, action.Method.Name, typeof(T).Name);
        Broadcast(new Response { Entity = ex, Type = ex.GetType(), EventName = EventName.OnException, Message = msg, ResponseUniqueKey = Guid.NewGuid() });
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
    public void Register<T>(RoutineName routineName, CommandName commandName, EventName eventName, Func<T, T> action, Func<T, T> failOver, Func<T, T> rollBack)
    {
      try
      {
        EnsureCommandHubKey(routineName);
        _routineHub[routineName].Enqueue(new RoutineItem { CommandName = commandName, EventName = eventName, Action = action, FailOver = failOver, Rollback = rollBack }); // wireup the action associated with this command, and the event channel to broadcast to when this command is processed.
      }
      catch (Exception ex)
      {
        var msg = String.Format(ResponseMessage, "Registration failed with msg: " + ex.Message, action.Method.Name, typeof(T).Name);
        Broadcast(new Response { Entity = ex, Type = ex.GetType(), EventName = EventName.OnException, Message = msg, ResponseUniqueKey = Guid.NewGuid() });
      }
    }

    /// <summary>
    /// Will await the registered method so the call returns immediately. Ensure the key exists, add the response to the Event stream. Cycle through all the subscribers and fire off the associated action.
    /// @SEE: For the apparoch to catch Target Invocation exceptions -> http://csharptest.net/350/throw-innerexception-without-the-loosing-stack-trace/
    /// </summary>
    /// <param name="response">The changed object, the original request, and other useful data is packaged in this object for easy sharing.</param>
    public void Broadcast(Response response)
    {

      // For the command response we want to loop through the echo subscribers first.
      // Go through each method that wants to 'trace' all events in the system.
      FireOffToEchoSubs(response);

      EnsureEventSubscriberKey(response.EventName);

      // Go through each subscriber to this event.
      // We'll also want to send this to all echo subscribers since technically it's tracable.
      foreach (var p in _eventSubscribers[response.EventName])
      {
        try
        {
          p.Invoke(response);
          var eventResponse = new Response(response.Request,
              String.Format("{0} Broadcast to method: {1}", response.EventName, p.Method.Name),
              ResponseTypes.Info) { EventName = response.EventName };

          FireOffToEchoSubs(eventResponse);
        }
        catch (TargetInvocationException ex)
        {
          BroadCastExceptionResponse(ex, response.Request);
        }
      }
    }

    /// <summary>
    /// A factored out helper for anytime an exception needs to be broadcsat. Also adds the failed request/response to the command stream. NOTE: Should also be used PostProcess calls outside of the dispatcher to log and broadcast errors.
    /// </summary>
    /// <param name="ex"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    public Guid BroadCastExceptionResponse(Exception ex, Request request)
    {
      var exResponse = new Response
      {
        Entity = ex,
        Type = ex.GetType(),
        EventName = EventName.OnException,
        Message = string.Format("[Exception Message: {0} [Request Id: {1} ]", (ex.InnerException == null) ? ex.Message : ex.InnerException.Message, request.RequestUniqueKey),
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

    private void FireOffToEchoSubs(Response response)
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
    public T RollBack<T>(RoutineName routineName, string message, object[] args)
    {

      if (!_routineHub.ContainsKey(routineName)) throw new ArgumentException("There is no routine by this name registered.");

      if (_routineHub[routineName].FirstOrDefault(x => x.Rollback == null) != null) throw new InvalidDataException("There is a method in this routine without a RollBack function assigned. This routine is not eligable for RollBack.");

      var result = default(T);

      var reverse = _routineHub[routineName].Reverse().ToList();

      for (var i = (reverse.Count - 1); i >= 0; i--)
      {
        var request = Request.Create(reverse[i].CommandName, reverse[i].BroadCastMessage + "[Rolling Back Routine: " + routineName + "][" + (i - 1) + "/" + reverse.Count() + "]", args);

        var response = new Response(request, request.Message + "[Routine: " + routineName + "] [Executing Rollback Method: " + reverse[i].Rollback.Method.Name + "]", ResponseTypes.Success);
        response.EventName = reverse[i].EventName;

        try
        {
          result = (T)reverse[i].Rollback.DynamicInvoke(args);
          response.Entity = result;

          // Broadcase the response to all listeners.
          Broadcast(response);
        }
        catch (TargetInvocationException rollbackException)
        {
          BroadCastExceptionResponse(rollbackException, request);

          response.Message = "[All is lost. RollBack method failed]" + response.Message;
          response.ResponseType = ResponseTypes.Error;

          CommandStream.Enqueue(new CommandStreamItem() { Command = reverse[i].CommandName, Response = response });

          // Broadcase the response to all listeners.
          Broadcast(response);

          break;
        }
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
    public T Process<T>(RoutineName routineName, string message, object[] args)
    {
      var result = default(T);
      var routine = _routineHub[routineName].ToList();

      for (var i = 0; i < routine.Count; i++)
      {
        try
        {
          result = ProcessRoutineAction<T>(routineName, routine[i], message + "(Routine: " + routineName + ")(" + (i + 1) + "/" + routine.Count + ")", args);

          //Replace whatever T was that was passed in.
          args[Array.FindIndex(args, x => x.GetType() == typeof(T))] = result;

        }
        catch (Exception ex)
        {
          var request = Request.Create(routine[i].CommandName, routine[i].BroadCastMessage, args);
          var exResponseUniqueKey = BroadCastExceptionResponse(ex, request);

          try
          {
            // If we have a fail over action then try it.
            if (routine[i].FailOver != null)
            {

              try
              {
                result = ProcessFailOver<T>(routineName, request, exResponseUniqueKey, routine[i], args);
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
    /// Fires off the action associated with this command. T is the returning type. You can register your command with a Func that takes and returns T, or an inline func that takes any number of parameters to be passed in here. However, the return time must match T (always).
    /// </summary>
    /// <typeparam name="T">The return type and possibly also the sole parameter.</typeparam>
    /// <param name="commandName">The command name is look up the registered delegate/func/action with.</param>
    /// <param name="message">The message you would like associated with this requested and stored in the audit log.</param>
    /// <param name="args">The arguments to pass to your registered delegate/func/action.</param>
    /// <returns></returns>
    public Response Process<T>(CommandName commandName, string message, object[] args)
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

    private T ProcessRoutineAction<T>(RoutineName routineName, RoutineItem r, string message, object[] args)
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

    private T ProcessFailOver<T>(RoutineName routineName, Request request, Guid exResponseUniqueKey, RoutineItem r, object[] args)
    {
      request.Message = request.Message;
      var response = new Response(request, "", ResponseTypes.Success);

      var result = (T)r.FailOver.DynamicInvoke(args);
      response.Entity = result;
      response.Type = result.GetType();
      response.EventName = r.EventName;
      response.Message = "[Executing Failover Method: " + r.FailOver.Method.Name + "]" + String.Format(ResponseMessage, "N/A", r.Action.Method.Name, response.Type.Name);

      CommandStream.Enqueue(new CommandStreamItem()
      {
        Command = r.CommandName,
        Response = response
      });

      // Broadcase the response to all listeners.
      Broadcast(response);

      return result;
    }


    private void EnsureEventSubscriberKey(EventName eventName)
    {
      if (!_eventSubscribers.ContainsKey(eventName)) _eventSubscribers.Add(eventName, new Queue<Action<Response>>());
    }

    private void EnsureCommandHubKey(RoutineName routineName)
    {
      if (!_routineHub.ContainsKey(routineName)) _routineHub.Add(routineName, new Queue<RoutineItem>());
    }
  }
}
