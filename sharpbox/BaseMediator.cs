using System;
using System.Collections.Generic;
using System.Net.Mail;
using sharpbox.Dispatch;
using sharpbox.Dispatch.Model;
using sharpbox.Localization.Model;
using sharpbox.Notification.Model;

namespace sharpbox
{
    [Serializable]
  public abstract class BaseMediator
  {
    protected BaseMediator(SmtpClient smtpClient, Io.Strategy.IStrategy ioStrategy)
    {
      Dispatch = new Client();
      Email = new Email.Client(smtpClient);
      File = new Io.Client(ioStrategy);

      // Setup auditing
      Audit = new Audit.Client(); // This is passed as a ref because the audit class will register itself to various events depending on the audit level chosen.

      // Setup Notification
      Notification = new Notification.Client(Email);

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
      Dispatch.Listen(EventNames.OnException, OnException);

      // Look at the concept of 'EchoAllEventsTo'. We can setup a filter that will get call for all events. This is helpful for Audit and Notification subsystems.
      Dispatch.Echo(Notification.ProcessEvent);
      Dispatch.Echo(Audit.Record);
    }

    public virtual MailMessage SendEmail(MailMessage mail)
    {
      Email.Send(mail);
      return mail;
    }


    public virtual void OnException(Response response)
    {
      throw new NotImplementedException();
    }

  }
}
