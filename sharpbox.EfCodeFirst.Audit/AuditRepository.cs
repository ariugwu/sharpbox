using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using sharpbox.Common.Dispatch.Model;

namespace sharpbox.EfCodeFirst.Audit
{
  public static class AuditRepository
  {
    public static List<Dispatch.Model.Response> AuditTrail(int take = 250)
    {
      using (var db = new AuditContext())
      {
        return db.Responses.Include(x => x.Request)
          .Include(x => x.EventName)
          .Include(x => x.Request.CommandName)
          .OrderByDescending(x => x.CreatedDate)
          .Take(take)
          .ToList();
      }
    }

    public static List<Dispatch.Model.Response> AuditTrailByAppId(Guid appId, int take = 250)
    {
      using (var db = new AuditContext())
      {
        return db.Responses.Include(x => x.Request)
          .Include(x => x.EventName)
          .Include(x => x.Request.CommandName)
          .Where(x => (Guid) x.ApplicationId == appId)
          .OrderByDescending(x => x.CreatedDate)
          .Take(250)
          .ToList();
      }
    }

    public static List<EventName> GetEventNames(string connectionStringName = "Audit")
    {
      using (var db = new AuditContext(connectionStringName))
      {
        return db.EventNames.ToList();
      }
    }

    public static List<CommandName> GetCommandName(string connectionStringName = "Audit")
    {
      using (var db = new AuditContext(connectionStringName))
      {
        return db.CommandNames.ToList();
      }
    }
  }
}
