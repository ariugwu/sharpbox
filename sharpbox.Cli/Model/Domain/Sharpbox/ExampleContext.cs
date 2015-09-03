using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Net.Mail;
using sharpbox.Dispatch;
using sharpbox.Dispatch.Model;
using sharpbox.EfCodeFirst.Audit;
using sharpbox.EfCodeFirst.Notification;
using sharpbox.Io.Model;
using sharpbox.Localization.Model;
using sharpbox.Notification.Domain.Notification.Model;

namespace sharpbox.Cli.Model.Domain.Sharpbox
{
  [Serializable]
  public class ExampleContext : AppContext
  {
    /// <summary>
    /// Extension of the AppContext which contains the dispatcher. All we've done is throw in some dispatcher friendly components.
    /// </summary>
    /// <param name="LogOn">Example of something you might want encapulated and updated.</param>
    /// <param name="smtpClient">Powers the email client.</param>
    public ExampleContext(string LogOn, SmtpClient smtpClient)
      : base()
    {

      Dispatch = new Client();

      this.LogOn = LogOn;

      Email = new Email.Client(smtpClient);
      File = new Io.Client(new Io.Strategy.Binary.BinaryStrategy());

      Audit = new Audit.Client(); // This is passed as a ref because the audit class will register itself to various events depending on the audit level chosen.

      // Bootstrap the notification client.
      Notification = new Notification.Client(Email);
      var subjectResource = new Resource() { Value = "Example Subject: {0}", ResourceType = ResourceType.EmailSubject};
      var bodyResource = new Resource { Value = "Example Body: {0}", ResourceType = ResourceType.EmailBody};

      var emailTemplate = new ExampleEmailTemplate(typeof(object[]), subjectResource, bodyResource);

      Notification.AddEmailTemplate(ExampleContext.OnUserChange, emailTemplate);

      WireUpListeners();
      WireUpRoutines();
      WireUpCommandHubItems();

    }

    public string LogOn { get; set; }

    #region Domain Specific Event(s)
    public static readonly EventName OnUserChange = new EventName("OnUserChange");
    public static readonly EventName Write = new EventName("OnUserChange");
    public static readonly EventName OnRandomFileWritten = new EventName("OnRandomFileWritten");
    public static readonly EventName OnWriteAuditTrailToDisk = new EventName("OnWriteAuditTrailToDisk");
    public static readonly EventName OnDummyPassThroughCommand = new EventName("OnDummyPassThroughCommand");
    #endregion

    #region Domain Specific Commands(s)
    public static readonly CommandName UserChange = new CommandName("ChangeUser");
    public static readonly CommandName WriteARandomFile = new CommandName("WriteARandomFile");
    public static readonly CommandName WriteAuditTrailToDisk = new CommandName("WriteAuditTrailToDisk");
    public static readonly CommandName DummyPassThroughCommand = new CommandName("DummyPassThroughCommand");
    #endregion

    public void WireUpCommandHubItems()
    {
      // Setup what a command should do and who it should broadcast to when it's done
      Dispatch.Register<String>(ExampleContext.UserChange, ChangeUser, ExampleContext.OnUserChange);

      // We use this command to showcase how you can wire up existing code that you want audited, or otherwise a part of the command stream but not necessarily processed.
      Dispatch.Register<String>(ExampleContext.DummyPassThroughCommand, (value) => value, ExampleContext.OnDummyPassThroughCommand);

      Dispatch.Register<BackLogItem>(sharpbox.Notification.Domain.Dispatch.NotificationCommands.SendNotification, Notification.Notify, sharpbox.Notification.Domain.Dispatch.NotificationEvents.OnNotificationNotify);
      Dispatch.Register<Subscriber>(sharpbox.Notification.Domain.Dispatch.NotificationCommands.AddNotificationSubscriber, new Func<Subscriber, Type, Subscriber>(Notification.AddSub), sharpbox.Notification.Domain.Dispatch.NotificationEvents.OnNotificationAddSubScriber);
      Dispatch.Register<MailMessage>(sharpbox.Email.Domain.Dispatch.EmailCommands.SendEmail, SendEmail, sharpbox.Email.Domain.Dispatch.EmailEvents.OnEmailSend);
      Dispatch.Register<FileDetail>(WriteARandomFile, WriteRandomTxtFile, OnRandomFileWritten);
      Dispatch.Register<List<Response>>(WriteAuditTrailToDisk, StoreAuditTrailAsBinary, OnWriteAuditTrailToDisk);

    }

    public void WireUpRoutines()
    {
      // Let's try a routine
      // Our first routine item will feed a string argument to the UserChange method, broadcast the event through the OnUserChange channel
      Dispatch.Register<string>(RoutineName.Example, ExampleContext.UserChange, ExampleContext.OnUserChange, ChangeUser, null, null);
      Dispatch.Register<string>(RoutineName.Example, ExampleContext.UserChange, ExampleContext.OnUserChange, ChangeUserStep2, ChangeUserStep2FailOver, null);
      Dispatch.Register<string>(RoutineName.Example, ExampleContext.UserChange, ExampleContext.OnUserChange, ChangeUserStep3, null, null);
    }

    public void WireUpListeners()
    {
      // Listen to an 'under the covers' system event
      Dispatch.Listen(EventName.OnException, ExampleListener);

      // All of our internal stuff uses the broadcast system so we'll listen on exception and rethrow.
      // TODO: Does this hide the info? Is there any benefit to throwing it from the offending method/call?
      Dispatch.Listen(EventName.OnException, FireOnException);

      Dispatch.Listen(sharpbox.Notification.Domain.Dispatch.NotificationEvents.OnNotificationAddSubScriber, ExampleListener);
      Dispatch.Listen(OnWriteAuditTrailToDisk, ExampleListener);

      // Look at the concept of 'Echo'. We can setup a filter that will get call for all events. This is helpful for Audit and Notification subsystems.
      Dispatch.Echo(Notification.ProcessEvent);
      Dispatch.Echo(Audit.Record);
    }

    #region Event and Command Method(s)

    public static void ExampleListener(Response response)
    {
      Console.WriteLine("{0} broadcasts: {1}", response.EventName, response.Message);
    }

    public static void FireOnException(Response response)
    {
      var exception = response.Entity as Exception;
      if (exception != null) Console.WriteLine("The dispatch is designed to catch all exceptions. You can listen for them and do what you need with the exception itself. Ex Message:" + exception.Message);
    }

    public void OutPutCommandStream()
    {
      Console.WriteLine("### Command Stream Dump ###");
      foreach (var e in Dispatch.CommandStream)
      {
        Console.WriteLine("Command: " + e.Command + "");
        Console.WriteLine("     Request : " + e.Response.Request.Message);
        Console.WriteLine("     Response: " + e.Response.Message);
      }
    }

    public List<Response> StoreAuditTrailAsBinary(List<Response> trail)
    {
      File.Write("AuditTrail.dat", trail);

      return trail;
    }

    public FileDetail WriteRandomTxtFile(FileDetail fileDetail)
    {
      File.Write(fileDetail);

      return fileDetail;
    }
    public string ChangeUser(string userId)
    {

      LogOn = userId;

      return LogOn;
    }

    public string ChangeUserStep2(string userId)
    {

      throw new NotImplementedException("Let's see if the app will failover.");
    }

    public string ChangeUserStep2FailOver(string userId)
    {

      LogOn = userId + "-We changed this through the routine's Second (Failover) Step.";

      return LogOn;
    }

    public string ChangeUserStep3(string userId)
    {

      LogOn = userId + "-We changed this through the routine's Third Step.";

      return LogOn;
    }

    public void Final(Response response)
    {
      Final();
    }

    public void Final()
    {
      using (var db = new AuditContext())
      {
        foreach (var a in Audit.Trail)
        {
          db.Responses.AddOrUpdate(a);
          db.SaveChanges();
        }
      }

      using (var db = new NotificationContext())
      {
        foreach (var b in Notification.BackLog)
        {
          db.BackLogItems.AddOrUpdate(b);
        }
      }
    }

    #endregion
  }
}
