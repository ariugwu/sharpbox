using System;
using System.Collections.Generic;
using System.Net.Mail;
using sharpbox.Dispatch;
using sharpbox.Dispatch.Model;
using sharpbox.Localization.Model;
using sharpbox.Notification.Model;

namespace sharpbox
{
  public abstract class BaseMediator
  {
    protected BaseMediator(List<EventNames> eventNames, SmtpClient smtpClient, Io.Strategy.IStrategy ioStrategy)
    {
      Dispatch = new Client();
      Email = new Email.Client(smtpClient);
      File = new Io.Client(ioStrategy);

      // The following modules require persistence
      var dispatcher = Dispatch;

      // Setup auditing
      Audit = new Audit.Client(ref dispatcher, eventNames); // This is passed as a ref because the audit class will register itself to various events depending on the audit level chosen.

      // Setup Notification
      Notification = new Notification.Client(ref dispatcher, Email, eventNames);

      RegisterCommands();
      MapListeners();

    }

    protected BaseMediator()
    {
      Dispatch = new Client();
    }

    public Dictionary<ResourceNames, string> Resources { get; set; }

    public Client Dispatch { get; set; }
    public Notification.Client Notification { get; set; } // A dispatch friendly notification system.
    public Email.Client Email { get; set; } // A dispatch friendly email client
    public Audit.Client Audit { get; set; } // A dispatch friendly Auditor
    public Io.Client File { get; set; } // A dispatch friendly file client

    /// <summary>
    /// Map our actions and listeners to the dispatch
    /// </summary>
    public void RegisterCommands()
    {
      // Dispatch
      Dispatch.Register<Queue<CommandStreamItem>>(CommandNames.BroadcastCommandStream, BroadCastCommandStream, ExtendedEventNames.OnBroadcastCommandStream);
      Dispatch.Register<Queue<CommandStreamItem>>(CommandNames.BroadcastCommandStreamAfterError, BroadCastCommandStream, ExtendedEventNames.OnBroadcastCommandStream);

      // Email
      Dispatch.Register<MailMessage>(ExtendedCommandNames.SendEmail, SendEmail, ExtendedEventNames.OnEmailSend);

      // IO
      //Dispatch.Register(ExtendedCommandNames.FileCreate, WriteFile, ExtendedEventNames.OnFileCreate);

      // Notification
      Dispatch.Register<BackLogItem>(ExtendedCommandNames.SendNotification, Notification.Notify, ExtendedEventNames.OnNotificationNotify);
      Dispatch.Register<Subscriber>(ExtendedCommandNames.AddNotificationSubscriber, Notification.AddSub, ExtendedEventNames.OnNotificationAddSubScriber);

    }

    public void MapListeners()
    {
      // Dispatch
      Dispatch.Listen(ExtendedEventNames.OnBroadcastCommandStream, OnBroadcastCommandStream);
      Dispatch.Listen(ExtendedEventNames.OnBroadcastCommandStreamAfterError, OnBroadcastCommandStreamAfterError);
      Dispatch.Listen(EventNames.OnException, OnException);
    }

    public virtual MailMessage SendEmail(MailMessage mail)
    {
      Email.Send(mail);
      return mail;
    }

    public virtual T CreateFile<T>(T entity, string filePath,bool append = false) where T : new()
    {
        File.Write(filePath, entity, append);

        return entity;
    }

      public virtual List<Response> PersistAuditTrail(List<Response> responses)
      {
          File.Write("Audit.dat", responses, false);

          return responses;
      } 

    public virtual Response ReadFile(Request request)
    {
      throw new NotImplementedException();
    }

    public virtual Response DeleteFile(Request request)
    {
      throw new NotImplementedException();
    }

    public Queue<CommandStreamItem> BroadCastCommandStream(object gottaPassSomething)
    {
      // Example of being able to bend the rules a little bit and simply use the system to kick off a response, as apposed to deliverying something to be processed.
      return Dispatch.CommandStream;
    }

    public virtual void OnFileCreate(Response response)
    {
      throw new NotImplementedException();
    }

    public virtual void OnFileRead(Response response)
    {
      throw new NotImplementedException();
    }

    public virtual void OnFileDelete(Response response)
    {
      throw new NotImplementedException();
    }

    public virtual void OnNotificationSent(Response response)
    {
      throw new NotImplementedException();
    }

    public virtual void OnNotificationSubscriberAdded(Response response)
    {
      throw new NotImplementedException();
    }

    public virtual void OnEmailSent(Response response)
    {
      throw new NotImplementedException();
    }

    public virtual void OnBroadcastCommandStream(Response response)
    {
      throw new NotImplementedException();
    }

    public virtual void OnBroadcastCommandStreamAfterError(Response response)
    {
      throw new NotImplementedException();
    }

    public virtual void OnException(Response response)
    {
      throw new NotImplementedException();
    }

    public virtual void OnAuditRecord(Response response)
    {
      throw new NotImplementedException();
    }
  }
}
