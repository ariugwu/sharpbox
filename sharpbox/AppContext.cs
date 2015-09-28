﻿using System;
using System.Collections.Generic;
using System.Net.Mail;
using sharpbox.Dispatch;
using sharpbox.Localization.Model;

namespace sharpbox
{
    using sharpbox.Email.Dispatch;
    using sharpbox.Notification.Dispatch;
    using sharpbox.Notification.Model;

    [Serializable]
  public class AppContext
  {
    /// <summary>
    /// A bit of a kitchen sink. Will instantiate Dispatch, Email, File, Audit, Notification, and map default commands and listeners.
    /// </summary>
    /// <param name="smtpClient"></param>
    /// <param name="ioStrategy"></param>
    public AppContext(SmtpClient smtpClient, Io.Strategy.IStrategy ioStrategy)
    {
      Dispatch = new Client();
      Email = new Email.Client(smtpClient);
      File = new Io.Client(ioStrategy);
      Audit = new Audit.Client();
      Notification = new Notification.Client(Email);

      RegisterCommands();
      MapListeners();
    }

    /// <summary>
    /// Do all the wiring yourself
    /// </summary>
    public AppContext()
    {
    }

    /// <summary>
    /// Handy encapsulation for resources you will/could/might use throughout the application
    /// </summary>
    public Dictionary<ResourceName, string> Resources { get; set; }
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
      Dispatch.Register<MailMessage>(EmailCommands.SendEmail, SendEmail, EmailEvents.OnEmailSend);

      // IO
      //Dispatch.Register(ExtendedCommandNames.FileCreate, WriteFile, ExtendedEventNames.OnFileCreate);

      // Notification
      Dispatch.Register<BackLogItem>(NotificationCommands.SendNotification, Notification.Notify, NotificationEvents.OnNotificationNotify);
      Dispatch.Register<Subscriber>(NotificationCommands.AddNotificationSubscriber, new Func<Subscriber, Type, Subscriber>(Notification.AddSub), NotificationEvents.OnNotificationAddSubScriber);

    }

    public void MapListeners()
    {
      // Look at the concept of 'EchoAllEventsTo'. We can setup a filter that will get call for all events. This is helpful for Audit and Notification subsystems.
      Dispatch.Echo(Notification.ProcessEvent);
      Dispatch.Echo(Audit.Record);
    }

    public virtual MailMessage SendEmail(MailMessage mail)
    {
      Email.Send(mail);
      return mail;
    }
  }
}
