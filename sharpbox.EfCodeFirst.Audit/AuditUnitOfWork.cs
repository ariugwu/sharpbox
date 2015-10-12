using System;
using System.Linq;
using sharpbox.Dispatch.Model;

namespace sharpbox.EfCodeFirst.Audit
{
  public class AuditUnitOfWork : Common.Data.IUnitOfWork<Response>
  {
    public AuditUnitOfWork(string connectionStringName)
    {
      ConnectionStringName = connectionStringName;
    }
    public string ConnectionStringName { get; set; }

    public Response Add(Response instance)
    {
      using (var db = new AuditContext(this.ConnectionStringName))
      {
        var eventNames = AuditRepository.GetEventNames().Where(x => x.ApplicationId == null || (x.ApplicationId != null && x.ApplicationId == instance.ApplicationId)).ToList();
        var commandNames = AuditRepository.GetCommandName().Where(x => x.ApplicationId == null || (x.ApplicationId != null && x.ApplicationId == instance.ApplicationId)).ToList();

        //Clean up the event name stuff to prevent duplicates
        var eventName = eventNames.FirstOrDefault(x => x.Name.ToLower().Trim() == instance.EventName.Name.ToLower().Trim()) ?? instance.EventName;
        var commandName = (instance.Request != null && instance.Request.CommandName != null) ? commandNames.FirstOrDefault(x => x.Name.ToLower().Trim() == instance.Request.CommandName.Name.ToLower().Trim()) : null;

        // Assign the fulled formed EventName values.
        instance.EventName = eventName;
        instance.EventNameId = eventName.EventNameId;

        // If we have a command then do the same. NOTE: If commandName isn't null then Request must not be null base on our check above. So we set the application id here.
        if (commandName != null)
        {
          instance.Request.CommandName = commandName;
          instance.Request.CommandNameId = commandName.CommandNameId;
          instance.Request.ApplicationId = instance.ApplicationId;
        }
        else if (instance.Request == null)
        {
          instance.Request = Request.Create(new CommandName("SystemMessage"), "No request. System message was generated", String.Empty);
        }

        db.Responses.Add(instance);
        db.SaveChanges();
      }

      return instance;
    }

    public Response Update(Response instance)
    {
      throw new NotImplementedException();
    }

    public Response Remove(Response instance)
    {
      using (var db = new AuditContext(this.ConnectionStringName))
      {
        db.Responses.Remove(instance);
      }

      return instance;
    }
  }
}
