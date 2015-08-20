using System;
using System.Collections.Generic;
using sharpbox.Dispatch.Model;
using sharpbox.Localization.Model;
using sharpbox.Membership.Domain.Membership.Model;

namespace sharpbox.Notification.Domain.Localization
{
  [Serializable]
  public abstract class EmailTemplate : ResourceFormatTemplate
  {
    protected EmailTemplate(Type entityType, Resource subjectResource, Resource bodyResource)
    {
      EntityType = entityType;
      _subjectResource = subjectResource;
      _bodyResource = bodyResource;
    }

    private readonly Resource _subjectResource;
    private readonly Resource _bodyResource;

      public Dictionary<EventName, Dictionary<Type, UserRoleType>> SubscribedRoles { get; set; }

    public Type EntityType { get; private set; }

      public string GetSubject(object entity)
    {
      return FormatResource(_subjectResource, entity); 
    }

    public string GetBody(object entity)
    {
      return FormatResource(_bodyResource, entity);
    }

    protected override string FormatResource(Resource resource, object entity)
    {
      if (resource.ResourceType.Equals(ResourceType.EmailSubject)) return FormatSubject(resource, entity);
      if (resource.ResourceType.Equals(ResourceType.EmailBody)) return FormatBody(resource, entity);
      
      throw new ArgumentException("Resource Type not supported by this template");
    }

    protected abstract string FormatSubject(Resource resource, object entity);
    protected abstract string FormatBody(Resource resource, object entity);
  }
}
