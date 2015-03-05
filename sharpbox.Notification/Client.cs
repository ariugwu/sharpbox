using System;
using System.Collections.Generic;
using sharpbox.Dispatch.Model;
using sharpbox.Notification.Model;

namespace sharpbox.Notification
{
  public class Client
  {
    public Client(Email.Client emailClient)
    {
      _emailClient = emailClient;
    }

    public Client()
    {

    }

    private Email.Client _emailClient;
    private Dictionary<EventNames, List<string>> _subscribers;
    private List<BackLogItem> _backLog;

    public Dictionary<EventNames, List<string>> Subscribers { get { return _subscribers ?? (_subscribers = new Dictionary<EventNames, List<string>>()); } set { _subscribers = value; } }

    public List<BackLogItem> BackLog { get { return _backLog ?? (_backLog = new List<BackLogItem>()); } set { _backLog = value; } }

 
    /// <summary>
    /// Whenever the dispatcher publishes an event we create a message for it and stick it on the queue. Then we see if anyone is requesting notification and we create a backlog entry for them to be processed at a later date. Likely by a scheduled task or explict request.
    /// </summary>
    /// <param name="response"></param>
    public void ProcessEvent(Response response)
    {
      if (!Subscribers.ContainsKey(response.EventName)) return; // Bail early if there are no subscribers.

      // Run through all of the subscribers for this publisher and generate a backlog item for them.
      foreach (var s in Subscribers[response.EventName])
      {
        // Add the backlog item
        var bli = new BackLogItem
        {
          AttempNumber = 0,
          BackLogItemUniqueId = Guid.NewGuid(),
          NextAttempTime = null,
          RequestId = response.RequestId,
          RequestUniqueKey = response.RequestUniqueKey,
          ResponseId = response.ResponseId,
          ResponseUniqueKey = response.ResponseUniqueKey,
          SentDate = null,
          UserId = s,
          WasSent = false,
          Message = response.Message
        };

        _backLog.Add(bli);
      }

    }

    public BackLogItem Notify(BackLogItem bli)
    {
      _emailClient.Send(bli.To, bli.From, bli.Subject, bli.Message);

      bli.WasSent = true;
      bli.SentDate = DateTime.Now;

      // If this doesn't exist then add it. If it does then update it.
      if (!_backLog.Exists(x => x.BackLogItemUniqueId.Equals(bli.BackLogItemUniqueId)))
      {
        _backLog.Add(bli);
      }
      else
      {
        // update the item in our backlog
        for (var i = 0; i < _backLog.Count; i++)
        {
          if (_backLog[i].BackLogItemUniqueId != bli.BackLogItemUniqueId) continue;

          _backLog[i] = bli;
          break;
        }
      }

      return bli;
    }


    public Subscriber AddSub(Subscriber subscriber)
    {
      if (!Subscribers.ContainsKey(subscriber.EventName)) Subscribers.Add(subscriber.EventName, new List<string>());
      Subscribers[subscriber.EventName].Add(subscriber.UserId);

      return subscriber;
    }

  }
}
