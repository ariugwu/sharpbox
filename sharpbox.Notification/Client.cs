﻿using System;
using System.Collections.Generic;
using sharpbox.Dispatch.Model;
using sharpbox.Notification.Domain.Localization;
using sharpbox.Notification.Domain.Notification.Model;

namespace sharpbox.Notification
{
  [Serializable]
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
    private Dictionary<string, Dictionary<Type, List<string>>> _subscribers;
    private List<BackLogItem> _backLog;
    private Dictionary<string, Dictionary<Type, EmailTemplate>> _emailTempalteLookup;
    /// <summary>
    /// Use the string value of the EventName as the key. The object is to complicated to ensure comparing against values easy. Specifically code first. (e.g - If the Id is set in one and not the other there's no match)
    /// </summary>
    public Dictionary<string, Dictionary<Type, List<string>>> Subscribers { get { return _subscribers ?? (_subscribers = new Dictionary<string, Dictionary<Type, List<string>>>()); } set { _subscribers = value; } }

    public List<BackLogItem> BackLog { get { return _backLog ?? (_backLog = new List<BackLogItem>()); } set { _backLog = value; } }

    public Dictionary<string, Dictionary<Type, EmailTemplate>> EmailTemplateLookup
    {
      get
      {
        return _emailTempalteLookup ??
               (_emailTempalteLookup = new Dictionary<string, Dictionary<Type, EmailTemplate>>());
      }

      set { _emailTempalteLookup = value; }
    }

    /// <summary>
    /// Whenever the dispatcher publishes an event we create a message for it and stick it on the queue. Then we see if anyone is requesting notification and we create a backlog entry for them to be processed at a later date. Likely by a scheduled task or explict request.
    /// </summary>
    /// <param name="response"></param>
    public void ProcessEvent(Response response)
    {
      if (!Subscribers.ContainsKey(response.EventName.Name)) return; // Bail early if there are no subscribers.

      // Run through all of the subscribers for this publisher and generate a backlog item for them.
      foreach (var s in Subscribers[response.EventName.Name][response.GetType()])
      {
        string subject;
        string body;

        if (EmailTemplateLookup.ContainsKey(response.EventName.Name) &&
            EmailTemplateLookup[response.EventName.Name].ContainsKey(response.Request.Entity.GetType()))
        {
          var temp = EmailTemplateLookup[response.EventName.Name][response.Request.Entity.GetType()];
          subject = temp.GetSubject(response.Entity);
          body = temp.GetBody(response.Entity);
        }
        else
        {
          subject = "Automated Notification: You were selected to be notified when the following event occurced: " + response.EventName.Name;
          body = response.Message;
        }

        // Add the backlog item
        var bli = new BackLogItem
        {
          AttempNumber = 0,
          AttemptMessage = "Created.",
          BackLogItemUniqueId = Guid.NewGuid(),
          NextAttempTime = null,
          RequestId = response.RequestId,
          RequestUniqueKey = response.RequestUniqueKey,
          ResponseId = response.ResponseId,
          ResponseUniqueKey = response.ResponseUniqueKey,
          SentDate = null,
          UserId = s,
          WasSent = false,
          Subject = subject,
          Message = body
        };

        BackLog.Add(bli);
      }

    }

    public BackLogItem Notify(BackLogItem bli)
    {
      try
      {
        _emailClient.Send(bli.To, bli.From, bli.Subject, bli.Message);
        bli.AttemptMessage = "Sent.";
        bli.AttempNumber = bli.AttempNumber + 1; 
        bli.WasSent = true;
        bli.SentDate = DateTime.Now;
      }
      catch(Exception ex)
      {
        bli.AttemptMessage = "Failed with message: '" + ex.Message + "'";
        bli.AttempNumber = bli.AttempNumber + 1;
      }

      // If this doesn't exist then add it. If it does then update it.
      if (!BackLog.Exists(x => x.BackLogItemUniqueId.Equals(bli.BackLogItemUniqueId)))
      {
        BackLog.Add(bli);
      }
      else
      {
        // update the item in our backlog
        for (var i = 0; i < BackLog.Count; i++)
        {
          if (BackLog[i].BackLogItemUniqueId != bli.BackLogItemUniqueId) continue;

          BackLog[i] = bli;
          break;
        }
      }

      return bli;
    }

    public Subscriber AddSub(Subscriber subscriber)
    {
      if (!Subscribers.ContainsKey(subscriber.EventName.Name)) Subscribers.Add(subscriber.EventName.Name, new List<string>());
      Subscribers[subscriber.EventName.Name].Add(subscriber.UserId);

      return subscriber;
    }

  }
}
